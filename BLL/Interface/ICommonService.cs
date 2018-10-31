using BLL.DTO;
using DAL.Entity;
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
        Task<PicAndPageDTO> GetAllPictures(string Name, int genreId, GetDTO getModel);
        Task<PictureDTO> GetPictureAsync(int id);
    }
}
