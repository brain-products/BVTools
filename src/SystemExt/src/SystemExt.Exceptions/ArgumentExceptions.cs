using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BrainVision.Lab.SystemExt.Internal
{
    // Using strings instead of Resources, causes the CA1303 warning. But shared project does not support Resources.
#pragma warning disable CA1303
    /// <summary>
    /// To test for NaN use for instance ThrowIfNotAFiniteNumber(). Comparison methods like ThrowIfGreater() do not test for NaN.
    /// </summary>
    internal static class ArgumentExceptions
    {
        #region ArgumentException
        /// <summary>
        /// General Argument Exception method. Use this, if the specific methods are not suitable.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DoesNotReturn]
        public static void Throw(string paramName, string message) =>
            throw new ArgumentException($"{ExceptionMessages.GeneralArgumentExceptionMessage} {message}", paramName);

        /// <summary>
        /// General Argument Exception method. Use this, if the specific methods are not suitable.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfTrue<T>(string paramName, T actualValue, Predicate<T> condition, string message)
        {
            if (condition(actualValue))
                throw new ArgumentException($"{ExceptionMessages.ArgumentConditionMessage} {message}", paramName);
        }

        /// <summary>
        /// General Argument Exception method. Use this, if the specific methods are not suitable.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfFalse<T>(string paramName, T actualValue, Predicate<T> condition, string message)
        {
            if (!condition(actualValue))
                throw new ArgumentException($"{ExceptionMessages.ArgumentConditionMessage} {message}", paramName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfUndefinedInEnum<T>(string paramName, object actualValue) where T : Enum
        {
            if (!Enum.IsDefined(typeof(T), actualValue))
                throw new ArgumentException(ExceptionMessages.ArgumentNotInEnumMessage, paramName);
        }

        #region Length Of List/Enumerable/Span/String
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfEmpty(string paramName, string actualValue)
        {
            if (actualValue.Length == 0)
                throw new ArgumentException(ExceptionMessages.ArgumentStringEmptyMessage, paramName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfEmpty<T>(string paramName, IEnumerable<T> actualValue)
        {
            if (!actualValue.Any())
                throw new ArgumentException(ExceptionMessages.ArgumentEmptyMessage, paramName);
        }

        /// <summary>
        /// Example: 
        /// int[] param = Array.Empty<int>();
        /// ArgumentExceptions.ThrowIfEmptyAsSpan<int>(nameof(param), param);
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfEmptyAsSpan<T>(string paramName, ReadOnlySpan<T> actualValue)
        {
            if (actualValue.IsEmpty)
                throw new ArgumentException(ExceptionMessages.ArgumentEmptyMessage, paramName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfUnequalLengths<T>(string paramName, IReadOnlyCollection<T> actualValue, int length)
        {
            if (actualValue.Count != length)
                throw new ArgumentException(ExceptionMessages.GetWrongLengthMessage(actualValue.Count, length), paramName);
        }

        /// <summary>
        /// Example: 
        /// int[] argument = Array.Empty<int>();
        /// ArgumentExceptions.ThrowIfUnequalLengthAsSpan<int>(nameof(argument), argument, length);
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfUnequalLengthsAsSpan<T>(string paramName, ReadOnlySpan<T> actualValue, int length)
        {
            if (actualValue.Length != length)
                throw new ArgumentException(ExceptionMessages.GetWrongLengthMessage(actualValue.Length, length), paramName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfUnequalLengths<T>(string paramName, string referenceName, IReadOnlyCollection<T> actualValue, IReadOnlyCollection<T> reference)
        {
            if (actualValue.Count != reference.Count)
                throw new ArgumentException(ExceptionMessages.GetDifferentLengthMessage(referenceName, actualValue.Count, reference.Count), paramName);
        }

        /// <summary>
        /// Example: 
        /// int[] param = new int[] { 1 };
        /// int[] ref = new int[] { 1, 2 };
        /// ArgumentExceptions.ThrowIfUnequalLengthAsSpan<int>(nameof(param), nameof(ref), param, ref);
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfUnequalLengthsAsSpan<T>(string paramName, string referenceName, ReadOnlySpan<T> actualValue, ReadOnlySpan<T> reference)
        {
            if (actualValue.Length != reference.Length)
                throw new ArgumentException(ExceptionMessages.GetDifferentLengthMessage(referenceName, actualValue.Length, reference.Length), paramName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfUnequalLengths<T>(string paramName, IReadOnlyCollection<IReadOnlyCollection<T>> actualValue)
        {
            ThrowIfEmpty(paramName, actualValue);
            if (actualValue.Any(col => col.Count != actualValue.ElementAt(0).Count))
                throw new ArgumentException(ExceptionMessages.ArgumentNotRectangularMessage, paramName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfUnequalLengthsOfArrayArray<T>(string paramName, IReadOnlyCollection<IReadOnlyCollection<T>> actualValue, int length)
        {
            if (actualValue.Any(col => col.Count != length))
                throw new ArgumentException(ExceptionMessages.ArgumentNotRectangularMessage, paramName);
        }
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotAFiniteNumber(string paramName, double actualValue)
        {
            if (double.IsNaN(actualValue))
                throw new ArgumentException(ExceptionMessages.ArgumentIsNan, paramName);
            if (double.IsInfinity(actualValue))
                throw new ArgumentException(ExceptionMessages.ArgumentIsInfinity, paramName);
        }
        #endregion

        #region ArgumentOutOfRangeException
        #region Range
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInOpenRange(string paramName, double actualValue, double min, double max)
        {
            bool valid = actualValue > min && actualValue < max;
            if (!valid)
            {
                string message = ExceptionMessages.GetOutsideOpenRangeMessage(paramName, actualValue, min, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInOpenRange(string paramName, int actualValue, int min, int max)
        {
            bool valid = actualValue > min && actualValue < max;
            if (!valid)
            {
                string message = ExceptionMessages.GetOutsideOpenRangeMessage(paramName, actualValue, min, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInClosedRange(string paramName, double actualValue, double min, double max)
        {
            bool valid = actualValue >= min && actualValue <= max;
            if (!valid)
            {
                string message = ExceptionMessages.GetOutsideClosedRangeMessage(paramName, actualValue, min, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInClosedRange(string paramName, int actualValue, int min, int max)
        {
            bool valid = actualValue >= min && actualValue <= max;
            if (!valid)
            {
                string message = ExceptionMessages.GetOutsideClosedRangeMessage(paramName, actualValue, min, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInOpenClosedRange(string paramName, double actualValue, double min, double max)
        {
            bool valid = actualValue > min && actualValue <= max;
            if (!valid)
            {
                string message = ExceptionMessages.GetOutsideOpenClosedRangeMessage(paramName, actualValue, min, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInOpenClosedRange(string paramName, int actualValue, int min, int max)
        {
            bool valid = actualValue > min && actualValue <= max;
            if (!valid)
            {
                string message = ExceptionMessages.GetOutsideOpenClosedRangeMessage(paramName, actualValue, min, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInClosedOpenRange(string paramName, double actualValue, double min, double max)
        {
            bool valid = actualValue >= min && actualValue < max;
            if (!valid)
            {
                string message = ExceptionMessages.GetOutsideClosedOpenRangeMessage(paramName, actualValue, min, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotInClosedOpenRange(string paramName, int actualValue, int min, int max)
        {
            bool valid = actualValue >= min && actualValue < max;
            if (!valid)
            {
                string message = ExceptionMessages.GetOutsideClosedOpenRangeMessage(paramName, actualValue, min, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }
        #endregion

        #region Positive/Negative
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNegative(string paramName, double actualValue)
        {
            if (actualValue < 0.0)
            {
                string message = ExceptionMessages.GetIsNegativeMessage(paramName, actualValue);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNegative(string paramName, int actualValue)
        {
            if (actualValue < 0)
            {
                string message = ExceptionMessages.GetIsNegativeMessage(paramName, actualValue);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNegativeOrZero(string paramName, double actualValue)
        {
            if (actualValue <= 0.0)
            {
                string message = ExceptionMessages.GetIsNegativeOrZeroMessage(paramName, actualValue);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNegativeOrZero(string paramName, int actualValue)
        {
            if (actualValue <= 0)
            {
                string message = ExceptionMessages.GetIsNegativeOrZeroMessage(paramName, actualValue);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }
        #endregion

        #region Greater/Less
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreater(string paramName, double actualValue, double max)
        {
            if (actualValue > max)
            {
                string message = ExceptionMessages.GetIsGreaterMessage(paramName, actualValue, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreater(string paramName, int actualValue, int max)
        {
            if (actualValue > max)
            {
                string message = ExceptionMessages.GetIsGreaterMessage(paramName, actualValue, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreaterOrEqual(string paramName, double actualValue, double max)
        {
            if (actualValue >= max)
            {
                string message = ExceptionMessages.GetIsGreaterEqualMessage(paramName, actualValue, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfGreaterOrEqual(string paramName, int actualValue, int max)
        {
            if (actualValue >= max)
            {
                string message = ExceptionMessages.GetIsGreaterEqualMessage(paramName, actualValue, max);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLess(string paramName, double actualValue, double min)
        {
            if (actualValue < min)
            {
                string message = ExceptionMessages.GetIsLessMessage(paramName, actualValue, min);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLess(string paramName, int actualValue, int min)
        {
            if (actualValue < min)
            {
                string message = ExceptionMessages.GetIsLessMessage(paramName, actualValue, min);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessOrEqual(string paramName, double actualValue, double min)
        {
            if (actualValue <= min)
            {
                string message = ExceptionMessages.GetIsLessEqualMessage(paramName, actualValue, min);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfLessOrEqual(string paramName, int actualValue, int min)
        {
            if (actualValue <= min)
            {
                string message = ExceptionMessages.GetIsLessEqualMessage(paramName, actualValue, min);
                throw new ArgumentOutOfRangeException(paramName, actualValue, message);
            }
        }
        #endregion
        #endregion
#pragma warning restore CA1303 // Do not pass literals as localized parameters
    }
}
