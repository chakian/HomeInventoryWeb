using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebUI.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        //[Route("TestMethod")]
        //[HttpGet]
        //[ValidateAntiForgeryToken]
        //public string TestMth(string email = "")
        //{
        //    var response = JsonConvert.SerializeObject(new { email });
        //    return response;
        //    //return string.IsNullOrEmpty(email);
        //}

        private readonly HomeInventoryDbContext _context;

        public UserController(HomeInventoryDbContext context)
        {
            _context = context;
        }
    }
}
