using SWMUFMWeb.BaseUtility;
using SWMUFMWeb.Models;

namespace SWMUFMWeb.Repository
{
    public interface ILeagueScoreRepository:IBaseRepository<LeagueScore>
    {
        public Task<List<LeagueScore>> QueryLeagueScoreByLeagueId(int leagueId);
        public Task<bool> DeleteLeagueScoreByTwoId(int leagueId, int teamId);
        public Task<int> UpdateLeagueScore(int leagueId, int teamId ,int score);
    }
}
