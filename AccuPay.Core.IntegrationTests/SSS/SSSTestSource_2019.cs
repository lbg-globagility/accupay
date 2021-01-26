using NUnit.Framework;
using System.Collections;

namespace AccuPay.Core.IntegrationTests.SSS
{
    public class SSSTestSource_2019
    {
        public static IEnumerable Brackets_SalaryBased
        {
            get
            {
                yield return new TestCaseData(0m, 80m, 170m);
                yield return new TestCaseData(1_125m, 80m, 170m);
                yield return new TestCaseData(2_249m, 80m, 170m);
                //
                yield return new TestCaseData(2_250m, 100m, 210m);
                yield return new TestCaseData(2_500m, 100m, 210m);
                yield return new TestCaseData(2_749m, 100m, 210m);
                //
                yield return new TestCaseData(2_750.00m, 120.00m, 250.00m);
                yield return new TestCaseData(3_000.00m, 120.00m, 250.00m);
                yield return new TestCaseData(3_249.99m, 120.00m, 250.00m);
                //
                yield return new TestCaseData(3_250.00m, 140.00m, 290.00m);
                yield return new TestCaseData(3_500.00m, 140.00m, 290.00m);
                yield return new TestCaseData(3_749.99m, 140.00m, 290.00m);
                //
                yield return new TestCaseData(3_750.00m, 160.00m, 330.00m);
                yield return new TestCaseData(4_000.00m, 160.00m, 330.00m);
                yield return new TestCaseData(4_249.99m, 160.00m, 330.00m);
                //
                yield return new TestCaseData(4_250.00m, 180.00m, 370.00m);
                yield return new TestCaseData(4_500.00m, 180.00m, 370.00m);
                yield return new TestCaseData(4_749.99m, 180.00m, 370.00m);
                //
                yield return new TestCaseData(4_750.00m, 200.00m, 410.00m);
                yield return new TestCaseData(5_000.00m, 200.00m, 410.00m);
                yield return new TestCaseData(5_249.99m, 200.00m, 410.00m);
                //
                yield return new TestCaseData(5_250.00m, 220.00m, 450.00m);
                yield return new TestCaseData(5_500.00m, 220.00m, 450.00m);
                yield return new TestCaseData(5_749.99m, 220.00m, 450.00m);
                //
                yield return new TestCaseData(5_750.00m, 240.00m, 490.00m);
                yield return new TestCaseData(6_000.00m, 240.00m, 490.00m);
                yield return new TestCaseData(6_249.99m, 240.00m, 490.00m);
                //
                yield return new TestCaseData(6_250.00m, 260.00m, 530.00m);
                yield return new TestCaseData(6_500.00m, 260.00m, 530.00m);
                yield return new TestCaseData(6_749.99m, 260.00m, 530.00m);
                //
                yield return new TestCaseData(6_750.00m, 280.00m, 570.00m);
                yield return new TestCaseData(7_000.00m, 280.00m, 570.00m);
                yield return new TestCaseData(7_249.99m, 280.00m, 570.00m);
                //
                yield return new TestCaseData(7_250.00m, 300.00m, 610.00m);
                yield return new TestCaseData(7_500.00m, 300.00m, 610.00m);
                yield return new TestCaseData(7_749.99m, 300.00m, 610.00m);
                //
                yield return new TestCaseData(7_750.00m, 320.00m, 650.00m);
                yield return new TestCaseData(8_000.00m, 320.00m, 650.00m);
                yield return new TestCaseData(8_249.99m, 320.00m, 650.00m);
                //
                yield return new TestCaseData(8_250.00m, 340.00m, 690.00m);
                yield return new TestCaseData(8_500.00m, 340.00m, 690.00m);
                yield return new TestCaseData(8_749.99m, 340.00m, 690.00m);
                //
                yield return new TestCaseData(8_750.00m, 360.00m, 730.00m);
                yield return new TestCaseData(9_000.00m, 360.00m, 730.00m);
                yield return new TestCaseData(9_249.99m, 360.00m, 730.00m);
                //
                yield return new TestCaseData(9_250.00m, 380.00m, 770.00m);
                yield return new TestCaseData(9_500.00m, 380.00m, 770.00m);
                yield return new TestCaseData(9_749.99m, 380.00m, 770.00m);
                //
                yield return new TestCaseData(9_750.00m, 400.00m, 810.00m);
                yield return new TestCaseData(10_000.00m, 400.00m, 810.00m);
                yield return new TestCaseData(10_249.99m, 400.00m, 810.00m);
                //
                yield return new TestCaseData(10_250.00m, 420.00m, 850.00m);
                yield return new TestCaseData(10_500.00m, 420.00m, 850.00m);
                yield return new TestCaseData(10_749.99m, 420.00m, 850.00m);
                //
                yield return new TestCaseData(10_750.00m, 440.00m, 890.00m);
                yield return new TestCaseData(11_000.00m, 440.00m, 890.00m);
                yield return new TestCaseData(11_249.99m, 440.00m, 890.00m);
                //
                yield return new TestCaseData(11_250.00m, 460.00m, 930.00m);
                yield return new TestCaseData(11_500.00m, 460.00m, 930.00m);
                yield return new TestCaseData(11_749.99m, 460.00m, 930.00m);
                //
                yield return new TestCaseData(11_750.00m, 480.00m, 970.00m);
                yield return new TestCaseData(12_000.00m, 480.00m, 970.00m);
                yield return new TestCaseData(12_249.99m, 480.00m, 970.00m);
                //
                yield return new TestCaseData(12_250.00m, 500.00m, 1_010.00m);
                yield return new TestCaseData(12_500.00m, 500.00m, 1_010.00m);
                yield return new TestCaseData(12_749.99m, 500.00m, 1_010.00m);
                //
                yield return new TestCaseData(12_750.00m, 520.00m, 1_050.00m);
                yield return new TestCaseData(13_000.00m, 520.00m, 1_050.00m);
                yield return new TestCaseData(13_249.99m, 520.00m, 1_050.00m);
                //
                yield return new TestCaseData(13_250.00m, 540.00m, 1_090.00m);
                yield return new TestCaseData(13_500.00m, 540.00m, 1_090.00m);
                yield return new TestCaseData(13_749.99m, 540.00m, 1_090.00m);
                //
                yield return new TestCaseData(13_750.00m, 560.00m, 1_130.00m);
                yield return new TestCaseData(14_000.00m, 560.00m, 1_130.00m);
                yield return new TestCaseData(14_249.99m, 560.00m, 1_130.00m);
                //
                yield return new TestCaseData(14_250.00m, 580.00m, 1_170.00m);
                yield return new TestCaseData(14_500.00m, 580.00m, 1_170.00m);
                yield return new TestCaseData(14_749.99m, 580.00m, 1_170.00m);
                //
                yield return new TestCaseData(14_750.00m, 600.00m, 1_230.00m);
                yield return new TestCaseData(15_000.00m, 600.00m, 1_230.00m);
                yield return new TestCaseData(15_249.99m, 600.00m, 1_230.00m);
                //
                yield return new TestCaseData(15_250.00m, 620.00m, 1_270.00m);
                yield return new TestCaseData(15_500.00m, 620.00m, 1_270.00m);
                yield return new TestCaseData(15_749.99m, 620.00m, 1_270.00m);
                //
                yield return new TestCaseData(15_750.00m, 640.00m, 1_310.00m);
                yield return new TestCaseData(16_000.00m, 640.00m, 1_310.00m);
                yield return new TestCaseData(16_249.99m, 640.00m, 1_310.00m);
                //
                yield return new TestCaseData(16_250.00m, 660.00m, 1_350.00m);
                yield return new TestCaseData(16_500.00m, 660.00m, 1_350.00m);
                yield return new TestCaseData(16_749.99m, 660.00m, 1_350.00m);
                //
                yield return new TestCaseData(16_750.00m, 680.00m, 1_390.00m);
                yield return new TestCaseData(17_000.00m, 680.00m, 1_390.00m);
                yield return new TestCaseData(17_249.99m, 680.00m, 1_390.00m);
                //
                yield return new TestCaseData(17_250.00m, 700.00m, 1_430.00m);
                yield return new TestCaseData(17_500.00m, 700.00m, 1_430.00m);
                yield return new TestCaseData(17_749.99m, 700.00m, 1_430.00m);
                //
                yield return new TestCaseData(17_750.00m, 720.00m, 1_470.00m);
                yield return new TestCaseData(18_000.00m, 720.00m, 1_470.00m);
                yield return new TestCaseData(18_249.99m, 720.00m, 1_470.00m);
                //
                yield return new TestCaseData(18_250.00m, 740.00m, 1_510.00m);
                yield return new TestCaseData(18_500.00m, 740.00m, 1_510.00m);
                yield return new TestCaseData(18_749.99m, 740.00m, 1_510.00m);
                //
                yield return new TestCaseData(18_750.00m, 760.00m, 1_550.00m);
                yield return new TestCaseData(19_000.00m, 760.00m, 1_550.00m);
                yield return new TestCaseData(19_249.99m, 760.00m, 1_550.00m);
                //
                yield return new TestCaseData(19_250.00m, 780.00m, 1_590.00m);
                yield return new TestCaseData(19_500.00m, 780.00m, 1_590.00m);
                yield return new TestCaseData(19_749.99m, 780.00m, 1_590.00m);
                //
                yield return new TestCaseData(19_750.00m, 800.00m, 1_630.00m);
                yield return new TestCaseData(59_875.00m, 800.00m, 1_630.00m);
                yield return new TestCaseData(999_999.00m, 800.00m, 1_630.00m);
            }
        }

