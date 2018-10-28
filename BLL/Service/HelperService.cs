using BLL.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DAL.Entity;
using AutoMapper;
using System.Data;
using BLL.Mapping;
using BLL.DTO;

namespace BLL.Service
{
    public class HelperService : IHelperService
    {
        private readonly string conn;
        private readonly IMapper _mapper;

        public HelperService(IConfiguration conf, IMapper m)
        {
            conn = conf.GetConnectionString("DefaultConnection");
            _mapper = m;
        }

        public async Task<IEnumerable<T>> Get<T>(string Table, string Collum, bool And, List<string> par) where T : CommonEntity
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                var res = new List<T>();
                using (SqlCommand command = connection.CreateCommand())
                {

                    try
                    {
                        string com = string.Format("SELECT" + Collum + " FROM " + Table);
                        if (par != null && par.Count != 0)
                        {
                            com += " Where " + par[0];

                            string AndOr = And ? " And " : " OR ";

                            for (int i = 1; i < par.Count; i++)
                            {
                                com += AndOr + par[i];
                            }
                        }

                        //com += $" OFFSET {getModel.Skip} ROWS";
                        //com += $" FETCH NEXT {getModel.Take} ROWS ONLY";

                        command.CommandText = com;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            while (await reader.ReadAsync())
                            {
                                res.Add(await Map<T>(reader));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return res;
            }
        }

        public async Task<T> GetById<T>(string Table, string Collum, int id) where T : CommonEntity
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                T res = null;
                using (SqlCommand command = connection.CreateCommand())
                {

                    try
                    {
                        string com = string.Format("SELECT" + Collum + " FROM " + Table + " Where Id = " + id);
                        command.CommandText = com;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                res = await Map<T>(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return res;
            }
        }

        public async Task DeleteById(string Table, int id)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        string com = string.Format("DELETE FROM " + Table + " WHERE Id = " + id);
                        command.CommandText = com;
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public async Task<T> Create<T>(T item) where T : CommonEntity
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        string com = string.Format("INSERT INTO [dbo].[" + item.GetNameOfTable() + "] (");

                        com += String.Join(", ", item.GetListOfFields());
                        com += ") VALUES ('" + String.Join("', '", item.GetFields()) + "')";
                        com += " SELECT CAST(SCOPE_IDENTITY() AS int)";

                        command.CommandText = com;
                        int resId = (int)await command.ExecuteScalarAsync();

                        item = await GetById<T>(item.GetNameOfTable(), "*", resId);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return item;
        }

        public async Task<T> Update<T>(T item) where T : CommonEntity
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        string com = string.Format("UPDATE " + item.GetNameOfTable() + " SET ");

                        List<string> listNameFields = (List<string>)item.GetListOfFields();
                        List<string> listFields = (List<string>)item.GetFields();

                        for (int i = 0, j = 0; i < listNameFields.Count && j < listFields.Count; i++, j++)
                        {
                            com += $"{listNameFields[i]} = '{listFields[j]}'";
                            if (i != listNameFields.Count - 1 || j != listFields.Count - 1)
                                com += ", ";
                        }

                        com += $" Where Id = {item.Id}";

                        command.CommandText = com;
                        command.ExecuteNonQuery();

                        item = await GetById<T>(item.GetNameOfTable(), "*", item.Id);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return item;
        }

        public async Task<T> Map<T>(IDataReader reader) where T : CommonEntity
        {
            if (typeof(T) == typeof(PersonInfo))
            {
                PersonInfo res = new PersonInfo
                {
                    Id = (int)reader[0],
                    FirstName = reader[1].ToString(),
                    MiddleName = reader[2].ToString(),
                    LastName = reader[3].ToString(),
                    Phone = reader[4].ToString(),
                    Photo = reader[5].ToString(),
                    Description = reader[6].ToString()
                };

                if (!DBNull.Value.Equals(reader[7]))
                    res.Autor = (bool)reader[7];

                res.Birthday = !DBNull.Value.Equals(reader[8]) ? (DateTime?)reader[8] : null;

                return res as T;
            }
            if (typeof(T) == typeof(Genres))
            {
                var res = new Genres
                {
                    Id = (int)reader[0],
                    Name = reader[1].ToString()
                };
                return res as T;
            }
            if (typeof(T) == typeof(Pictures))
            {
                var res = new Pictures()
                {
                    Id = (int)reader[0],
                    Name = reader[1].ToString(),
                    AutorId = (int)reader[2],
                    Description = reader[3].ToString(),
                    GenreId = (int)reader[5],
                    PicturePath = reader[6].ToString(),
                    ParentId = !DBNull.Value.Equals(reader[8]) ? (int?)reader[8] : null
                };

                if (!DBNull.Value.Equals(reader[4]))
                    res.Date = (DateTime)reader[4];

                if (!DBNull.Value.Equals(reader[7]))
                    res.Status = (bool)reader[7];

                return res as T;
            };

            return null;
        }
    }
}
