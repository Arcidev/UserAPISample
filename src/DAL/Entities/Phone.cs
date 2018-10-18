using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Phone
    {
        public Guid UserId { get; set; }

        [Required]
        public string Number { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User Useer { get; set; }
    }
}
