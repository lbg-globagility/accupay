﻿using System;

namespace AccuPay.Core.Helpers
{
    public static class Enums<TEnum> where TEnum : struct
    {
        public static TEnum Parse(string name)
        {
            var value = Enum.Parse(typeof(TEnum), name);
            return (TEnum)value;
        }
    }
}