using NUnit.Framework;
using System.Collections;

namespace AccuPay.Core.UnitTests.SSS
{
    public class SSSTestSource_2021
    {
        public static IEnumerable Brackets_SalaryBased
        {
            get
            {
                yield return new TestCaseData(1000.00m, 135.0m, 265.0m);
                yield return new TestCaseData(3249.99m, 135.0m, 265.0m);
                yield return new TestCaseData(3250.00m, 157.5m, 307.5m);
                yield return new TestCaseData(3749.99m, 157.5m, 307.5m);
                yield return new TestCaseData(3750.00m, 180.0m, 350.0m);
                yield return new TestCaseData(4249.99m, 180.0m, 350.0m);
                yield return new TestCaseData(4250.00m, 202.5m, 392.5m);
                yield return new TestCaseData(4749.99m, 202.5m, 392.5m);
                yield return new TestCaseData(4750.00m, 225.0m, 435.0m);
                yield return new TestCaseData(5249.99m, 225.0m, 435.0m);
                yield return new TestCaseData(5250.00m, 247.5m, 477.5m);
                yield return new TestCaseData(5749.99m, 247.5m, 477.5m);
                yield return new TestCaseData(5750.00m, 270.0m, 520.0m);
                yield return new TestCaseData(6249.99m, 270.0m, 520.0m);
                yield return new TestCaseData(6250.00m, 292.5m, 562.5m);
                yield return new TestCaseData(6749.99m, 292.5m, 562.5m);
                yield return new TestCaseData(6750.00m, 315.0m, 605.0m);
                yield return new TestCaseData(7249.99m, 315.0m, 605.0m);
                yield return new TestCaseData(7250.00m, 337.5m, 647.5m);
                yield return new TestCaseData(7749.99m, 337.5m, 647.5m);
                yield return new TestCaseData(7750.00m, 360.0m, 690.0m);
                yield return new TestCaseData(8249.99m, 360.0m, 690.0m);
                yield return new TestCaseData(8250.00m, 382.5m, 732.5m);
                yield return new TestCaseData(8749.99m, 382.5m, 732.5m);
                yield return new TestCaseData(8750.00m, 405.0m, 775.0m);
                yield return new TestCaseData(9249.99m, 405.0m, 775.0m);
                yield return new TestCaseData(9250.00m, 427.5m, 817.5m);
                yield return new TestCaseData(9749.99m, 427.5m, 817.5m);
                yield return new TestCaseData(9750.00m, 450.0m, 860.0m);
                yield return new TestCaseData(10249.99m, 450.0m, 860.0m);
                yield return new TestCaseData(10250.00m, 472.5m, 902.5m);
                yield return new TestCaseData(10749.99m, 472.5m, 902.5m);
                yield return new TestCaseData(10750.00m, 495.0m, 945.0m);
                yield return new TestCaseData(11249.99m, 495.0m, 945.0m);
                yield return new TestCaseData(11250.00m, 517.5m, 987.5m);
                yield return new TestCaseData(11749.99m, 517.5m, 987.5m);
                yield return new TestCaseData(11750.00m, 540.0m, 1030.0m);
                yield return new TestCaseData(12249.99m, 540.0m, 1030.0m);
                yield return new TestCaseData(12250.00m, 562.5m, 1072.5m);
                yield return new TestCaseData(12749.99m, 562.5m, 1072.5m);
                yield return new TestCaseData(12750.00m, 585.0m, 1115.0m);
                yield return new TestCaseData(13249.99m, 585.0m, 1115.0m);
                yield return new TestCaseData(13250.00m, 607.5m, 1157.5m);
                yield return new TestCaseData(13749.99m, 607.5m, 1157.5m);
                yield return new TestCaseData(13750.00m, 630.0m, 1200.0m);
                yield return new TestCaseData(14249.99m, 630.0m, 1200.0m);
                yield return new TestCaseData(14250.00m, 652.5m, 1242.5m);
                yield return new TestCaseData(14749.99m, 652.5m, 1242.5m);
                yield return new TestCaseData(14750.00m, 675.0m, 1305.0m);
                yield return new TestCaseData(15249.99m, 675.0m, 1305.0m);
                yield return new TestCaseData(15250.00m, 697.5m, 1347.5m);
                yield return new TestCaseData(15749.99m, 697.5m, 1347.5m);
                yield return new TestCaseData(15750.00m, 720.0m, 1390.0m);
                yield return new TestCaseData(16249.99m, 720.0m, 1390.0m);
                yield return new TestCaseData(16250.00m, 742.5m, 1432.5m);
                yield return new TestCaseData(16749.99m, 742.5m, 1432.5m);
                yield return new TestCaseData(16750.00m, 765.0m, 1475.0m);
                yield return new TestCaseData(17249.99m, 765.0m, 1475.0m);
                yield return new TestCaseData(17250.00m, 787.5m, 1517.5m);
                yield return new TestCaseData(17749.99m, 787.5m, 1517.5m);
                yield return new TestCaseData(17750.00m, 810.0m, 1560.0m);
                yield return new TestCaseData(18249.99m, 810.0m, 1560.0m);
                yield return new TestCaseData(18250.00m, 832.5m, 1602.5m);
                yield return new TestCaseData(18749.99m, 832.5m, 1602.5m);
                yield return new TestCaseData(18750.00m, 855.5m, 1645.0m);
                yield return new TestCaseData(19249.99m, 855.5m, 1645.0m);
                yield return new TestCaseData(19250.00m, 877.5m, 1687.5m);
                yield return new TestCaseData(19749.99m, 877.5m, 1687.5m);
                yield return new TestCaseData(19750.00m, 900.0m, 1730.0m);
                yield return new TestCaseData(20249.99m, 900.0m, 1730.0m);
                yield return new TestCaseData(20250.00m, 922.5m, 1772.5m);
                yield return new TestCaseData(20749.99m, 922.5m, 1772.5m);
                yield return new TestCaseData(20750.00m, 945.0m, 1815.0m);
                yield return new TestCaseData(21249.99m, 945.0m, 1815.0m);
                yield return new TestCaseData(21250.00m, 967.5m, 1857.5m);
                yield return new TestCaseData(21749.99m, 967.5m, 1857.5m);
                yield return new TestCaseData(21750.00m, 990.0m, 1900.0m);
                yield return new TestCaseData(22249.99m, 990.0m, 1900.0m);
                yield return new TestCaseData(22250.00m, 1012.5m, 1942.5m);
                yield return new TestCaseData(22749.99m, 1012.5m, 1942.5m);
                yield return new TestCaseData(22750.00m, 1035.0m, 1985.0m);
                yield return new TestCaseData(23249.99m, 1035.0m, 1985.0m);
                yield return new TestCaseData(23250.00m, 1057.5m, 2027.5m);
                yield return new TestCaseData(23749.99m, 1057.5m, 2027.5m);
                yield return new TestCaseData(23750.00m, 1080.0m, 2070.0m);
                yield return new TestCaseData(24249.99m, 1080.0m, 2070.0m);
                yield return new TestCaseData(24250.00m, 1102.5m, 2112.5m);
                yield return new TestCaseData(24749.99m, 1102.5m, 2112.5m);
                yield return new TestCaseData(24750.00m, 1125.0m, 2155.0m);
                yield return new TestCaseData(9999999.99m, 1125.0m, 2155.0m);
            }
        }

