using AccuPay.Core.Interfaces;
using System;

namespace AccuPay.Infrastructure.Services.Encryption
{
    public class AccuPayDesktopEncryption : IEncryption
    {
        private const int NumberConvert = 133;

        public string Decrypt(string input)
        {
            string encrypted = null;
            if (input != null)
            {
                foreach (char x in input)
                {
                    long converted = Convert.ToInt64(x) - NumberConvert;
                    encrypted += Convert.ToChar(Convert.ToInt64(converted));
                }
            }

            return encrypted;
        }

        public string Encrypt(string input)
        {
            string encrypted = null;
            if (input != null)
            {
                foreach (char x in input)
                {
                    long converted = Convert.ToInt64(x) + 133;
                    encrypted += Convert.ToChar(Convert.ToInt64(converted));
                }
            }

            return encrypted;
        }
    }
}