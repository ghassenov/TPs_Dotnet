using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesCrudApp.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string EntityName { get; set; }

        [Required]
        public int EntityId { get; set; }

        [Required]
        [StringLength(50)]
        public string ChangeType { get; set; } // Added, Modified, Deleted

        [Required]
        public DateTime ChangeDate { get; set; } = DateTime.Now;

        public string OldValues { get; set; }

        public string NewValues { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }
    }
}
