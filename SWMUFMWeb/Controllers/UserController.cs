using Microsoft.AspNetCore.Mvc;
using SWMUFMWeb.Models;
using SWMUFMWeb.Views.ViewModel;
using System.Net;

namespace SWMUFMWeb.Controllers
{
    public class UserController : Controller
    {
        public static string? userId;
        public IActionResult Index()
        {
			NavbarHelper.IsRegister();
			LayoutOfUser a = new LayoutOfUser
			{
				isHome = NavbarHelper.home,
				isArticle = NavbarHelper.article,
				isMatch = NavbarHelper.match,
				isRank = NavbarHelper.rank,
				isRegister = NavbarHelper.register,
				isTeam = NavbarHelper.team,
			};
			ViewBag.userNickName = Request.Cookies["UserNickname"];
            return View(a);
        }

		public IActionResult Login(string userId,string userPassword)
		{
			try
			{
				User userInfo = SqlSugarHelper.Db.Queryable<User>().Where(x => x.UserId == userId).First();
				if (userInfo == null)
				{
					return View("LoginDef");
                }
				if (userInfo.UserId == "manager" && userInfo.UserPassword == "manager123")
				{
					Response.Cookies.Append("UserId", userInfo.UserId);
                    Response.Cookies.Append("UserPassword", userInfo.UserPassword);
                    Response.Cookies.Append("UserNickName", userInfo.UserNickname);
                    return View("/Views/Manager/Index.cshtml");
				}
				if (userPassword == userInfo.UserPassword)
				{
                    Response.Cookies.Append("UserId", userInfo.UserId);
                    Response.Cookies.Append("UserPassword", userInfo.UserPassword);
                    Response.Cookies.Append("UserNickName", userInfo.UserNickname);
                    return View("LoginSuc");
				}
				return View("LoginDef");
			}
			catch
			{
                return View("LoginDef");
            }
		}
		public IActionResult Register()
		{
			return View();
		}
		public async Task<IActionResult> RegisterIn(string userId,string userPassword,string userNickname)
		{
			try
			{
				int num=await SqlSugarHelper.Db.Insertable(new User
				{
					UserId = userId,
					UserPassword = userPassword,
					UserNickname = userNickname
				}).ExecuteCommandAsync();
				return View("RegisterSuc");
			}
			catch
			{
				return View("RegisterDef");
			}
		}
	}
}
