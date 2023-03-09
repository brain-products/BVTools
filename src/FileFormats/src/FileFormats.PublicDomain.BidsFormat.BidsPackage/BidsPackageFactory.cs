using BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat;

public static class BidsPackageFactory
{
    /// <summary>
    /// A class to manage (load, save) files being a part of a single examination BIDS package.
    /// </summary>
    /// <param name="rootFolder">path to the folder where the subject folder persists</param>
    /// <param name="subject">determines subject folder and is part of file names</param>
    /// <param name="session">determines session folder and is part of file names. Optional, may be null. In this case session folder will not be considered and file names will not refer to session</param>
    /// <param name="task">is part of file names</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="rootFolder"/>, <paramref name="subject"/>, <paramref name="session"/> or <paramref name="task"/> consists only of white characters or when <paramref name="subject"/>, <paramref name="session"/> or <paramref name="task"/> contains one of BIDS special characters: '-', '_'.</exception>
    public static IBidsPackage Create(string rootFolder, string subject, string? session, string task)
        => new BidsPackage(rootFolder, subject, session, task);
}
