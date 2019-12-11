using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    internal class TaskEventTsv : IConvertibleToStringTable
    {
        private readonly TaskEvent _m;
        public TaskEventTsv(TaskEvent taskEvent)
            => _m = taskEvent;

        private static readonly string[] s_tsvIdentifiers = new string[]
        {
            "onset",
            "duration",
            "sample",
            "trial_type",
            "response_time",
            "stim_file ",
            "value",
            "HED",
        };

        public static IReadOnlyList<string> TsvIdentifiers => s_tsvIdentifiers;

        public List<string?> ToList()
        {
            List<string?> items = new List<string?>
            {
                //REQUIRED
                _m.Onset.ToString(CultureInfo.InvariantCulture),
                _m.Duration.ToString(CultureInfo.InvariantCulture),
                //OPTIONAL
                _m.Sample?.ToString(CultureInfo.InvariantCulture),
                _m.TrialType,
                _m.ResponseTime?.ToString(CultureInfo.InvariantCulture),
                _m.StimFile,
                _m.Value,
                _m.HED,
            };

            Debug.Assert(items.Count == s_tsvIdentifiers.Length);

            return items;
        }
    }
}
