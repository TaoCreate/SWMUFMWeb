using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class Match
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int MatchId { get; set; }
        public int TeamId1 { get; set; }
        public int TeamId2 { get; set;}
        public DateTime MatchDate { get; set; }
        public string MatchPosition { get; set; }
        [SugarColumn(IsNullable =true)]
        public int LeagueId{ get; set; }
        [SugarColumn(IsNullable = true)]
        public int  CupId { get; set;}
        public bool IsOver { get; set; }
    }
}
