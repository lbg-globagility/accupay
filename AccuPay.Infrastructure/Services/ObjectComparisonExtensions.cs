using System;
using System.Reflection;
using System.Text;

public static class ObjectComparisonExtensions
{
    public static string GetPropertyChanges<T>(this T oldObj, T newObj) where T : class
    {
        // Handle null checks
        if (oldObj == null && newObj == null) return "Both objects are null.";
        if (oldObj == null) return "Old object was null.";
        if (newObj == null) return "New object was null.";

        var changes = new StringBuilder();
        Type type = typeof(T);

        // Get all public instance properties
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo prop in properties)
        {
            // 1. Skip properties that cannot be read or are indexers
            if (!prop.CanRead || prop.GetIndexParameters().Length > 0)
                continue;

            // 2. Ignore read-only properties (must have a public setter)
            if (!prop.CanWrite || prop.SetMethod == null || !prop.SetMethod.IsPublic)
                continue;

            // 3. Ignore virtual properties (and overridden virtual properties)
            MethodInfo getMethod = prop.GetMethod;
            if (getMethod != null && getMethod.IsVirtual && !getMethod.IsFinal)
                continue;

            object oldValue = prop.GetValue(oldObj, null);
            object newValue = prop.GetValue(newObj, null);

            // Compare values (handling nulls safely)
            if (!AreValuesEqual(oldValue, newValue))
            {
                string oldDisplay = oldValue?.ToString() ?? "<null>";
                string newDisplay = newValue?.ToString() ?? "<null>";

                changes.AppendLine($"{prop.Name}: '{oldDisplay}' -> '{newDisplay}'");
            }
        }

        string result = changes.ToString().TrimEnd();
        return string.IsNullOrEmpty(result) ? string.Empty : result;
    }

    private static bool AreValuesEqual(object val1, object val2)
    {
        if (val1 == null && val2 == null) return true;
        if (val1 == null || val2 == null) return false;

        return val1.Equals(val2);
    }
}
