using Microsoft.AspNetCore.Mvc;
using SWMUFMWeb.Models;
using SWMUFMWeb.Views.ViewModel;

namespace SWMUFMWeb.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
			NavbarHelper.IsTeam();
			LayoutOfUser a = new LayoutOfUser
			{
				isHome = NavbarHelper.home,
				isArticle = NavbarHelper.article,
				isMatch = NavbarHelper.match,
				isRank = NavbarHelper.rank,
				isRegister = NavbarHelper.register,
				isTeam = NavbarHelper.team,
			};
            @ViewBag.userNickName = Request.Cookies["UserNickname"];
			List<Team> teamList=SqlSugarHelper.Db.Queryable<Team>().OrderBy(x=>x.TeamId).ToList();
            a.teamList= teamList;
			return View(a);
        }
		public IActionResult SearchTeam(string teamName = " ")
		{
			LayoutOfUser a = new LayoutOfUser
			{
				isHome = NavbarHelper.home,
				isArticle = NavbarHelper.article,
				isMatch = NavbarHelper.match,
				isRank = NavbarHelper.rank,
				isRegister = NavbarHelper.register,
				isTeam = NavbarHelper.team,
			};
			@ViewBag.userNickName = Request.Cookies["UserNickname"];
			List<Team> teams = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamName == teamName).ToList();
			a.teamList= teams;
			return View(a);
		}
		public IActionResult ReturnPlayers(string teamName = " ")
		{
			LayoutOfUser a = new LayoutOfUser
			{
				isHome = NavbarHelper.home,
				isArticle = NavbarHelper.article,
				isMatch = NavbarHelper.match,
				isRank = NavbarHelper.rank,
				isRegister = NavbarHelper.register,
				isTeam = NavbarHelper.team,
			};
			@ViewBag.userNickName = Request.Cookies["UserNickname"];
			
			List<Player> players = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.TeamName == teamName).ToList();
			a.playerList= players;
			return View(a);
		}

	}
}
