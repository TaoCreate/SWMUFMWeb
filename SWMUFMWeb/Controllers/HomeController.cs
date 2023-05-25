
using Microsoft.AspNetCore.Mvc;
using SWMUFMWeb.Models;
using SWMUFMWeb.Views.ViewModel;
using System.Diagnostics;

namespace SWMUFMWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
			NavbarHelper.IsHome();
			LayoutOfUser a = new LayoutOfUser
			{
				isHome = NavbarHelper.home,
				isArticle = NavbarHelper.article,
				isMatch = NavbarHelper.match,
				isRank = NavbarHelper.rank,
				isRegister = NavbarHelper.register,
				isTeam = NavbarHelper.team,
			};
            List<Match> ml= SqlSugarHelper.Db.Queryable<Match>()
                .Where(x => x.MatchDate.Year == DateTime.Now.Year&& x.MatchDate.Month == DateTime.Now.Month&& x.MatchDate.Day == DateTime.Now.Day&&x.IsOver==false)
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
                }) ; 
            }
            
            a.matchAndTeamNameList = matchAndTeamNames;
			List<Article> articleList = SqlSugarHelper.Db.Queryable<Article>().OrderByDescending(x => x.ArticleDate).ToList();
            if (articleList.Count > 8)
            {
                articleList.GetRange(0, 8);
            }
			a.articleList = articleList;
			@ViewBag.userNickName = Request.Cookies["UserNickname"];
            return View(a);
        }

		public IActionResult ShowArticle(int articleId)
		{
			LayoutOfUser a = new LayoutOfUser();

			List<Comment_Nickname> comment_Nicknames = new List<Comment_Nickname>();

			List<Comment> comments = SqlSugarHelper.Db.Queryable<Comment>().Where(x => x.ArticleId == articleId).OrderByDescending(x => x.CommentDate).ToList();

			foreach (var item in comments)
			{
				comment_Nicknames.Add(new Comment_Nickname
				{
					Comment = item,
					Nickname = SqlSugarHelper.Db.Queryable<User>().Where(x => x.UserId == item.UserId).ToList().First().UserNickname
				});
			}
			a.commentAndNickname = comment_Nicknames;

			Article article = SqlSugarHelper.Db.Queryable<Article>().Where(x => x.ArticleId == articleId).ToList().First();
			User user = SqlSugarHelper.Db.Queryable<User>().Where(x => x.UserId == article.UserId).ToList().First();
			a.article = article;

			ViewBag.articleId = articleId;
			ViewBag.userNickname = user.UserNickname;
			return View("/Views/Article/ShowArticle.cshtml", a);
		}
	}
}