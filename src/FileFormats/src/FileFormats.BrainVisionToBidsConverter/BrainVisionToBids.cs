using System;
using System.Collections.Generic;
using System.IO;
using BrainVision.Lab.FileFormats.Internal;
using BrainVision.Lab.FileFormats.Properties;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;
using static System.FormattableString;

namespace BrainVision.Lab.FileFormats
{
    public class BrainVisionToBidsConverter
    {
        private readonly string _destinationFolderPath;

        /// <exception cref="ArgumentException">Thrown when <paramref name="destinationFolderPath"/>consists only of white characters.</exception>
        public BrainVisionToBidsConverter(string destinationFolderPath)
        {
            if (string.IsNullOrWhiteSpace(destinationFolderPath))
                throw new ArgumentException(Resources.ArgumentEmptyOrWhiteCharactersExceptionMessage, nameof(destinationFolderPath));

            _destinationFolderPath = destinationFolderPath;
        }

        public IReadOnlyList<string> Warnings { get; private set; } = Array.Empty<string>();

        /// <exception cref="ArgumentException">Thrown when <paramref name="brainVisionHeaderFilePath"/>consists only of white characters.</exception>
        /// <exception cref="InvalidOperationException">Thrown when vision file fails to load.</exception>
        /// <exception cref="NotSupportedException">Thrown when units are not recognized.</exception>
        public void Convert(string brainVisionHeaderFilePath, CustomizationInfo info)
        {
            if (string.IsNullOrWhiteSpace(brainVisionHeaderFilePath))
                throw new ArgumentException(Resources.ArgumentEmptyOrWhiteCharactersExceptionMessage, nameof(brainVisionHeaderFilePath));

            GenerateSubjectSessionAndTaskIfNotProvided(info, brainVisionHeaderFilePath, out string subjectName, out string? sessionName, out string taskName);

            IBidsPackage bidsPackage = CreateBidsPackage(subjectName, sessionName, taskName);
            BrainVisionPackage bvPackage = CreateBvPackage(brainVisionHeaderFilePath);
            Convert(bvPackage, bidsPackage, info);
        }

        private void GenerateSubjectSessionAndTaskIfNotProvided(CustomizationInfo info, string brainVisionHeaderFilePath,
            out string subjectName, out string? sessionName, out string taskName)
        {
            subjectName = info.SubjectName ?? GetNextAvailableSubjectName(_destinationFolderPath);
            sessionName = info.SessionName; // it is allowed session to be null
            taskName = info.TaskName ?? RemoveBidsSpecialCharacters(Path.GetFileNameWithoutExtension(brainVisionHeaderFilePath));
        }

        private static string RemoveBidsSpecialCharacters(string txt)
        {
            char[] bidsSpecialChars = new char[] { '-', '_' };

            foreach (char specialChar in bidsSpecialChars)
            {
                txt = txt.Replace(Invariant($"{specialChar}"), string.Empty, StringComparison.Ordinal);
            }
            return txt;
        }

        private static BrainVisionPackage CreateBvPackage(string brainVisionHeaderFilePath)
            => BrainVisionPackage.Load(brainVisionHeaderFilePath);

        private IBidsPackage CreateBidsPackage(string subjectName, string? sessionName, string taskName)
            => BidsPackageFactory.Create(_destinationFolderPath, subjectName, sessionName, taskName);

        private void Convert(BrainVisionPackage bvPackage, IBidsPackage bidsPackage, CustomizationInfo info)
        {
            Warnings = Array.Empty<string>();
            PackageConverter converter = new PackageConverter(bvPackage, bidsPackage, info);
            converter.CopyBrainVisionFilesToBidsEegModalityFolder();
            bvPackage.UpdateMissingKeysWithDefaultValues();
            converter.ConvertBrainVisionFilesToBidsFormatFiles();
            Warnings = converter.Warnings;
        }

        private static string GetNextAvailableSubjectName(string destinationFolderPath)
            => GetNextAvailableItemName(destinationFolderPath, BidsPackageFiles.SubjectFolderPrefix, "");

        //private static string GetNextAvailableSessionName(string subjectFolderPath)
        //    => GetNextAvailableItemName(subjectFolderPath, BidsPackageFiles.SessionFolderPrefix, "");

        private static string GetNextAvailableItemName(string destinationPath, string folderPrefix, string itemPattern)
        {
            for (int i = 0; ; ++i)
            {
                string itemName = $"{itemPattern}{i + 1:D2}";
                string folderName = $"{folderPrefix}{itemName}";
                if (!Directory.Exists(Path.Combine(destinationPath, folderName)))
                    return itemName;
            }
        }
    }
}
