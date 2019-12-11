using System;
using System.Linq;
using System.Text;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Properties;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    internal class BidsPackage : IBidsPackage
    {
        private readonly DatasetInfo _datasetInfo;
        /// <summary>
        /// characters used by BIDS to concatenate subject, session, task and derivative togother into a file name
        /// </summary>
        private readonly char[] _bidsSpecialChars = new char[] { '-', '_' };

        /// <summary>
        /// A class to manage (load, save) files being a part of a single examination BIDS package.
        /// </summary>
        /// <param name="rootFolder">path to the folder where the subject folder persists</param>
        /// <param name="subject">determines subject folder and is part of file names</param>
        /// <param name="session">determines session folder and is part of file names. May be null, in this case session folder will not be considered and file names will not refer to session</param>
        /// <param name="task">is part of file names</param>
        public BidsPackage(string rootFolder, string subject, string? session, string task)
        {
            ThrowExceptionIfInvalidRootFolder(rootFolder);
            ThrowExceptionIfInvalidSubject(subject);
            ThrowExceptionIfInvalidSession(session);
            ThrowExceptionIfInvalidTask(task);

            _datasetInfo = new DatasetInfo(rootFolder, subject, session, task);
            RootFolder = new RootFolder(_datasetInfo);
        }

        public IRootFolder RootFolder { get; }
        public string Subject => _datasetInfo.Subject;
        public string? Session => _datasetInfo.Session;
        public string Task => _datasetInfo.Task;

        /// <summary>
        /// File name pattern, files named according to this pattern are supposed to belong to the BIDS package
        /// </summary>
        public string FileNamePattern => _datasetInfo.FileNamePattern;

        private static void ThrowExceptionIfInvalidRootFolder(string rootFolder)
        {
            if (string.IsNullOrWhiteSpace(rootFolder))
                throw new ArgumentException(Resources.ArgumentEmptyOrWhiteCharactersExceptionMessage, nameof(rootFolder));
        }

        private void ThrowExceptionIfInvalidSubject(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException(Resources.ArgumentEmptyOrWhiteCharactersExceptionMessage, nameof(subject));
            else if (ContainsBidsSpecialCharacters(subject))
                throw new ArgumentException($"{Resources.ArgumentInvalidCharectersExceptionMessage} {ConvertCharArrayToCommaSeparatedString(_bidsSpecialChars)}.", nameof(subject));
        }

        private void ThrowExceptionIfInvalidSession(string? session)
        {
            if (session != null)
            {
                if (string.IsNullOrWhiteSpace(session))
                    throw new ArgumentException(Resources.ArgumentEmptyOrWhiteCharactersExceptionMessage, nameof(session));
                else if (ContainsBidsSpecialCharacters(session))
                    throw new ArgumentException($"{Resources.ArgumentInvalidCharectersExceptionMessage} {ConvertCharArrayToCommaSeparatedString(_bidsSpecialChars)}.", nameof(session));
            }
        }

        private void ThrowExceptionIfInvalidTask(string task)
        {
            if (string.IsNullOrWhiteSpace(task))
                throw new ArgumentException(Resources.ArgumentEmptyOrWhiteCharactersExceptionMessage, nameof(task));
            else if (ContainsBidsSpecialCharacters(task))
                throw new ArgumentException($"{Resources.ArgumentInvalidCharectersExceptionMessage} {ConvertCharArrayToCommaSeparatedString(_bidsSpecialChars)}.", nameof(task));
        }

        private bool ContainsBidsSpecialCharacters(string s)
            => s.Any(p => _bidsSpecialChars.Contains(p));

        private static string ConvertCharArrayToCommaSeparatedString(char[] characters)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char ch in characters)
            {
                if (sb.Length != 0)
                    sb.Append(", ");

                sb.Append(ch);
            }

            return sb.ToString();
        }
    }
}
