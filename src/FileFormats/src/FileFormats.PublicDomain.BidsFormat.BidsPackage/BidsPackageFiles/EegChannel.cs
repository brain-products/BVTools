namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    /// <summary>
    /// see https://bids-specification.readthedocs.io/en/stable/04-modality-specific-files/03-electroencephalography.html
    /// </summary>
    public class EegChannel
    {
        public EegChannel(string name, ChannelType type, PrefixedUnit units)
        {
            Name = name;
            Type = type;
            Units = units;
        }

        #region REQUIRED
        /// <summary>
        /// REQUIRED. Channel name (e.g., FC1, Cz)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// REQUIRED. Type of channel; MUST use the channel types listed below.
        /// </summary>
        public ChannelType Type { get; set; }

        /// <summary>
        /// REQUIRED. Physical unit of the data values recorded by this channel in SI units (see Appendix V: Units for allowed symbols).
        /// </summary>
        public PrefixedUnit Units { get; set; }
        #endregion

        #region OPTIONAL
        /// <summary>
        /// OPTIONAL. Free-form text description of the channel, or other information of interest. See examples below.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// OPTIONAL. Sampling rate of the channel in Hz.
        /// </summary>
        public double? SamplingFrequency { get; set; }

        /// <summary>
        /// OPTIONAL. Name of the reference electrode(s) (not needed when it is common to all channels, in that case it can be specified in *_eeg.json as EEGReference).
        /// </summary>
        public string? Reference { get; set; }

        /// <summary>
        /// OPTIONAL. Frequencies used for the high-pass filter applied to the channel in Hz. If no high-pass filter applied, use n/a.
        /// </summary>
        public string? LowCutoff { get; set; }

        /// <summary>
        /// OPTIONAL. Frequencies used for the low-pass filter applied to the channel in Hz. If no low-pass filter applied, use n/a. Note that hardware anti-aliasing in A/D conversion of all EEG electronics applies a low-pass filter; specify its frequency here if applicable.
        /// </summary>
        public string? HighCutoff { get; set; }

        /// <summary>
        /// OPTIONAL. Frequencies used for the notch filter applied to the channel, in Hz. If no notch filter applied, use n/a.
        /// </summary>
        public string? Notch { get; set; }

        /// <summary>
        /// OPTIONAL. Data quality observed on the channel (good/bad). A channel is considered bad if its data quality is compromised by excessive noise. Description of noise type SHOULD be provided in [status_description].
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// OPTIONAL. Free-form text description of noise or artifact affecting data quality on the channel. It is meant to explain why the channel was declared bad in [status].
        /// </summary>
        public string? StatusDescription { get; set; }
        #endregion
    }
}
