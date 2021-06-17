using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contracts
{
    public interface ISucursalCajaUsuarioService
    {
        Task<List<CAJA>> GetAllBoxes(SUCURSAL_CAJA_USUARIO obj);
        Task<List<USUARIO>> GetAllUsersByCampusId(string campusId);
    }
}
