using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models.Data
{
    [Table("Consumer")]
    public class Consumer : ConsumerBase
    {
        [ForeignKey("GridSectionID")]
        public int GridSectionID { get; set; }
        public GridSection GridSection { get; set; }

        public Consumer()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            CompanyName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            MonthlyBaseFee = 0m; // monthly base fee for grid connection as per contract
            UnitPrice = 0m; // price for kwh cooling energy as per contract
            GridSection = new GridSection();
        }
    }

    // Data Transfer Object specifically for creating new consumers
    public class CreateConsumerDto : ConsumerBase
    {
        [Required(ErrorMessage = "Grid section ID is required.")]
        [Display(Name = "Grid Section ID")]
        public int GridSectionID { get; set; }
        public CreateConsumerDto()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            CompanyName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            MonthlyBaseFee = 0m;
            UnitPrice = 0m;
            GridSectionID = 0;
        }
    }

}



public class ConsumerBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConsumerID { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 50 characters.")]
    [Display(Name = "Company name")]
    public string CompanyName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(15, MinimumLength = 8, ErrorMessage = "Phone number must be between 8 and 15 characters.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Monthly base fee as per contract is required.")]
    public decimal MonthlyBaseFee { get; set; }

    [Required(ErrorMessage = "Unit price as per contract is required.")]
    public decimal UnitPrice { get; set; }

    public ConsumerBase()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        CompanyName = string.Empty;
        Email = string.Empty;
        Phone = string.Empty;
        MonthlyBaseFee = 0m; // monthly base fee for grid connection as per contract
        UnitPrice = 0m; // price for kwh cooling energy as per contract
    }
}