using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DAL.Entities
{
    public class Result
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
        public int FileId { get; set; }
        public File File { get; set; }
    }
}
