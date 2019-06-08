using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    /// <summary>
    /// Phone entity
    /// </summary>
    public class Phone
    {
        /// <summary>
        /// User Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        [Required, StringLength(50)]
        public string Number { get; set; }

        /// <summary>
        /// Reference to user to whom the phone belongs
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
