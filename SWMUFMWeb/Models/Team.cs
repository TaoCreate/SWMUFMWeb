using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class Team
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        [SugarColumn(IsNullable  = true)]
        public int Goal { get; set; }
    }
}
