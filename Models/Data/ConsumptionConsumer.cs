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

        [Required(ErrorMessage = "Start time is required.")]
        public DateTimeOffset DateTimeStart { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTimeOffset DateTimeEnd { get; set; }

        [Required(ErrorMessage = "Consumer ID is required.")]
        [ForeignKey("ConsumerID")]
        public int ConsumerID { get; set; }
        public Consumer Consumer { get; set; }

        public ConsumptionConsumer()
        {
            ConsumptionValue = 0m;
            LogDate = DateTime.Today;
            // Defining the related time period where the value was measured for
            // This maintains the flexibility of the table. The associated data is not tied to a specific time frame.
            DateTimeStart = DateTimeOffset.MinValue;
            DateTimeEnd = DateTimeOffset.MinValue;
            Consumer = new Consumer();
        }
    }
}
