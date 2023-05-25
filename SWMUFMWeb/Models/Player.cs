using SqlSugar;

namespace SWMUFMWeb.Models
{
    public class Player
    {
        [SugarColumn(IsPrimaryKey = true)] //数据库是自增才配自增
        public int PlyerId { get; set; }
        public string Name { get; set; }
        public string TeamName { get; set; }
        public int TeamNumber { get; set; }
        public string Position { get; set; }
        public int Height { get; set; }
        public int Weight {get; set;}
        public string Class { get; set; }
        public string Origin { get; set; }
        [SugarColumn(IsNullable = true)]
        public int GoalCount { get; set; }
        [SugarColumn(IsNullable = true)]
        public int AssistCount { get; set; }
        [SugarColumn(IsNullable = true)]
        public int YellowCardCount { get; set; }
        [SugarColumn(IsNullable = true)]
        public int RedCardCount { get; set; }
    }
}
