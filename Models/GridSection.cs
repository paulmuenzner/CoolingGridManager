using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models.Data
{
    [Table("GridSection")]
    public class GridSection
    {
        [Key]
        public int GridSectionID { get; set; }

        [Required(ErrorMessage = "Section Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Section Name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Section Name can only contain letters, numbers, and spaces.")]
        public string GridSectionName { get; set; }

        [ForeignKey("GridID")]
        public int GridID { get; set; }
        public Grid Grid { get; set; }
        public GridSection()
        {
            GridSectionName = string.Empty;
            Grid = new Grid();
        }
    }
}