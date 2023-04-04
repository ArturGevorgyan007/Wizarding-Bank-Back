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
        private readonly Services.UserServices _service;
        public UserController(Services.UserServices service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("user/{id:int}")]
        public List<User> GetById(int id)
        {

            return _service.GetUser(id);

        }
        [HttpGet]
        [Route("user/byEmail/{email:string}")]
        public int GetByEmail(string email)
        {

            List<User> user = _service.GetUser(email);
            return user[0].Id;

        }
        [HttpGet]
        [Route("user/{usr}/{pas}")]
        public List<User> GetById(string usr, string pas)
        {

            return _service.GetUser(usr, pas);

        }

        [HttpGet]
        [Route("users")]
        public List<User> GetAll()
        {
            return _service.GetAll();
        }



        [HttpPost]

        [Route("user/create")]
        public List<User> Create(User u)
        {
            _service.CreateUser(u);
            return _service.GetAll();
        }

        [HttpPut]
        [Route("user/update")]
        public User UpdateUser(User u)
        {
            return _service.UpdateUser(u);

        }
        [HttpGet]
        [Route("user/wallet/update/{id:int}/{amt:int}")]
        public User GetById(int id, int amt)
        {

            return _service.UpdateWallet(id, amt);

        }

        [HttpDelete]
        [Route("user/Delete")]
        public User DeleteUser(User u)
        {
            return _service.DeleteUser(u);
        }





    }



}