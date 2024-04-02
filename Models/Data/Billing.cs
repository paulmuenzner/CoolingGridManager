// MonthlyBilling.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models.Data
{
    [Table("MonthlyBilling")]
    public class Billing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillingId { get; set; }

        [Required(ErrorMessage = "Consumer is required.")]
        public int ConsumerID { get; set; }

        [ForeignKey("ConsumerID")]
        public Consumer? Consumer { get; set; }


        [Required(ErrorMessage = "Month is required.")]
        public int BillingMonth { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public int BillingYear { get; set; }

        [Required(ErrorMessage = "Total Consumption is required.")]
        public decimal TotalConsumption { get; set; }

        [Required(ErrorMessage = "Billing Amount is required.")]
        public decimal BillingAmount { get; set; }

        // Additional attributes...

        public Billing()
        {
            Consumer = null;
            BillingMonth = 0;
            BillingYear = 0;
            TotalConsumption = 0m;
            BillingAmount = 0m;
        }

    }
}
