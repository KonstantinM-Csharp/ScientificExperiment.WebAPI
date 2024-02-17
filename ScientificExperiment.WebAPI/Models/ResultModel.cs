namespace ScientificExperiment.WebAPI.Models
{
    public class ResultModel
    {
        public string FileName { get; set; }
        public TimeSpan FirstTime { get; set; }
        public TimeSpan LastTime { get; set; }
        public int MaxTimeWork { get; set; }
        public int MinTimeWork { get; set; }
        public double AverageTimeWork { get; set; }
        public double AverageIndicator { get; set; }
        public double MedianByIndicator { get; set; }
        public double MaxIndicator { get; set; }
        public double MinIndicator { get; set; }
        public int CountOfExperiments { get; set; }
    }
}
