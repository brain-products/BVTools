using System.Diagnostics.CodeAnalysis;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.HeaderFileEnums;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal sealed class FileLoader
{
    private readonly StreamReader _reader;

    public FileLoader(FileStream file)
    {
        _reader = new StreamReader(file); // reader is never explicitly disposed to avoid closing the underlying stream
    }

    #region Version and IdentificationText recognition
    public Version ReadVersion()
    {
        _reader.BaseStream.Seek(0, SeekOrigin.Begin);
        string? identificationText = _reader.ReadLine();

        if (!TryParseVersionFromIdentificationText(identificationText, out Version? version))
            throw new InvalidHeaderFileFormatException(0, $"{Resources.UnrecognizedIdentificationText} {Definitions.IdentificationText}");

        return version;
    }

    private static bool TryParseVersionFromIdentificationText(string? line, [NotNullWhen(true)] out Version? version)
    {
        if (line != null)
        {
            string? recognizedHeaderLine = ExtractIdentificationTextIfRecognized(line);
            if (recognizedHeaderLine != null)
            {
                string textExpectedToContainVersion = line[(recognizedHeaderLine.Length + 1)..];// +1 for space between header and version number
                if (Version.TryParse(textExpectedToContainVersion, out version))
                    return true;
            }
        }

        version = new Version();
        return false;
    }

    private static string? ExtractIdentificationTextIfRecognized(string line)
    {
        string? recognizedHeaderLine;
        if (line.StartsWith($"{Definitions.IdentificationText} ", StringComparison.Ordinal))
            recognizedHeaderLine = Definitions.IdentificationText;
        else if (line.StartsWith($"{Definitions.IdentificationTextOldFashion} ", StringComparison.Ordinal))
            recognizedHeaderLine = Definitions.IdentificationTextOldFashion;
        else
            recognizedHeaderLine = null;
        return recognizedHeaderLine;
    }
    #endregion

    public async Task<IHeaderFileContentVer1> LoadVer1Async()
    {
        _reader.BaseStream.Seek(0, SeekOrigin.Begin);

        Definitions.Section currentSection = Definitions.Section.NoSection;
        int currentLineNumber = -1;

        string? identificationText = await _reader.ReadLineAsync().ConfigureAwait(false);
        ++currentLineNumber;

        if (!TryParseVersionFromIdentificationText(identificationText, out Version? version))
            throw new InvalidHeaderFileFormatException(0, $"{Resources.UnrecognizedIdentificationText} {Definitions.IdentificationText}"); // should never happen

        if (version.Major != 1)
            throw new InvalidHeaderFileFormatException(0, $"{Resources.UnsupportedVersion} {version}");

        HeaderFileContent content = new(identificationText!, version);

        // storing sections and keys for duplication checks
        List<string> alreadyCreatedSections = new();
        List<string> alreadyCreatedKeysInCurrentSection = new();

        string? keyInlinedComments = null;
        bool isInSectionInlineComment = true; // section in-line comment is any comment between [section] and first key in section

        string? line;
        while ((line = await _reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            ++currentLineNumber;

            if (IniFormat.IsCommentLine(line))
            {
                string commentLineText = line[1..]; // removing ';' from text
                if (isInSectionInlineComment)
                {
                    InlinedCommentsLoader.ProcessSectionComment(content, currentSection, commentLineText);
                }
                else
                {
                    keyInlinedComments = FileLoaderCommon.ConcatenateWithNewLine(keyInlinedComments, commentLineText);
                }
            }
            else if (currentSection == Definitions.Section.Comment)
            {
                CommentInfosSectionLoader.TryProcess(content, line);
            }
            else if (IniFormat.IsValidKeyLine(line, out string? keyName, out string? keyValue))
            {
                if (keyInlinedComments != null)
                {
                    InlinedCommentsLoader.ProcessKeyComment(content, currentSection, keyName, keyInlinedComments);
                    keyInlinedComments = null;
                }

                if (alreadyCreatedKeysInCurrentSection.Any(p => keyName.Equals(p, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidHeaderFileFormatException(currentLineNumber, $"{Resources.DuplicatedKey} {keyName}");

                if (!TryProcessKey(content, currentSection, keyName, keyValue, out string? exceptionMessage))
                    throw new InvalidHeaderFileFormatException(currentLineNumber, exceptionMessage);

                alreadyCreatedKeysInCurrentSection.Add(keyName);
                isInSectionInlineComment = false;
            }
            else if (IniFormat.IsSectionLine(line, out string? sectionName))
            {
                Definitions.Section section = Definitions.ParseSectionName(sectionName);

                bool isValidSection = section != Definitions.Section.Unknown && section != Definitions.Section.NoSection;
                if (!isValidSection)
                    throw new InvalidHeaderFileFormatException(currentLineNumber, $"{Resources.UnrecognizedSection} {sectionName}");

                if (alreadyCreatedSections.Any(p => sectionName.Equals(p, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidHeaderFileFormatException(currentLineNumber, $"{Resources.DuplicatedSection} {sectionName}");

                InitializeSectionContent(content, section);

                currentSection = section;
                isInSectionInlineComment = true;

                alreadyCreatedSections.Add(sectionName);
                alreadyCreatedKeysInCurrentSection.Clear();
            }
            else if (string.IsNullOrWhiteSpace(line))
            {
                // do nothing
            }
            else
            {
                throw new InvalidHeaderFileFormatException(currentLineNumber, $"{Resources.InvalidLine} {line}");
            }
        }

        ThrowExceptionIfMandatoryFieldMissing(content);
        ThrowExceptionIfMandatoryFieldHasInvalidKeyValue(content);
        ThrowExceptionIfNumberOfChannelsDoesNotMatchChannels(content);
        ThrowExceptionIfCoordinatesDoNotMatchChannels(content);
        return content;
    }

    private static void InitializeSectionContent(HeaderFileContent content, Definitions.Section section)
    {
        switch (section)
        {
            case Definitions.Section.ChannelInfos:
                content.SetChannelInfos(new List<ChannelInfo>());
                break;
            case Definitions.Section.Coordinates:
                content.SetChannelCoordinates(new List<Coordinates>());
                break;
        }
    }

    private static bool TryProcessKey(HeaderFileContent content, Definitions.Section section, string keyName, string keyValue, [NotNullWhen(false)] out string? exceptionMessage)
    {
        return section switch
        {
            Definitions.Section.NoSection => GlobalSectionLoader.TryProcess(keyName, out exceptionMessage), // (content, keyName, keyValue)
            Definitions.Section.CommonInfos => CommonInfosSectionLoader.TryProcess(content, keyName, keyValue, out exceptionMessage),
            Definitions.Section.BinaryInfos => BinaryInfosSectionLoader.TryProcess(content, keyName, keyValue, out exceptionMessage),
            Definitions.Section.ChannelInfos => ChannelInfosSectionLoader.TryProcess(content, keyName, keyValue, out exceptionMessage),
            Definitions.Section.Coordinates => CoordinatesInfosSectionLoader.TryProcess(content, keyName, keyValue, out exceptionMessage),
            Definitions.Section.Comment => throw new NotImplementedException(), // should never happen
            Definitions.Section.Unknown => throw new NotImplementedException(), // should never happen
            _ => throw new NotImplementedException(), // should never happen
        };
    }

    private static void ThrowExceptionIfMandatoryFieldMissing(HeaderFileContent content)
    {
        // --- CommonInfosKeys ---
        if (content.CodePage == null)
            ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.Codepage.ToString());

        if (content.DataFile == null)
            ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.DataFile.ToString());

        if (content.DataFormat == null)
            ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.DataFormat.ToString());

        if (content.DataOrientation == null)
            ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.DataOrientation.ToString());

        if (content.NumberOfChannels == null)
            ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.NumberOfChannels.ToString());

        if (content.SamplingInterval == null)
            ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.SamplingInterval.ToString());

        bool averaged = content.Averaged ?? (content as IHeaderFileContentVer1).DefaultAveraged;
        if (averaged)
        {
            if (content.SegmentationType == null)
                ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.SegmentationType.ToString());

            if (content.SegmentDataPoints == null)
                ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.SegmentDataPoints.ToString());

            if (content.AveragedSegments == null)
                ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.AveragedSegments.ToString());
        }

        // --- ChannelInfos ---
        IList<ChannelInfo>? channelInfo = content.GetChannelInfos();
        if (channelInfo == null)
            ThrowMandatorySectionNotPresent(Definitions.GetSectionName(Definitions.Section.ChannelInfos)!);

        // --- BinaryInfosKeys ---
        if (content.BinaryFormat == null)
            ThrowMandatoryKeyNotPresent(Definitions.BinaryInfosKeys.BinaryFormat.ToString());
    }

    /// <summary>
    /// These checks are dependent from context (from other fields) and cannot be performed while single key parsing
    /// </summary>
    private static void ThrowExceptionIfMandatoryFieldHasInvalidKeyValue(HeaderFileContent content)
    {
        SegmentationType segmentationType = content.SegmentationType ?? (content as IHeaderFileContentVer1).DefaultSegmentationType;
        if (segmentationType != SegmentationType.NOTSEGMENTED)
        {
            if (content.SegmentDataPoints <= 0)
                ThrowInvalidKeyValue($"{Resources.ValueCannotBeNegativeOrZero}", Definitions.CommonInfosKeys.SegmentDataPoints.ToString());
        }

        bool averaged = content.Averaged ?? (content as IHeaderFileContentVer1).DefaultAveraged;
        if (averaged)
        {
            // SegmentationType must not be NOTSEGMENTED.
            if (content.SegmentationType == SegmentationType.NOTSEGMENTED)
                ThrowInvalidKeyValue($"{Resources.SegmentationTypeMustNotBeNotSegmented}", Definitions.CommonInfosKeys.SegmentationType.ToString());

            if (content.AveragedSegments <= 0)
                ThrowInvalidKeyValue($"{Resources.ValueCannotBeNegativeOrZero}", Definitions.CommonInfosKeys.AveragedSegments.ToString());
        }
    }

    private static void ThrowMandatorySectionNotPresent(string sectionName)
        => throw new InvalidHeaderFileFormatException($"{Resources.MandatorySectionNotPresent} {sectionName}");

    private static void ThrowMandatoryKeyNotPresent(string keyName)
        => throw new InvalidHeaderFileFormatException($"{Resources.MandatoryKeyNotPresent} {keyName}");

    private static void ThrowInvalidKeyValue(string message, string keyName)
        => throw new InvalidHeaderFileFormatException($"{message} {keyName}");

    /// <summary>
    /// This check is dependent from context (from other fields) and cannot be performed while single key parsing
    /// </summary>
    private static void ThrowExceptionIfNumberOfChannelsDoesNotMatchChannels(HeaderFileContent content)
    {
        IList<ChannelInfo> channels = content.GetChannelInfos()!; // it is Mandatory field, can't be null
        int numberOfChannels = content.NumberOfChannels!.Value;  // it is Mandatory field, can't be null

        if (numberOfChannels != channels.Count)
            throw new InvalidHeaderFileFormatException(Resources.ChannelInfosDoNotMatchNumberOfChannels);
    }

    /// <summary>
    /// This check is dependent from context (from other fields) and cannot be performed while single key parsing
    /// </summary>
    private static void ThrowExceptionIfCoordinatesDoNotMatchChannels(HeaderFileContent content)
    {
        int numberOfChannels = content.NumberOfChannels!.Value;  // it is Mandatory field, can't be null
        IList<Coordinates>? coordinates = content.GetChannelCoordinates();

        if (coordinates != null)
        {
            if (coordinates.Count != numberOfChannels)
                throw new InvalidHeaderFileFormatException(Resources.CoordinatesDoNotMatchNumberOfChannels);
        }
    }
}
