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
        public LoanController(Services.LoanServices service) {
            _service = service;
        }

        [HttpGet("Info")]
        public List<Loan> GetAllBusinessUserLoan(Loan loan) {
            return _service.GetAllBusinessLoan(loan);
        }

        [HttpPost("New")]
        public List<Loan> CreateLoan(Loan loan) {
            _service.CreateBusinessLoan(loan);
            return _service.GetAllBusinessLoan(loan);
        }

        [HttpPut("Pay")]
        public Loan PayBusinessLoan(Loan loan, int amount) {
            return _service.PayLoan(loan, amount);
        }
    }
}
