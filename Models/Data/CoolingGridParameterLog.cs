using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models.Data
{
    [Table("CoolingGridParameterLog")]
    public class CoolingGridParameterLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }

        [Required(ErrorMessage = "Mass flow rate is required.")]
        public decimal MassFlowRate { get; set; }

        [Required(ErrorMessage = "Specific heat capacity is required.")]
        public decimal SpecificHeatCapacity { get; set; }

        [Required(ErrorMessage = "Temperature inlet is required.")]
        public decimal TemperatureIn { get; set; }

        [Required(ErrorMessage = "Temperature outlet is required.")]
        public decimal TemperatureOut { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTimeOffset StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTimeOffset EndDate { get; set; }


        public int GridID { get; set; } // Foreign key property

        [ForeignKey("GridID")]
        public Grid Grid { get; set; }


        public CoolingGridParameterLog()
        {
            MassFlowRate = 0;
            SpecificHeatCapacity = 0;
            TemperatureIn = 0m;
            TemperatureOut = 0m;
            StartDate = DateTimeOffset.MinValue;
            EndDate = DateTimeOffset.MinValue;
            Grid = new Grid();
        }
    }
}