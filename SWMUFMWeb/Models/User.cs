using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class User
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public string UserNickname { get; set; }
    }
}
 