using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class DeputiesController : ApiController
    {
        private readonly SessionDbContext _dbContext = new SessionDbContext();

        // GET: api/deputies
        /// <summary>
        /// Get the list of all deputies
        /// </summary>
        /// <returns>IQueryable<![CDATA[<Deputy>]]></returns>
        public IQueryable<Deputy> GetDeputies()
        {
            return _dbContext.Deputies;
        }

        // GET: api/deputies/5
        /// <summary>
        /// Get the deputy by primary key
        /// </summary>
        /// <param name="id">Primary key of deputy</param>
        /// <returns>OK(200) and single Deputy model or NotFound</returns>
        [ResponseType(typeof(Deputy))]
        public IHttpActionResult GetDeputy(short id)
        {
            Deputy result = _dbContext.Deputies.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/deputies/5/deputyVotes
        /// <summary>
        /// Get the list of votes for specified deputy
        /// </summary>
        /// <param name="id">Primary key of deputy</param>
        /// <returns>OK(200) and list of DeputyVote model or NotFound</returns>
        [Route("api/deputies/{id}/deputyVotes")]
        [ResponseType(typeof(DeputyVote))]
        public IHttpActionResult GetVotingVotes(int id)
        {
            if (!_dbContext.Deputies.Any(deputy => deputy.Id == id))
                return NotFound();
            return Ok(_dbContext.DeputyVote.Where(vote => vote.DeputyId == id));
        }

        // GET: api/deputies/5/influence/5
        /// <summary>
        /// Get the zone of influence for specified deputy and session.
        /// If <c>sessionId</c> is not specified, influence will  be calculated for every session.
        /// </summary>
        /// <param name="deputyId">Primary key of Deputy model</param>
        /// <param name="sessionId">Primary key of Session model</param>
        /// <returns>OK(200) and the list of deputies which voted the same as specified deputy in clause of specified session or NotFound</returns>
        [Route("api/deputies/{deputyId}/influence/{sessionId:int?}")]
        [ResponseType(typeof(IEnumerable<DeputyVote>))]
        public IHttpActionResult GetZonesOfInfluence(int deputyId, int sessionId = 0)
        {
            var deputy = _dbContext.Deputies.Find(deputyId);
            if (deputy == null)
            {
                return NotFound();
            }

            var sessions = sessionId != 0 ? _dbContext.Sessions.Where(session => session.Id == sessionId).ToArray() : _dbContext.Sessions.ToArray();
            if (!sessions.Any())
            {
                return NotFound();
            }

            // List of votes that will be ignored
            var votesToIgnore = new[] { "Відсутній", "Утримався", "Не голосував" };
            // List of votings with same votes as in specified deputy
            var sameVotes = new List<DeputyVote[]>();

            foreach (var session in sessions)
            {
                var votings = _dbContext.Votings.Where(voting => voting.SessionId == session.Id).ToArray();
                foreach (var voting in votings)
                {
                    // Vote type of specified deputy at current voting
                    var center = _dbContext.DeputyVote.First(vote => vote.VotingId == voting.Id && vote.DeputyId == deputy.Id);
                    // Ignored type of votes
                    if (votesToIgnore.Contains(center.VoteType))
                        continue;
                    // List of votes with the same type of vote
                    var votes = _dbContext.DeputyVote.Where(vote => vote.VotingId == voting.Id && vote.VoteType == center.VoteType).ToArray();
                    sameVotes.Add(votes);
                }
            }

            var result = sameVotes[0].ToArray();
                result = sameVotes.Skip(1).Aggregate(result, (current, deputyVotes) => current.Intersect(deputyVotes, new SpecialComparer()).ToArray());

            return Ok(result);
        }
    }

    class SpecialComparer : IEqualityComparer<DeputyVote>
    {
        public bool Equals(DeputyVote x, DeputyVote y)
        {
            return x.DeputyId == y.DeputyId;
        }
        public int GetHashCode(DeputyVote codeh)
        {
            return codeh.DeputyId * 31;
        }
    }
}