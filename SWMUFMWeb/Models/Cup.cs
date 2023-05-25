using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class Cup
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int CupId { get; set; } 
        public string CupName { get; set;}
        public bool IsOver { get; set; }
    }
}
