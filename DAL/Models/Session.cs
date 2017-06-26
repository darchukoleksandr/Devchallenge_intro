using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Model that represents session
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Date of session
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        /// <summary>
        /// Name of session
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
