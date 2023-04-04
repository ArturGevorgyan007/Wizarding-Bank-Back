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

    public class TransactionController : ControllerBase
    {
        private readonly TransactionServices _services;

        public TransactionController(TransactionServices services)
        {
            _services = services;
        }

        [HttpGet]
        public List<Transaction> GetAllTransactions()
        {
            return _services.GetAllTransactions();
        }

        [HttpGet]
        [Route("transaction/{id:int}")]
        public IActionResult GetTransactionsByID(int id)
        {
            return Ok(_services.GetTransactionsByUserId(id));
        }
        [HttpGet]
        [Route("transaction/number/{id:int}")]
        public IActionResult GetLimitedTransactionsByID(int id)
        {
            return Ok(_services.GetLimitedTransactionsByUserId(id));
        }

        [HttpPost]
        public List<Transaction> CreateTransaction(Transaction transact)
        {
            _services.CreateTransaction(transact);
            return _services.GetAllTransactions();
        }

        [HttpPut]
        public Transaction UpdateTransaction(Transaction transact)
        {
            return _services.UpdateTransaction(transact);
        }

        [HttpDelete]
        public Transaction DeleteTransaction(Transaction transact)
        {
            return _services.DeleteTransaction(transact);
        }
    }
}