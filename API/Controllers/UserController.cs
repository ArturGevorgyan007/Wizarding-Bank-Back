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
    public class UserController : ControllerBase
    {
        private readonly UserServices _repo;
        public UserController(UserServices repo) {
            _repo = repo;
        }

        [HttpGet]
        [Route("user/{id:int}")] 
        public List<User> GetById(int id){

            return _repo.GetUser(id);

        }
        [HttpGet]
        [Route("user/{usr}/{pas}")] 
        public List<User> GetById(string usr, string pas){

            return _repo.GetUser(usr,pas);

        }

        [HttpGet]
        [Route("users")] 
        public List<User> GetAll() {
            return _repo.GetAll();
        }



        [HttpPost]
        [Route("user/create")] 
        public List<User> Create(User u) {
            _repo.CreateUser(u);
            return _repo.GetAll();
        }

        [HttpPut]
        [Route("user/update")]
        public User UpdateUser(User u)
        {
            return _repo.UpdateUser(u);
             
        }

        [HttpDelete]
        [Route("user/Delete")]
        public User DeleteUser(User u)
        {
            return _repo.DeleteUser(u);
        }





    }



}