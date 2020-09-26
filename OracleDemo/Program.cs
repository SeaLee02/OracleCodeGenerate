using System;

namespace OracleDemo
{
    class Program
    {

        const string connstr = "Data Source=localhost/orcl;User ID=LISAN;Password=1234;";


        static void Main(string[] args)
        {
            string sql = @"SELECT  table_name name from user_tables where
                        table_name!='HELP' 
                        AND table_name NOT LIKE '%$%'
                        AND table_name NOT LIKE 'LOGMNRC_%'
                        AND table_name!='LOGMNRP_CTAS_PART_MAP'
                        AND table_name!='LOGMNR_LOGMNR_BUILDLOG'
                        AND table_name!='SQLPLUS_PRODUCT_PROFILE'  
                         ";
            string sql2 = $@"SELECT
	utc.TABLE_NAME TableName,utc.COMMENTS TableDes,
utc.TABLESPACE_NAME Schemname,
	CAST(case when t.keyname is null then 0 else 1 end as NUMBER(1)) as HasKey,
	t.keyname 
FROM
	user_tab_comments utc
	LEFT JOIN (
	SELECT DISTINCT
		cu.COLUMN_name KEYNAME,
		cu.table_name 
	FROM
		user_cons_columns cu,
		user_constraints au 
	WHERE
		cu.constraint_name = au.constraint_name 
		AND au.constraint_type = 'P' 
	) t ON t.table_name = utc.table_name 
WHERE
	utc.table_type = 'TABLE' 
	AND   utc.table_name!='HELP' 
                        AND utc.table_name NOT LIKE '%$%'
                        AND utc.table_name NOT LIKE 'LOGMNRC_%'
                        AND utc.table_name!='LOGMNRP_CTAS_PART_MAP'
                        AND utc.table_name!='LOGMNR_LOGMNR_BUILDLOG'
                        AND utc.table_name!='SQLPLUS_PRODUCT_PROFILE'    ";
            //var ddd = DbUtil.GetDataTable(connstr, sql2);

            //var str = DbUtil.GetDbTables(connstr);

            string abc = @" select a.Table_name,a.column_name,a.data_type ,a.data_length ,a.data_precision ,a.Data_Scale ,a.nullable,a.column_id,b.comments
    from user_tab_columns a left join user_col_comments b on a.TABLE_NAME = b.table_name and a.COLUMN_NAME = b.column_name
     where a.table_name = 'Student' order by column_id ";
            //var ddd = DbUtil.GetDataTable(connstr, abc);

           var sql9= DbUtil.GetDbTables(connstr);
            Console.WriteLine("DDD");
            Console.ReadKey();
        }
    }
}
