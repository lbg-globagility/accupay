using NUnit.Framework;
using System.Collections;

namespace AccuPay.Core.UnitTests.PhilHealth
{
    public class PhilHealthTestSource
    {
        // this data is based on 2020's philhealth
        public static IEnumerable TestData
        {
            get
            {
                yield return new TestCaseData(0m, 150m, 150m);
                yield return new TestCaseData(150m, 150m, 150m);
                yield return new TestCaseData(300m, 150m, 150m);
                yield return new TestCaseData(9_999m, 150m, 150m);
                yield return new TestCaseData(10_000m, 150m, 150m);
                yield return new TestCaseData(10_002m, 150.03m, 150.03m);
                yield return new TestCaseData(13_962m, 209.43m, 209.43m);
                yield return new TestCaseData(20_000m, 300m, 300m);
                yield return new TestCaseData(30_000m, 450m, 450m);
                yield return new TestCaseData(59_000m, 885m, 885m);
                yield return new TestCaseData(60_000m, 900m, 900m);
                yield return new TestCaseData(120_000m, 900m, 900m);

                // testing the OddCentDifference
                yield return new TestCaseData(10_003m, 150.04m, 150.05m);
            }
        }
    }
}
