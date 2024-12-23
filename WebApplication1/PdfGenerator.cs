using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System.Text;

namespace WebApplication1
{
    public class PdfGenerator
    {
        public static byte[] GenerateCertificate(string studentName)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var document = new PdfDocument();
            var page = document.AddPage();
            page.Orientation = PdfSharp.PageOrientation.Landscape;
            var gfx = XGraphics.FromPdfPage(page);

            // Modern color scheme
            var primary = XColor.FromArgb(63, 81, 181);    // Indigo
            var accent = XColor.FromArgb(233, 30, 99);     // Pink
            var dark = XColor.FromArgb(48, 48, 48);        // Dark Grey
            var light = XColor.FromArgb(250, 250, 250);    // Light Grey

            // Page setup
            double pageWidth = page.Width.Point;
            double pageHeight = page.Height.Point;
            double margin = 50;

            // Background with gradient
            var rect = new XRect(0, 0, pageWidth, pageHeight);
            var gradient = new XLinearGradientBrush(rect, XColor.FromArgb(245, 245, 245), XColor.FromArgb(235, 235, 235), 0);
            gfx.DrawRectangle(gradient, rect);

            // Add geometric patterns
            DrawGeometricPattern(gfx, pageWidth, pageHeight, primary);

            // Modern border
            var borderRect = new XRect(margin, margin, pageWidth - (2 * margin), pageHeight - (2 * margin));
            gfx.DrawRectangle(new XPen(primary, 2), borderRect);

            // Add certificate seal
            DrawModernSeal(gfx, pageWidth - margin - 80, pageHeight - margin - 80, 60, primary, accent);

            // Fonts
            var titleFont = new XFont("Arial", 42, XFontStyleEx.Bold);
            var subtitleFont = new XFont("Arial", 24, XFontStyleEx.Regular);
            var nameFont = new XFont("Arial", 38, XFontStyleEx.Bold);
            var bodyFont = new XFont("Arial", 16, XFontStyleEx.Regular);

            // Content positioning
            double centerY = margin + 120;

            // Title with gradient effect
            var title = "CERTIFICATE OF COMPLETION";
            var titleSize = gfx.MeasureString(title, titleFont);
            var titleRect = new XRect((pageWidth - titleSize.Width) / 2, centerY, titleSize.Width, titleSize.Height);
            
            // Draw title background
            var titleGradient = new XLinearGradientBrush(new XPoint(titleRect.Left, titleRect.Top), new XPoint(titleRect.Right, titleRect.Bottom), primary, accent);
            gfx.DrawString(title, titleFont, titleGradient, new XPoint((pageWidth - titleSize.Width) / 2, centerY));

            // Badge or achievement icon
            DrawAchievementBadge(gfx, pageWidth / 2 - 30, centerY + 40, 60, primary, accent);

            // Certificate text
            centerY += 120;
            DrawCenteredText(gfx, "This is to certify that", subtitleFont, dark, pageWidth / 2, centerY);

            // Student name with underline effect
            centerY += 60;
            var nameRect = new XRect(margin + 100, centerY - 10, pageWidth - (2 * margin) - 200, 60);
            gfx.DrawString(studentName, nameFont, XBrushes.Black, nameRect, XStringFormats.Center);
            
            // Decorative underline
            var underlineGradient = new XLinearGradientBrush(
                new XPoint(nameRect.Left, nameRect.Bottom + 5),
                new XPoint(nameRect.Right, nameRect.Bottom + 5),
                primary, accent);
            gfx.DrawLine(new XPen(primary, 3), 
                nameRect.Left, nameRect.Bottom + 5,
                nameRect.Right, nameRect.Bottom + 5);

            centerY += 80;
            DrawCenteredText(gfx, "has successfully completed the course", bodyFont, dark, pageWidth / 2, centerY);
            
            


            // Date and signatures
            double signatureY = pageHeight - margin - 100;
            DrawSignature("Course Director", gfx, pageWidth / 3, signatureY, dark);
            DrawSignature("Academic Advisor", gfx, (pageWidth * 2) / 3, signatureY, dark);

            // Current date
            var date = DateTime.Now.ToString("MMMM dd, yyyy");
            DrawCenteredText(gfx, date, bodyFont, dark, pageWidth / 2, signatureY + 50);

            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }

