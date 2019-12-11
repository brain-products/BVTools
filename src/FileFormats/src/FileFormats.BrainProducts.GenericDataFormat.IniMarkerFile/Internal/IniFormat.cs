using System;
using System.Diagnostics.CodeAnalysis;
// Identical file is in IniHeaderFile

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.Internal
{
    internal static class IniFormat
    {
        private static readonly char[] s_invalidKeyCharacters = new char[] { '=', ';' };

        public static bool IsSectionLine(string line, [NotNullWhen(true)] out string? sectionName)
        {
            bool lineStartsWithBracket = line.StartsWith("[", StringComparison.Ordinal);
            bool lineEndsWithBracket = line.EndsWith("]", StringComparison.Ordinal);

            bool isSection = lineStartsWithBracket && lineEndsWithBracket;

            sectionName = isSection ? line[1..^1] : null; // removing brackets

            return isSection;
        }

        public static string FormatSectionName(string sectionName) => $"[{sectionName}]";

        public static string FormatKeyValueLine(string key, string keyValue) => $"{key}={keyValue}";

        public static bool IsCommentLine(string line) => line.TrimStart().StartsWith(';');

        public static bool KeyNameContainsAnyInvalidCharacter(string keyName) => keyName.IndexOfAny(s_invalidKeyCharacters) >= 0;

        public static bool IsValidKeyLine(string line, [NotNullWhen(true)] out string? keyName, [NotNullWhen(true)] out string? keyValue)
        {
            keyName = null;
            keyValue = null;

            int separatorPos = line.LastIndexOf('=');
            if (separatorPos < 0)
                return false;

            string candidateForKeyStr = line.Substring(0, separatorPos);
            if (string.IsNullOrWhiteSpace(candidateForKeyStr))
                return false;

            if (KeyNameContainsAnyInvalidCharacter(candidateForKeyStr))
                return false;

            keyName = candidateForKeyStr;
            keyValue = line.Substring(separatorPos + 1);

            return true;
        }
    }
}
