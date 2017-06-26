using System.Data.Entity;
using DAL.Models;

namespace DAL
{
    /// <summary>
    /// Entity Framework main database context
    /// </summary>
    public class SessionDbContext : DbContext
    {
        /// <summary>
        /// Constructor for database context
        /// </summary>
        public SessionDbContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SessionDbContext>());

            // Required for DbSet.Add() speedup
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }
        /// <summary>
        /// DbSet for Deputy model
        /// </summary>
        public DbSet<Deputy> Deputies { get; set; }
        /// <summary>
        /// DbSet for Session model
        /// </summary>
        public DbSet<Session> Sessions { get; set; }
        /// <summary>
        /// DbSet for Voting model
        /// </summary>
        public DbSet<Voting> Votings { get; set; }
        /// <summary>
        /// DbSet for DeputyVote model
        /// </summary>
        public DbSet<DeputyVote> DeputyVote { get; set; }
    }
}