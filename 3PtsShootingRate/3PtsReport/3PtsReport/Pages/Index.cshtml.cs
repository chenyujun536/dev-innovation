using _3PtsReport.Models;
using _3PtsReport.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace _3PtsReport.Pages
{
    public class IndexModel : PageModel
    {
        private IHostingEnvironment _environment;
        
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public List<Record> Records { get; set; } = new();

        public void OnGet()
        {
        }

        public Record Summary { get; set; } = new();
        public string FileName { get; set; } = "";
        [BindProperty]
        public IFormFile UploadFile { get; set; }
        public async Task OnPostAsync()
        {
            if (string.IsNullOrEmpty(UploadFile?.FileName))
                return;

            var file = Path.Combine(_environment.ContentRootPath, "uploads", UploadFile.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await UploadFile.CopyToAsync(fileStream);
            }

            Records = await RecordService.Parse(file);
            Summary = RecordService.Summary(Records);
            FileName = UploadFile.FileName.Remove(UploadFile.FileName.IndexOf('.'));
        }

        public IActionResult OnPostOneItemDetails(string recordText)
        {
            return RedirectToPage("/Details", "RecordDetails", new {recordData=recordText});
        }
    }
}