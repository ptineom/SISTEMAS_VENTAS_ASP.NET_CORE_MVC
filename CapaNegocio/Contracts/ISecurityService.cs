using Entidades;
using Helper.DTOGeneric;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contracts
{
    public interface ISecurityService
    {
        Task<ResponseObject> UserValidateAsync(string userId, string password, bool noValidar = false);
        Task<List<APLICACION>> GetMenuByUserIdAsync(string userId);
    }
}
