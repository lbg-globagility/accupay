using System;
using System.Collections.Generic;

namespace AccuPay.Core.Helpers
{
    public class FunctionResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        protected FunctionResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static FunctionResult Success(string message = null)
        {
            return new FunctionResult(true, message);
        }

        public static FunctionResult Failed(string message = null)
        {
            return new FunctionResult(false, message);
        }

        public static implicit operator bool(FunctionResult result)
        {
            return result == null ? false : result.IsSuccess;
        }

        public static bool operator ==(FunctionResult result, FunctionResult result2)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(result, null))
            {
                if (Object.ReferenceEquals(result2, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return result.Equals(result2);
        }

        public static bool operator !=(FunctionResult result, FunctionResult result2)
        {
            return !(result == result2);
        }

        public override bool Equals(object obj)
        {
            return obj is FunctionResult result &&
                    IsSuccess == result.IsSuccess;
        }

        public override int GetHashCode()
        {
            int hashCode = -1576371132;
            hashCode = hashCode * -1521134295 + IsSuccess.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            return hashCode;
        }
    }

    // TODO: add unit tests on operators of this class
    public class FunctionResult<T> : FunctionResult
    {
        public T Result { get; }

        private FunctionResult(bool isSuccess, T result, string message) : base(isSuccess, message)
        {
            Result = result;
        }

        public static FunctionResult<T> Success(T result)
        {
            return new FunctionResult<T>(true, result, string.Empty);
        }

        public static new FunctionResult<T> Failed(string message = null)
        {
            return new FunctionResult<T>(false, default(T), message);
        }
    }
}