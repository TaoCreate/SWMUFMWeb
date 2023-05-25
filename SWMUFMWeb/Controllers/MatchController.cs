using Microsoft.AspNetCore.Mvc;
using SWMUFMWeb.Models;
using SWMUFMWeb.Views.ViewModel;

namespace SWMUFMWeb.Controllers
{
	public class MatchController : Controller
	{
		public IActionResult Index()
		{
			NavbarHelper.IsMatch();
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
			List<Match> ml = SqlSugarHelper.Db.Queryable<Match>()
				.OrderByDescending(x=>x.MatchDate)
				.ToList();
			if (ml.Count >= 8)
			{
				ml.GetRange(0, 8);
			}
			List<TeamName_Match> matchAndTeamNames = new List<TeamName_Match>();
			foreach (var item in ml)
			{
				matchAndTeamNames.Add(new TeamName_Match
				{
					teamName1 = SqlSugarHelper.Db.Queryable<Team>()
						.Where(x => x.TeamId == item.TeamId1)
						.ToList()
						.First().TeamName,
					teamName2 = SqlSugarHelper.Db.Queryable<Team>()
						.Where(x => x.TeamId == item.TeamId2)
						.ToList()
						.First().TeamName,
					match = item
				});
			}
			a.matchAndTeamNameList = matchAndTeamNames;

			List<Cup> cups = SqlSugarHelper.Db.Queryable<Cup>().ToList();
			a.cupList= cups;

			List<League> leagues =SqlSugarHelper.Db.Queryable<League>().ToList();
			a.leagueList= leagues;

			return View(a);
		}
		public IActionResult ReturnMatchResult(int matchId,bool isOver,int teamId1,int teamId2)
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
			if (isOver)
			{
				MatchResult matchResult = SqlSugarHelper.Db.Queryable<MatchResult>().Where(x => x.MatchId == matchId).ToList().First();
				List<Comment_Nickname> comment_Nicknames = new List<Comment_Nickname>();
				
				List<Comment> comments = SqlSugarHelper.Db.Queryable<Comment>().Where(x =>x.MatchId==matchId ).OrderByDescending(x=>x.CommentDate).ToList();

				foreach (var item in comments)
				{
					comment_Nicknames.Add(new Comment_Nickname
					{
						Comment = item,
						Nickname=SqlSugarHelper.Db.Queryable<User>().Where(x=>x.UserId==item.UserId).ToList().First().UserNickname
					});
				}
				a.commentAndNickname= comment_Nicknames;
				if (matchResult.WinTeamId != 0)
				{
					ViewBag.winTeam = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == matchResult.WinTeamId).ToList().First().TeamName+"获胜";
				}
				else
				{
					ViewBag.winTeam = "平局";
				}
				ViewBag.team1 = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == teamId1).ToList().First().TeamName;
				ViewBag.team1Goal = matchResult.Team1Goal;
				ViewBag.team2 = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == teamId2).ToList().First().TeamName;
				ViewBag.team1Goa2 = matchResult.Team2Goal;
				ViewBag.matchId = matchId;
				return View(a);
			}
			else
			{
				return View("IsNotOver");
			}
		}
		public IActionResult InsertComment(string commentContent,int matchId)
		{
			try
			{
				if (Request.Cookies["UserId"] != null)
				{
					Comment comment = new Comment
					{
						CommentContent = commentContent,
						UserId = Request.Cookies["UserId"],
						CommentDate = DateTime.Now,
						MatchId = matchId,
						ArticleId = 0
					};

					int num = SqlSugarHelper.Db.Insertable<Comment>(comment).ExecuteCommand();
					return View("InsertSuc");
				}
				return View("InsertDef");
			}
			catch
			{
                return View("InsertDef");
            }
		}
		public async Task<IActionResult> ReturnCupMatch(int cupId,string cupName)
		{
			LayoutOfUser a=new LayoutOfUser();
			List<Match> ml = SqlSugarHelper.Db.Queryable<Match>()
				.Where(x=>x.CupId== cupId)
				.OrderByDescending(x => x.MatchDate)
				.ToList();
			List<TeamName_Match> matchAndTeamNames = new List<TeamName_Match>();
			foreach (var item in ml)
			{
				matchAndTeamNames.Add(new TeamName_Match
				{
					teamName1 = SqlSugarHelper.Db.Queryable<Team>()
						.Where(x => x.TeamId == item.TeamId1)
						.ToList()
						.First().TeamName,
					teamName2 = SqlSugarHelper.Db.Queryable<Team>()
						.Where(x => x.TeamId == item.TeamId2)
						.ToList()
						.First().TeamName,
					match = item
				});
			}
			a.matchAndTeamNameList = matchAndTeamNames;
			ViewBag.cupName = cupName;

			List<CupStates_Teamname> cupStates_Teamname =new List<CupStates_Teamname>();
			List<CupState> cupStates = await SqlSugarHelper.Db.Queryable<CupState>().Where(x => x.CupId == cupId)
				.OrderBy(x => x.Group)
				.ToListAsync();
			foreach (var item in cupStates)
			{
				cupStates_Teamname.Add(new CupStates_Teamname
				{
					teamname = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == item.TeamId).ToList().First().TeamName,
					cupState = item
				});
			}
			a.cupStates_Teamnames = cupStates_Teamname;
			if (SqlSugarHelper.Db.Queryable<Cup>().Where(x => x.CupId == cupId).ToList().First().IsOver == true)
			{
				ViewBag.leagueIsOver = "本杯赛已经结束";
			}
			ViewBag.leagueIsOver = "";
			ViewBag.isOut = "已淘汰";
			ViewBag.isNotOut = "未淘汰";
			return View(a);
		}
		public async Task<IActionResult> ReturnLeagueMatch(int leagueId,string leagueName)
		{
			LayoutOfUser a = new LayoutOfUser();
			List<Match> ml = await SqlSugarHelper.Db.Queryable<Match>()
				.Where(x => x.LeagueId == leagueId)
				.OrderByDescending(x => x.MatchDate)
				.ToListAsync();
			List<TeamName_Match> matchAndTeamNames = new List<TeamName_Match>();
			foreach (var item in ml)
			{
				matchAndTeamNames.Add(new TeamName_Match
				{
					teamName1 = SqlSugarHelper.Db.Queryable<Team>()
						.Where(x => x.TeamId == item.TeamId1)
						.ToList()
						.First().TeamName,
					teamName2 = SqlSugarHelper.Db.Queryable<Team>()
						.Where(x => x.TeamId == item.TeamId2)
						.ToList()
						.First().TeamName,
					match = item
				});
			}
			ViewBag.leagueName = leagueName;
			a.matchAndTeamNameList = matchAndTeamNames;

			List<LeagueScore_Teamname> leagueScore_Teamname=new List<LeagueScore_Teamname>();
			List<LeagueScore> leagueScores = await SqlSugarHelper.Db.Queryable<LeagueScore>().Where(x => x.LeagueId == leagueId)
				.OrderByDescending(x=>x.Score)
				.ToListAsync();
			foreach (var item in leagueScores)
			{
				leagueScore_Teamname.Add(new LeagueScore_Teamname
				{
					teamname=SqlSugarHelper.Db.Queryable<Team>().Where(x=>x.TeamId==item.TeamId).ToList().First().TeamName,
					leagueScore=item
				});
			}
			a.leagueScore_Teamnames=leagueScore_Teamname;

			if (SqlSugarHelper.Db.Queryable<League>().Where(x => x.LeagueId == leagueId).ToList().First().IsOver == true)
			{
				ViewBag.leagueIsOver = "本联赛已经结束";
			}
			ViewBag.leagueIsOver = "";
			ViewBag.userNickName = Request.Cookies["UserNickname"];
			return View(a);
		}
	}
}
