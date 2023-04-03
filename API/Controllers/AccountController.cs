using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Entities;
using Services; 

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountServices _service;

        public AccountController(AccountServices service){
            _service = service;
        }

        [HttpGet("Accounts")]
        public List<Account> GetAll(){
            return _service.getAllAccounts();
        }

        [HttpGet("UserAccounts")]
        public List<Account> getAllAccounts(int id){
            return _service.getAccounts(id);
        }

        ///Should we change the routing and account num to int? OR is it string bc we are not modifying these numbers at all?
        /// Maybe we can have business id and uid start with specific two first numbers to tell them apart
        [HttpPost]
        public Account createAccount([FromQuery] string routingNumber,[FromQuery] string accountNumber,[FromQuery] int uid, [FromQuery] int bid,[FromQuery] decimal balance){
            Account acct = new();
            acct.Balance = balance; acct.RoutingNumber = routingNumber; acct.AccountNumber = accountNumber;
            if(uid != 0) { acct.UserId = uid;} 
            else {
                acct.BusinessId = bid;
            }
            return _service.createAccount(acct);
        }

        ///we  have to make accountNumber unique
        [HttpPut]
        public Account updateAccount([FromQuery] string routingNumber,[FromQuery] string accountNumber,[FromQuery] int uid, [FromQuery] int bid,[FromQuery] decimal balance){
            Account acct = new();
            acct.Balance = balance; acct.RoutingNumber = routingNumber; acct.AccountNumber = accountNumber;
            if(uid != 0) { 
            acct.UserId = uid;
            } else {
                acct.BusinessId = bid;
            }
            return _service.updateAccount(acct);
        }

        ///We need to save deleted transactions onto new table before delete account
        [HttpDelete]
        public bool deleteAccount([FromQuery] int acctId, [FromQuery] int Id){
            return _service.deleteAccount(acctId, Id);
        }
        // public bool deleteAccount([FromBody] Account acct){
        //     return _service.deleteAccount(acct);
        // }


    }
    
}