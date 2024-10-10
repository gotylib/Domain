using Data_Base_Controll.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data_Base_Controll.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DBController : ControllerBase
    {
        private readonly ILogger<DBController> _logger;
        public DBController(ILogger<DBController> logger)
        {
            _logger = logger;
        }

        [HttpGet("ListOfDomainsWithMinExpirationDate")]
        public async Task<ActionResult<List<Domain>>> GetListOfDomainsWithMinExpirationDate()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var minExpirationDate = await db.Domains.MinAsync(d => d.ExpirationDate);

                if (minExpirationDate == default)
                {
                    return NotFound("No domains found.");
                }

                var domains = await db.Domains
                                       .Where(d => d.ExpirationDate == minExpirationDate)
                                       .ToListAsync();

                return Ok(domains);
            }
        }

        [HttpPost("PostNewDomain")]
        public ActionResult PostNewDomain(string domain)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Domain? _domain = domain.ConvertStringToDomain();
                if(domain != null)
                {
                    db.Domains.Add(domain.ConvertStringToDomain());
                    return Ok(domain);
                }
                else
                {
                    return BadRequest("Error converting a string to a domain");
                }
                
            }
        }

        [HttpGet("GetInformationAboutDomain")]
        public 
        
    }
}
