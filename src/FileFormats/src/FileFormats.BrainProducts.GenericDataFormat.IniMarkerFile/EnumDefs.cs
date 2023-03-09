using System.Diagnostics.CodeAnalysis;

namespace BrainVision.Lab.FileFormats.BrainProducts.GenericDataFormat.MarkerFileEnums;

[SuppressMessage("Design", "CA1008:Enums should have zero value", Justification = "zero value is not valid value that user may use in ini file")]
public enum Codepage { Utf8 = 1 }
