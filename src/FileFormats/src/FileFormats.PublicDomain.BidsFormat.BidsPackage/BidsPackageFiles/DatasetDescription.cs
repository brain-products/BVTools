namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

/// <summary>
/// for definition, see: https://bids-specification.readthedocs.io/en/stable/03-modality-agnostic-files.html
/// </summary>
public class DatasetDescription
{
    public DatasetDescription(string name)
    {
        Name = name;
        BIDSVersion = new Version(1, 2, 1);
    }

    #region REQUIRED
    /// <summary>
    /// REQUIRED. Name of the dataset.
    /// </summary>
    public string Name { get; set; } // Name of the dataset.

    /// <summary>
    /// REQUIRED. The version of the BIDS standard that was used.
    /// Suggestion: use 1.2.0, this library has been designed based on this bids version.
    /// </summary>
    public Version BIDSVersion { get; set; }
    #endregion

    #region RECOMMENDED
    /// <summary>
    /// RECOMMENDED. What license is this dataset distributed under? The use of license name abbreviations is suggested for specifying a license. A list of common licenses with suggested abbreviations can be found in Appendix II.
    /// It may be one of predefined licenses: <see cref="Defs.LicenseType"/>, or any other value
    /// </summary>
    public string? License { get; set; }
    #endregion

    #region OPTIONAL
    /// <summary>
    /// OPTIONAL. List of individuals who contributed to the creation/curation of the dataset.
    /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only. Suppression Reason: setter must be available for JSON serializer,
    public IList<string>? Authors { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    /// <summary>
    /// OPTIONAL. Text acknowledging contributions of individuals or institutions beyond those listed in Authors or Funding.
    /// </summary>
    public string? Acknowledgements { get; set; }

    /// <summary>
    /// OPTIONAL. Instructions how researchers using this dataset should acknowledge the original authors. This field can also be used to define a publication that should be cited in publications that use the dataset.
    /// </summary>
    public string? HowToAcknowledge { get; set; }

    /// <summary>
    /// OPTIONAL. List of sources of funding (grant numbers)
    /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only. Suppression Reason: setter must be available for JSON serializer,
    public IList<string>? Funding { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    /// <summary>
    /// OPTIONAL.List of references to publication that contain information on the dataset, or links.
    /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only. Suppression Reason: setter must be available for JSON serializer,
    public IList<string>? ReferencesAndLinks { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    /// <summary>
    /// OPTIONAL. The Document Object Identifier of the dataset (not the corresponding paper).
    /// </summary>
    public string? DatasetDOI { get; set; }
    #endregion
}
