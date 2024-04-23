using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models.Data
{
    [Table("EfficiencyGrid")]
    public class EfficiencyGrid : EfficiencyGridBase
    {
        public int GridID { get; set; }

        [ForeignKey("GridID")]
        public Grid Grid { get; set; }

        public EfficiencyGrid()
        {
            Efficiency = 0m;
            LossesAbsolute = 0;
            Month = 0;
            Year = 0;
            Grid = new Grid();
        }


        // Data Transfer Object specifically for creating new grid efficiency entries
        public class EfficiencyGridDto : EfficiencyGridBase
        {
            [Required(ErrorMessage = "Grid ID is required.")]
            [Display(Name = "Grid ID")]
            public int GridID { get; set; }
            public EfficiencyGridDto()
            {
                Efficiency = 0m;
                LossesAbsolute = 0;
                Month = 0;
                Year = 0;
                GridID = 0;
            }
        }

    }
}



public class EfficiencyGridBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Efficiency value is required.")]
    public decimal Efficiency { get; set; }

    [Required(ErrorMessage = "Absolute losses required.")]
    public decimal LossesAbsolute { get; set; }

    [Required(ErrorMessage = "Month is required.")]
    [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
    public int Month { get; set; }

    [Required(ErrorMessage = "Year is required.")]
    [Range(2020, 2040, ErrorMessage = "Year must be between 2020 and 2040.")]
    public int Year { get; set; }

    public EfficiencyGridBase()
    {
        Efficiency = 0m;
        LossesAbsolute = 0;
        Month = 0;
        Year = 0;
    }
}