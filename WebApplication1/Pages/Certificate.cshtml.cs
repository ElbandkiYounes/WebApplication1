using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class CertificateModel : PageModel
    {
        private readonly ILogger<CertificateModel> _logger;

        public CertificateModel(ILogger<CertificateModel> logger)
        {
            _logger = logger;
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Score { get; set; }

        public IActionResult OnGet()
        {
            // Get values from session
            FirstName = HttpContext.Session.GetString("FirstName");
            LastName = HttpContext.Session.GetString("LastName");
            Score = HttpContext.Session.GetInt32("Score") ?? 0;

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                return RedirectToPage("/Quiz");
            }

            return Page();
        }

        public IActionResult OnPostDownload()
        {
            try
            {
                FirstName = HttpContext.Session.GetString("FirstName");
                LastName = HttpContext.Session.GetString("LastName");
                string fullName = $"{FirstName} {LastName}";

                if (string.IsNullOrEmpty(fullName.Trim()))
                {
                    return RedirectToPage("/Quiz");
                }

                byte[] pdfBytes = PdfGenerator.GenerateCertificate(fullName);
                return File(pdfBytes, "application/pdf", $"Certificate_{fullName.Replace(" ", "_")}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating certificate");
                return RedirectToPage("/Error");
            }
        }
    }
}
