using AccuPay.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AccuPay.Web.TimeLogs
{
    internal static class ApprovalTokenHelper
    {
        // How long an approval-email link stays valid before an employee is allowed to resend it.
        public static TimeSpan GetTokenTtl(IConfiguration configuration, int defaultHours = 24)
        {
            var hours = defaultHours;
            if (int.TryParse(configuration["App:ApprovalTokenHours"], out var configuredHours) && configuredHours > 0)
                hours = configuredHours;

            return TimeSpan.FromHours(hours);
        }

        // Resending is blocked while a previously sent approval link is still valid, so approvers
        // don't get flooded with duplicate emails/tokens for the same filing.
        public static void EnsureResendAllowed(bool isNotifyEmail, DateTime? notifyEmailSentAt, TimeSpan ttl)
        {
            if (!isNotifyEmail || !notifyEmailSentAt.HasValue) return;

            var expiresAt = notifyEmailSentAt.Value.Add(ttl);
            if (DateTime.Now < expiresAt)
            {
                throw new BusinessLogicException(
                    $"An approval email was already sent and its link is still valid until {expiresAt:yyyy-MM-dd HH:mm} UTC. You can resend it once that link expires.");
            }
        }

        // token without an approver email = "{expiryUnixSeconds}.{signatureBase64Url}"
        // token with an approver email    = "{expiryUnixSeconds}.{emailBase64Url}.{signatureBase64Url}"
        public static string GenerateToken(int filingId, string secret, TimeSpan ttl, string approverEmail = null)
        {
            var expiry = DateTimeOffset.Now.Add(ttl).ToUnixTimeSeconds();

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

            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
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
