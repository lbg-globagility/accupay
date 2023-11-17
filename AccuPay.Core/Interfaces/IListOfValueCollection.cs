namespace AccuPay.Core.Interfaces
{
    public interface IListOfValueCollection
    {
        bool Exists(string lic);

        bool GetBoolean(string name, bool @default = false);

        bool GetBoolean(string type, string lic, bool @default = false);

        decimal GetDecimal(string name, decimal @default = 0);

        decimal? GetDecimalOrNull(string name);

        T GetEnum<T>(string name, T @default = default, bool findByOrganization = false, int? organizationId = null) where T : struct;

        T GetEnum<T>(string type, string lic, T @default = default, bool findByOrganization = false, int? organizationId = null) where T : struct;

        string GetString(string name, string @default = "", bool findByOrganization = false, int? organizationId = null);

        string GetStringOrDefault(string name, string @default = "", bool findByOrganization = false, int? organizationId = null);

        string GetStringOrNull(string name, bool findByOrganization = false, int? organizationId = null);

        string GetValue(string lic, bool findByOrganization = false, int? organizationId = null);

        string GetValue(string type, string lic, bool findByOrganization = false, int? organizationId = null);
    }
}
