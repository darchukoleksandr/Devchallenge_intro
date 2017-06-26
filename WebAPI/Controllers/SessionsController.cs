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
    public class SessionsController : ApiController
    {
        private readonly SessionDbContext _dbContext = new SessionDbContext();

        // GET api/Session
        /// <summary>
        /// Get the list of all sessions
        /// </summary>
        /// <returns>IQueryable<![CDATA[<Session>]]></returns>
        public IQueryable<Session> Get()
        {
            return _dbContext.Sessions;
        }
        // GET api/Session/5
        /// <summary>
        /// Get the session by primary key
        /// </summary>
        /// <param name="id">Primary key of session</param>
        /// <returns>OK(200) and single Session model or NotFound</returns>
        [ResponseType(typeof(Session))]
        public IHttpActionResult Get(int id)
        {
            var result = _dbContext.Sessions.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        /// <summary>
        /// Get the list of votings for specified session
        /// </summary>
        /// <param name="id">Primary key of session</param>
        /// <returns>OK(200) and list of Voting model or NotFound</returns>
        [Route("api/sessions/{id}/votings")]
        [ResponseType(typeof(Voting))]
        public IHttpActionResult GetSessionVotings(int id)
        {
            if (!_dbContext.Sessions.Any(session => session.Id == id))
                return NotFound();
            return Ok(_dbContext.Votings.Where(voting => voting.SessionId == id));
        }
    }
}
