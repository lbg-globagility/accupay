using AccuPay.Data.Entities;
using AccuPay.Web.Files.Models;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace AccuPay.Web.Files.Services
{
    public class GenerateDefaultImageService
    {
        private const string FontLocation = "./Fonts/Roboto-Bold.ttf";

        private const int FontSize = 128;

        private const int Width = 300;

        private const int Height = 300;

        public VirtualFile Create(Employee employee)
        {
            var virtualFile = CreateImage(employee);
            return virtualFile;
        }

        private string GetInitials(Employee employee)
        {
            // If the user has a name, use the user's intials,
            // otherwise use the 1st and 2nd letter of the email address
            if (employee.FirstName != null && employee.LastName != null)
            {
                return employee.FirstName[0].ToString().ToUpper() + employee.LastName[0].ToString().ToUpper();
            }
            else
            {
                return employee.EmailAddress[0].ToString().ToUpper() + employee.EmailAddress[1].ToString().ToUpper();
            }
        }

        private VirtualFile CreateImage(Employee employee)
        {
            var initials = GetInitials(employee);
            var colorCombination = PickColorCombination(employee);

            using var bitmap = CreateBitmap(initials, colorCombination);
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;

            var randomId = DateTime.UtcNow.ToFileTime();
            var filename = $"{employee.RowID.Value.ToString()}-{randomId}.jpg";

            return new VirtualFile(filename, stream);
        }

        private Bitmap CreateBitmap(string displayName, ColorCombination colorCombination)
        {
            var bitmap = new Bitmap(250, 250);
            var backgroundColor = ColorTranslator.FromHtml(colorCombination.Background);
            var foregroundColor = ColorTranslator.FromHtml(colorCombination.Foreground);

            var fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(FontLocation);

            var fontFamily = fontCollection.Families[0];

            // Create the Font object for the image text drawing.
            using var font = new Font(fontFamily, FontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            using var brush = new SolidBrush(foregroundColor);

            // Create a graphics object to measure the text's width and height.
            var graphics = Graphics.FromImage(bitmap);
            // This is where the bitmap size is determined.
            var size = graphics.MeasureString(displayName, font);
            var intWidth = (int)size.Width;
            var intHeight = (int)size.Height;

            // Create the bitmap again with the correct size for the text and font.
            bitmap = new Bitmap(bitmap, new Size(Width, Height));

            // Add the colors to the new bitmap.
            graphics = Graphics.FromImage(bitmap);

            // Set Background color
            graphics.Clear(backgroundColor);

            graphics.SmoothingMode = SmoothingMode.HighQuality;

            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.DrawString(
                displayName,
                font,
                brush,
                (Width - intWidth) / 2,
                ((Height - intHeight) / 2) + ((intHeight - FontSize) / 4),
                StringFormat.GenericDefault);

            graphics.Flush();

            return bitmap;
        }

        private ColorCombination PickColorCombination(Employee employee)
        {
            var colorCombinations = ColorCombination.ColorCombinations();
            var code = GetCode(employee.EmailAddress, colorCombinations.Count);
            var colorCombination = colorCombinations[code];

            return colorCombination;
        }

        private int GetCode(string text, int totalCombinations)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text ?? string.Empty));

            // Guarantees hash will be positively signed
            Array.Resize(ref hash, hash.Length + 1);

            var number = new BigInteger(hash);
            var code = number % totalCombinations;

            return (int)code;
        }
    }
}
