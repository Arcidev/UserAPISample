using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    /// <summary>
    /// Phone DTO
    /// </summary>
    public class PhoneDTO
    {
        /// <summary>
        /// Phone number
        /// </summary>
        [Required]
        public string Number { get; set; }
    }
}
