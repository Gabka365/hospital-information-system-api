using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace blog.Presentation
{
    [ApiController]
    [Route("/")]
    public class BaseController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            string _ = "hello from index";
            return Ok(_);
        }
    }
}
