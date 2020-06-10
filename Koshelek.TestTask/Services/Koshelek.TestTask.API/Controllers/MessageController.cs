using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Koshelek.TestTask.API.Controllers
{
    [Route("api/Messages")]
    [ApiController]
    public class MessageController : Controller
    {
        [HttpGet("{Id}"), ActionName("Get")]
        public Message GetById(int Id)
        {
            return "WIP";
        }

    }
}
