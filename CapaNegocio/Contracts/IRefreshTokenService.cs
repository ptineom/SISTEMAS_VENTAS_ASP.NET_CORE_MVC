using Entidades;
using Helper.DTOGeneric;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contracts
{
    public interface IRefreshTokenService
    {
        Task<ResponseObject> RegisterAsync(REFRESH_TOKEN obj);
        Task<REFRESH_TOKEN> GetByIdAsync(string idRefreshToken);

    }
}
