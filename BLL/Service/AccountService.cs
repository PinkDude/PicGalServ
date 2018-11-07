using BLL.DTO.Account;
using BLL.Interface;
using BLL.Service.AccountHelp;
using DAL.Entity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class AccountService : IAccountService
    {
        private readonly IHelperService _helper;
        private ApplicationUsers user;

        public AccountService(IHelperService h)
        {
            _helper = h;
        }

        public async Task Registration(RegistrationDTO model)
        {
            var ac = await _helper.GetUserAsync(model.Email, model.Password);
            if (ac == null)
            {

                var per = new PersonInfo()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Phone = model.Number,
                    Birthday = model.Birthday
                };
                per = await _helper.Create<PersonInfo>(per);

                var account = new ApplicationUsers
                {
                    Email = model.Email,
                    EmailNorm = model.Email.ToUpper(),
                    PasswordHash = PasswordHash.HashPassword(model.Password),
                    PersonInfoId = per.Id,
                    RoleId = 1
                };

                await _helper.Create<ApplicationUsers>(account);
            }
            else
            {
                throw new Exception("Уже есть пользователь с таким Email'ом");
            }
        }

        public async Task<OperationResult> Token(LogDTO model)
        {
            var identity = await GetIdentity(model.Email, model.Password);
            if (identity == null)
            {
                return new OperationResult { isSuccesed = false };
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOption.ISSUER,
                    audience: AuthOption.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOption.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOption.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var person = await _helper.GetById<PersonInfo>("PersonInfo", "*", user.PersonInfoId);

            var response = new OperationResult
            {
                Access_token = encodedJwt,
                Username = identity.Name,
                Role = user.Role.Replace(" ", ""),
                Id = person.Id,
                Photo = person.Photo
            };

            return response;
            // сериализация ответа
            //await JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            try
            {
                user = await _helper.GetUserAsync(username, password);

                if (user != null)
                {
                    var person = new
                    {
                        Name = user.Email,
                        Role = user.Role.Replace(" ", "")
                    };

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, person.Name),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                    };
                    ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }

                // если пользователя не найдено
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
