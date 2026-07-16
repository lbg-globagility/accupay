using System;
using System.Security.Cryptography;
using System.Text;

namespace AccuPay.Web.TimeLogs
{
    internal static class ApprovalTokenHelper
    {
        // token = "{expiryUnixSeconds}.{signatureBase64Url}"
        public static string GenerateToken(int filingId, string secret, TimeSpan ttl)
        {
            var expiry = DateTimeOffset.UtcNow.Add(ttl).ToUnixTimeSeconds();
            var payload = $"{filingId}:{expiry}";
            var signature = ComputeHmac(payload, secret);
            return $"{expiry}.{signature}";
        }

        public static bool ValidateToken(string token, int filingId, string secret, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(token))
            {
                error = "Token is missing.";
                return false;
            }

            var parts = token.Split('.');
            if (parts.Length != 2)
            {
                error = "Invalid token format.";
                return false;
            }

            if (!long.TryParse(parts[0], out var expiry))
            {
                error = "Invalid expiry in token.";
                return false;
            }

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (now > expiry)
            {
                error = "Token has expired.";
                return false;
            }

            var expectedPayload = $"{filingId}:{expiry}";
            var expectedSig = ComputeHmac(expectedPayload, secret);

            // constant time compare
            if (!AreEqualBase64Url(parts[1], expectedSig))
            {
                error = "Invalid token signature.";
                return false;
            }

            return true;
        }

        private static string ComputeHmac(string payload, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret ?? string.Empty);
            var data = Encoding.UTF8.GetBytes(payload);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hash = hmac.ComputeHash(data);
                return Base64UrlEncode(hash);
            }
        }

        private static string Base64UrlEncode(byte[] input)
        {
            var s = Convert.ToBase64String(input);
            s = s.TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return s;
        }

        private static bool AreEqualBase64Url(string a, string b)
        {
            if (a == null || b == null) return false;
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);
            if (aBytes.Length != bBytes.Length) return false;
            var diff = 0;
            for (int i = 0; i < aBytes.Length; i++) diff |= aBytes[i] ^ bBytes[i];
            return diff == 0;
        }
    }
}
