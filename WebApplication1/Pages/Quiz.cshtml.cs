using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class QuizModel : PageModel
    {
        [BindProperty]
        public string? FirstName { get; set; }

        [BindProperty]
        public string? LastName { get; set; }

        [BindProperty]
        public string? Q1 { get; set; }

        [BindProperty]
        public string? Q2 { get; set; }

        [BindProperty]
        public string? Q3 { get; set; }

        [BindProperty]
        public string? Q4 { get; set; }

        [BindProperty]
        public string? Q5 { get; set; }

        [BindProperty]
        public string? Q6 { get; set; }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                ModelState.AddModelError("", "Please enter your name");
                return Page();
            }

            if (string.IsNullOrEmpty(Q1) || string.IsNullOrEmpty(Q2) || 
                string.IsNullOrEmpty(Q3) || string.IsNullOrEmpty(Q4) || 
                string.IsNullOrEmpty(Q5) || string.IsNullOrEmpty(Q6))
            {
                ModelState.AddModelError("", "Please answer all questions");
                return Page();
            }

            int score = 0;
            if (Q1 == "1") score++;
            if (Q2 == "1") score++;
            if (Q3 == "1") score++;
            if (Q4 == "1") score++;
            if (Q5 == "1") score++;
            if (Q6 == "1") score++;

            HttpContext.Session.SetString("FirstName", FirstName);
            HttpContext.Session.SetString("LastName", LastName);
            HttpContext.Session.SetInt32("Score", score);

            return RedirectToPage("/Certificate");
        }
    }
}
