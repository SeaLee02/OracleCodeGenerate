using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OracleDemo
{
    public class DbUtil
    {

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static List<DbTable> GetDbTables(string connectionString, string tables = "")
        {
            //查询条件，如果包含,就是要in,否则就是要like查询
            if (!string.IsNullOrEmpty(tables))
            {
                tables = ((!tables.Contains(",")) ? $" AND  utc.table_name like '{tables}%'  " : string.Format(" and utc.table_name in ('{0}')", tables.Replace(",", "','")));
            }
            else
            {
                tables = " AND 1=1 ";
            }

            string sql = $@"SELECT
    utc.TABLE_NAME TableName, utc.COMMENTS TableDesc,
     CAST(case when t.keyname is null then 0 else 1 end as NUMBER(1)) as HasPrimaryKey,
	t.keyname  TablePrimarkeyName,
    t.data_type TablePrimarkeyType,
    t.data_precision Precision,
    t.Data_Scale Scale
FROM
    user_tab_comments utc
    LEFT JOIN(
    SELECT DISTINCT
        cu.COLUMN_name KEYNAME,
        cu.table_name,
	    uc.data_type,
        uc.data_precision,
        uc.Data_Scale 
    FROM
        user_cons_columns cu,
        user_constraints au,
        user_tab_columns uc
    WHERE
        cu.constraint_name = au.constraint_name  and cu.table_name=uc.table_name and cu.COLUMN_name=uc.column_name
        AND au.constraint_type = 'P'
    ) t ON t.table_name = utc.table_name
WHERE
    utc.table_type = 'TABLE'
    AND utc.table_name != 'HELP'
                        AND utc.table_name NOT LIKE '%$%'
                        AND utc.table_name NOT LIKE 'LOGMNRC_%'
                        AND utc.table_name != 'LOGMNRP_CTAS_PART_MAP'
                        AND utc.table_name != 'LOGMNR_LOGMNR_BUILDLOG'
                        AND utc.table_name != 'SQLPLUS_PRODUCT_PROFILE'  {tables} ";

            DataTable dataTable = GetDataTable(connectionString, sql);
            List<DbTable> list = StringUtil.Mapper<DbTable>(dataTable);

            foreach (DbTable item in list)
            {
                item.DbColumns = GetDbColumns(connectionString, item.TableName, item);
            }

            return list;
        }

        public List<string> Nvarchar()
        {
            return new List<string>
            {
                "varchar","varchar2","nvarchar2","char","nchar","clob","long","nclob","rowid"
            };
        }

        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<DbColumn> GetDbColumns(string connectionString, string tableName, DbTable table = null)
        {
            string sql = $@" select a.Table_name TableName,a.column_name ColumnName,a.data_type  ColumnType,(case a.data_type when 'VARCHAR' then a.data_length/2
when 'VARCHAR2'then a.data_length/2
when 'NVARCHAR2' then a.data_length/2
when 'CHAR' then a.data_length/2
when 'NCHAR'then a.data_length/2
when 'CLOB' then a.data_length/2
 when 'LONG'then a.data_length/2
when 'NCLOB'then a.data_length/2
when 'ROWID'then a.data_length/2
else  a.data_length end) CharLength,
a.data_precision Precision,
a.Data_Scale Scale,
CAST(case a.nullable when 'N' then 0 else 1 end as NUMBER(1)) as IsNullable
,a.column_id ColumnID,b.comments ColumnDesc
    from user_tab_columns a left join user_col_comments b on a.TABLE_NAME = b.table_name and a.COLUMN_NAME = b.column_name   
     where a.table_name = '{tableName}' order by column_id ";
            DataTable dataTable = GetDataTable(connectionString, sql);
            List<DbColumn> list = StringUtil.Mapper<DbColumn>(dataTable);
            if (table != null)
            {
                var column = list.Find(x => x.ColumnName == table.TablePrimarkeyName);
                column.IsPrimaryKey = true;
            }
            return list;
        }

        /// <summary>
        /// 执行Sql语句获取DataTable
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string connectionString, string commandText, params OracleParameter[] parms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.Parameters.AddRange(parms);
                OracleDataAdapter adapter = new OracleDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }



    }
}
