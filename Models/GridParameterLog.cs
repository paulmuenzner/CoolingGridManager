using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models.Data
{
    [Table("CoolingGridParameterLog")]
    public class GridParameterLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }

        [Required(ErrorMessage = "Mass flow rate is required.")]
        public decimal MassFlowRate { get; set; }

        [Required(ErrorMessage = "Specific heat capacity is required.")]
        public decimal SpecificHeatCapacity { get; set; }

        [Required(ErrorMessage = "Temperature inlet is required.")]
        public decimal MeanTemperatureIn { get; set; }

        [Required(ErrorMessage = "Temperature outlet is required.")]
        public decimal MeanTemperatureOut { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public DateTime DateTimeStart { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTime DateTimeEnd { get; set; }

        [ForeignKey("GridID")]
        public int GridID { get; set; } // Foreign key property

        public Grid Grid { get; set; }


        public GridParameterLog()
        {
            MassFlowRate = 0;
            SpecificHeatCapacity = 0;
            MeanTemperatureIn = 0m;
            MeanTemperatureOut = 0m;
            // Defining the related time period where the value was measured for
            // The associated log data is not tied to a specific time frame. This maintains the flexibility of the table. 
            DateTimeStart = DateTime.MinValue;
            DateTimeEnd = DateTime.MinValue;
            Grid = new Grid();
        }
    }
}
