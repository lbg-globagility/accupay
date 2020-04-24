using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class ListOfValueCollection
    {
        private IReadOnlyList<ListOfValue> _values;

        public ListOfValueCollection(IEnumerable<ListOfValue> values)
        {
            _values = new ReadOnlyCollection<ListOfValue>(values.ToList());
        }

        public bool Exists(string lic)
        {
            var value = _values?.FirstOrDefault(f => f.LIC == lic);
            return value != null;
        }

        public string GetValue(string lic, bool findByOrganization = false, int? organizationId = null)
        {
            ListOfValue value;

            if (findByOrganization)
            {
                value = _values?.FirstOrDefault(f => f.LIC == lic &&
                                                f.OrganizationID != null &&
                                                f.OrganizationID.Value == organizationId);

                if (value == null)
                    value = _values?.FirstOrDefault(f => f.LIC == lic);
            }
            else
                value = _values?.FirstOrDefault(f => f.LIC == lic);

            return value?.DisplayValue;
        }

        public string GetValue(string type, string lic, bool findByOrganization = false, int? organizationId = null)
        {
            ListOfValue value;

            if (findByOrganization)
            {
                value = _values?.FirstOrDefault(f => f.Type == type &&
                                                    f.LIC == lic &&
                                                    f.OrganizationID != null &&
                                                    f.OrganizationID.Value == organizationId);

                if (value == null)
                    value = _values?.FirstOrDefault(f => f.Type == type && f.LIC == lic);
            }
            else
                value = _values?.FirstOrDefault(f => f.Type == type && f.LIC == lic);

            return value?.DisplayValue;
        }

        public ListOfValueCollection GetSublist(string type)
        {
            return new ListOfValueCollection(_values.Where(l => l.Type == type));
        }

        public string GetString(string name, string @default = "")
        {
            var names = Split(name);
            var value = GetStringValue(names.Item1, names.Item2);

            return value ?? @default;
        }

        public string GetStringOrNull(string name)
        {
            var names = Split(name);

            return GetStringValue(names.Item1, names.Item2);
        }

        public string GetStringOrDefault(string name, string @default = "")
        {
            var names = Split(name);

            return GetStringValue(names.Item1, names.Item2) ?? @default;
        }

        public T GetEnum<T>(string name, T @default = default(T), bool findByOrganization = false) where T : struct
        {
            var names = Split(name);
            return GetEnum<T>(names.Item1, names.Item2, @default, findByOrganization);
        }

        public T GetEnum<T>(string type, string lic, T @default = default(T), bool findByOrganization = false) where T : struct
        {
            var value = GetValue(type, lic, findByOrganization);

            if (value == null)
                return @default;

            return Enums<T>.Parse(value);
        }

        public bool GetBoolean(string name, bool @default = false)
        {
            var names = Split(name);
            return GetBoolean(names.Item1, names.Item2, @default);
        }

        public bool GetBoolean(string type, string lic, bool @default = false)
        {
            var value = GetListOfValue(type, lic);

            if (string.IsNullOrEmpty(value?.DisplayValue))
                return @default;

            var intValue = ObjectUtils.ToNullableInteger(value?.DisplayValue);

            if (intValue != null)
                return Convert.ToBoolean(intValue);

            return Convert.ToBoolean(value?.DisplayValue);
        }

        public decimal GetDecimal(string name, decimal @default = 0)
        {
            var names = Split(name);
            var value = GetStringValue(names.Item1, names.Item2);

            return value != null ? decimal.Parse(value) : @default;
        }

        public decimal? GetDecimalOrNull(string name)
        {
            var names = Split(name);
            var value = GetStringValue(names.Item1, names.Item2);

            return value != null ? decimal.Parse(value) : default(Decimal);
        }

        private Tuple<string, string> Split(string name)
        {
            var names = name.Split(new[] { "." }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (names.Count() == 2)
            {
                var type = names[0];
                var lic = names[1];

                return new Tuple<string, string>(type, lic);
            }
            else if (names.Count() == 1)
            {
                var lic = names[0];

                return new Tuple<string, string>(null, lic);
            }

            return new Tuple<string, string>(null, null);
        }

        private string GetStringValue(string type, string lic)
        {
            ListOfValue value = null/* TODO Change to default(_) if this is not a reference type */;

            if (type == null)
                value = _values?.FirstOrDefault(f => f.LIC == lic);
            else
                value = _values?.FirstOrDefault(f => f.LIC == lic & f.Type == type);

            return value?.DisplayValue;
        }

        private ListOfValue GetListOfValue(string type, string lic)
        {
            return _values?.FirstOrDefault(f => f.Type == type & f.LIC == lic);
        }
    }
}