        private static void DrawCornerDecoration(XGraphics gfx, double x, double y, XColor color, bool flipX = false, bool flipY = false)
        {
            var size = 20;
            var pen = new XPen(color, 2);
            
            double x1 = flipX ? x - size : x;
            double x2 = flipX ? x : x + size;
            double y1 = flipY ? y - size : y;
            double y2 = flipY ? y : y + size;

            gfx.DrawLine(pen, x1, y, x2, y);
            gfx.DrawLine(pen, x, y1, x, y2);
        }

        private static void DrawSignatureLine(XGraphics gfx, double x, double y, string title)
        {
            var lineWidth = 200;
            var pen = new XPen(XColors.Black, 0.5);
            var font = new XFont("Arial", 10, XFontStyleEx.Regular);

            gfx.DrawLine(pen, x, y, x + lineWidth, y);
            gfx.DrawString(title, font, XBrushes.Black, x + (lineWidth / 2) - 40, y + 20);
        }

        private static void DrawCenteredText(XGraphics gfx, string text, XFont font, XColor color, double x, double y)
        {
            var size = gfx.MeasureString(text, font);
            gfx.DrawString(text, font, new XSolidBrush(color), x - (size.Width / 2), y);
        }

        private static void DrawAngularLogo(XGraphics gfx, double x, double y, double size)
        {
            // Shield outline
            var points = new XPoint[]
            {
                new XPoint(x + size/2, y),              // Top
                new XPoint(x, y + size * 0.8),          // Bottom left
                new XPoint(x + size/6, y + size),       // Left indent
                new XPoint(x + size/2, y + size * 0.9), // Bottom middle
                new XPoint(x + size*5/6, y + size),     // Right indent
                new XPoint(x + size, y + size * 0.8),   // Bottom right
            };

            // Draw shield
            var path = new XGraphicsPath();
            path.AddPolygon(points);
            gfx.DrawPath(new XPen(XColors.DarkRed, 2), new XSolidBrush(XColor.FromArgb(221, 0, 49)), path);

            // Inner design (simplified Angular logo)
            var innerPoints = new XPoint[]
            {
                new XPoint(x + size/2, y + size * 0.2),
                new XPoint(x + size * 0.3, y + size * 0.7),
                new XPoint(x + size * 0.7, y + size * 0.7)
            };

            path = new XGraphicsPath();
            path.AddPolygon(innerPoints);
            gfx.DrawPath(new XPen(XColors.White, 2), new XSolidBrush(XColors.White), path);
        }

        private static void DrawGeometricPattern(XGraphics gfx, double width, double height, XColor color)
        {
            var pen = new XPen(XColor.FromArgb(20, color.R, color.G, color.B), 1);
            var random = new Random(123); // Fixed seed for consistent pattern
            
            for (int i = 0; i < 50; i++)
            {
                double x = random.NextDouble() * width;
                double y = random.NextDouble() * height;
                double size = random.NextDouble() * 30 + 10;
                
                if (i % 2 == 0)
                    gfx.DrawEllipse(pen, x, y, size, size);
                else
                    gfx.DrawRectangle(pen, x, y, size, size);
            }
        }

        private static void DrawModernSeal(XGraphics gfx, double x, double y, double size, XColor primary, XColor accent)
        {
            var outerCircle = new XPen(primary, 3);
            var innerCircle = new XPen(accent, 2);
            
            gfx.DrawEllipse(outerCircle, x - size/2, y - size/2, size, size);
            gfx.DrawEllipse(innerCircle, x - size/3, y - size/3, size*2/3, size*2/3);
        }

        private static void DrawSignature(string title, XGraphics gfx, double x, double y, XColor color)
        {
            var font = new XFont("Arial", 12, XFontStyleEx.Regular);
            var pen = new XPen(color, 0.5);
            
            gfx.DrawLine(pen, x - 100, y, x + 100, y);
            gfx.DrawString(title, font, new XSolidBrush(color), x - 50, y + 20, XStringFormats.Center);
        }

        private static void DrawAchievementBadge(XGraphics gfx, double x, double y, double size, XColor primary, XColor accent)
        {
            // Draw a modern achievement badge
            var points = new XPoint[]
            {
                new XPoint(x + size/2, y),
                new XPoint(x + size, y + size/2),
                new XPoint(x + size/2, y + size),
                new XPoint(x, y + size/2),
            };

            var path = new XGraphicsPath();
            path.AddPolygon(points);
            
            var badgeGradient = new XLinearGradientBrush(
                new XPoint(x, y),
                new XPoint(x + size, y + size),
                primary, accent);
            
            gfx.DrawPath(new XPen(primary, 2), badgeGradient, path);
        }
    }
}
