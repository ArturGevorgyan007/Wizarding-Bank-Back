using Microsoft.AspNetCore.Mvc;
using DataAccess.Entities;
using DataAccess;
using Services;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]

    public class BusinessController : ControllerBase
    {

        private readonly BusinessServices _busService;
        public BusinessController(BusinessServices busService)
        {
            _busService = busService;
        }

        [HttpGet]
        public List<Business> GetAll()
        {
            return _busService.GetAllBusinesses();
        }

        [HttpPost]
        public List<Business> Create(Business bus)
        {
            _busService.CreateBusiness(bus);
            return _busService.GetAllBusinesses();
        }

        [HttpPut]
        public Business Update(Business bus)
        {
            return _busService.UpdateBusiness(bus);
        }

        [HttpDelete]
        public Business Delete(Business bus)
        {
            return _busService.DeleteBusiness(bus);
        }
        [HttpGet]
        [Route("busId/{email}")]
        public IActionResult GetBusinessByEmail(string email)
        {
            return Ok(_busService.GetBusiness(email).Id);
        }

        [HttpGet]
        [Route("busType/{email}")]
        public IActionResult GetBusinessTypeByEmail(string email)
        { 
            return Ok(_busService.GetBusiness(email).BusinessType);
        }

    }
}