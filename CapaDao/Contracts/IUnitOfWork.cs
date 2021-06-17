using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CapaDao.Contracts
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        SqlTransaction Transaction { get; }
        IProveedorRepository ProveedorRepository { get; }
        ICajaAperturaRepository CajaAperturaRepository { get; }
        IDocCompraRepository DocCompraRepository { get; }
    }
}