        public static IEnumerable Brackets_PaystubBased
        {
            get
            {
                yield return new TestCaseData(500.00m, 500.00m, 500.00m, 500.00m, 500.00m, 500.00m, 135.0m, 265.0m);
                yield return new TestCaseData(3000.00m, 249.99m, 3000.00m, 249.99m, 3000.00m, 249.99m, 135.0m, 265.0m);
                yield return new TestCaseData(3000.00m, 250.00m, 3000.00m, 250.00m, 3000.00m, 250.00m, 157.5m, 307.5m);
                yield return new TestCaseData(3000.00m, 749.99m, 3000.00m, 749.99m, 3000.00m, 749.99m, 157.5m, 307.5m);
                yield return new TestCaseData(3000.00m, 750.00m, 3000.00m, 750.00m, 3000.00m, 750.00m, 180.0m, 350.0m);
                yield return new TestCaseData(4000.00m, 249.99m, 4000.00m, 249.99m, 4000.00m, 249.99m, 180.0m, 350.0m);
                yield return new TestCaseData(4000.00m, 250.00m, 4000.00m, 250.00m, 4000.00m, 250.00m, 202.5m, 392.5m);
                yield return new TestCaseData(4000.00m, 749.99m, 4000.00m, 749.99m, 4000.00m, 749.99m, 202.5m, 392.5m);
                yield return new TestCaseData(4000.00m, 750.00m, 4000.00m, 750.00m, 4000.00m, 750.00m, 225.0m, 435.0m);
                yield return new TestCaseData(5000.00m, 249.99m, 5000.00m, 249.99m, 5000.00m, 249.99m, 225.0m, 435.0m);
                yield return new TestCaseData(5000.00m, 250.00m, 5000.00m, 250.00m, 5000.00m, 250.00m, 247.5m, 477.5m);
                yield return new TestCaseData(5000.00m, 749.99m, 5000.00m, 749.99m, 5000.00m, 749.99m, 247.5m, 477.5m);
                yield return new TestCaseData(5000.00m, 750.00m, 5000.00m, 750.00m, 5000.00m, 750.00m, 270.0m, 520.0m);
                yield return new TestCaseData(6000.00m, 249.99m, 6000.00m, 249.99m, 6000.00m, 249.99m, 270.0m, 520.0m);
                yield return new TestCaseData(6000.00m, 250.00m, 6000.00m, 250.00m, 6000.00m, 250.00m, 292.5m, 562.5m);
                yield return new TestCaseData(6000.00m, 749.99m, 6000.00m, 749.99m, 6000.00m, 749.99m, 292.5m, 562.5m);
                yield return new TestCaseData(6000.00m, 750.00m, 6000.00m, 750.00m, 6000.00m, 750.00m, 315.0m, 605.0m);
                yield return new TestCaseData(7000.00m, 249.99m, 7000.00m, 249.99m, 7000.00m, 249.99m, 315.0m, 605.0m);
                yield return new TestCaseData(7000.00m, 250.00m, 7000.00m, 250.00m, 7000.00m, 250.00m, 337.5m, 647.5m);
                yield return new TestCaseData(7000.00m, 749.99m, 7000.00m, 749.99m, 7000.00m, 749.99m, 337.5m, 647.5m);
                yield return new TestCaseData(7000.00m, 750.00m, 7000.00m, 750.00m, 7000.00m, 750.00m, 360.0m, 690.0m);
                yield return new TestCaseData(8000.00m, 249.99m, 8000.00m, 249.99m, 8000.00m, 249.99m, 360.0m, 690.0m);
                yield return new TestCaseData(8000.00m, 250.00m, 8000.00m, 250.00m, 8000.00m, 250.00m, 382.5m, 732.5m);
                yield return new TestCaseData(8000.00m, 749.99m, 8000.00m, 749.99m, 8000.00m, 749.99m, 382.5m, 732.5m);
                yield return new TestCaseData(8000.00m, 750.00m, 8000.00m, 750.00m, 8000.00m, 750.00m, 405.0m, 775.0m);
                yield return new TestCaseData(9000.00m, 249.99m, 9000.00m, 249.99m, 9000.00m, 249.99m, 405.0m, 775.0m);
                yield return new TestCaseData(9000.00m, 250.00m, 9000.00m, 250.00m, 9000.00m, 250.00m, 427.5m, 817.5m);
                yield return new TestCaseData(9000.00m, 749.99m, 9000.00m, 749.99m, 9000.00m, 749.99m, 427.5m, 817.5m);
                yield return new TestCaseData(9000.00m, 750.00m, 9000.00m, 750.00m, 9000.00m, 750.00m, 450.0m, 860.0m);
                yield return new TestCaseData(10000.00m, 249.99m, 10000.00m, 249.99m, 10000.00m, 249.99m, 450.0m, 860.0m);
                yield return new TestCaseData(10000.00m, 250.00m, 10000.00m, 250.00m, 10000.00m, 250.00m, 472.5m, 902.5m);
                yield return new TestCaseData(10000.00m, 749.99m, 10000.00m, 749.99m, 10000.00m, 749.99m, 472.5m, 902.5m);
                yield return new TestCaseData(10000.00m, 750.00m, 10000.00m, 750.00m, 10000.00m, 750.00m, 495.0m, 945.0m);
                yield return new TestCaseData(11000.00m, 249.99m, 11000.00m, 249.99m, 11000.00m, 249.99m, 495.0m, 945.0m);
                yield return new TestCaseData(11000.00m, 250.00m, 11000.00m, 250.00m, 11000.00m, 250.00m, 517.5m, 987.5m);
                yield return new TestCaseData(11000.00m, 749.99m, 11000.00m, 749.99m, 11000.00m, 749.99m, 517.5m, 987.5m);
                yield return new TestCaseData(11000.00m, 750.00m, 11000.00m, 750.00m, 11000.00m, 750.00m, 540.0m, 1030.0m);
                yield return new TestCaseData(12000.00m, 249.99m, 12000.00m, 249.99m, 12000.00m, 249.99m, 540.0m, 1030.0m);
                yield return new TestCaseData(12000.00m, 250.00m, 12000.00m, 250.00m, 12000.00m, 250.00m, 562.5m, 1072.5m);
                yield return new TestCaseData(12000.00m, 749.99m, 12000.00m, 749.99m, 12000.00m, 749.99m, 562.5m, 1072.5m);
                yield return new TestCaseData(12000.00m, 750.00m, 12000.00m, 750.00m, 12000.00m, 750.00m, 585.0m, 1115.0m);
                yield return new TestCaseData(13000.00m, 249.99m, 13000.00m, 249.99m, 13000.00m, 249.99m, 585.0m, 1115.0m);
                yield return new TestCaseData(13000.00m, 250.00m, 13000.00m, 250.00m, 13000.00m, 250.00m, 607.5m, 1157.5m);
                yield return new TestCaseData(13000.00m, 749.99m, 13000.00m, 749.99m, 13000.00m, 749.99m, 607.5m, 1157.5m);
                yield return new TestCaseData(13000.00m, 750.00m, 13000.00m, 750.00m, 13000.00m, 750.00m, 630.0m, 1200.0m);
                yield return new TestCaseData(14000.00m, 249.99m, 14000.00m, 249.99m, 14000.00m, 249.99m, 630.0m, 1200.0m);
                yield return new TestCaseData(14000.00m, 250.00m, 14000.00m, 250.00m, 14000.00m, 250.00m, 652.5m, 1242.5m);
                yield return new TestCaseData(14000.00m, 749.99m, 14000.00m, 749.99m, 14000.00m, 749.99m, 652.5m, 1242.5m);
                yield return new TestCaseData(14000.00m, 750.00m, 14000.00m, 750.00m, 14000.00m, 750.00m, 675.0m, 1305.0m);
                yield return new TestCaseData(15000.00m, 249.99m, 15000.00m, 249.99m, 15000.00m, 249.99m, 675.0m, 1305.0m);
                yield return new TestCaseData(15000.00m, 250.00m, 15000.00m, 250.00m, 15000.00m, 250.00m, 697.5m, 1347.5m);
                yield return new TestCaseData(15000.00m, 749.99m, 15000.00m, 749.99m, 15000.00m, 749.99m, 697.5m, 1347.5m);
                yield return new TestCaseData(15000.00m, 750.00m, 15000.00m, 750.00m, 15000.00m, 750.00m, 720.0m, 1390.0m);
                yield return new TestCaseData(16000.00m, 249.99m, 16000.00m, 249.99m, 16000.00m, 249.99m, 720.0m, 1390.0m);
                yield return new TestCaseData(16000.00m, 250.00m, 16000.00m, 250.00m, 16000.00m, 250.00m, 742.5m, 1432.5m);
                yield return new TestCaseData(16000.00m, 749.99m, 16000.00m, 749.99m, 16000.00m, 749.99m, 742.5m, 1432.5m);
                yield return new TestCaseData(16000.00m, 750.00m, 16000.00m, 750.00m, 16000.00m, 750.00m, 765.0m, 1475.0m);
                yield return new TestCaseData(17000.00m, 249.99m, 17000.00m, 249.99m, 17000.00m, 249.99m, 765.0m, 1475.0m);
                yield return new TestCaseData(17000.00m, 250.00m, 17000.00m, 250.00m, 17000.00m, 250.00m, 787.5m, 1517.5m);
                yield return new TestCaseData(17000.00m, 749.99m, 17000.00m, 749.99m, 17000.00m, 749.99m, 787.5m, 1517.5m);
                yield return new TestCaseData(17000.00m, 750.00m, 17000.00m, 750.00m, 17000.00m, 750.00m, 810.0m, 1560.0m);
                yield return new TestCaseData(18000.00m, 249.99m, 18000.00m, 249.99m, 18000.00m, 249.99m, 810.0m, 1560.0m);
                yield return new TestCaseData(18000.00m, 250.00m, 18000.00m, 250.00m, 18000.00m, 250.00m, 832.5m, 1602.5m);
                yield return new TestCaseData(18000.00m, 749.99m, 18000.00m, 749.99m, 18000.00m, 749.99m, 832.5m, 1602.5m);
                yield return new TestCaseData(18000.00m, 750.00m, 18000.00m, 750.00m, 18000.00m, 750.00m, 855.5m, 1645.0m);
                yield return new TestCaseData(19000.00m, 249.99m, 19000.00m, 249.99m, 19000.00m, 249.99m, 855.5m, 1645.0m);
                yield return new TestCaseData(19000.00m, 250.00m, 19000.00m, 250.00m, 19000.00m, 250.00m, 877.5m, 1687.5m);
                yield return new TestCaseData(19000.00m, 749.99m, 19000.00m, 749.99m, 19000.00m, 749.99m, 877.5m, 1687.5m);
                yield return new TestCaseData(19000.00m, 750.00m, 19000.00m, 750.00m, 19000.00m, 750.00m, 900.0m, 1730.0m);
                yield return new TestCaseData(20000.00m, 249.99m, 20000.00m, 249.99m, 20000.00m, 249.99m, 900.0m, 1730.0m);
                yield return new TestCaseData(20000.00m, 250.00m, 20000.00m, 250.00m, 20000.00m, 250.00m, 922.5m, 1772.5m);
                yield return new TestCaseData(20000.00m, 749.99m, 20000.00m, 749.99m, 20000.00m, 749.99m, 922.5m, 1772.5m);
                yield return new TestCaseData(20000.00m, 750.00m, 20000.00m, 750.00m, 20000.00m, 750.00m, 945.0m, 1815.0m);
                yield return new TestCaseData(21000.00m, 249.99m, 21000.00m, 249.99m, 21000.00m, 249.99m, 945.0m, 1815.0m);
                yield return new TestCaseData(21000.00m, 250.00m, 21000.00m, 250.00m, 21000.00m, 250.00m, 967.5m, 1857.5m);
                yield return new TestCaseData(21000.00m, 749.99m, 21000.00m, 749.99m, 21000.00m, 749.99m, 967.5m, 1857.5m);
                yield return new TestCaseData(21000.00m, 750.00m, 21000.00m, 750.00m, 21000.00m, 750.00m, 990.0m, 1900.0m);
                yield return new TestCaseData(22000.00m, 249.99m, 22000.00m, 249.99m, 22000.00m, 249.99m, 990.0m, 1900.0m);
                yield return new TestCaseData(22000.00m, 250.00m, 22000.00m, 250.00m, 22000.00m, 250.00m, 1012.5m, 1942.5m);
                yield return new TestCaseData(22000.00m, 749.99m, 22000.00m, 749.99m, 22000.00m, 749.99m, 1012.5m, 1942.5m);
                yield return new TestCaseData(22000.00m, 750.00m, 22000.00m, 750.00m, 22000.00m, 750.00m, 1035.0m, 1985.0m);
                yield return new TestCaseData(23000.00m, 249.99m, 23000.00m, 249.99m, 23000.00m, 249.99m, 1035.0m, 1985.0m);
                yield return new TestCaseData(23000.00m, 250.00m, 23000.00m, 250.00m, 23000.00m, 250.00m, 1057.5m, 2027.5m);
                yield return new TestCaseData(23000.00m, 749.99m, 23000.00m, 749.99m, 23000.00m, 749.99m, 1057.5m, 2027.5m);
                yield return new TestCaseData(23000.00m, 750.00m, 23000.00m, 750.00m, 23000.00m, 750.00m, 1080.0m, 2070.0m);
                yield return new TestCaseData(24000.00m, 249.99m, 24000.00m, 249.99m, 24000.00m, 249.99m, 1080.0m, 2070.0m);
                yield return new TestCaseData(24000.00m, 250.00m, 24000.00m, 250.00m, 24000.00m, 250.00m, 1102.5m, 2112.5m);
                yield return new TestCaseData(24000.00m, 749.99m, 24000.00m, 749.99m, 24000.00m, 749.99m, 1102.5m, 2112.5m);
                yield return new TestCaseData(24000.00m, 750.00m, 24000.00m, 750.00m, 24000.00m, 750.00m, 1125.0m, 2155.0m);
                yield return new TestCaseData(9999000.00m, 999.99m, 9999000.00m, 999.99m, 9999000.00m, 999.99m, 1125.0m, 2155.0m);
            }
        }
    }
}
