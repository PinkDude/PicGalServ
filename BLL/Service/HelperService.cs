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
using BLL.Service.AccountHelp;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;

namespace BLL.Service
{
    public class HelperService : IHelperService
    {
        private readonly IHostingEnvironment _environment;
        private readonly string conn;
        private readonly IMapper _mapper;

        public HelperService(IConfiguration conf, IMapper m, IHostingEnvironment env)
        {
            conn = conf.GetConnectionString("DefaultConnection");
            _mapper = m;
            _environment = env;
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
                        com += ") VALUES (N'" + String.Join("', N'", item.GetFields()) + "')";

                        if(typeof(T) != typeof(ApplicationUsers))
                            com += " SELECT CAST(SCOPE_IDENTITY() AS int)";

                        command.CommandText = com;
                        if (typeof(T) != typeof(ApplicationUsers))
                        {
                            int resId = (int)await command.ExecuteScalarAsync();

                            item = await GetById<T>(item.GetNameOfTable(), "*", resId);
                        }
                        else
                        {
                            await command.ExecuteNonQueryAsync();
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
                            com += $"{listNameFields[i]} = N'{listFields[j]}'";
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
                else
                    res.Date = null;

                if (!DBNull.Value.Equals(reader[7]))
                    res.Status = (bool)reader[7];

                return res as T;
            }
            if(typeof(T) == typeof(ApplicationUsers))
            {
                var res = new ApplicationUsers
                {
                    Id = (int)reader[0],
                    Email = reader[1].ToString(),
                    EmailNorm = reader[2].ToString(),
                    PasswordHash = reader[3].ToString(),
                    PersonInfoId = (int)reader[4],
                    RoleId = (int)reader[5]
                };
                return res as T;
            }
            if(typeof(T) == typeof(ApplicationRoles))
            {
                var res = new ApplicationRoles
                {
                    Id = (int)reader[0],
                    RoleName = reader[1].ToString()
                };

                return res as T;
            }

            return null;
        }

        public async Task<ApplicationUsers> GetUserAsync(string Email, string Password)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                ApplicationUsers res = null;
                using (SqlCommand command = connection.CreateCommand())
                {

                    try
                    {
                        string com = string.Format($"SELECT * FROM ApplicationUsers Where Email = '{Email}'");
                        command.CommandText = com;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while(await reader.ReadAsync())
                                res = await Map<ApplicationUsers>(reader);
                        }

                        if (res != null)
                        {
                            if (!PasswordHash.VerifyHashedPassword(res.PasswordHash, Password))
                            {
                                throw new NotImplementedException("Не удалось найти пользователя");
                            }

                            var Role = await GetById<ApplicationRoles>("ApplicationRoles", "*", res.RoleId);

                            res.Role = Role.RoleName;

                            return res;
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
                    return null;
                }
            }
        }

        public async Task<string> SaveImage(IFormFile file)
        {
            return await Task.Run(() =>
            {
                if (file == null || string.IsNullOrEmpty(_environment.WebRootPath))
                    return null;
                Image<Rgba32> image = Image.Load(file.OpenReadStream());

                string hash = GetHashFromFile(file.OpenReadStream());
                
                string dir1 = _environment.WebRootPath + "/Files/Images/" + hash.Substring(0, 2);
                string dir2 = $"{dir1}/{hash.Substring(2, 2)}/";
                
                if (!Directory.Exists(dir1))
                {
                    Directory.CreateDirectory(dir1);
                    Directory.CreateDirectory(dir2);
                }
                else if (!Directory.Exists(dir2))
                    Directory.CreateDirectory(dir2);
                
                string result = dir2 + file.FileName;
                image.Save(result);

                return result.Replace(_environment.WebRootPath, "");
            });
        }

        private string GetHashFromFile(Stream stream)
        {
            var hash = SHA1.Create().ComputeHash(stream);
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
