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
    public class DeputyVotesController : ApiController
    {
        private readonly SessionDbContext _db = new SessionDbContext();

        // GET: api/DeputyVotes
        /// <summary>
        /// Get the list of all deputies votes
        /// </summary>
        /// <returns></returns>
        public IQueryable<DeputyVote> GetDeputyVote()
        {
            return _db.DeputyVote;
        }

        // GET: api/DeputyVotes/5
        /// <summary>
        /// Get the deputy vote by primary key
        /// </summary>
        /// <param name="id">Primary key of DeputyVote model</param>
        /// <returns>OK(200) and single DeputyVote model or NotFound</returns>
        [ResponseType(typeof(DeputyVote))]
        public IHttpActionResult GetDeputyVote(int id)
        {
            DeputyVote result = _db.DeputyVote.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}