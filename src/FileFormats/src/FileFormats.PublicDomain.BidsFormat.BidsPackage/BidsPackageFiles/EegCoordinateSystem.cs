namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

/// <summary>
/// see https://bids-specification.readthedocs.io/en/stable/04-modality-specific-files/03-electroencephalography.html
/// </summary>
public class EegCoordinateSystem
{
    public EegCoordinateSystem(CoordinateSystem eegCoordinateSystem, PrefixedUnit eegCoordinateUnits)
    {
        EEGCoordinateSystem = eegCoordinateSystem;
        EEGCoordinateUnits = eegCoordinateUnits;
    }

    #region OPTIONAL General fields
    /// <summary>
    /// OPTIONAL. Relative path to associate the electrodes, landmarks and fiducials to an MRI/CT.
    /// </summary>
    public string? IntendedFor { get; set; }     //Relative path to associate the electrodes, landmarks and fiducials to an MRI/CT.
    #endregion

    #region REQUIRED Fields relating to the EEG electrode positions
    /// <summary>
    /// REQUIRED. Refers to the coordinate system in which the EEG electrode positions are to be interpreted (see Appendix VIII).
    /// </summary>
    public CoordinateSystem EEGCoordinateSystem { get; set; }

    /// <summary>
    /// REQUIRED. Units in which the coordinates that are listed in the field EEGCoordinateSystem are represented (e.g., "mm", "cm").
    /// </summary>
    public PrefixedUnit EEGCoordinateUnits { get; set; }
    #endregion

    #region RECOMMENDED Fields relating to the EEG electrode positions
    /// <summary>
    /// RECOMMENDED. Free-form text description of the coordinate system. May also include a link to a documentation page or paper describing the system in greater detail.
    /// </summary>
    public string? EEGCoordinateSystemDescription { get; set; }
    #endregion

    #region OPTIONAL Fields relating to the position of fiducials measured during an EEG session/run
    /// <summary>
    /// OPTIONAL. Free-form text description of how the fiducials such as vitamin-E capsules were placed relative to anatomical landmarks, and how the position of the fiducials were measured (e.g., both with Polhemus and with T1w MRI).
    /// </summary>
    public string? FiducialsDescription { get; set; }
    #endregion

    #region RECOMMENDED Fields relating to the position of fiducials measured during an EEG session/run
    /// <summary>
    /// RECOMMENDED. Key:value pairs of the labels and 3-D digitized position of anatomical landmarks, interpreted following the FiducialsCoordinateSystem (e.g., {"NAS": [12.7,21.3,13.9], "LPA": [5.2,11.3,9.6], "RPA": [20.2,11.3,9.1]}).
    /// </summary>
    public string? FiducialsCoordinates { get; set; }

    /// <summary>
    /// RECOMMENDED. Refers to the coordinate space to which the landmarks positions are to be interpreted - preferably the same as the EEGCoordinateSystem.
    /// </summary>
    public string? FiducialsCoordinateSystem { get; set; }

    /// <summary>
    /// RECOMMENDED. Units in which the coordinates that are listed in the field AnatomicalLandmarkCoordinateSystem are represented (e.g., "mm", "cm").
    /// </summary>
    public PrefixedUnit? FiducialsCoordinateUnits { get; set; }

    /// <summary>
    /// RECOMMENDED. Free-form text description of the coordinate system. May also include a link to a documentation page or paper describing the system in greater detail.
    /// </summary>
    public string? FiducialsCoordinateSystemDescription { get; set; }
    #endregion

    #region RECOMMENDED Fields relating to the position of anatomical landmark measured during an EEG session/run
    /// <summary>
    /// RECOMMENDED. Key:value pairs of the labels and 3-D digitized position of anatomical landmarks, interpreted following the AnatomicalLandmarkCoordinateSystem (e.g., {"NAS": [12.7,21.3,13.9], "LPA": [5.2,11.3,9.6], "RPA": [20.2,11.3,9.1]}).
    /// </summary>
    public string? AnatomicalLandmarkCoordinates { get; set; }

    /// <summary>
    /// RECOMMENDED. Refers to the coordinate space to which the landmarks positions are to be interpreted - preferably the same as the EEGCoordinateSystem.
    /// </summary>
    public string? AnatomicalLandmarkCoordinateSystem { get; set; }

    /// <summary>
    /// RECOMMENDED. Units in which the coordinates that are listed in the field AnatomicalLandmarkCoordinateSystem are represented (e.g., "mm", "cm").
    /// </summary>
    public PrefixedUnit? AnatomicalLandmarkCoordinateUnits { get; set; }

    /// <summary>
    /// RECOMMENDED. Free-form text description of the coordinate system. May also include a link to a documentation page or paper describing the system in greater detail.
    /// </summary>
    public string? AnatomicalLandmarkCoordinateSystemDescription { get; set; }
    #endregion
}
