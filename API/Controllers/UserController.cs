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
    public class UserController : ControllerBase
    {
        private readonly Services.UserServices _service;
        public UserController(Services.UserServices service) {
            _service = service;
        }

        [HttpGet]
        public List<User> GetAll() {
            return _service.GetAll();
        }


        [HttpPost]
        public List<User> Create(User a) {
            _service.CreateUser(a);
            return _service.GetAll();
        }
    
    
    
    }



}