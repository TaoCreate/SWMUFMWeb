using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class Article
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        [SugarColumn(Length =2000)]
        public string ArticleContent { get; set; }
        public DateTime ArticleDate { get; set; }
        public string UserId { get; set; }
    }
}
