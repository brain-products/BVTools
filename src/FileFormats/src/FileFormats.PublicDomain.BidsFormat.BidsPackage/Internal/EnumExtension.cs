using System;
using System.Reflection;

namespace BrainVision.Lab.FileFormats.PublicDomain.BidsFormat.Internal
{
    internal static class EnumExtension
    {
        public static string ToCustomString<T>(this T enumValue) where T : Enum
        {
            //Locate the definition of enum's value
            MemberInfo[] memberInfo = typeof(T).GetMember(enumValue.ToString());
            if (memberInfo?.Length > 0)
            {
                //Find the attribute if any
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(CustomEnumTextAttribute), false);

                if (attrs?.Length > 0)
                {
                    //Get the attribute value
                    return ((CustomEnumTextAttribute)attrs[0]).CustomText;
                }
            }

            //If no attribute found, return the standard ToString of the enum
            return enumValue.ToString();
        }
    }
}
