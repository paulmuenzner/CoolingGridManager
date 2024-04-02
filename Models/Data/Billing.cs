using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models.Data
{
    [Table("Bills")]
    public class Billing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillingId { get; set; }

        [Required(ErrorMessage = "Consumer is required.")]
        [ForeignKey("ConsumerID")]
        public int ConsumerID { get; set; }
        public Consumer? Consumer { get; set; }


        [Required(ErrorMessage = "Month is required.")]
        public int BillingMonth { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public int BillingYear { get; set; }

        [Required(ErrorMessage = "Total Consumption is required.")]
        public decimal TotalConsumption { get; set; }

        [Required(ErrorMessage = "Provide information if bill is paid.")]
        public bool IsPaid { get; set; }

        [Required(ErrorMessage = "Billing Amount is required.")]
        public decimal BillingAmount { get; set; }


        public Billing()
        {
            Consumer = null;
            BillingMonth = 0;
            BillingYear = 0;
            TotalConsumption = 0m;
            IsPaid = false;
            BillingAmount = 0m;
        }

    }
}
