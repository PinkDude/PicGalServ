using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interface;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PicGalServ.Controllers
{
    [AllowAnonymous]
    [Route("api/picture")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IHelperService _helperService;

        public PictureController(IHelperService help)
        {
            _helperService = help;
        }

        [HttpGet("/test")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetTest()
        {
            var res = "Ok";
            return Ok(res);
        }

        [HttpGet("/person-info")]
        [ProducesResponseType(typeof(IEnumerable<PersonInfo>), 200)]
        public async Task<IActionResult> GetPersonInfo()
        {
            var res = await _helperService.Get<PersonInfo>("PersonInfo", "*", true, null);
            return Ok(res);
        }

        [HttpDelete("/person-info")]
        public async Task<IActionResult> DeletePersonInfoById(int id)
        {
            await _helperService.DeleteById("PersonInfo", id);
            return Ok();
        }

        [HttpPost("/person-info")]
        [ProducesResponseType(typeof(PersonInfo), 200)]
        public async Task<IActionResult> CreatePersonInfo([FromBody]PersonInfo per)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _helperService.Create<PersonInfo>( per);

            return Ok(res);
        }

        [HttpPut("/person-info")]
        [ProducesResponseType(typeof(PersonInfo), 200)]
        public async Task<IActionResult> UpdatePersonInfo([FromBody]PersonInfo per)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _helperService.Update<PersonInfo>( per);

            return Ok(res);
        }
    }
}