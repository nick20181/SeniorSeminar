using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Campus.Service.Address.Controllers
{
    public class ServiceAddressController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public string Post()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public string Put()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public string Delete()
        {
            throw new NotImplementedException();
        }

    }
}