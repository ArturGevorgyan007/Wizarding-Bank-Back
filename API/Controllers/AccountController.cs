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

        [HttpPost]
        public Account createAccount([FromQuery] string routingNumber,[FromQuery] string accountNumber,[FromQuery] int uid, [FromQuery] int bid,[FromQuery] decimal balance){
            Account acct = new();
            acct.Balance = balance; acct.RoutingNumber = routingNumber; acct.AccountNumber = accountNumber;
            if(uid != 0) { acct.UserId = uid; acct.BusinessId = 0;} 
            else {
                acct.BusinessId = bid;
                acct.UserId = 0;
            }
            return _service.createAccount(acct);
        }

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

        [HttpDelete]
        public bool deleteAccount([FromQuery] int acctId, [FromQuery] int uId){
            return _service.deleteAccount(acctId, uId);
        }


    }
    
}