using BLL.DTO;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IHelperService
    {
        Task<IEnumerable<T>> Get<T>(string Table, string Collum, bool And, List<string> par) where T : CommonEntity;
        Task DeleteById(string Table, int id);
        Task<T> Create<T>(T item) where T : CommonEntity;
        Task<T> GetById<T>(string Table, string Collum, int id) where T : CommonEntity;
        Task<T> Update<T>(T item) where T : CommonEntity;
    }
}
