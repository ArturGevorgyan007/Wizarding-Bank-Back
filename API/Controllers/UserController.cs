using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using DataAccess.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _repo;
        public UserController(UserServices repo) {
            _repo = repo;
        }

        [HttpGet]
        public List<User> GetAll() {
            return _repo.GetAll();
        }


        [HttpPost]
        public List<User> Create(User a) {
            _repo.CreateUser(a);
            return _repo.GetAll();
        }
    
    
    
    }



}