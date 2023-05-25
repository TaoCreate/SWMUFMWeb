namespace SWMUFMWeb.BaseUtility
{
    public interface IBaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// 单条数据插入
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>返回数据库受影响的行数</returns>
        Task<int> InsertAsync(T entity);
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entity">数据实体列表</param>
        /// <returns>返回数据库受影响的行数</returns>
        Task<int> InsertAsync(List<T> entity);
        /// <summary>
        /// 根据主键修改数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>数据是否改变</returns>
        Task<int> UpdateAsync(T entity);
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="id">主键的值为int</param>
        /// <returns>删除是否成功</returns>
        Task<bool> DeleteByIdAsync(int id);
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="id">字符串主键</param>
        /// <returns>是否成功删除</returns>
        Task<bool> DeleteByIdAsync(string id);
        /// <summary>
        /// 根据id查询单条数据
        /// </summary>
        /// <param name="id">id主键</param>
        /// <returns>数据实体</returns>
        Task<T> FindAsync(int id);
        /// <summary>
        /// 查询所有内容
        /// </summary>
        /// <returns></returns>
        List<T> Query();
        List<int> GetTeamId();
        List<int> GetCupId();
        List<int> GetLeagueId();
    }
}
