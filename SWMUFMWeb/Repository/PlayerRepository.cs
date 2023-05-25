using Microsoft.VisualBasic;
using SWMUFMWeb.BaseUtility;
using SWMUFMWeb.Models;

namespace SWMUFMWeb.Repository
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        /// <summary>
        /// 根据名字删除数据
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByPlayerNameAsync(string playerName)
        {
            return await SqlSugarHelper.Db.Deleteable<Player>().Where(x=>x.Name==playerName).ExecuteCommandHasChangeAsync();
        }
        /// <summary>
        /// 根据主键修改单条数据
        /// </summary>
        /// <param name="player"></param>
        /// <returns>修改数量</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> UpdateByIdAsync(Player player)
        {
            return await SqlSugarHelper.Db.Updateable(player).IgnoreColumns(x => new { x.GoalCount,
                x.AssistCount,x.YellowCardCount,x.RedCardCount})
                .ExecuteCommandAsync();
        }
    }
}
