using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Campus.Custodial.Chemicals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChemicalController : ControllerBase
    {
        [HttpGet]
        public Chemical Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Chemical Post()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Chemical Put()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Chemical Delete()
        {
            throw new NotImplementedException();
        }
    }
}