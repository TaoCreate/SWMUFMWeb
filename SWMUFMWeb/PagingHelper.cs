using System.Drawing.Printing;

namespace SWMUFMWeb
{
    public static class PagingHelper<T>
    {   
        /// <summary>
        /// 将数据列表分页
        /// </summary>
        /// <param name="tList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> Paging(List<T> tList, int pageIndex, int pageSize)
        {
            List<T> list = tList
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToList();
            return list;
        }
        /// <summary>
        /// 计算分出的页面总和
        /// </summary>
        /// <param name="count"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int GetTotalPage(int count,int pageSize)
        {
            int totalPage = count / pageSize;
            float comparePage = (float)count / pageSize;
            if (totalPage < comparePage)
            {
                totalPage += 1;
            }
            return totalPage;
        }
        public static List<T> OrderList(List<T> tList,Func<T,int> func)
        {
            return tList.OrderBy(func).ToList();
        }
    }
}
