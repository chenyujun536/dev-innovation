using _3PtsReport.Models;

namespace _3PtsReport.Services
{
    public static class RecordService
    {
        public static async Task<List<Record>> Parse(string file)
        {
            var records = new List<Record>();

            var lines = await System.IO.File.ReadAllLinesAsync(file);

            foreach (var line in lines)
            {
                Record record = TryParseOne(line);
                records.Add(record);
            }

            return records;
        }

        public static Record Summary(IList<Record> records)
        {
            Record record = new Record();
            record.ExerciseDate = DateTime.Now;
            record.Hits = records.SelectMany(x => x.Hits).ToList();
            record.TryCount = record.Hits.Count * 10;
            record.HitCount = record.Hits.Sum();
            return record;
        }

        public static Record TryParseOne(string line)
        {
            Record record = new Record();

            try
            {
                var words = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                record.ExerciseDate = DateTime.Parse(words[0]);

                record.Hits = words.Skip(1).Select(x => Convert.ToInt32(x)).ToList();
                record.TryCount = record.Hits.Count* 10;
                record.HitCount = record.Hits.Sum();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return record;
        }

        public static string EncodeToHtml(this Record record)
        {
            return (record.ExerciseDate.ToString("yyyy.MM.dd") + " " +
                    string.Join(' ', record.Hits.Select(x => x.ToString()).ToArray()))
                .Replace(" ", "BKSPC");
        }

        public static string DecodeToCSharp(this string recordInHtml)
        {
            return recordInHtml.Replace("BKSPC", " ");
        }
    }
}
