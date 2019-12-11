using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal class FileLoader
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
            string identificationText = _reader.ReadLine();

            if (!TryParseVersionFromIdentificationText(identificationText, out Version version))
                throw new InvalidMarkerFileFormatException(0, $"{Resources.UnrecognizedIdentificationText} {Definitions.IdentificationText}");

            return version;
        }

        private static bool TryParseVersionFromIdentificationText(string? line, out Version version)
        {
            if (line != null)
            {
                string? recognizedHeaderLine = ExtractIdentificationTextIfRecognized(line);
                if (recognizedHeaderLine != null)
                {
                    string textExpectedToContainVersion = line.Substring(recognizedHeaderLine.Length + 1); // +1 for space between header and version number
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
            else if (line.StartsWith($"{Definitions.IdentificationTextOldFashion1} ", StringComparison.Ordinal))
                recognizedHeaderLine = Definitions.IdentificationTextOldFashion1;
            else if (line.StartsWith($"{Definitions.IdentificationTextOldFashion2} ", StringComparison.Ordinal))
                recognizedHeaderLine = Definitions.IdentificationTextOldFashion2;
            else
                recognizedHeaderLine = null;
            return recognizedHeaderLine;
        }
        #endregion

        public IMarkerFileContentVer1 LoadVer1()
        {
            _reader.BaseStream.Seek(0, SeekOrigin.Begin);

            Definitions.Section currentSection = Definitions.Section.NoSection;
            int currentLineNumber = -1;

            string? identificationText = _reader.ReadLine();
            ++currentLineNumber;

            if (!TryParseVersionFromIdentificationText(identificationText, out Version version))
                throw new InvalidMarkerFileFormatException(0, $"{Resources.UnrecognizedIdentificationText} {Definitions.IdentificationText}"); // should never happen

            if (version.Major != 1)
                throw new InvalidMarkerFileFormatException(0, $"{Resources.UnsupportedVersion} {version}");

            MarkerFileContent content = new MarkerFileContent(identificationText, version);

            // storing sections and keys for duplication checks
            List<string> alreadyCreatedSections = new List<string>();
            List<string> alreadyCreatedKeysInCurrentSection = new List<string>();

            string? keyInlinedComments = null;
            bool isInSectionInlineComment = true; // section in-line comment is any comment between [section] and first key in section

            string? line;
            while ((line = _reader.ReadLine()) != null)
            {
                ++currentLineNumber;

                if (IniFormat.IsCommentLine(line))
                {
                    string commentLineText = line.Substring(1); // removing ';' from text
                    if (isInSectionInlineComment)
                    {
                        InlinedCommentsLoader.ProcessSectionComment(content, currentSection, commentLineText);
                    }
                    else
                    {
                        // currently the key comments are not present in the file (only section comments).
                        // This code is never executed but I leave it to keep exactly the same code structure as in Header file
                        keyInlinedComments = FileLoaderCommon.ConcatenateWithNewLine(keyInlinedComments, commentLineText);
                    }
                }
                else if (IniFormat.IsValidKeyLine(line, out string? keyName, out string? keyValue))
                {
                    if (keyInlinedComments != null)
                    {
                        // currently the key comments are not present in the file (only section comments).
                        // This code is never executed but I leave it to keep exactly the same code structure as in Header file
                        InlinedCommentsLoader.ProcessKeyComments(currentSection);//, keyName, keyEntryComments);
                        keyInlinedComments = null;
                    }

                    if (alreadyCreatedKeysInCurrentSection.Any(p => 0 == string.Compare(p, keyName, true, CultureInfo.InvariantCulture)))
                        throw new InvalidMarkerFileFormatException(currentLineNumber, $"{Resources.DuplicatedKey} {keyName}");

                    if (!TryProcessKey(content, currentSection, keyName, keyValue, out string? exceptionMessage))
                        throw new InvalidMarkerFileFormatException(currentLineNumber, exceptionMessage);

                    alreadyCreatedKeysInCurrentSection.Add(keyName);
                    isInSectionInlineComment = false;
                }
                else if (IniFormat.IsSectionLine(line, out string? sectionName))
                {
                    Definitions.Section section = Definitions.ParseSectionName(sectionName);

                    bool isValidSection = section != Definitions.Section.Unknown && section != Definitions.Section.NoSection;
                    if (!isValidSection)
                        throw new InvalidMarkerFileFormatException(currentLineNumber, $"{Resources.UnrecognizedSection}{sectionName}");

                    if (alreadyCreatedSections.Any(p => 0 == string.Compare(p, sectionName, true, CultureInfo.InvariantCulture)))
                        throw new InvalidMarkerFileFormatException(currentLineNumber, $"{Resources.DuplicatedSection} {sectionName}");

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
                    throw new InvalidMarkerFileFormatException(currentLineNumber, $"{Resources.InvalidLine} {line}");
                }
            }

            ThrowExceptionIfMandatoryFieldMissing(content);
            return content;
        }

        private static void InitializeSectionContent(MarkerFileContent content, Definitions.Section section)
        {
            switch (section)
            {
                case Definitions.Section.MarkerInfos:
                    content.SetMarkers(new List<MarkerInfo>());
                    break;
            }
        }

        private static bool TryProcessKey(MarkerFileContent content, Definitions.Section section, string keyName, string keyValue, [NotNullWhen(false)] out string? exceptionMessage)
        {
            return section switch
            {
                Definitions.Section.NoSection => GlobalSectionLoader.TryProcess(keyName, out exceptionMessage),//(_content, keyName, keyValue);
                Definitions.Section.CommonInfos => CommonInfosSectionLoader.TryProcess(content, keyName, keyValue, out exceptionMessage),
                Definitions.Section.MarkerInfos => MarkerInfosSectionLoader.TryProcess(content, keyName, keyValue, out exceptionMessage),
                _ => throw new NotImplementedException(), // should never happen
            };
        }

        private static void ThrowExceptionIfMandatoryFieldMissing(MarkerFileContent content)
        {
            // --- CommonInfosKeys ---
            if (content.CodePage == null)
                ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.Codepage.ToString());

            if (content.DataFile == null)
                ThrowMandatoryKeyNotPresent(Definitions.CommonInfosKeys.DataFile.ToString());

            // --- MarkerInfos ---
            List<MarkerInfo>? markers = content.GetMarkers();
            if (markers == null)
                ThrowMandatorySectionNotPresent(Definitions.GetSectionName(Definitions.Section.MarkerInfos)!);
        }

        private static void ThrowMandatorySectionNotPresent(string sectionName)
            => throw new InvalidMarkerFileFormatException($"{Resources.MandatorySectionNotPresent} {sectionName}");

        private static void ThrowMandatoryKeyNotPresent(string keyName)
            => throw new InvalidMarkerFileFormatException($"{Resources.MandatoryKeyNotPresent} {keyName}");
    }
}
