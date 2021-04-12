using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace XTool
{
    public static class DataMapper
    {
        internal static DbType Map(string dataType, int maxLength)
        {
            DbType dbtype = DbType.Object;
            SqlDbType sqldbtype;
            if (Enum.TryParse<SqlDbType>(dataType, true, out sqldbtype))
            {
                try
                {
                    SqlParameter param = new SqlParameter() { SqlDbType = sqldbtype };
                    dbtype = param.DbType;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("dataType={0} could not be resolved. Error={1}", ex.Message));
                }

            }
            return dbtype;

        }
    }
}
