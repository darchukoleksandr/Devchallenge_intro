using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Model that represents a vote of each deputy
    /// </summary>
    public class DeputyVote
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Type of vote (Не голосував|За|Відсутній|Проти|Утримався)
        /// </summary>
        public string VoteType { get; set; }
        /// <summary>
        /// Foreign key to Deputy model
        /// </summary>
        public short DeputyId { get; set; }
        /// <summary>
        /// Navigation property to Deputy model
        /// </summary>
        [ForeignKey("DeputyId")]
        public Deputy Deputy { get; set; }
        /// <summary>
        /// Foreign key to Voting model
        /// </summary>
        public int VotingId { get; set; }
        /// <summary>
        /// Navigation property to Voting model
        /// </summary>
        [ForeignKey("VotingId")]
        public Voting Voting { get; set; }
    }
}
