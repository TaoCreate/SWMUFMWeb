using Microsoft.AspNetCore.Mvc;
using SWMUFMWeb.Models;
using SWMUFMWeb.Views.ViewModel;

namespace SWMUFMWeb.Controllers
{
	public class ArticleController : Controller
	{
		public IActionResult Index()
		{
			NavbarHelper.IsArticle();
			LayoutOfUser a = new LayoutOfUser
			{
				isHome = NavbarHelper.home,
				isArticle=NavbarHelper.article,
				isMatch=NavbarHelper.match,
				isRank=NavbarHelper.rank,
				isRegister=NavbarHelper.register,
				isTeam=NavbarHelper.team,
			};
            @ViewBag.userNickName = Request.Cookies["UserNickname"];
			
			List<Article> articleList=SqlSugarHelper.Db.Queryable<Article>().OrderByDescending(x=>x.ArticleDate).ToList();
			a.articleList= articleList;

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
			User user= SqlSugarHelper.Db.Queryable<User>().Where(x => x.UserId == article.UserId).ToList().First();
			a.article= article;

			ViewBag.articleId = articleId;
			ViewBag.userNickname = user.UserNickname;
			return View(a);
		}
		public IActionResult InsertComment(string commentContent, int articleId)
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
						MatchId = 0,
						ArticleId = articleId
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
		public async Task<IActionResult> InsertArticle(string articleTitle,string articleContent)
		{
			try
			{
				int num = await SqlSugarHelper.Db.Insertable<Article>(new Article
				{
					ArticleContent = articleContent,
					ArticleTitle = articleTitle,
					ArticleDate = DateTime.Now,
					UserId = Request.Cookies["UserId"],
				}).ExecuteCommandAsync();
				ViewBag.num=num;
				return View("CommentInsertSuc");
			}
			catch
			{
				return View("CommentInsertDef");
			}
		}
	}
}
