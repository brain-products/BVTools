using System;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    /// <summary>
    /// Attribute created to handle µ as special character in unit prefix.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class CustomEnumTextAttribute : Attribute
    {
        public string CustomText { get; } = string.Empty;
        public CustomEnumTextAttribute(string customString) => CustomText = customString;
    }
}
