using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class ChapterModel : PageModel
    {
        public string? ChapterTitle { get; set; }
        public string? VideoUrl { get; set; }
        public string? ChapterDescription { get; set; }
        public int? PreviousChapterId { get; set; }
        public int? NextChapterId { get; set; }

        public void OnGet(int chapterId)
        {
            var chapters = new[]
            {
                new { 
                    Id = 1, 
                    Title = "Angular Core",
                    Video = "/videos/chapter1.mp4",
                    Description = "Learn about core Angular concepts including databinding, routing, and directives."
                },
                new { 
                    Id = 2, 
                    Title = "Advanced Angular",
                    Video = "/videos/chapter2.mp4",
                    Description = "Dive into advanced topics like RxJS, components, and Services."
                },
                new { 
                    Id = 3, 
                    Title = "Summary",
                    Video = "/videos/outro.mp4",
                    Description = "Review key concepts and prepare for the final quiz to earn your certificate!"
                }
            };

            // Find the chapter by ID
            var currentChapter = chapters.FirstOrDefault(c => c.Id == chapterId);
            if (currentChapter == null)
            {
                // If chapter not found, redirect to an error page or show a 404
                RedirectToPage("/Error");
                return;
            }

            // Set current chapter data
            ChapterTitle = currentChapter.Title;
            VideoUrl = currentChapter.Video;
            ChapterDescription = currentChapter.Description;

            // Set navigation links
            PreviousChapterId = chapters.FirstOrDefault(c => c.Id == chapterId - 1)?.Id;
            NextChapterId = chapters.FirstOrDefault(c => c.Id == chapterId + 1)?.Id;
        }
    }
}
