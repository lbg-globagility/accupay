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
    public class GenerateDefaultUserImageService
    {
        private const string FontLocation = "./Fonts/Roboto-Bold.ttf";

        private const int FontSize = 128;

        private const int Width = 300;

        private const int Height = 300;

        public VirtualFile Create(AspNetUser user)
        {
            var virtualFile = CreateImage(user);
            return virtualFile;
        }

        private string GetInitials(AspNetUser user)
        {
            // If the user has a name, use the user's intials,
            // otherwise use the 1st and 2nd letter of the email address
            if (user.FirstName != null && user.LastName != null)
            {
                return user.FirstName[0].ToString().ToUpper() + user.LastName[0].ToString().ToUpper();
            }
            else
            {
                return user.Email[0].ToString().ToUpper() + user.Email[1].ToString().ToUpper();
            }
        }

        private VirtualFile CreateImage(AspNetUser user)
        {
            var initials = GetInitials(user);
            var colorCombination = PickColorCombination(user);

            using var bitmap = CreateBitmap(initials, colorCombination);
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;

            var randomId = DateTime.UtcNow.ToFileTime();
            var filename = $"{user.Id.ToString()}-{randomId}.jpg";

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

        private ColorCombination PickColorCombination(AspNetUser user)
        {
            var colorCombinations = ColorCombination.ColorCombinations();
            var code = GetCode(user.Email, colorCombinations.Count);
            var colorCombination = colorCombinations[code];

            return colorCombination;
        }

        private int GetCode(string text, int totalCombinations)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));

            // Guarantees hash will be positively signed
            Array.Resize(ref hash, hash.Length + 1);

            var number = new BigInteger(hash);
            var code = number % totalCombinations;

            return (int)code;
        }
    }
}
