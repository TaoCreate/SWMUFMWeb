using SWMUFMWeb.BaseUtility;
using SWMUFMWeb.Models;

namespace SWMUFMWeb.Repository
{
    public interface ICupStateRepository: IBaseRepository<CupState>
    {
        public Task<List<CupState>> QueryByCupId(int cupId);
        public Task<bool> DeleteByTeamIdAndCupId(int teamId,int cupId);
        public Task<int> UpdateByTeamIdAndCupId(int teamId, int cupId, string group, int groupScore,bool isKnockout);
    }
}
