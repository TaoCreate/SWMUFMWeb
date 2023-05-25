using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class LeagueScore
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true)]
        public int LeagueScoreId { get; set; }
        public int TeamId { get; set; }
        public int Score { get; set; }
        public int LeagueId { get; set; }
    }
}
