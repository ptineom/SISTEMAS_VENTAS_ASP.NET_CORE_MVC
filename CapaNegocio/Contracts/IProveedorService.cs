using Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.Contracts
{
    public interface IProveedorService: IBaseService<PROVEEDOR>, IProveedorServiceExtention<PROVEEDOR>
    {
    }
}
