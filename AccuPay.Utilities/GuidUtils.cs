using System;

namespace AccuPay.Utilities
{
    public class GuidUtils
    {
        /// <summary>
        /// Converts a <see cref="Guid"/> into its big-endian bytes array representation
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(Guid? guid)
        {
            if (guid == null) return null;

            var bytes = guid.Value.ToByteArray();

            var temp = bytes[0];
            bytes[0] = bytes[3];
            bytes[3] = temp;

            temp = bytes[1];
            bytes[1] = bytes[2];
            bytes[2] = temp;

            temp = bytes[4];
            bytes[4] = bytes[5];
            bytes[5] = temp;

            temp = bytes[6];
            bytes[6] = bytes[7];
            bytes[7] = temp;

            return bytes;
        }

        /// <summary>
        /// Converts a big-endian byte form of a guid into a <see cref="Guid"/>
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Guid ToGuid(byte[] bytes)
        {
            var temp = bytes[0];
            bytes[0] = bytes[3];
            bytes[3] = temp;

            temp = bytes[1];
            bytes[1] = bytes[2];
            bytes[2] = temp;

            temp = bytes[4];
            bytes[4] = bytes[5];
            bytes[5] = temp;

            temp = bytes[6];
            bytes[6] = bytes[7];
            bytes[7] = temp;

            return new Guid(bytes);
        }
    }
}