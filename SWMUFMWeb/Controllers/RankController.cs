using Microsoft.AspNetCore.Mvc;
using SWMUFMWeb.Models;
using SWMUFMWeb.Views.ViewModel;

namespace SWMUFMWeb.Controllers
{
	public class RankController : Controller
	{
		public IActionResult Index()
		{
			NavbarHelper.IsRank();
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

			List<Player> playersGoalRank = SqlSugarHelper.Db.Queryable<Player>().OrderByDescending(x=>x.GoalCount).ToList();
			if (playersGoalRank.Count > 20)
			{
				playersGoalRank.GetRange(0, 20);
			}

			List<Player> playersAssistRank = SqlSugarHelper.Db.Queryable<Player>().OrderByDescending(x => x.AssistCount).ToList();
			if (playersAssistRank.Count > 20)
			{
				playersAssistRank.GetRange(0, 20);
			}

			List<Player> playersRedCardRank = SqlSugarHelper.Db.Queryable<Player>().OrderByDescending(x => x.RedCardCount).ToList();
			if (playersRedCardRank.Count > 20)
			{
				playersRedCardRank.GetRange(0, 20);
			}

			List<Player> playersYellowCardRank = SqlSugarHelper.Db.Queryable<Player>().OrderByDescending(x => x.YellowCardCount).ToList();
			if (playersYellowCardRank.Count > 20)
			{
				playersYellowCardRank.GetRange(0, 20);
			}

			List<Team> teamGoalRank = SqlSugarHelper.Db.Queryable<Team>().OrderByDescending(x => x.Goal).ToList();
			a.playerGoalRank=playersGoalRank;
			a.playerAssistRank=playersAssistRank;
			a.playerRedCardRank=playersRedCardRank;
			a.playerYellowCardRank=playersYellowCardRank;
			a.teamGoalRank=teamGoalRank;
			return View(a);
		}
	}
}
