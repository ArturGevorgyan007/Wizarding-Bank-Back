using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using DataAccess;
using DataAccess.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly Services.LoanServices _service;
        public LoanController(Services.LoanServices service)
        {
            _service = service;
        }

        [HttpGet("Info/{busId}")]
        public IActionResult GetAllBusinessUserLoan(int busId)
        {
            Console.WriteLine(busId);
            return Ok(_service.GetAllBusinessLoan(busId)[0]);
        }

        [HttpPost("New")]
        public IActionResult CreateLoan(Loan loan)
        {
            return Ok(_service.CreateBusinessLoan(loan));
        }


        [HttpPut("Pay/{id}/{amount}")]
        public IActionResult PayBusinessLoan(int id, decimal amount)
        {
            return Ok(_service.PayLoan(id, amount));
        }
    }
}