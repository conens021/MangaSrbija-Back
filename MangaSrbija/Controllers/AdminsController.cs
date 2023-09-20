using MangaSrbija.BLL.mappers.Admins;
using Microsoft.AspNetCore.Mvc;

namespace MangaSrbija.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreateAdmin([FromBody] CreateAdminDTO createAdminDTO)
        {





            return Ok();
        }
    }
}
