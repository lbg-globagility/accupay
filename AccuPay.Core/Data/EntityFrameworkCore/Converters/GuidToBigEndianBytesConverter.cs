using AccuPay.Utilities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq.Expressions;

namespace AccuPay.Core.Data.EntityFrameworkCore
{
    public class GuidToBigEndianBytesConverter : ValueConverter<Guid, byte[]>
    {
        public GuidToBigEndianBytesConverter(ConverterMappingHints mappingHints = null)
            : base(convertToExpr, convertFromExpr, mappingHints)
        {
        }

        private static readonly Expression<Func<Guid, byte[]>> convertToExpr = x => GuidUtils.ToByteArray(x);
        private static readonly Expression<Func<byte[], Guid>> convertFromExpr = x => GuidUtils.ToGuid(x);
    }
}