using SqlSugar;
using SWMUFMWeb.Models;

namespace SWMUFMWeb.BaseUtility
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await SqlSugarHelper.Db.Deleteable<T>().In(id).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            return await SqlSugarHelper.Db.Deleteable<T>().In(id).ExecuteCommandHasChangeAsync();
        }

        public async Task<T> FindAsync(int id)
        {
            return await SqlSugarHelper.Db.Queryable<T>().InSingleAsync(id);
        }

        public async Task<int> InsertAsync(T entity)
        {
            return await SqlSugarHelper.Db.Insertable(entity).ExecuteCommandAsync();
        }

        public async Task<int> InsertAsync(List<T> entity)
        {
            return await SqlSugarHelper.Db.Insertable(entity).ExecuteCommandAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            return await SqlSugarHelper.Db.Updateable(entity).ExecuteCommandAsync();
        }

        public List<T> Query()
        {
            return SqlSugarHelper.Db.Queryable<T>().ToList();
        }

        public List<int> GetTeamId()
        {
            return SqlSugarHelper.Db.Queryable<Team>().Select(x => x.TeamId).ToList();
        }

        public List<int> GetCupId()
        {
            return SqlSugarHelper.Db.Queryable<Cup>().Select(x=>x.CupId).ToList();
        }

        public List<int> GetLeagueId()
        {
            return SqlSugarHelper.Db.Queryable<League>().Select(x => x.LeagueId).ToList();
        }
    }
}