        public static IEnumerable Brackets_PaystubBased
        {
            get
            {
                yield return new TestCaseData(0m, 0m, 0m, 0m, 0m, 0m, 80m, 170m);
                yield return new TestCaseData(1_000m, 125m, 1_000m, 125m, 1_000m, 125m, 80m, 170m);
                yield return new TestCaseData(2_000m, 249m, 2_000m, 249m, 2_000m, 249m, 80m, 170m);
                //
                yield return new TestCaseData(2_000m, 250m, 2_000m, 250m, 2_000m, 250m, 100m, 210m);
                yield return new TestCaseData(2_000m, 500m, 2_000m, 500m, 2_000m, 500m, 100m, 210m);
                yield return new TestCaseData(2_000m, 749m, 2_000m, 749m, 2_000m, 749m, 100m, 210m);
                //
                yield return new TestCaseData(2_000.00m, 750m, 2_000.00m, 750m, 2_000.00m, 750m, 120.00m, 250.00m);
                yield return new TestCaseData(3_000.00m, 000m, 3_000.00m, 000m, 3_000.00m, 000m, 120.00m, 250.00m);
                yield return new TestCaseData(3_000.99m, 249m, 3_000.99m, 249m, 3_000.99m, 249m, 120.00m, 250.00m);
                //
                yield return new TestCaseData(3_000.00m, 250m, 3_000.00m, 250m, 3_000.00m, 250m, 140.00m, 290.00m);
                yield return new TestCaseData(3_000.00m, 500m, 3_000.00m, 500m, 3_000.00m, 500m, 140.00m, 290.00m);
                yield return new TestCaseData(3_000.99m, 749m, 3_000.99m, 749m, 3_000.99m, 749m, 140.00m, 290.00m);
                //
                yield return new TestCaseData(3_000.00m, 750m, 3_000.00m, 750m, 3_000.00m, 750m, 160.00m, 330.00m);
                yield return new TestCaseData(4_000.00m, 000m, 4_000.00m, 000m, 4_000.00m, 000m, 160.00m, 330.00m);
                yield return new TestCaseData(4_000.99m, 249m, 4_000.99m, 249m, 4_000.99m, 249m, 160.00m, 330.00m);
                //
                yield return new TestCaseData(4_000.00m, 250m, 4_000.00m, 250m, 4_000.00m, 250m, 180.00m, 370.00m);
                yield return new TestCaseData(4_000.00m, 500m, 4_000.00m, 500m, 4_000.00m, 500m, 180.00m, 370.00m);
                yield return new TestCaseData(4_000.99m, 749m, 4_000.99m, 749m, 4_000.99m, 749m, 180.00m, 370.00m);
                //
                yield return new TestCaseData(4_000.00m, 750m, 4_000.00m, 750m, 4_000.00m, 750m, 200.00m, 410.00m);
                yield return new TestCaseData(5_000.00m, 000m, 5_000.00m, 000m, 5_000.00m, 000m, 200.00m, 410.00m);
                yield return new TestCaseData(5_000.99m, 249m, 5_000.99m, 249m, 5_000.99m, 249m, 200.00m, 410.00m);
                //
                yield return new TestCaseData(5_000.00m, 250m, 5_000.00m, 250m, 5_000.00m, 250m, 220.00m, 450.00m);
                yield return new TestCaseData(5_000.00m, 500m, 5_000.00m, 500m, 5_000.00m, 500m, 220.00m, 450.00m);
                yield return new TestCaseData(5_000.99m, 749m, 5_000.99m, 749m, 5_000.99m, 749m, 220.00m, 450.00m);
                //
                yield return new TestCaseData(5_000.00m, 750m, 5_000.00m, 750m, 5_000.00m, 750m, 240.00m, 490.00m);
                yield return new TestCaseData(6_000.00m, 000m, 6_000.00m, 000m, 6_000.00m, 000m, 240.00m, 490.00m);
                yield return new TestCaseData(6_000.99m, 249m, 6_000.99m, 249m, 6_000.99m, 249m, 240.00m, 490.00m);
                //
                yield return new TestCaseData(6_000.00m, 250m, 6_000.00m, 250m, 6_000.00m, 250m, 260.00m, 530.00m);
                yield return new TestCaseData(6_000.00m, 500m, 6_000.00m, 500m, 6_000.00m, 500m, 260.00m, 530.00m);
                yield return new TestCaseData(6_000.99m, 749m, 6_000.99m, 749m, 6_000.99m, 749m, 260.00m, 530.00m);
                //
                yield return new TestCaseData(6_000.00m, 750m, 6_000.00m, 750m, 6_000.00m, 750m, 280.00m, 570.00m);
                yield return new TestCaseData(7_000.00m, 000m, 7_000.00m, 000m, 7_000.00m, 000m, 280.00m, 570.00m);
                yield return new TestCaseData(7_000.99m, 249m, 7_000.99m, 249m, 7_000.99m, 249m, 280.00m, 570.00m);
                //
                yield return new TestCaseData(7_000.00m, 250m, 7_000.00m, 250m, 7_000.00m, 250m, 300.00m, 610.00m);
                yield return new TestCaseData(7_000.00m, 500m, 7_000.00m, 500m, 7_000.00m, 500m, 300.00m, 610.00m);
                yield return new TestCaseData(7_000.99m, 749m, 7_000.99m, 749m, 7_000.99m, 749m, 300.00m, 610.00m);
                //
                yield return new TestCaseData(7_000.00m, 750m, 7_000.00m, 750m, 7_000.00m, 750m, 320.00m, 650.00m);
                yield return new TestCaseData(8_000.00m, 000m, 8_000.00m, 000m, 8_000.00m, 000m, 320.00m, 650.00m);
                yield return new TestCaseData(8_000.99m, 249m, 8_000.99m, 249m, 8_000.99m, 249m, 320.00m, 650.00m);
                //
                yield return new TestCaseData(8_000.00m, 250m, 8_000.00m, 250m, 8_000.00m, 250m, 340.00m, 690.00m);
                yield return new TestCaseData(8_000.00m, 500m, 8_000.00m, 500m, 8_000.00m, 500m, 340.00m, 690.00m);
                yield return new TestCaseData(8_000.99m, 749m, 8_000.99m, 749m, 8_000.99m, 749m, 340.00m, 690.00m);
                //
                yield return new TestCaseData(8_000.00m, 750m, 8_000.00m, 750m, 8_000.00m, 750m, 360.00m, 730.00m);
                yield return new TestCaseData(9_000.00m, 000m, 9_000.00m, 000m, 9_000.00m, 000m, 360.00m, 730.00m);
                yield return new TestCaseData(9_000.99m, 249m, 9_000.99m, 249m, 9_000.99m, 249m, 360.00m, 730.00m);
                //
                yield return new TestCaseData(9_000.00m, 250m, 9_000.00m, 250m, 9_000.00m, 250m, 380.00m, 770.00m);
                yield return new TestCaseData(9_000.00m, 500m, 9_000.00m, 500m, 9_000.00m, 500m, 380.00m, 770.00m);
                yield return new TestCaseData(9_000.99m, 749m, 9_000.99m, 749m, 9_000.99m, 749m, 380.00m, 770.00m);
                //
                yield return new TestCaseData(9_000.00m, 750m, 9_000.00m, 750m, 9_000.00m, 750m, 400.00m, 810.00m);
                yield return new TestCaseData(10_000.00m, 000m, 10_000.00m, 000m, 10_000.00m, 000m, 400.00m, 810.00m);
                yield return new TestCaseData(10_000.99m, 249m, 10_000.99m, 249m, 10_000.99m, 249m, 400.00m, 810.00m);
                //
                yield return new TestCaseData(10_000.00m, 250m, 10_000.00m, 250m, 10_000.00m, 250m, 420.00m, 850.00m);
                yield return new TestCaseData(10_000.00m, 500m, 10_000.00m, 500m, 10_000.00m, 500m, 420.00m, 850.00m);
                yield return new TestCaseData(10_000.99m, 749m, 10_000.99m, 749m, 10_000.99m, 749m, 420.00m, 850.00m);
                //
                yield return new TestCaseData(10_000.00m, 750m, 10_000.00m, 750m, 10_000.00m, 750m, 440.00m, 890.00m);
                yield return new TestCaseData(11_000.00m, 000m, 11_000.00m, 000m, 11_000.00m, 000m, 440.00m, 890.00m);
                yield return new TestCaseData(11_000.99m, 249m, 11_000.99m, 249m, 11_000.99m, 249m, 440.00m, 890.00m);
                //
                yield return new TestCaseData(11_000.00m, 250m, 11_000.00m, 250m, 11_000.00m, 250m, 460.00m, 930.00m);
                yield return new TestCaseData(11_000.00m, 500m, 11_000.00m, 500m, 11_000.00m, 500m, 460.00m, 930.00m);
                yield return new TestCaseData(11_000.99m, 749m, 11_000.99m, 749m, 11_000.99m, 749m, 460.00m, 930.00m);
                //
                yield return new TestCaseData(11_000.00m, 750m, 11_000.00m, 750m, 11_000.00m, 750m, 480.00m, 970.00m);
                yield return new TestCaseData(12_000.00m, 000m, 12_000.00m, 000m, 12_000.00m, 000m, 480.00m, 970.00m);
                yield return new TestCaseData(12_000.99m, 249m, 12_000.99m, 249m, 12_000.99m, 249m, 480.00m, 970.00m);
                //
                yield return new TestCaseData(12_000.00m, 250m, 12_000.00m, 250m, 12_000.00m, 250m, 500.00m, 1_010.00m);
                yield return new TestCaseData(12_000.00m, 500m, 12_000.00m, 500m, 12_000.00m, 500m, 500.00m, 1_010.00m);
                yield return new TestCaseData(12_000.99m, 749m, 12_000.99m, 749m, 12_000.99m, 749m, 500.00m, 1_010.00m);
                //
                yield return new TestCaseData(12_000.00m, 750m, 12_000.00m, 750m, 12_000.00m, 750m, 520.00m, 1_050.00m);
                yield return new TestCaseData(13_000.00m, 000m, 13_000.00m, 000m, 13_000.00m, 000m, 520.00m, 1_050.00m);
                yield return new TestCaseData(13_000.99m, 249m, 13_000.99m, 249m, 13_000.99m, 249m, 520.00m, 1_050.00m);
                //
                yield return new TestCaseData(13_000.00m, 250m, 13_000.00m, 250m, 13_000.00m, 250m, 540.00m, 1_090.00m);
                yield return new TestCaseData(13_000.00m, 500m, 13_000.00m, 500m, 13_000.00m, 500m, 540.00m, 1_090.00m);
                yield return new TestCaseData(13_000.99m, 749m, 13_000.99m, 749m, 13_000.99m, 749m, 540.00m, 1_090.00m);
                //
                yield return new TestCaseData(13_000.00m, 750m, 13_000.00m, 750m, 13_000.00m, 750m, 560.00m, 1_130.00m);
                yield return new TestCaseData(14_000.00m, 000m, 14_000.00m, 000m, 14_000.00m, 000m, 560.00m, 1_130.00m);
                yield return new TestCaseData(14_000.99m, 249m, 14_000.99m, 249m, 14_000.99m, 249m, 560.00m, 1_130.00m);
                //
                yield return new TestCaseData(14_000.00m, 250m, 14_000.00m, 250m, 14_000.00m, 250m, 580.00m, 1_170.00m);
                yield return new TestCaseData(14_000.00m, 500m, 14_000.00m, 500m, 14_000.00m, 500m, 580.00m, 1_170.00m);
                yield return new TestCaseData(14_000.99m, 749m, 14_000.99m, 749m, 14_000.99m, 749m, 580.00m, 1_170.00m);
                //
                yield return new TestCaseData(14_000.00m, 750m, 14_000.00m, 750m, 14_000.00m, 750m, 600.00m, 1_230.00m);
                yield return new TestCaseData(15_000.00m, 000m, 15_000.00m, 000m, 15_000.00m, 000m, 600.00m, 1_230.00m);
                yield return new TestCaseData(15_000.99m, 249m, 15_000.99m, 249m, 15_000.99m, 249m, 600.00m, 1_230.00m);
                //
                yield return new TestCaseData(15_000.00m, 250m, 15_000.00m, 250m, 15_000.00m, 250m, 620.00m, 1_270.00m);
                yield return new TestCaseData(15_000.00m, 500m, 15_000.00m, 500m, 15_000.00m, 500m, 620.00m, 1_270.00m);
                yield return new TestCaseData(15_000.99m, 749m, 15_000.99m, 749m, 15_000.99m, 749m, 620.00m, 1_270.00m);
                //
                yield return new TestCaseData(15_000.00m, 750m, 15_000.00m, 750m, 15_000.00m, 750m, 640.00m, 1_310.00m);
                yield return new TestCaseData(16_000.00m, 000m, 16_000.00m, 000m, 16_000.00m, 000m, 640.00m, 1_310.00m);
                yield return new TestCaseData(16_000.99m, 249m, 16_000.99m, 249m, 16_000.99m, 249m, 640.00m, 1_310.00m);
                //
                yield return new TestCaseData(16_000.00m, 250m, 16_000.00m, 250m, 16_000.00m, 250m, 660.00m, 1_350.00m);
                yield return new TestCaseData(16_000.00m, 500m, 16_000.00m, 500m, 16_000.00m, 500m, 660.00m, 1_350.00m);
                yield return new TestCaseData(16_000.99m, 749m, 16_000.99m, 749m, 16_000.99m, 749m, 660.00m, 1_350.00m);
                //
                yield return new TestCaseData(16_000.00m, 750m, 16_000.00m, 750m, 16_000.00m, 750m, 680.00m, 1_390.00m);
                yield return new TestCaseData(17_000.00m, 000m, 17_000.00m, 000m, 17_000.00m, 000m, 680.00m, 1_390.00m);
                yield return new TestCaseData(17_000.99m, 249m, 17_000.99m, 249m, 17_000.99m, 249m, 680.00m, 1_390.00m);
                //
                yield return new TestCaseData(17_000.00m, 250m, 17_000.00m, 250m, 17_000.00m, 250m, 700.00m, 1_430.00m);
                yield return new TestCaseData(17_000.00m, 500m, 17_000.00m, 500m, 17_000.00m, 500m, 700.00m, 1_430.00m);
                yield return new TestCaseData(17_000.99m, 749m, 17_000.99m, 749m, 17_000.99m, 749m, 700.00m, 1_430.00m);
                //
                yield return new TestCaseData(17_000.00m, 750m, 17_000.00m, 750m, 17_000.00m, 750m, 720.00m, 1_470.00m);
                yield return new TestCaseData(18_000.00m, 000m, 18_000.00m, 000m, 18_000.00m, 000m, 720.00m, 1_470.00m);
                yield return new TestCaseData(18_000.99m, 249m, 18_000.99m, 249m, 18_000.99m, 249m, 720.00m, 1_470.00m);
                //
                yield return new TestCaseData(18_000.00m, 250m, 18_000.00m, 250m, 18_000.00m, 250m, 740.00m, 1_510.00m);
                yield return new TestCaseData(18_000.00m, 500m, 18_000.00m, 500m, 18_000.00m, 500m, 740.00m, 1_510.00m);
                yield return new TestCaseData(18_000.99m, 749m, 18_000.99m, 749m, 18_000.99m, 749m, 740.00m, 1_510.00m);
                //
                yield return new TestCaseData(18_000.00m, 750m, 18_000.00m, 750m, 18_000.00m, 750m, 760.00m, 1_550.00m);
                yield return new TestCaseData(19_000.00m, 000m, 19_000.00m, 000m, 19_000.00m, 000m, 760.00m, 1_550.00m);
                yield return new TestCaseData(19_000.99m, 249m, 19_000.99m, 249m, 19_000.99m, 249m, 760.00m, 1_550.00m);
                //
                yield return new TestCaseData(19_000.00m, 250m, 19_000.00m, 250m, 19_000.00m, 250m, 780.00m, 1_590.00m);
                yield return new TestCaseData(19_000.00m, 500m, 19_000.00m, 500m, 19_000.00m, 500m, 780.00m, 1_590.00m);
                yield return new TestCaseData(19_000.99m, 749m, 19_000.99m, 749m, 19_000.99m, 749m, 780.00m, 1_590.00m);
                //
                yield return new TestCaseData(19_000.00m, 750m, 19_000.00m, 750m, 19_000.00m, 750m, 800.00m, 1_630.00m);
                yield return new TestCaseData(59_000.00m, 875m, 59_000.00m, 875m, 59_000.00m, 875m, 800.00m, 1_630.00m);
                yield return new TestCaseData(999_000.00m, 999m, 999_000.00m, 999m, 999_000.00m, 999m, 800.00m, 1_630.00m);
            }
        }
    }
}
