using Microsoft.AspNetCore.Mvc;

namespace ChickenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EggController : Controller
    {
        [HttpGet("/GetMeEggs")]
        public IActionResult GetMeEggs()
        {
            return Ok("Here are your eggs!");
        }
    }
}