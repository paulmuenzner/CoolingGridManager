using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models
{
    [Table("ConsumptionLog")]
    public class Consumption
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


        public Consumption()
        {
            Consumer = null;
            ConsumptionValue = 0m;
            LogDate = DateTime.MinValue;
        }
    }
}
