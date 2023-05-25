using SqlSugar;
using System.Security.Principal;

namespace SWMUFMWeb.Models
{
    public class Comment
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int CommentId { get; set; }
        public string CommentContent { get; set; }
        public string UserId { get;set; }
        public DateTime CommentDate { get; set; }
        public int MatchId { get; set; }
        public int ArticleId { get; set; }

    }
}
