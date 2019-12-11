internal enum YesNo { YES = 1, NO = 2 }
public enum Codepage { Utf8 = 1 }
public enum DataFormat { BINARY = 1 }
#pragma warning disable CA1707 // Identifiers should not contain underscores
public enum BinaryFormat { IEEE_FLOAT_32 = 1, INT_16 = 2 }
#pragma warning restore CA1707 // Identifiers should not contain underscores
public enum DataOrientation { MULTIPLEXED = 1, VECTORIZED = 2 }
public enum DataType { TIMEDOMAIN = 1 }
public enum SegmentationType { NOTSEGMENTED = 1, MARKERBASED = 2 }
