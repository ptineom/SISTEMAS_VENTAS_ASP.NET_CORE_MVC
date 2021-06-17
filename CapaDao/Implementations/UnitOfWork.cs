using CapaDao.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CapaDao.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private IConnection _connection = null;
        private SqlTransaction _transaction;
        private IProveedorRepository _proveedorRepository;
        private ICajaAperturaRepository _cajaAperturaRepository;
        private IDocCompraRepository _docCompraRepository;
        public UnitOfWork(IConnection connection)
        {
            _connection = connection;
        }
        public void BeginTransaction()
        {
            _transaction = _connection.DbConnection.BeginTransaction();
        }
        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }
        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }
        public void Dispose() => _transaction?.Dispose();

        public SqlTransaction Transaction
        {
            get { return _transaction; }
        }
        public IProveedorRepository ProveedorRepository
        {
            get
            {
                return _proveedorRepository ?? (_proveedorRepository = new ProveedorRepository(_connection));
            }
        }
        public ICajaAperturaRepository CajaAperturaRepository
        {
            get
            {
                return _cajaAperturaRepository ?? (_cajaAperturaRepository = new CajaAperturaRepository(_connection));
            }
        }
        public IDocCompraRepository DocCompraRepository
        {
            get
            {
                return _docCompraRepository ?? (_docCompraRepository = new DocCompraRepository(_connection));
            }
        }
    }
}
