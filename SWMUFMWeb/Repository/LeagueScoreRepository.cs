using SWMUFMWeb.BaseUtility;
using SWMUFMWeb.Models;

namespace SWMUFMWeb.Repository
{
    public class LeagueScoreRepository : BaseRepository<LeagueScore>, ILeagueScoreRepository
    {
        public async Task<bool> DeleteLeagueScoreByTwoId(int leagueId, int teamId)
        {
            return await SqlSugarHelper.Db.Deleteable<LeagueScore>().Where(x=>x.LeagueId==leagueId&&x.TeamId==teamId).ExecuteCommandHasChangeAsync();
        }

        public async Task<List<LeagueScore>> QueryLeagueScoreByLeagueId(int leagueId)
        {
            return await SqlSugarHelper.Db.Queryable<LeagueScore>().Where(x=>x.LeagueId==leagueId).ToListAsync();
        }

        public async Task<int> UpdateLeagueScore(int leagueId, int teamId, int score)
        {
             return await SqlSugarHelper.Db.Updateable<LeagueScore>()
                .SetColumns(x=>new LeagueScore { Score=score})
                .Where(x=>x.LeagueId==leagueId&&x.TeamId== teamId)
                .ExecuteCommandAsync();
        }
    }
}
