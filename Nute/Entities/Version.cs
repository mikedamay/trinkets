using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Nute.Entities
{
    public class Version
    {
        public long Id { get; private set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; private set; }
        [Column(TypeName = "date")]
        public DateTime? EndDate { get; private set; }
        public NutrientProfile NutrientProfile { get; private set; }

        public Version()
        {
            
        }

        public Version(DateTime startDate, DateTime? endDate = null, long id = 0)
        {
            Debug.Assert(startDate.TimeOfDay == TimeSpan.Zero);
            Debug.Assert(!endDate.HasValue || endDate?.TimeOfDay == TimeSpan.Zero);
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}