using SWMUFMWeb.Models;

namespace SWMUFMWeb.Views.ViewModel
{
    public class Match_Team
    {
        public Match match { get; set; }
        public Team team1 { get; set; }
        public Team team2 { get; set; }
        public MatchResult matchResult { get; set; }
    }
}
