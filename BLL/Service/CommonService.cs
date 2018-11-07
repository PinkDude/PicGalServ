using BLL.DTO;
using BLL.Interface;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace BLL.Service
{
    public class CommonService : ICommonService
    {
        private readonly IHelperService _helper;
        private readonly IMapper _mapper;

        public CommonService(IHelperService h, IMapper m)
        {
            _helper = h;
            _mapper = m;
        }

        public async Task<AuAndPageDTO> GetAllPersonInfo(string Name, GetDTO getModel)
        {
            List<string> list = new List<string>()
            {
                "FirstName",
                "MiddleName",
                "LastName"
            };

            for (int i = 0; i < list.Count; i++)
                list[i] += $" LIKE N'%{Name}%'";

            list.Add("Autor = 1");

            var res = await _helper.Get<PersonInfo>("PersonInfo", "*", false, list);

            var resDTO = new AuAndPageDTO();
            
            res = res.Where(c => c.Autor == true)
                .Skip(getModel.Skip)
                .Take(getModel.Take)
                .OrderBy(c => c.LastName)
                    .ThenBy(c => c.FirstName);

            resDTO.Count = res.Count();

            resDTO.Persons = res;

            return resDTO;
        }

        public async Task<PersonInfo> GetPersonInfoById(int id)
        {
            var res = await _helper.GetById<PersonInfo>("PersonInfo", "*", id);
            return res;
        }

        public async Task SaveImagePerson(int id, IFormFile file)
        {
            var path = await _helper.SaveImage(file);

            var per = await _helper.GetById<PersonInfo>("PersonInfo", "*", id);

            per.Photo = path;

            await _helper.Update<PersonInfo>(per);
        }

        public async Task<PicAndPageDTO> GetAllPictures(string Name, int genreId, GetDTO getModel, int? autorId, params string[] par)
        {
            List<string> list = new List<string>()
            {
                $"Name LIKE N'%{Name}%'"
            };

            if (genreId != 0)
                list.Add($"GenreId = {genreId}");

            if (autorId != null)
            {
                list.Add($"AutorId = {autorId}");
            }
            else
            {
                list.Add("Status = 1");
            }

            if (par != null)
            {
                foreach (var s in par)
                {
                    list.Add(s);
                }
            }

            var pictures = await _helper.Get<Pictures>("Pictures", "*", true, list);

            PicAndPageDTO resDTO = new PicAndPageDTO();

            resDTO.Count = pictures.Count();

            pictures = pictures.Skip(getModel.Skip)
                     .Take(getModel.Take)
                     .OrderBy(c => c.Name);

            var picturesDTO = _mapper.Map<IEnumerable<PictureDTO>>(pictures);

            foreach (var i in picturesDTO)
            {
                var autor = await GetPersonInfoById(i.AutorId);
                i.Autor = _mapper.Map<AutorDTO>(autor);

                var genre = await _helper.GetById<Genres>("Genres", "*", i.GenreId);
                i.Genre = genre.Name;
            }

            resDTO.Pictures = picturesDTO;

            return resDTO;
        }

        public async Task<PictureDTO> GetPictureAsync(int id)
        {
            var res = await _helper.GetById<Pictures>("Pictures", "*", id);

            var resDTO = _mapper.Map<PictureDTO>(res);

            var autor = await GetPersonInfoById(res.AutorId);
            resDTO.Autor = _mapper.Map<AutorDTO>(autor);

            var genre = await _helper.GetById<Genres>("Genres", "*", res.GenreId);
            resDTO.Genre = genre.Name;

            return resDTO;
        }

        public async Task<PictureDTO> UpdatePictureAsync(int id, Pictures pic)
        {
            if (await GetPersonInfoById(pic.AutorId) == null)
                throw new NotImplementedException("Не удалось найти автора");

            if (await _helper.GetById<Genres>("Genres", "*", pic.GenreId) == null)
                throw new NotImplementedException("Не удалось найти жанр");

            GetDTO get = new GetDTO
            {
                Take = 100000,
                Skip = 0
            };

            var pictures = await GetAllPictures("", 0, get, pic.AutorId, $"ParentId = {pic.Id}");
            if (pictures.Count != 0)
                throw new Exception("Уже есть изменения. Сначала подтвердите другие изменения");

            var picture = await _helper.GetById<Pictures>("Pictures", "*", pic.Id);
            if (picture == null)
                throw new NotImplementedException("Не удалось найти картину");

            var res = _mapper.Map(pic, picture);

            res = await _helper.Create<Pictures>(res);

            //await _helper.Update<Pictures>(res);

            var resDTO = await GetPictureAsync(res.Id);

            return resDTO;
        }

        public async Task SaveImagePicture(int id, IFormFile file)
        {
            var path = await _helper.SaveImage(file);

            var pic = await _helper.GetById<Pictures>("Pictures", "*", id);

            pic.PicturePath = path;

            await _helper.Update<Pictures>(pic);
        }

        public async Task ConfirmUpdatePicture(int id, bool yes)
        {
            var pic = await _helper.GetById<Pictures>("Pictures", "*", id);
            if (pic != null && pic.ParentId != null)
            {
                if (yes)
                {
                    if(pic.ParentId != 0)
                        await _helper.DeleteById("Pictures", (int)pic.ParentId);

                    pic.ParentId = null;
                    pic.Status = true;
                    await _helper.Update<Pictures>(pic);
                }
                else
                {
                    await _helper.DeleteById("Pictures", id);
                }
            }
            else
            {
                throw new NotImplementedException("Не удалось найти картину");
            }
        }

        public async Task CreatePicture(Pictures pic)
        {
            if (await _helper.GetById<PersonInfo>("PersonInfo", "*", pic.AutorId) == null)
                throw new NotImplementedException("Не удалось найти автора");

            if (await _helper.GetById<Genres>("Genres", "*", pic.GenreId) == null)
                throw new NotImplementedException("Не удалось найти жанр");

            pic.Status = false;

            await _helper.Create<Pictures>(pic);
        }

        public async Task<PicAndPageDTO> GetConfPictures(string Name, int genreId, GetDTO getModel)
        {
            List<string> list = new List<string>()
            {
                $"Name LIKE N'%{Name}%'",
                "Status = 0"
            };

            if (genreId != 0)
                list.Add($"GenreId = {genreId}");

            var pictures = await _helper.Get<Pictures>("Pictures", "*", true, list);

            PicAndPageDTO resDTO = new PicAndPageDTO();

            resDTO.Count = pictures.Count();

            pictures = pictures
                .Skip(getModel.Skip)
                     .Take(getModel.Take)
                     .OrderBy(c => c.Name);

            var picturesDTO = _mapper.Map<IEnumerable<PictureDTO>>(pictures);

            foreach (var i in picturesDTO)
            {
                var autor = await GetPersonInfoById(i.AutorId);
                i.Autor = _mapper.Map<AutorDTO>(autor);

                var genre = await _helper.GetById<Genres>("Genres", "*", i.GenreId);
                i.Genre = genre.Name;
            }

            resDTO.Pictures = picturesDTO;

            return resDTO;
        }
    }
}
