using SqlSugar;
using SWMUFMWeb.BaseUtility;
using SWMUFMWeb.Models;
using SWMUFMWeb.Repository;
using SWMUFMWeb.Repository.MatchRepository;

namespace SWMUFMWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ITeamRepository,TeamRepository>();
            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<ICupRepository, CupRepository>();
            builder.Services.AddScoped<ICupStateRepository, CupStateRepository>();
            builder.Services.AddScoped<ILeagueRepository, LeagueRepository>();
            builder.Services.AddScoped<ILeagueScoreRepository, LeagueScoreRepository>();
            builder.Services.AddScoped<IMatchRepository, MatchRepository>();

            //创建数据库
            SqlSugarHelper.Db.DbMaintenance.CreateDatabase();
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(Player));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(Team));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(Match));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(MatchResult));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(Article));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(Comment));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(Cup));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(CupState));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(League));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(LeagueScore));
            SqlSugarHelper.Db.CodeFirst.InitTables(typeof(User));

            //创建管理员账号
            List<User> user= SqlSugarHelper.Db.Queryable<User>().Where(x => x.UserId == "manager").ToList();
            if (user.Count() == 0)
            {
                int num=SqlSugarHelper.Db.Insertable(new User {UserId="manager",UserPassword="manager123",UserNickname="manager" }).ExecuteCommand();
                Console.WriteLine(num);
            }

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}