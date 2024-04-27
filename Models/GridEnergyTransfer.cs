using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CoolingGridManager.Models.Data
{
    [Table("GridEnergyTransfer")]
    public class GridEnergyTransfer : GridEnergyTransferBase
    {
        public int GridID { get; set; }

        [ForeignKey("GridID")]
        public Grid Grid { get; set; }

        public GridEnergyTransfer()
        {

            EnergyTransfer = 0m;
            Month = 0;
            Year = 0;
            Grid = new Grid();
        }

        /// <summary>
        /// Grid Energy Transfer in kWh including consumption and losses
        /// </summary>

        // Data Transfer Object specifically for creating new grid entries for energy transfer based on flow, return flow
        public class CreateGridEnergyTransferDto : GridEnergyTransferBase
        {
            [Required(ErrorMessage = "Grid ID is required.")]
            [Display(Name = "Grid ID")]
            public int GridID { get; set; }
            public CreateGridEnergyTransferDto()
            {
                EnergyTransfer = 0m;
                Month = 0;
                Year = 0;
                GridID = 0;
            }
        }

    }
}



public class GridEnergyTransferBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Grid Energy Transfer in kWh
    /// </summary>
    [Required(ErrorMessage = "Energy transfer value is required.")]
    public decimal EnergyTransfer { get; set; }

    [Required(ErrorMessage = "Month is required.")]
    [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
    public int Month { get; set; }

    [Required(ErrorMessage = "Year is required.")]
    [Range(2020, 2040, ErrorMessage = "Year must be between 2020 and 2040.")]
    public int Year { get; set; }

    public GridEnergyTransferBase()
    {
        EnergyTransfer = 0m;
        Month = 0;
        Year = 0;
    }
}