using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CoolingGridManager.Models.Data
{
    [Table("Grid")]
    public class Grid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GridID { get; set; }

        [Required(ErrorMessage = "Grid Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Grid Name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Grid Name can only contain letters, numbers, and spaces.")]
        public string GridName { get; set; }

        public ICollection<GridSection> GridSection { get; set; }
        public ICollection<GridParameterLog> GridParameterLog { get; set; }
        public ICollection<GridEnergyTransfer> GridEnergyTransfer { get; set; }
        public ICollection<GridEfficiency> GridEfficiency { get; set; }

        public Grid()
        {
            GridName = string.Empty;
            GridSection = new List<GridSection>();
            GridParameterLog = new List<GridParameterLog>();
            GridEnergyTransfer = new List<GridEnergyTransfer>();
            GridEfficiency = new List<GridEfficiency>();
        }
    }
}
