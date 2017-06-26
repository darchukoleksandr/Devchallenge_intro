using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Model that represents a voting of session
    /// </summary>
    public class Voting
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Foreign key to Session model
        /// </summary>
        public int SessionId { get; set; }
        /// <summary>
        /// Navigation property to Session model
        /// </summary>
        [ForeignKey("SessionId")]
        public Session Session { get; set; }
        /// <summary>
        /// Purpose of voting
        /// </summary>
        [Required]
        public string About { get; set; }
    }
}
