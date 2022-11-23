using System.ComponentModel.DataAnnotations;

namespace _3PtsReport.Models
{
    public class Record
    {
        [DataType(DataType.Date)]
        public DateTime ExerciseDate { get; set; }

        public List<int> Hits { get; set; } = new();

        public int TryCount { get; set; }
        public int HitCount { get; set; }

        public float HitRate => (float) (TryCount > 0 ?  (100.0 * HitCount) / TryCount : 0);

        public string RawText { get; set; }
    }
}
