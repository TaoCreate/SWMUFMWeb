using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class MatchResult
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ResultId { get; set; }
        public int MatchId { get; set; }
        //平局输入0
        public int WinTeamId { get; set; }
        public int Team1Goal { get; set; }
        public int Team2Goal { get; set;}
        //格式为“id;id;id   无进球输入none”
        public string PlayerIdGoal { get; set; }
        public string PlayerIdAssists { get; set; }
        //“id;id;id  无黄牌输入none”
        public string PlayerIdYellowcard { get; set; }
        public string PlayerIdRedcard { get; set;}

    }
}
