using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class League
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int LeagueId { get; set; }
        public string LeagueName { get; set;}
        public bool IsOver { get; set; }

    }
}
