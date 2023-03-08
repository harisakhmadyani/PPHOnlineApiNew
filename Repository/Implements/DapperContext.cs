using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using newplgapi.Repository.Interfaces;

namespace newplgapi.Repository.Implements
{
    public class DapperContext : IDapperContext
    {
        private IDbConnection _db;
        private IDbTransaction _transaction;
        private readonly string _providerName;
        private readonly string _connectionString;

        public DapperContext()
        {
        }

        public DapperContext(string factory)
        {
            _providerName = "Microsoft.Data.SqlClient";
            //_connectionString = "Data Source=192.168.12.5; Initial Catalog=RSUPDB; User ID=uKoneksi; Password=sm@rt2018; MultipleActiveResultSets=True;";

            if (factory == "RSUP")
                _connectionString = "Data Source=192.168.12.5; Initial Catalog=RSUPDB; User ID=uKoneksi; Password=sm@rt2018; MultipleActiveResultSets=True; Connection Timeout=1200";
            else if (factory == "PSG")
                _connectionString = "Data Source= 192.168.2.3; Initial Catalog=MyPSG; User ID=appkoneksi; Password=app@1psg; MultipleActiveResultSets=True; Connection Timeout=1200";
            else if (factory == "STI")
                _connectionString = "Data Source=192.168.3.33; Initial Catalog=PKBDB; User ID=sa; Password=sti2016.com; MultipleActiveResultSets=True;";
            else
                _connectionString = "Data Source=192.168.9.9; Initial Catalog=PKBDB; User ID=sa; Password=p@ssw0rd; MultipleActiveResultSets=True;";

            if(_db == null){
                _db = GetOpenConnection(_providerName, _connectionString);
            }
        }

        public IDbConnection db  {
            get { return _db ?? (_db = GetOpenConnection(_providerName, _connectionString)); }
        }

        private IDbConnection GetOpenConnection(string providerName, string connectionString)
        {
            DbConnection conn = null;

            try
            {
                // DbProviderFactory provider = DbProviderFactories.GetFactory(providerName);
                SqlClientFactory provider = SqlClientFactory.Instance;
                conn = provider.CreateConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
            }
            catch
            {
            }

            return conn;
        }

        public IDbTransaction transaction {
             get { return _transaction; }
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_transaction == null)
                _transaction = _db.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction = null;
            }  
        }

        public string GetGUID()
        {
            var result = string.Empty;

            try
            {
                result = Guid.NewGuid().ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }    
        }

        public void Dispose()
        {
            if (_db != null)
            {
                try
                {
                    if (_db.State != ConnectionState.Closed)
                    {
                        if (_transaction != null)
                        {
                            _transaction.Rollback();
                        }

                        _db.Close();
                    }                        
                }
                finally
                {
                    _db.Dispose();
                }
            }

            GC.SuppressFinalize(this);
        }
    }
}