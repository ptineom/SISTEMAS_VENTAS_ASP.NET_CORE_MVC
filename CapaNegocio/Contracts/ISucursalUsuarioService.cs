using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contracts
{
    public interface ISucursalUsuarioService
    {
        Task<List<SUCURSAL>> GetAllByCampusId(string userId);
    }
}
