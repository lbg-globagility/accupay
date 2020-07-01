namespace AccuPay.Data.Interfaces
{
    public interface IEncryption
    {
        string Encrypt(string input);

        string Decrypt(string input);
    }
}