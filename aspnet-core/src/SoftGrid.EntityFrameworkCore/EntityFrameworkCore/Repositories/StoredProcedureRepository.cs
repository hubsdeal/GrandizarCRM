using Abp.Data;
using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftGrid.Authorization.Users;
using SoftGrid.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace SoftGrid.EntityFrameworkCore.Repositories
{
    public interface IStoredProcedureRepository
    {
        Task<T> ExecuteStoredProcedure<T>(string commandText, CommandType commandType, params SqlParameter[] parameters);
        Task<T> ExecuteViewQuery<T>(string commandText, params SqlParameter[]? parameters);
    }

    public class StoredProcedureRepository : SoftGridRepositoryBase<User, long>, IStoredProcedureRepository
    {
        private readonly IActiveTransactionProvider _transactionProvider;
        private readonly IAppConfigurationAccessor _appConfigurationAccessor;
        private readonly IDbContextProvider<SoftGridDbContext> _dbContextProvider;
        public StoredProcedureRepository(IDbContextProvider<SoftGridDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider
           , IAppConfigurationAccessor appConfigurationAccessor)
            : base(dbContextProvider)
        {
            _transactionProvider = transactionProvider;
            _appConfigurationAccessor = appConfigurationAccessor;
            _dbContextProvider= dbContextProvider;
        }

        public async Task<T> ExecuteStoredProcedure<T>(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            EnsureConnectionOpen();
            using (var command = CreateCommand(commandText, CommandType.StoredProcedure, parameters))
            {
                command.CommandTimeout = 2000;
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    StringBuilder json = new StringBuilder();
                    while (dataReader.Read())
                    {
                        json.Append(dataReader["JSON_F52E2B61-18A1-11d1-B105-00805F49916B"].ToString());
                    }
                    return (JsonConvert.DeserializeObject<T>(json.ToString()));
                }
            }

        }

        public async Task<T> ExecuteViewQuery<T>(string commandText, params SqlParameter[]? parameters)
        {
            EnsureConnectionOpen();
            var createCommand = parameters != null ? CreateCommand(commandText, CommandType.Text, parameters) : CreateCommand(commandText, CommandType.Text);
            using (var command = createCommand)
            {
                command.CommandTimeout = 2000;
                using (var dataReader = await command.ExecuteReaderAsync())
                {
                    StringBuilder databaseViewDataJson = new StringBuilder();
                    while (dataReader.Read())
                    {
                        databaseViewDataJson.Append(dataReader["JSON_F52E2B61-18A1-11d1-B105-00805F49916B"].ToString());
                    }
                    return (JsonConvert.DeserializeObject<T>(databaseViewDataJson.ToString()));
                }
            }

        }

        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = _dbContextProvider.GetDbContext().Database.GetDbConnection().CreateCommand();

            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = GetActiveTransaction();

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        private void EnsureConnectionOpen()
        {
            var connection = _dbContextProvider.GetDbContext().Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
            {
                {"ContextType", typeof(SoftGridDbContext) },
                {"MultiTenancySide", MultiTenancySide }
            });
        }
    }
}
