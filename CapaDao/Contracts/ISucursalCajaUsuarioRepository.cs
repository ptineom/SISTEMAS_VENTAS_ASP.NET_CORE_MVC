using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Contracts
{
    public interface ISucursalCajaUsuarioRepository
    {
        Task<List<CAJA>> GetAllBoxes(SUCURSAL_CAJA_USUARIO obj);

        Task<List<USUARIO>> GetAllUsersByCampusId(string campusId);
    }
}
