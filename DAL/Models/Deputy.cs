using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Model that represent a deputy
    /// </summary>
    public class Deputy
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }
        /// <summary>
        /// Full name of deputy
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Identifier for party of which deputy consist of (1-37)
        /// </summary>
        public short Party { get; set; }
    }
}
