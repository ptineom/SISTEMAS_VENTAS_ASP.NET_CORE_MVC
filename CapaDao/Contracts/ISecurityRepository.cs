using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Contracts
{
    public interface ISecurityRepository
    {
        Task<USUARIO> UserValidateAsync(string userId, string password, bool noValidar = false);
        Task<List<APLICACION>> GetMenuByUserId(string userId);
    }
}
