using System.Collections.Generic;
using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

namespace BrainVision.Lab.FileFormats
{
    public class CustomizationInfo
    {
        /// <summary>
        /// REQUIRED. Name of the dataset.
        /// Dataset - a set of neuroimaging and behavioral data acquired for a purpose of a particular study. A dataset consists of data acquired from one or more subjects, possibly from multiple sessions
        /// Used as <see cref="DatasetDescription.Name"/>
        /// </summary>
        public string DatasetName { get; set; } = "";

        /// <summary>
        /// A person or animal participating in the study.
        /// Used to construct folder and file names
        /// if null, consecutive numbers will be taken, i.e.: 01
        /// </summary>
        public string? SubjectName { get; set; } = null;

        /// <summary>
        /// Session - A logical grouping of neuroimaging and behavioral data consistent across subjects. Session can (but doesn't have to) be synonymous to a visit in a longitudinal study. In general, subjects will stay in the scanner during one session. However, for example, if a subject has to leave the scanner room and then be re-positioned on the scanner bed, the set of MRI acquisitions will still be considered as a session and match sessions acquired in other subjects. Similarly, in situations where different data types are obtained over several visits (for example fMRI on one day followed by DWI the day after) those can be grouped in one session. Defining multiple sessions is appropriate when several identical or similar data acquisitions are planned and performed on all -or most- subjects, often in the case of some intervention between sessions (e.g., training).
        /// Used to construct folder and file names
        /// </summary>
        public string? SessionName { get; set; } = null;

        /// <summary>
        /// Task - A set of structured activities performed by the participant. Tasks are usually accompanied by stimuli and responses, and can greatly vary in complexity. For the purpose of this protocol we consider the so-called “resting state” a task. In the context of brain scanning, a task is always tied to one data acquisition. Therefore, even if during one acquisition the subject performed multiple conceptually different behaviors (with different sets of instructions) they will be considered one (combined) task.
        /// Used to construct folder and file names.
        /// if null, file name will be taken
        /// </summary>
        public string? TaskName { get; set; } = null;

        /// <summary>
        /// RECOMMENDED. What license is this dataset distributed under? The use of license name abbreviations is suggested for specifying a license. A list of common licenses with suggested abbreviations can be found in Appendix II.
        /// It may be one of predefined licenses: <see cref="Defs.LicenseType"/>, or any other value
        /// Used as <see cref="DatasetDescription.License"/>
        /// </summary>
        public string? License { get; set; }

        // CA2227 suppressed because Authors must follow the same logic as DatasetDescription.Authors
        // This meaning of property differs depending if it is null or Empty.
        // null:  key in the json file will not be created at all.
        // empty: key in the json file will be created but its value will be empty.
#pragma warning disable CA2227 // Collection properties should be read only
        /// <summary>
        /// OPTIONAL. List of individuals who contributed to the creation/curation of the dataset.
        /// Used as <see cref="DatasetDescription.Authors"/>
        /// </summary>
        public List<string>? Authors { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        /// <summary>
        /// RECOMMENDED. Manufacturer of the EEG system (e.g., Biosemi, Brain Products, Neuroscan).
        /// Used as <see cref="EegSidecar.Manufacturer"/>
        /// </summary>
        public string? Manufacturer { get; set; } = "Brain Products";

        /// <summary>
        /// REQUIRED. General description of the reference scheme used and (when applicable) of location of the reference electrode in the raw recordings (e.g., "left mastoid", "Cz", "CMS"). If different channels have a different reference, this field should have a general description and the channel specific reference should be defined in the _channels.tsv file.
        /// Used as <see cref="EegSidecar.EEGReference"/>
        /// </summary>
        public string EEGReference { get; set; } = "Unknown";

        /// <summary>
        /// REQUIRED. Frequency (in Hz) of the power grid at the geographical location of the EEG instrument (i.e., 50 or 60)
        /// Used as <see cref="EegSidecar.PowerLineFrequency"/>
        /// </summary>
        public double PowerLineFrequency { get; set; } = 50.0;
    }
}
