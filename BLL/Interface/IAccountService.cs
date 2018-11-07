using BLL.DTO.Account;
using BLL.Service.AccountHelp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IAccountService
    {
        Task Registration(RegistrationDTO model);
        Task<OperationResult> Token(LogDTO model);
    }
}
