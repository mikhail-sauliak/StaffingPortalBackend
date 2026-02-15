using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffingPortalBackend.Models
{
    public class TitleHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public string OldTitle { get; set; }
        public string NewTitle { get; set; }
        public DateTime DateChanged { get; set; }

        [Column(TypeName = "text")]
        public string Justification { get; set; } // Why the title was changed
    }
}
