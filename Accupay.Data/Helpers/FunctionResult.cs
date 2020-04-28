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
    }
}