namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat
{
    /// <summary>
    /// see https://bids-specification.readthedocs.io/en/stable/04-modality-specific-files/03-electroencephalography.html#electrodes-description-electrodes-tsv
    /// </summary>
    public class EegElectrode
    {
        public EegElectrode(string name, double x, double y, double z)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
        }

        #region REQUIRED
        /// <summary>
        /// REQUIRED. Name of the electrode
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// REQUIRED. Recorded position along the x-axis
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// REQUIRED. Recorded position along the y-axis
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// REQUIRED. Recorded position along the z-axis
        /// </summary>
        public double Z { get; set; }
        #endregion

        #region RECOMMENDED
        /// <summary>
        /// RECOMMENDED. Type of the electrode (e.g., cup, ring, clip-on, wire, needle)
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// RECOMMENDED. Material of the electrode, e.g., Tin, Ag/AgCl, Gold
        /// </summary>
        public string? Material { get; set; }

        /// <summary>
        /// RECOMMENDED. Impedance of the electrode in kOhm
        /// </summary>
        public double? Impedance { get; set; }
        #endregion
    }
}
