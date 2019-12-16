using MySql.Data.MySqlClient;
using SampleBioK30.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccupayFingerPrintApp.Repo
{
    internal class MachineInfoRepo
    {
        private static readonly string tableName = "machinelog";
        //string viewName = "";

        public static List<MachineInfo> GetMachineLogs()
        {

            var dbConnection = new DBConnection();
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT MachineNumber, IndRegID, DateTimeRecord, InOutMode")
                    .Append(" FROM " + tableName);
                var command = dbConnection.GetCommand(sb.ToString(), connection);
                var reader = command.ExecuteReader();
                var list = dbConnection.GetList(reader, new MachineInfo());
                return list;
            }
        }


        public static void InsertMachineLogs(List<MachineInfo> machineInfos)
        {
            var dbConnection = new DBConnection();
            var lastInsertedLog = GetLastInsertedItem();
            if (lastInsertedLog != null)
            {
                var lastDateInserted = DateTime.Parse(lastInsertedLog.DateTimeRecord);
                machineInfos = machineInfos.Where(mi => DateTime.Parse(mi.DateTimeRecord) > lastDateInserted).ToList();
            }
            if (machineInfos.Count > 0)
            {
                InsertLogs(machineInfos, dbConnection, lastInsertedLog);
            }
        }

        private static void InsertLogs(List<MachineInfo> machineInfos, DBConnection dbConnection, MachineInfo lastInsertedLog)
        {
            using (var connection = dbConnection.GetConnection())
            {
                MySqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    // Remove Flag 
                    if (lastInsertedLog != null)
                    {
                        RemoveLastInserted(dbConnection, connection);
                    }
                    // Insert Value
                    BulkInsert(machineInfos, dbConnection, connection);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

            }
        }

        private static void RemoveLastInserted(DBConnection dbConnection, MySqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" UPDATE machinelog")
                .Append(" SET IsLastInserted = 0")
                .Append(" WHERE IsLastInserted = 1");
            var updateCommand = dbConnection.GetCommand(sb.ToString(), connection);
            updateCommand.ExecuteNonQuery();
        }

        private static void BulkInsert(List<MachineInfo> machineInfos, DBConnection dbConnection, MySqlConnection connection)
        {
            var sb = new StringBuilder();
            sb.Append(" INSERT INTO " + tableName)
                .Append(" (MachineNumber, IndRegID, DateTimeRecord, InOutMode, DateOnlyRecord, TimeOnlyRecord, IsLastInserted)")
                .Append(" VALUES");
            var insertCount = machineInfos.Count;
            var sqlParams = new MySqlParameter[insertCount, 6];
            for (int i = 0; i < insertCount; i++)
            {
                sb.Append(" (")
                    .Append(" @MachineNumber" + i + ",")
                    .Append(" @IndRegID" + i + ",")
                    .Append(" @DateTimeRecord" + i + ",")
                    .Append(" @InOutMode" + i + ",")
                    .Append(" @DateOnlyRecord" + i + ",")
                    .Append(" @TimeOnlyRecord" + i + ",")
                    .Append(" 1")
                    .Append(" )");
                if (i < insertCount - 1)
                {
                    sb.Append(",");
                }
                sqlParams[i, 0] = new MySqlParameter("@MachineNumber" + i, machineInfos[i].MachineNumber);
                sqlParams[i, 1] = new MySqlParameter("@IndRegID" + i, machineInfos[i].IndRegID);
                sqlParams[i, 2] = new MySqlParameter("@DateTimeRecord" + i, machineInfos[i].DateTimeRecord);
                sqlParams[i, 3] = new MySqlParameter("@InOutMode" + i, machineInfos[i].InOutMode);
                sqlParams[i, 4] = new MySqlParameter("@DateOnlyRecord" + i, machineInfos[i].DateOnlyRecord);
                sqlParams[i, 5] = new MySqlParameter("@TimeOnlyRecord" + i, machineInfos[i].TimeOnlyRecord);
            }

            var insertCommand = dbConnection.GetCommand(sb.ToString(), connection);
            insertCommand.Parameters.AddRange(sqlParams);
            insertCommand.ExecuteNonQuery();
        }



        public static MachineInfo GetLastInsertedItem()
        {
            var dbConnection = new DBConnection();
            MachineInfo machineInfo = null;
            using (var connection = dbConnection.GetConnection())
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append(" SELECT MachineNumber, IndRegID, DateTimeRecord, InOutMode")
                    .Append(" FROM machinelog")
                    .Append(" WHERE IsLastInserted = 1")
                    .Append(" ORDER BY RowID DESC LIMIT 1");
                var command = dbConnection.GetCommand(sb.ToString(), connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    machineInfo = dbConnection.CreateObjectFromReader(reader, new MachineInfo());
                }

                return machineInfo;
            }
        }


        //private List<MachineInfo> GetListFromDataReader(MySqlDataReader reader)
        //{
        //    var list = new List<MachineInfo>();
        //    while (reader.Read())
        //    {
        //        var machineInfo = CreateMachineInfoFromDataReader(reader);
        //        list.Add(machineInfo);
        //    }
        //    return list;
        //}



        //private MachineInfo CreateMachineInfoFromDataReader(MySqlDataReader reader)
        //{
        //    var machineInfo = new MachineInfo();
        //    machineInfo.MachineNumber = int.Parse(reader[0].ToString());
        //    machineInfo.IndRegID = reader[1].ToString();
        //    machineInfo.DateTimeRecord = reader[2].ToString();
        //    machineInfo.InOutMode = int.Parse(reader[3].ToString());
        //    return machineInfo;
        //}
    }
}
