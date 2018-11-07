using BLL.DTO;
using DAL.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICommonService
    {
        Task<AuAndPageDTO> GetAllPersonInfo(string Name, GetDTO getModel);
        Task<PersonInfo> GetPersonInfoById(int id);
        Task<PicAndPageDTO> GetAllPictures(string Name, int genreId, GetDTO getModel, int? autorId, params string[] par);
        Task<PictureDTO> GetPictureAsync(int id);
        Task<PictureDTO> UpdatePictureAsync(int id, Pictures pic);
        Task ConfirmUpdatePicture(int id, bool yes);
        Task CreatePicture(Pictures pic);
        Task<PicAndPageDTO> GetConfPictures(string Name, int genreId, GetDTO getModel);
        Task SaveImagePerson(int id, IFormFile file);
        Task SaveImagePicture(int id, IFormFile file);
    }
}
