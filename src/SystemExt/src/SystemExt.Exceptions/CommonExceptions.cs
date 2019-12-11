using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace BrainVision.Lab.SystemExt.Internal
{
    /// <summary>
    /// Overview over the exceptions, which are not ArgumentExceptions:
    /// - NotImplementedException: This member is not implemented yet but will be very soon. Use your own throw.
    /// - NotSupportedException: This member will never be implemented by this type.
    /// - InvalidOperationException: This member is already implemented but current instance state disallows its use.
    /// </summary>
    internal static class CommonExceptions
    {
        /// <summary>
        /// This exception is thrown in scenarios in which the operation is always invalid during the whole object's lifetime. 
        /// A typical case is that an abstract type expects that the derived type implements the method because base class cannot
        /// deliver any standard implementation and throws an exception.
        /// Other case is when derived class cannot implement fully an interface (see Stream).
        /// In this case usually properties shall be defined helping to determine whether a functionality is available or not.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DoesNotReturn]
        public static void ThrowNotSupported(string message, [CallerMemberName] string? methodName = null) =>
            throw new NotSupportedException(ExceptionMessages.GetNotSupportedMessage(message, methodName!));

        /// <summary>
        /// This exception is thrown in scenarios in which it is generally sometimes possible for the object to perform
        /// the requested operation, and the object state determines whether the operation can be performed.
        /// Also if an argument needs an complex check or, a calculation runs in a dead end, this exception shall be thrown.
        /// Message: Operation is not valid due to the current state of the object.
        /// This exception generally speaking says: you are doing something wrong
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DoesNotReturn]
        public static void ThrowInvalidOperation(string message, [CallerMemberName] string? methodName = null) =>
            throw new InvalidOperationException(ExceptionMessages.GetInvalidOperationMessage(message, methodName!));
    }
}
