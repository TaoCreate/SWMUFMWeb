using SWMUFMWeb.BaseUtility;
using SWMUFMWeb.Models;

namespace SWMUFMWeb.Repository
{
    public interface IPlayerRepository:IBaseRepository<Player>
    {
        public Task<bool> DeleteByPlayerNameAsync(string playerName);
        public Task<int> UpdateByIdAsync(Player player);
    }
}
