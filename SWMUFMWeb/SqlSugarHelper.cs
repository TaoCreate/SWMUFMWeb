using SqlSugar;

namespace SWMUFMWeb
{
    public class SqlSugarHelper
    {
        private static WebApplicationBuilder builder = WebApplication.CreateBuilder();
        public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
        {
            DbType = DbType.SqlServer,
            ConnectionString = builder.Configuration["ConnectionString"],
            IsAutoCloseConnection = true
        });
    }
}
