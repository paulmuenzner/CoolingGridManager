using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolingGridManager.Models
{
    public class TicketModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "Reported by is required.")]
        public string ReportedBy { get; set; }

        [Required(ErrorMessage = "Responsible person is required.")]
        public string Responsible { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Status history is required.")]
        public List<StatusChange> StatusHistory { get; set; } = new List<StatusChange>();

        public TicketModel()
        {
            Title = string.Empty;
            Description = string.Empty;
            Category = string.Empty;
            Priority = string.Empty;
            ReportedBy = string.Empty;
            Responsible = string.Empty;
            Status = string.Empty;
        }
    }

    public class StatusChange
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusChangeId { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Changed date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime ChangedDate { get; set; }

        public StatusChange()
        {
            Status = string.Empty;
            ChangedDate = DateTime.MinValue;
        }
    }
}
