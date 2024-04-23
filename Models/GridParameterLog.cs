using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Logging grid parameter data from external sources/meters
namespace CoolingGridManager.Models.Data
{
    [Table("CoolingGridParameterLog")]
    public class GridParameterLog : GridParameterLogBase
    {
        [ForeignKey("GridID")]
        public int GridID { get; set; }
        public Grid Grid { get; set; }

        public GridParameterLog()
        {
            ElementID = string.Empty;
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


        // Data Transfer Object specifically for creating new grid parameter log entries
        public class CreateGridParameterLogDto : GridParameterLogBase
        {
            [Required(ErrorMessage = "Grid ID is required.")]
            [Display(Name = "Grid ID")]
            public int GridID { get; set; }
            public CreateGridParameterLogDto()
            {
                ElementID = string.Empty;
                MassFlowRate = 0;
                SpecificHeatCapacity = 0;
                MeanTemperatureIn = 0m;
                MeanTemperatureOut = 0m;
                DateTimeStart = DateTime.MinValue;
                DateTimeEnd = DateTime.MinValue;
                GridID = 0;
            }
        }
    }
}


public class GridParameterLogBase
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

    public GridParameterLogBase()
    {
        ElementID = string.Empty;
        MassFlowRate = 0;
        SpecificHeatCapacity = 0;
        MeanTemperatureIn = 0m;
        MeanTemperatureOut = 0m;
        DateTimeStart = DateTime.MinValue;
        DateTimeEnd = DateTime.MinValue;
    }
}