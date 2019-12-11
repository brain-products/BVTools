using static System.FormattableString;

namespace BrainVision.Lab.SystemExt.Internal
{
    internal static class ExceptionMessages
    {
        public const string GeneralArgumentExceptionMessage = "Specified argument does not fall within the expected range.";
        public const string ArgumentConditionMessage = "Specified argument does not fulfill a required condition.";
        public const string ArgumentDifferentLengthMessage = "Specified argument has a different length than the reference.";
        public const string ArgumentEmptyMessage = "Specified argument is empty.";
        public const string ArgumentIsInfinity = "Specified argument is Infinity.";
        public const string ArgumentIsNan = "Specified argument is NaN.";
        public const string ArgumentNotInEnumMessage = "Specified enum does not contain the value.";
        public const string ArgumentNotRectangularMessage = "Specified argument has sub lists/arrays of different lengths.";
        public const string ArgumentOutOfRangeMessage = "Specified argument was out of the range of valid values.";
        public const string ArgumentStringEmptyMessage = "Specified string is empty.";
        public const string ArgumentWrongLengthMessage = "Specified argument has the wrong length.";

        #region NotSupported/InvalidOperation Messages
        public static string GetNotSupportedMessage(string message, string methodName) =>
            Invariant($"{methodName} is not supported. {message}");

        public static string GetInvalidOperationMessage(string message, string methodName) =>
            Invariant($"{methodName} is not valid due to the current state of the object. {message}");
        #endregion

        #region Length Messages
        public static string GetWrongLengthMessage(int actualLength, int expectedLength) =>
            Invariant($"{ArgumentWrongLengthMessage} Expected length is {expectedLength}, actual length is {actualLength}.");

        public static string GetDifferentLengthMessage(string referenceName, int actualLength, int referenceLength) =>
            Invariant($"{ArgumentDifferentLengthMessage} Reference {referenceName} has length {referenceLength}, actual length is {actualLength}.");
        #endregion

        #region Range Messages
        // start/end bracket shall be open or closed: ([)]
        private static string GetOutsideRangeMessage(string name, object value, object min, object max, char startBracket, char endBracket) =>
            Invariant($"{ArgumentOutOfRangeMessage} Value is {value}, but allowed range is {startBracket}{min}, {max}{endBracket}. {GetParamString(name)}");

        public static string GetOutsideOpenRangeMessage(string name, object value, object min, object max) =>
            GetOutsideRangeMessage(name, value, min, max, '(', ')');

        public static string GetOutsideClosedRangeMessage(string name, object value, object min, object max) =>
            GetOutsideRangeMessage(name, value, min, max, '[', ']');

        public static string GetOutsideOpenClosedRangeMessage(string name, object value, object min, object max) =>
            GetOutsideRangeMessage(name, value, min, max, '(', ']');

        public static string GetOutsideClosedOpenRangeMessage(string name, object value, object min, object max) =>
            GetOutsideRangeMessage(name, value, min, max, '[', ')');
        #endregion

        #region Greater/Less Messages
        public static string GetIsNegativeMessage(string name, object value) =>
            Invariant($"{ArgumentOutOfRangeMessage} Value is {value}, but shall be positive. {GetParamString(name)}");

        public static string GetIsNegativeOrZeroMessage(string name, object value) =>
            Invariant($"{ArgumentOutOfRangeMessage} Value is {value}, but shall be positive or zero. {GetParamString(name)}");

        public static string GetIsGreaterMessage(string name, object value, object max) =>
            Invariant($"{ArgumentOutOfRangeMessage} Value {value} is greater than {max}. {GetParamString(name)}");

        public static string GetIsGreaterEqualMessage(string name, object value, object max) =>
            Invariant($"{ArgumentOutOfRangeMessage} Value {value} is greater than or equal {max}. {GetParamString(name)}");

        public static string GetIsLessMessage(string name, object value, object min) =>
            Invariant($"{ArgumentOutOfRangeMessage} Value {value} is less than {min}. {GetParamString(name)}");

        public static string GetIsLessEqualMessage(string name, object value, object min) =>
            Invariant($"{ArgumentOutOfRangeMessage} Value {value} is less than or equal {min}. {GetParamString(name)}");
        #endregion

        private static string GetParamString(string name) => Invariant($"(Parameter '{name}')");
    }
}
