using System;
using System.Security.Cryptography;
using System.Text;

namespace AccuPay.Web.TimeLogs
{
    internal static class ApprovalTokenHelper
    {
        // token without an approver email = "{expiryUnixSeconds}.{signatureBase64Url}"
        // token with an approver email    = "{expiryUnixSeconds}.{emailBase64Url}.{signatureBase64Url}"
        public static string GenerateToken(int filingId, string secret, TimeSpan ttl, string approverEmail = null)
        {
            var expiry = DateTimeOffset.UtcNow.Add(ttl).ToUnixTimeSeconds();

            if (string.IsNullOrEmpty(approverEmail))
            {
                var payload = $"{filingId}:{expiry}";
                var signature = ComputeHmac(payload, secret);
                return $"{expiry}.{signature}";
            }

            var emailPayload = $"{filingId}:{expiry}:{approverEmail}";
            var emailSignature = ComputeHmac(emailPayload, secret);
            var encodedEmail = Base64UrlEncode(Encoding.UTF8.GetBytes(approverEmail));
            return $"{expiry}.{encodedEmail}.{emailSignature}";
        }

        public static bool ValidateToken(string token, int filingId, string secret, out string error)
        {
            return ValidateToken(token, filingId, secret, out error, out _);
        }

        public static bool ValidateToken(string token, int filingId, string secret, out string error, out string approverEmail)
        {
            error = null;
            approverEmail = null;

            if (string.IsNullOrWhiteSpace(token))
            {
                error = "Token is missing.";
                return false;
            }

            var parts = token.Split('.');
            if (parts.Length != 2 && parts.Length != 3)
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

            string expectedPayload;
            string signaturePart;
            string email = null;

            if (parts.Length == 3)
            {
                try
                {
                    email = Encoding.UTF8.GetString(Base64UrlDecode(parts[1]));
                }
                catch
                {
                    error = "Invalid token format.";
                    return false;
                }

                expectedPayload = $"{filingId}:{expiry}:{email}";
                signaturePart = parts[2];
            }
            else
            {
                expectedPayload = $"{filingId}:{expiry}";
                signaturePart = parts[1];
            }

            var expectedSig = ComputeHmac(expectedPayload, secret);

            // constant time compare
            if (!AreEqualBase64Url(signaturePart, expectedSig))
            {
                error = "Invalid token signature.";
                return false;
            }

            approverEmail = string.IsNullOrEmpty(email) ? null : email;
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

        private static byte[] Base64UrlDecode(string input)
        {
            var s = input.Replace('-', '+').Replace('_', '/');
            switch (s.Length % 4)
            {
                case 2: s += "=="; break;
                case 3: s += "="; break;
            }
            return Convert.FromBase64String(s);
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
