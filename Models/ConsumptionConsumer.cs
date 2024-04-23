using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CoolingGridManager.Models.Data
{
    [Table("ConsumptionConsumer")]
    public class ConsumptionConsumer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }

        /// <summary>
        /// Data integrity measure
        /// Associate each entry/element in database with element of sender (eg. measured meter station) to avoid duplication
        /// </summary>
        [Required(ErrorMessage = "Element ID is required.")]
        public string ElementID { get; set; }

        [Required(ErrorMessage = "Consumption value is required.")]
        public decimal ConsumptionValue { get; set; }

        [Required(ErrorMessage = "Log Date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime LogDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.DateTime)]
        public DateTime DateTimeStart { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTime DateTimeEnd { get; set; }

        [Required(ErrorMessage = "Consumer ID is required.")]
        [ForeignKey("ConsumerID")]
        public int ConsumerID { get; set; }
        public Consumer Consumer { get; set; }

        public ConsumptionConsumer()
        {
            ElementID = string.Empty;
            ConsumptionValue = 0m;
            LogDate = DateTime.MinValue;
            // Defining the related time period where the value was measured for
            // This maintains the flexibility of the table. The associated data is not tied to a specific time frame.
            DateTimeStart = DateTime.MinValue;
            DateTimeEnd = DateTime.MinValue;
            Consumer = new Consumer();
        }
    }
}
