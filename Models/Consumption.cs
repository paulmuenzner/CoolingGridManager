using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models
{
    [Table("ConsumptionLog")]
    public class ConsumptionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required(ErrorMessage = "Consumer is required.")]
        public int ConsumerID { get; set; }

        [ForeignKey("ConsumerID")]
        public Consumer? Consumer { get; set; }

        [Required(ErrorMessage = "Consumption value is required.")]
        public decimal ConsumptionValue { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime LogDate { get; set; }

        [Required(ErrorMessage = "Consumption date for day of consumption is required.")]
        public DateTime ConsumptionDate { get; set; }

        public ConsumptionLog()
        {
            Consumer = null;
            ConsumptionValue = 0m;
            LogDate = DateTime.MinValue;
            ConsumptionDate = DateTime.MinValue;
        }
    }
}
