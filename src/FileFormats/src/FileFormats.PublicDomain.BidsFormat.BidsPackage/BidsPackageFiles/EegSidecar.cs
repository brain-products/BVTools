namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    /// <summary>
    /// for definition, see: https://bids-specification.readthedocs.io/en/stable/04-modality-specific-files/03-electroencephalography.html
    /// </summary>
    public class EegSidecar
    {
        public EegSidecar(string taskName, string eegReference, double samplingFrequency, double powerLineFrequency, string softwareFilters)
        {
            TaskName = taskName;
            EEGReference = eegReference;
            SamplingFrequency = samplingFrequency;
            PowerLineFrequency = powerLineFrequency;
            SoftwareFilters = softwareFilters;
        }

        #region Enums
        public enum Recordingtype { continuous = 1, epoched = 2 }
        #endregion

        #region REQUIRED Generic fields
        /// <summary>
        ///  REQUIRED. Name of the task (for resting state use the rest prefix). Different Tasks SHOULD NOT have the same name. The Task label is derived from this field by removing all non alphanumeric ([a-zA-Z0-9]) characters.
        /// </summary>
        public string TaskName { get; set; }
        #endregion

        #region RECOMMENDED Generic fields
        /// <summary>
        /// RECOMMENDED.The name of the institution in charge of the equipment that produced the composite instances.
        /// </summary>
        public string? InstitutionName { get; set; }

        /// <summary>
        /// RECOMMENDED. The address of the institution in charge of the equipment that produced the composite instances.
        /// </summary>
        public string? InstitutionAddress { get; set; }

        /// <summary>
        /// RECOMMENDED. Manufacturer of the EEG system (e.g., Biosemi, Brain Products, Neuroscan).
        /// </summary>
        public string? Manufacturer { get; set; }

        /// <summary>
        /// RECOMMENDED. Manufacturer's designation of the EEG system model (e.g., BrainAmp DC).
        /// </summary>
        public string? ManufacturersModelName { get; set; }

        /// <summary>
        /// RECOMMENDED. Manufacturer's designation of the acquisition software.
        /// </summary>
        public string? SoftwareVersions { get; set; }

        /// <summary>
        /// RECOMMENDED. Description of the task.
        /// </summary>
        public string? TaskDescription { get; set; }

        /// <summary>
        /// RECOMMENDED. Text of the instructions given to participants before the scan. This is not only important for behavioral or cognitive tasks but also in resting state paradigms (e.g., to distinguish between eyes open and eyes closed).
        /// </summary>
        public string? Instructions { get; set; }

        /// <summary>
        /// RECOMMENDED. URL of the corresponding Cognitive Atlas term that describes the task (e.g., Resting State with eyes closed "http://www.cognitiveatlas.org/term/id/trm_54e69c642d89b").
        /// </summary>
        public string? CogAtlasID { get; set; }

        /// <summary>
        /// RECOMMENDED. URL of the corresponding CogPO term that describes the task (e.g., Rest "http://wiki.cogpo.org/index.php?title=Rest") .
        /// </summary>
        public string? CogPOID { get; set; }

        /// <summary>
        /// RECOMMENDED. The serial number of the equipment that produced the composite instances. A pseudonym can also be used to prevent the equipment from being identifiable, as long as each pseudonym is unique within the dataset.
        /// </summary>
        public string? DeviceSerialNumber { get; set; }
        #endregion

        #region REQUIRED EEG fields
        /// <summary>
        /// REQUIRED. General description of the reference scheme used and (when applicable) of location of the reference electrode in the raw recordings (e.g., "left mastoid", "Cz", "CMS"). If different channels have a different reference, this field should have a general description and the channel specific reference should be defined in the _channels.tsv file.
        /// </summary>
        public string EEGReference { get; set; }

        /// <summary>
        /// REQUIRED. Sampling frequency (in Hz) of all the data in the recording, regardless of their type (e.g., 2400).
        /// </summary>
        public double SamplingFrequency { get; set; }

        /// <summary>
        /// REQUIRED. Frequency (in Hz) of the power grid at the geographical location of the EEG instrument (i.e., 50 or 60)
        /// </summary>
        public double PowerLineFrequency { get; set; }

        /// <summary>
        /// REQUIRED. List of temporal software filters applied. Ideally key:value pairs of pre-applied software filters and their parameter values: e.g., {"Anti-aliasing filter": {"half-amplitude cutoff (Hz)": 500, "Roll-off": "6dB/Octave"}}. Write n/a if no software filters applied.
        /// </summary>
        public string SoftwareFilters { get; set; }
        #endregion

        #region RECOMMENDED EEG fields
        /// <summary>
        /// RECOMMENDED. Name of the cap manufacturer (e.g., "EasyCap").
        /// </summary>
        public string? CapManufacturer { get; set; }

        /// <summary>
        /// RECOMMENDED. Manufacturer's designation of the EEG cap model (e.g., "actiCAP 64 Ch Standard-2").
        /// </summary>
        public string? CapManufacturersModelName { get; set; }

        /// <summary>
        /// RECOMMENDED. Number of EEG channels included in the recording (e.g., 128).
        /// it should be always written, despite it is not REQUIRED value
        /// </summary>
        public int EEGChannelCount { get; set; }

        /// <summary>
        /// RECOMMENDED. Number of ECG channels.
        /// it should be always written, despite it is not REQUIRED value
        /// </summary>
        public int ECGChannelCount { get; set; }

        /// <summary>
        /// RECOMMENDED. Number of EMG channels.
        /// it should be always written, despite it is not REQUIRED value
        /// </summary>
        public int EMGChannelCount { get; set; }

        /// <summary>
        /// RECOMMENDED. Number of EOG channels.
        /// it should be always written, despite it is not REQUIRED value
        /// </summary>
        public int EOGChannelCount { get; set; }

        /// <summary>
        /// RECOMMENDED. Number of miscellaneous analog channels for auxiliary signals.
        /// it should be always written, despite it is not REQUIRED value
        /// </summary>
        public int MiscChannelCount { get; set; }

        /// <summary>
        /// RECOMMENDED. Number of channels for digital (TTL bit level) trigger.
        /// it should be always written, despite it is not REQUIRED value
        /// </summary>
        public int TriggerChannelCount { get; set; }

        /// <summary>
        /// RECOMMENDED. Length of the recording in seconds (e.g., 3600).
        /// </summary>
        public double? RecordingDuration { get; set; }

        /// <summary>
        /// RECOMMENDED. Defines whether the recording is continuous or epoched.
        /// </summary>
        public Recordingtype? RecordingType { get; set; }

        /// <summary>
        /// RECOMMENDED. Duration of individual epochs in seconds (e.g., 1) in case of epoched data.
        /// </summary>
        public double? EpochLength { get; set; } // [s] in case of epoched data.

        /// <summary>
        /// RECOMMENDED. Circumference of the participants head, expressed in cm (e.g., 58).
        /// </summary>
        public double? HeadCircumference { get; set; }   // [cm]

        /// <summary>
        /// RECOMMENDED. Placement scheme of EEG electrodes. Either the name of a standardized placement system (e.g., "10-20") or a list of standardized electrode names (e.g., ["Cz", "Pz"]).
        /// </summary>
        public string? EEGPlacementScheme { get; set; }  // "10-20"

        /// <summary>
        /// RECOMMENDED. Description of the location of the ground electrode (e.g., "placed on right mastoid (M2)").
        /// </summary>
        public string? EEGGround { get; set; }

        /// <summary>
        /// RECOMMENDED. List of temporal hardware filters applied. Ideally key:value pairs of pre-applied hardware filters and their parameter values: e.g., {"HardwareFilters": {"Highpass RC filter": {"Half amplitude cutoff (Hz)": 0.0159, "Roll-off": "6dB/Octave"}}}. Write n/a if no hardware filters applied.
        /// </summary>
        public string? HardwareFilters { get; set; }

        /// <summary>
        /// RECOMMENDED. Free-form description of the observed subject artifact and its possible cause (e.g., "Vagus Nerve Stimulator", "non-removable implant"). If this field is set to n/a, it will be interpreted as absence of major source of artifacts except cardiac and blinks.
        /// </summary>
        public string? SubjectArtefactDescription { get; set; }
        #endregion
    }
}
