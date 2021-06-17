using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contracts
{
    public interface IProveedorServiceExtention<T> where T: class
    {
        Task<T> GetByDocument(T obj);
    }
}
