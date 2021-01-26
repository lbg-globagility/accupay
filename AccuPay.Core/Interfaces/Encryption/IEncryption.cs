namespace AccuPay.Core.Interfaces
{
    public interface IEncryption
    {
        string Encrypt(string input);

        string Decrypt(string input);
    }
}