using AccuPay.Utilities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq.Expressions;

namespace AccuPay.Data.Data.EntityFrameworkCore
{
    internal class NullableGuidToBigEndianBytesConverter : ValueConverter<Guid?, byte[]>
    {
        public NullableGuidToBigEndianBytesConverter(ConverterMappingHints mappingHints = null)
            : base(convertToExpr, convertFromExpr, mappingHints)
        {
        }

        private static readonly Expression<Func<Guid?, byte[]>> convertToExpr = x => GuidUtils.ToByteArray(x);
        private static readonly Expression<Func<byte[], Guid?>> convertFromExpr = x => GuidUtils.ToGuid(x);
    }
}