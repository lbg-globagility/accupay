using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace AccuPay.Data.Services
{
    /// <summary>
    /// This should be replaced by using repositories and ef core calls instead of stored procedures
    /// </summary>
    public class StoredProcedureDataService
    {
        private readonly PayrollContext _context;

        public StoredProcedureDataService(PayrollContext context)
        {
            _context = context;
        }

        protected DataTable CallRawSql(string procedureCall)
        {
            DataTable dataTable = new DataTable();
            var connection = (MySqlConnection)_context.Database.GetDbConnection();
            var cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = procedureCall;

            DbDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}