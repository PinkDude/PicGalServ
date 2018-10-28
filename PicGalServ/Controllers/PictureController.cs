using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interface;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PicGalServ.Controllers
{
    [Produces("application/json")]
    [Route("api/picture")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IHelperService _helperService;
        private readonly ICommonService _commonService;

        public PictureController(IHelperService help, ICommonService com)
        {
            _helperService = help;
            _commonService = com;
        }

        #region Person Info

        [HttpGet("person-info")]
        [ProducesResponseType(typeof(IEnumerable<PersonInfo>), 200)]
        public async Task<IActionResult> GetAllPersonInfo(string Name, [FromQuery]GetDTO getModel)
        {
            var res = await _commonService.GetAllPersonInfo(Name, getModel);
            return Ok(res);
        }

        [HttpGet("person-info/{id}")]
        [ProducesResponseType(typeof(PersonInfo), 200)]
        public async Task<IActionResult> GetPersonInfoById([FromRoute]int id)
        {
            var res = await _commonService.GetPersonInfoById(id);
            return Ok(res);
        }

        [HttpDelete("person-info/{id}")]
        public async Task<IActionResult> DeletePersonInfoById(int id)
        {
            await _helperService.DeleteById("PersonInfo", id);
            return Ok();
        }

        [HttpPost("person-info")]
        [ProducesResponseType(typeof(PersonInfo), 200)]
        public async Task<IActionResult> CreatePersonInfo([FromBody]PersonInfo per)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _helperService.Create<PersonInfo>( per);

            return Ok(res);
        }

        [HttpPut("person-info/{id}")]
        [ProducesResponseType(typeof(PersonInfo), 200)]
        public async Task<IActionResult> UpdatePersonInfo([FromRoute]int id , [FromBody]PersonInfo per)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            per.Id = id;
            var res = await _helperService.Update<PersonInfo>( per);

            return Ok(res);
        }
        #endregion

        #region Pictures

        [HttpGet("pictures")]
        [ProducesResponseType(typeof(IEnumerable<PictureDTO>), 200)]
        public async Task<IActionResult> GetAllPictures(string Name, int genreId, [FromQuery] GetDTO getModel)
        {
            var res = await _commonService.GetAllPictures(Name, genreId, getModel);
            return Ok(res);
        }

        [HttpGet("pictures/{id}")]
        [ProducesResponseType(typeof(PictureDTO), 200)]
        public async Task<IActionResult> GetPictureById(int id)
        {
            var res = await _commonService.GetPictureAsync(id);
            return Ok(res);
        }

        #endregion

        #region Genre

        [HttpGet("genres")]
        [ProducesResponseType(typeof(IEnumerable<Genres>), 200)]
        public async Task<IActionResult> GetAllGenres()
        {
            List<string> list = new List<string>();
            var res = await _helperService.Get<Genres>("Genres", "*", false, list);
            return Ok(res);
        }

        #endregion
    }
}