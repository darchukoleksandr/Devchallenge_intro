using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using DAL.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class VotingsController : ApiController
    {
        private readonly SessionDbContext _dbContext = new SessionDbContext();

        // GET: api/Votings
        /// <summary>
        /// Get the list of all votings
        /// </summary>
        /// <returns>IQueryable<![CDATA[<Voting>]]></returns>
        public IQueryable<Voting> GetVotings()
        {
            return _dbContext.Votings;
        }
        // GET: api/Votings/5
        /// <summary>
        /// Get the voting by primary key
        /// </summary>
        /// <param name="id">Primary key of voting</param>
        /// <returns>OK(200) and single Voting model or NotFound</returns>
        [ResponseType(typeof(Voting))]
        public async Task<IHttpActionResult> GetVoting(int id)
        {
            var result = await _dbContext.Votings.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        // GET: api/Votings/5/deputyVotes
        /// <summary>
        /// Get the list of votes for specified voting
        /// </summary>
        /// <param name="id">Primary key of voting</param>
        /// <returns>OK(200) and list of DeputyVote model or NotFound</returns>
        [Route("api/votings/{id}/deputyVotes")]
        [ResponseType(typeof(DeputyVote))]
        public IHttpActionResult GetVotingVotes(int id)
        {
            if (!_dbContext.Votings.Any(voting => voting.Id == id))
            {
                return NotFound();
            }
            return Ok(_dbContext.DeputyVote.Where(vote => vote.VotingId == id));
        }

        // GET: api/Votings/5/result
        /// <summary>
        /// Get the result of voting (Прийнято|Не прийнято)
        /// </summary>
        /// <param name="id">Primary key of voting</param>
        /// <returns>String representing the result of voting (Прийнято|Не прийнято)</returns>
        [Route("api/votings/{id}/result")]
        [ResponseType(typeof(string))]
        public IHttpActionResult GetVotingResult(int id)
        {
            if (!_dbContext.Votings.Any(voting => voting.Id == id))
            {
                return NotFound();
            }
            var result = _dbContext.DeputyVote.Where(vote => vote.VotingId == id)
                .Select(vote => vote.VoteType)
                .GroupBy(v => v)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
            return Ok(result == "За" ? "Прийнято" : "Не прийнято");
        }
    }
}