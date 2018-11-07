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
        [ProducesResponseType(typeof(AuAndPageDTO), 200)]
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

        [HttpPost("person-info/{id}/pic")]
        public async Task<IActionResult> LoadImage([FromRoute]int id)
        {
            var file = Request.Form.Files;
            if(file != null)
            {
                await _commonService.SaveImagePerson(id, file[0]);
                return Ok();
            }
            return BadRequest("Загрузка не прошла");
        }

        //[HttpPost("person-info")]
        //[ProducesResponseType(typeof(PersonInfo), 200)]
        //public async Task<IActionResult> CreatePersonInfo([FromBody]PersonInfo per)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var res = await _helperService.Create<PersonInfo>( per);

        //    return Ok(res);
        //}

        [Authorize(Roles = "Autor, Admin")]
        [HttpPut("person-info/{id}")]
        [ProducesResponseType(typeof(PersonInfo), 200)]
        public async Task<IActionResult> UpdatePersonInfo([FromRoute]int id , [FromBody]PersonInfo per)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id == per.Id || User.IsInRole("Admin"))
            {
                var res = await _helperService.Update<PersonInfo>(per);

                return Ok(res);
            }
            return BadRequest("Это же не твой профиль");
        }
        #endregion

        #region Pictures

        [HttpGet("pictures")]
        [ProducesResponseType(typeof(PicAndPageDTO), 200)]
        public async Task<IActionResult> GetAllPictures(string Name, int genreId, [FromQuery] GetDTO getModel, int? autorId = null)
        {
            var res = await _commonService.GetAllPictures(Name, genreId, getModel, autorId);
            return Ok(res);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("pictures/conf")]
        [ProducesResponseType(typeof(PicAndPageDTO), 200)]
        public async Task<IActionResult> GetConfPictures(string Name, int genreId, [FromQuery] GetDTO getModel)
        {
            var res = await _commonService.GetConfPictures(Name, genreId, getModel);
            return Ok(res);
        }

        [HttpGet("pictures/{id}")]
        [ProducesResponseType(typeof(PictureDTO), 200)]
        public async Task<IActionResult> GetPictureById([FromRoute]int id)
        {
            var res = await _commonService.GetPictureAsync(id);
            return Ok(res);
        }

        [Authorize(Roles = "Autor, Admin")]
        [HttpPut("pictures/{id}")]
        [ProducesResponseType(typeof(PictureDTO), 200)]
        public async Task<IActionResult> UpdatePictureNotConf([FromRoute]int id, [FromBody]Pictures pic)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var person = await _helperService.GetById<PersonInfo>("PersonInfo", "*", id);

            if (person != null && (person.Id == pic.AutorId || User.IsInRole("Admin")))
            {
                var res = await _commonService.UpdatePictureAsync(id, pic);

                return Ok(res);
            }

            return BadRequest("Что-то не так");
        }

        [HttpPost("pictures/{id}/pic")]
        public async Task<IActionResult> LoadImageForPicture([FromRoute]int id)
        {
            var file = Request.Form.Files;
            if (file != null)
            {
                await _commonService.SaveImagePicture(id, file[0]);
                return Ok();
            }
            return BadRequest("Загрузка не прошла");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pictures/{id}/conf")]
        public async Task<IActionResult> ConfirmPicture([FromRoute]int id, [FromQuery]bool yes)
        {
            await _commonService.ConfirmUpdatePicture(id, yes);

            return Ok();
        }

        [Authorize(Roles = "Autor, Admin")]
        [HttpPost("pictures")]
        public async Task<IActionResult> CreatePicture([FromBody]Pictures pic)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _commonService.CreatePicture(pic);

            return Ok();
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