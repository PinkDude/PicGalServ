using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using AutoMapper;
using DAL.Entity;

namespace BLL.Mapping
{
    public class HelperProfile : Profile
    {
        public HelperProfile()
        {
            CreateMap<SqlDataReader, PersonInfo>();
            CreateMap<IDataReader, PersonInfo>();
        }
    }
}
