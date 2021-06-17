using Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaDao.Contracts
{
    public interface IProveedorRepository : IBaseRepository<PROVEEDOR>, IProveedorExtention<PROVEEDOR>
    {
    }
}
