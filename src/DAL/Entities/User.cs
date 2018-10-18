using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class User : IEntity<Guid>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string PasswordSalt { get; set; }

        [Required, StringLength(100)]
        public string PasswordHash { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public DateTime LastLoginOn { get; set; }

        public virtual ICollection<Phone> Telephones { get; set; }
    }
}
