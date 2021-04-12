using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chronometrix;
using System.Data.SqlClient;

namespace XTool
{
    public static class TimekeeperManager
    {
        public static List<Task> Get(string identity)
        {
            List<Task> list = new List<Task>();

            list = SqlHelper.ExecuteReader<Task>(GetConnection,GetTasksCommand,BorrowTasks);

            return list;
        }



        private static SqlConnection GetConnection()
        {
            return SqlConnectionProvider.GetConnection("");
        }

        #region Task
        private  static void GetTasksCommand(SqlCommand cmd)
        {
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from [dbo].[Task]; select * from [dbo].[Assignment]";
        }

        private static void BorrowTasks(SqlDataReader reader, List<Task> list)
        {
            throw new NotImplementedException();
        }
        #endregion











    }
}
