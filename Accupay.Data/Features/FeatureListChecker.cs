using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public class FeatureListChecker
{
    /// <summary>
    ///     ''' The default file name of the feature list.
    ///     ''' </summary>
    private const string DefaultFileName = "./features.xml";

    /// <summary>
    ///     ''' The list of optional features that AccuPay has and its corresponding correct password.
    ///     ''' </summary>
    private Dictionary<string, string> _featureList = new Dictionary<string, string>()
    {
        {
            "MassOvertime",
            "1189C349-DDA2-4654-8E7F-DC5FC62513C3"
        },
        {
            "JobLevel",
            "1189C349-DDA2-4654-8E7F-DC5FC62513C1"
        },
        {
            "AdditionalVacationLeaveType",
            "29abbfc8-4645-4153-9a9f-84794fad672f"
        },
        {
            "LoanDeductFromBonus",
            "8d066d73-775d-44b3-bd86-0bff6824aea5"
        },
        {
            "TripTicket",
            "58afca0b-7e27-44d3-b040-82031a5e75bd"
        }
    };

    /// <summary>
    ///     ''' The list of features that is enabled for a client.
    ///     ''' </summary>
    private Dictionary<string, string> _availableFeatures = new Dictionary<string, string>();

    private static readonly Lazy<FeatureListChecker> _instance = new Lazy<FeatureListChecker>(() =>
    {
        return new FeatureListChecker(DefaultFileName);
    }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

    public static FeatureListChecker Instance
    {
        get
        {
            return _instance.Value;
        }
    }

    public FeatureListChecker(Stream stream)
    {
        Initialize(stream);
    }

    public FeatureListChecker(string filename)
    {
        try
        {
            var stream = new MemoryStream(File.ReadAllBytes(filename));
            Initialize(stream);
        }
        catch (FileNotFoundException)
        {
        }
    }

    private void Initialize(Stream stream)
    {
        var document = XDocument.Load(stream);

        var features = from e in document.Root.Elements("feature")
                       select e;

        foreach (var feature in features)
        {
            var name = feature.Attribute("name").Value;
            var password = feature.Attribute("password").Value;

            _availableFeatures.Add(name, password);
        }
    }

    public bool HasAccess(Feature feature)
    {
        var featureName = feature.ToString();

        if (_availableFeatures.ContainsKey(featureName))
            return string.Equals(_availableFeatures[featureName], _featureList[featureName]);

        return false;
    }
}

public enum Feature
{
    MassOvertime,
    JobLevel,
    AdditionalVacationLeaveType,
    LoanDeductFromBonus,
    TripTicket
}