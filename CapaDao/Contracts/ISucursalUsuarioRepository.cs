using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Contracts
{
    public interface ISucursalUsuarioRepository
    {
        Task<List<SUCURSAL>> GetAllByCampusId(string userId);
    }
}
