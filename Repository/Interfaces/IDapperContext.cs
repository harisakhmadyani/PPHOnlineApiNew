using System;
using System.Data;

namespace newplgapi.Repository.Interfaces
{
    public interface IDapperContext : IDisposable
    {
        IDbConnection db { get; }
		IDbTransaction transaction { get; }
		// bool IsOpenConnection();
        // bool ExecSQL(string sql);
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        string GetGUID();
    }
}