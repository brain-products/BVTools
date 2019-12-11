namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    internal class DatasetInfo
    {
        public DatasetInfo(string root, string subject, string? session, string task)
        {
            Root = root;
            Subject = subject;
            Session = session;
            Task = task;
        }

        public string Root { get; }
        public string Subject { get; }
        public string? Session { get; }
        public string Task { get; }

        public string FileNamePattern
        {
            get
            {
                string subjectPattern = $"sub-{Subject}";
                string? sessionPattern = Session == null ? null : $"_ses-{Session}";
                string taskPattern = $"_task-{Task}";
                string fileNamePattern = $"{subjectPattern}{sessionPattern}{taskPattern}";

                return fileNamePattern;
            }
        }
    }
}
