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
            return Ok(_services.GetTransactionsWithEmails(id));
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

        [HttpPost]
        [Route("transaction/walletToAccount")]
        public Transaction? walletToAccount(Transaction transact){
            return _services.walletToAccount(transact);
        }

        [HttpPost]
        [Route("transaction/internal")]
        public Transaction? internalTransaction([FromQuery] int type, Transaction transact){
            switch(type){
                case 1:
                    return _services.walletToAccount(transact);
                case 2:
                    return _services.walletToCard(transact);
                case 3: 
                    return _services.cardToWallet(transact);
                case 4:
                    return _services.acctToWallet(transact);
                default: return null!;
            }
        }
    }
}