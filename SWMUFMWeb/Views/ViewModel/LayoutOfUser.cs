using SWMUFMWeb.Models;

namespace SWMUFMWeb.Views.ViewModel
{
	public class LayoutOfUser
	{
		public string isHome { get; set; }
		public string isTeam { get; set; }
		public string isRegister { get; set; }
		public string isRank { get; set; }
		public string isMatch { get; set; }
		public string isArticle { get; set; }
		public Article article { get; set; }
		public List<TeamName_Match> matchAndTeamNameList { get; set; }
		public List<Comment_Nickname> commentAndNickname { get; set; }
		public List<Article> articleList { get; set; }
		public List<Team> teamList { get; set; }
		public List<Team> teamGoalRank { get; set; }
		public List<Player> playerList { get; set; }
		public List<Player> playerGoalRank { get; set; }
		public List<Player> playerAssistRank { get; set; }
		public List<Player> playerRedCardRank { get; set; }
		public List<Player> playerYellowCardRank { get; set; }
		public List<Cup> cupList { get; set; }
		public List<League> leagueList { get; set; }
		public List<MatchResult> matchResultList { get; set; }
		public List<Comment> commentList { get; set; }
		public List<LeagueScore_Teamname> leagueScore_Teamnames { get; set; }
		public List<CupStates_Teamname> cupStates_Teamnames { get; set; }
	}
}
