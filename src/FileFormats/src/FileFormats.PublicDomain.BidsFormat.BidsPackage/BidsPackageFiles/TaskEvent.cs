namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    public class TaskEvent
    {
        public TaskEvent(double onset, double duration)
        {
            Onset = onset;
            Duration = duration;
        }

        #region REQUIRED
        /// <summary>
        /// REQUIRED. Onset(in seconds) of the event measured from the beginning of the acquisition of the first volume in the corresponding task imaging data file. If any acquired scans have been discarded before forming the imaging data file, ensure that a time of 0 corresponds to the first image stored. In other words negative numbers in "onset" are allowed5.
        /// </summary>
        public double Onset { get; set; }

        /// <summary>
        /// REQUIRED. Duration of the event (measured from onset) in seconds. Must always be either zero or positive.A "duration" value of zero implies that the delta function or event is so short as to be effectively modeled as an impulse.
        /// </summary>
        public double Duration { get; set; }
        #endregion

        #region OPTIONAL
        /// <summary>
        /// OPTIONAL. Onset of the event according to the sampling scheme of the recorded modality(i.e., referring to the raw data file that the events.tsv file accompanies).
        /// Info: I suppose bids uses 1-based sample position. This info is unfortunately not available in the official bids documentations
        /// </summary>
        public int? Sample { get; set; }

        /// <summary>
        /// OPTIONAL. Primary categorisation of each trial to identify them as instances of the experimental conditions. For example: for a response inhibition task, it could take on values "go" and "no-go" to refer to response initiation and response inhibition experimental conditions.
        /// </summary>
        public string? TrialType { get; set; }

        /// <summary>
        /// OPTIONAL. Response time measured in seconds. A negative response time can be used to represent preemptive responses and "n/a" denotes a missed response.
        /// </summary>
        public double? ResponseTime { get; set; }

        /// <summary>
        /// OPTIONAL. Represents the location of the stimulus file (image, video, sound etc.) presented at the given onset time. There are no restrictions on the file formats of the stimuli files, but they should be stored in the /stimuli folder (under the root folder of the dataset; with optional subfolders). The values under the stim_file column correspond to a path relative to "/stimuli". For example "images/cat03.jpg" will be translated to "/stimuli/images/cat03.jpg".
        /// </summary>
        public string? StimFile { get; set; }

        /// <summary>
        /// OPTIONAL. Marker value associated with the event (e.g., the value of a TTL trigger that was recorded at the onset of the event).
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// OPTIONAL. Hierarchical Event Descriptor (HED) Tag. See Appendix III for details.
        /// </summary>
        public string? HED { get; set; }
        #endregion
    }
}
