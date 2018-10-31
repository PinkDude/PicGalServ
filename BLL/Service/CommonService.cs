﻿using BLL.DTO;
using BLL.Interface;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;

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

            var res = await _helper.Get<PersonInfo>("PersonInfo", "*", false, list);

            var resDTO = new AuAndPageDTO();
            resDTO.Count = res.Count();

            res = res.Skip(getModel.Skip)
                     .Take(getModel.Take);

            resDTO.Persons = res;

            return resDTO;
        }

        public async Task<PersonInfo> GetPersonInfoById(int id)
        {
            var res = await _helper.GetById<PersonInfo>("PersonInfo", "*", id);
            return res;
        }

        public async Task<PicAndPageDTO> GetAllPictures(string Name, int genreId, GetDTO getModel)
        {
            List<string> list = new List<string>()
            {
                $"Name LIKE N'%{Name}%'",
                "Status = 1"
            };

            if (genreId != 0)
                list.Add($"GenreId = {genreId}");

            var pictures = await _helper.Get<Pictures>("Pictures", "*", true, list);

            PicAndPageDTO resDTO = new PicAndPageDTO();

            resDTO.Count = pictures.Count();

            pictures = pictures.Skip(getModel.Skip)
                     .Take(getModel.Take);

            var picturesDTO = _mapper.Map<IEnumerable<PictureDTO>>(pictures);

            foreach(var i in picturesDTO)
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
    }
}
