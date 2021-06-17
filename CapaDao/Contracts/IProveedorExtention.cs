using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Contracts
{
    public interface IProveedorExtention<T> where T : class
    {
        Task<T> GetByDocument(T obj);
    }
}
