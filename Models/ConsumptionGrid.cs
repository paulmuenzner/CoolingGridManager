using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models.Data
{
    [Table("ConsumptionGrid")]
    public class ConsumptionGrid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Consumption value is required.")]
        public decimal Consumption { get; set; }

        public int GridID { get; set; }

        [ForeignKey("GridID")]
        public Grid Grid { get; set; }

        [Required(ErrorMessage = "Month is required.")]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(2020, 2040, ErrorMessage = "Year must be between 2020 and 2040.")]
        public int Year { get; set; }

        public ConsumptionGrid()
        {
            Consumption = 0m;
            Month = 0;
            Year = 0;
            Grid = new Grid();
        }
    }
}
