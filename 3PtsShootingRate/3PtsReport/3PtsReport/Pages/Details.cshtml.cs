using _3PtsReport.Models;
using _3PtsReport.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _3PtsReport.Pages
{
    public class DetailsModel : PageModel
    {

        public Record OneRecord { get; set; } = new();
        public Dictionary<string, int> DataDistribution { get; set; } = new();

        public void OnGetRecordDetails(string recordData)
        {
            OneRecord = RecordService.TryParseOne(recordData.DecodeToCSharp());
            DataDistribution = OneRecord.Hits.GroupBy(x => x)
                .OrderBy(x=>x.Key)
                .ToDictionary(x=>x.Key.ToString(), x=>x.Count());
        }
    }
}
