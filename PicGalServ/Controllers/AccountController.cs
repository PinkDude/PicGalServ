using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO.Account;
using BLL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PicGalServ.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService ac)
        {
            _accountService = ac;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody]RegistrationDTO model)
        {
            await _accountService.Registration(model);

            return Ok();
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody]LogDTO model)
        {
            var res = await _accountService.Token(model);

            if (!res.isSuccesed)
            {
                return BadRequest("Не верный логин или пароль");
            }

            //var response = new
            //{
            //    access_token = res.Access_token,
            //    username = res.Username
            //    Role
            //};

            //Response.ContentType = "application/json";
            return Ok(JsonConvert.SerializeObject(res, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
    }
}