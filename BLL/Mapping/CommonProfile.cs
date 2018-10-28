using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using AutoMapper;
using BLL.DTO;
using DAL.Entity;

namespace BLL.Mapping
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<SqlDataReader, PersonInfo>();
            CreateMap<IDataReader, PersonInfo>();
            CreateMap<Pictures, PictureDTO>();
            CreateMap<PersonInfo, AutorDTO>();
        }
    }
}
