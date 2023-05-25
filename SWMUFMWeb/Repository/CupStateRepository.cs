using SWMUFMWeb.BaseUtility;
using SWMUFMWeb.Models;

namespace SWMUFMWeb.Repository
{
    public class CupStateRepository : BaseRepository<CupState>, ICupStateRepository
    {
        public async Task<bool> DeleteByTeamIdAndCupId(int teamId, int cupId)
        {
            return await SqlSugarHelper.Db.Deleteable<CupState>()
                .Where(x => x.TeamId == teamId && x.CupId == cupId)
                .ExecuteCommandHasChangeAsync();
        }

        public async Task<List<CupState>> QueryByCupId(int cupId)
        {
            return await SqlSugarHelper.Db.Queryable<CupState>().Where(it => it.CupId==cupId).ToListAsync();
        }

        public async Task<int> UpdateByTeamIdAndCupId(int teamId, int cupId, string group, int groupScore,bool isKnockout)
        {
            return await SqlSugarHelper.Db.Updateable<CupState>()
                .SetColumns(x => new CupState {Group=group,GroupScore=groupScore,IsKnockout=isKnockout})
                .Where(x=>x.TeamId==teamId&&x.CupId==cupId)
                .ExecuteCommandAsync();
        }
    }
}
