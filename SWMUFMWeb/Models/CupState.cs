using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class CupState
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true)]
        public int CupStateId { get; set; }
        public int TeamId { get; set; }
        public string Group { get; set; }
        public int GroupScore { get; set; }
        public bool IsKnockout { get; set; }
        public int CupId { get; set; }

    }
}
