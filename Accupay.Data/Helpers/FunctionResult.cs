using System.Collections.Generic;

namespace AccuPay.Data.Helpers
{
    public class FunctionResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        private FunctionResult(bool isSuccess, string message)
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
            return new FunctionResult(true, message);
        }

        public static implicit operator bool(FunctionResult result)
        {
            return result == null ? false : result.IsSuccess;
        }

        public static bool operator ==(FunctionResult result, FunctionResult result2)
        {
            if (result == null && result2 == null)
                return true;

            if (result == null || result2 == null)
                return false;

            return (result.IsSuccess && result2.IsSuccess) ||
                    (!result.IsSuccess && !result2.IsSuccess);
        }

        public static bool operator !=(FunctionResult result, FunctionResult result2)
        {
            return !(result == result2);
        }

        public override bool Equals(object obj)
        {
            return obj is FunctionResult result &&
                   IsSuccess == result.IsSuccess &&
                   Message == result.Message;
        }

        public override int GetHashCode()
        {
            int hashCode = -1576371132;
            hashCode = hashCode * -1521134295 + IsSuccess.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            return hashCode;
        }
    }
}