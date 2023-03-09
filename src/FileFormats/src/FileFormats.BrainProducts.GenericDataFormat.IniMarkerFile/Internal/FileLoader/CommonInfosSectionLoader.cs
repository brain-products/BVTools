using System.Diagnostics.CodeAnalysis;
using System.Text;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.MarkerFileEnums;
using BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Properties;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal;

internal static class CommonInfosSectionLoader
{
    private static readonly string[] s_validEegFileExtensions = new string[] { "eeg", "avg", "seg" };

    public static bool TryProcess(MarkerFileContent content, string keyName, string keyValue, [NotNullWhen(false)] out string? exceptionMessage)
    {
        if (!Enum.TryParse(keyName, true, out Definitions.CommonInfosKeys key))
        {
            exceptionMessage = $"{Resources.UnrecognizedKey} {keyName}";
            return false;
        }

        switch (key)
        {
            case Definitions.CommonInfosKeys.Codepage:
                {
                    // resolving Utf-8 string as Utf8 enum
                    if (keyValue.Equals(Definitions.Utf8Enum, StringComparison.OrdinalIgnoreCase))
                    {
                        content.CodePage = Codepage.Utf8;
                    }
                    else
                    {
                        if (!Enum.TryParse(keyValue, true, out Codepage value))
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        // Utf8 is not valid, only Utf-8 is valid
                        if (value == Codepage.Utf8)
                        {
                            exceptionMessage = $"{Resources.UnrecognizedKeyValue} {keyValue}";
                            return false;
                        }

                        content.CodePage = value;
                    }
                }
                break;

            case Definitions.CommonInfosKeys.DataFile:
                if (string.IsNullOrWhiteSpace(keyValue))
                {
                    exceptionMessage = $"{Resources.FileNameCannotBeEmpty} {keyName}";
                    return false;
                }

                string eegFileExtension = Path.GetExtension(keyValue);
                bool isEegFileExtensionValid = s_validEegFileExtensions.Select(p => $".{p}").Contains(eegFileExtension);
                if (!isEegFileExtensionValid)
                {
                    exceptionMessage = $"{Resources.UnrecognizedFileExtension} {ToCommaSeparatedText(s_validEegFileExtensions)}: {keyName}";
                    return false;
                }
                content.DataFile = keyValue;
                break;

            default:
                throw new NotImplementedException(); // should never happen
        }

        exceptionMessage = null;
        return true;
    }

    private static string ToCommaSeparatedText(string[] texts)
    {
        StringBuilder sb = new();
        Array.ForEach(texts, p => sb.Append(sb.Length == 0 ? string.Empty : ", ").Append(p));
        return sb.ToString();
    }
}
