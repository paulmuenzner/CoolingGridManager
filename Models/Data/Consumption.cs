using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models.Data
{
    [Table("ConsumptionConsumer")]
    public class ConsumptionConsumer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required(ErrorMessage = "Consumption value is required.")]
        public decimal ConsumptionValue { get; set; }

        [Required(ErrorMessage = "Log Date is required.")]
        public DateTime LogDate { get; set; }

        [Required(ErrorMessage = "Consumption date is required.")]
        public DateTime ConsumptionDate { get; set; }

        [Required(ErrorMessage = "Consumer ID is required.")]
        [ForeignKey("ConsumerID")]
        public int ConsumerID { get; set; }
        public Consumer Consumer { get; set; }

        public ConsumptionConsumer()
        {
            ConsumptionValue = 0m;
            LogDate = DateTime.Today;
            ConsumptionDate = DateTime.Today;
            Consumer = new Consumer();
        }
    }
}
