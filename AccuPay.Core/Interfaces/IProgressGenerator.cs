namespace AccuPay.Core.Interfaces
{
    public interface IProgressGenerator
    {
        int Progress { get; }
        string CurrentMessage { get; }
    }
}