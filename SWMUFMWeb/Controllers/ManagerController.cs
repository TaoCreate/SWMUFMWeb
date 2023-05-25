using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Differencing;
using SWMUFMWeb.BaseUtility;
using SWMUFMWeb.Models;
using SWMUFMWeb.Repository;
using SWMUFMWeb.Repository.MatchRepository;
using SWMUFMWeb.Views.ViewModel;
using System;
using System.Diagnostics.Metrics;
using System.Drawing.Printing;

namespace SWMUFMWeb.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ITeamRepository _iTeamRepository;
        private readonly IPlayerRepository _iPlyerRepository;
        private readonly ICupRepository _iCupReposity;
        private readonly ICupStateRepository _iCupStateRepository;
        private readonly ILeagueRepository _iLeagueRepository;
        private readonly ILeagueScoreRepository _iLeagueScoreRepository;
        private readonly IMatchRepository _iMatchRepository;
        public ManagerController(ITeamRepository iTeamRepository, IPlayerRepository iPlayerRepository,
            ICupRepository iCupRepository, ICupStateRepository iCupStateRepository, ILeagueRepository iLeagueRepository
            , ILeagueScoreRepository iLeagueScoreRepository, IMatchRepository iMatchRepository)
        {
            _iTeamRepository = iTeamRepository;
            _iPlyerRepository = iPlayerRepository;
            _iCupReposity = iCupRepository;
            _iCupStateRepository = iCupStateRepository;
            _iLeagueRepository = iLeagueRepository;
            _iLeagueRepository = iLeagueRepository;
            _iLeagueScoreRepository = iLeagueScoreRepository;
            _iMatchRepository = iMatchRepository;
        }

        //主页
        public IActionResult Index()
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            return View();
        }

        //球队
        public IActionResult Team(int pageIndex = 1, int pageSize = 8)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            int count = _iTeamRepository.Query().Count();
            List<Team> teamList = _iTeamRepository.Query()
                .OrderBy(x => x.TeamId)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToList();
            //计算页面数量 页面数量向上取整
            int totalPage = count / pageSize;
            float comparePage = (float)count / pageSize;
            if (totalPage < comparePage)
            {
                totalPage += 1;
            }
            //将数据传入razor页面
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPage = totalPage;
            return View(teamList);
        }
        public async Task<IActionResult> Insert(int teamId, string teamName)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            //获取球队列表所有的主键Id 用来判断插入的数据是否与数据库中主键重复
            List<int> idList = SqlSugarHelper.Db.Queryable<Team>().Select(it => it.TeamId).ToList();
            if (idList.Contains(teamId))
            {
                return View("InsertDefeated");
            }
            int num = await _iTeamRepository.InsertAsync(new Team { TeamId = teamId, TeamName = teamName });
            ViewData["num"] = num;
            return View();
        }
        public async Task<IActionResult> Delete(int teamId)
        {
            bool isDeleted = await _iTeamRepository.DeleteByIdAsync(teamId);
            ViewData["isDelete"] = isDeleted;
            return View();
        }

        //球员
        public IActionResult Player(int pageIndex = 1, int pageSize = 8)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            //分页展示

            //数据条数总数
            int count = _iPlyerRepository.Query().Count();
            //获取数据条数
            List<Player> playerList = _iPlyerRepository.Query()
                .OrderBy(x => x.PlyerId) //按某列排序
                .Skip(pageSize * (pageIndex - 1)) //跳过的数据条数
                .Take(pageSize) //获取的数据条数
                .ToList();
            //计算页面数量 页面数量向上取整
            int totalPage = count / pageSize;
            float comparePage = (float)count / pageSize;
            if (totalPage < comparePage)
            {
                totalPage += 1;
            }
            //将数据传入razor页面
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPage = totalPage;

            List<Team> teamList = SqlSugarHelper.Db.Queryable<Team>().ToList();
            SelectList teamSelectList = new SelectList(teamList, "TeamName", "TeamName");
            ViewBag.teamSelectList = teamSelectList;
            return View(playerList);
        }
        //插入球员数据
        public async Task<IActionResult> InsertPlayer(IFormCollection collection,string teamName)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
				int num = await _iPlyerRepository.InsertAsync(new Player
				{
					PlyerId = Convert.ToInt32(collection["playerId"]),
					Name = collection["name"],
					TeamName = teamName,
					TeamNumber = Convert.ToInt32(collection["teamNumber"]),
					Position = collection["position"],
					Height = Convert.ToInt32(collection["height"]),
					Weight = Convert.ToInt32(collection["weight"]),
					Class = collection["class"],
					Origin = collection["origin"]
				});
				ViewData["num"] = num;
				Console.WriteLine();
				return View("Insert");
			}
            catch
            {
                return View("InsertDefeated");
            }
        }

        //pageSize是每页展示数量
        //public ActionResult PagePart(int pageIndex = 1, int pageSize = 5)
        //{
        //    //总量
        //    int count = _iTeamRepository.Query().Count();
        //    List<Team> rows = _iTeamRepository.Query()
        //        .OrderBy(x => x.TeamId)
        //        .Skip(pageSize * (pageIndex - 1))
        //        .Take(pageSize)
        //        .ToList();

        //    int totalPage = count / pageSize;
        //    float comparePage = (float)count/pageSize;

        //    //页面数量
        //    if(totalPage<comparePage)
        //    {
        //        totalPage += 1;
        //    }

        //    ViewBag.pageIndex = pageIndex;
        //    ViewBag.pageSize = pageSize;
        //    ViewBag.totalPage = totalPage;

        //    return View(rows);
        //}

        //删除球员
        public async Task<IActionResult> DeletePlayerByName(string name)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                bool isDeleted = await _iPlyerRepository.DeleteByPlayerNameAsync(name);
                ViewData["isDelete"] = isDeleted;
                return View("Delete");
            }
            catch
            {
                return View("InsertDefeated");
            }
        }

        public async Task<IActionResult> UpdatePlayerById(IFormCollection collection, string teamName)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                int num = await _iPlyerRepository.UpdateByIdAsync(new Player
                {
                    PlyerId = Convert.ToInt32(collection["playerId"]),
                    Name = collection["name"],
                    TeamName = teamName,
                    TeamNumber = Convert.ToInt32(collection["teamNumber"]),
                    Position = collection["position"],
                    Height = Convert.ToInt32(collection["height"]),
                    Weight = Convert.ToInt32(collection["weight"]),
                    Class = collection["class"],
                    Origin = collection["origin"]
                });
                ViewBag.num = num;
                return View("UpdateSuccessful");
            }
            catch
            {
                return View("UpdateDefeated");
            }
        }

        public ActionResult Cup(int pageIndex = 1, int pageSize = 8)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            int count = _iCupReposity.Query().Count();
            List<Cup> cupList = _iCupReposity.Query()
                .OrderBy(x => x.CupId)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToList();
            //计算页面数量 页面数量向上取整
            int totalPage = count / pageSize;
            float comparePage = (float)count / pageSize;
            if (totalPage < comparePage)
            {
                totalPage += 1;
            }
            //将数据传入razor页面
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPage = totalPage;

            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Text="是",Value="true"},
                new SelectListItem { Text = "否", Value = "false" }
            };
            ViewBag.boolSelectList = new SelectList(selectListItems, "Value", "Text");
            ViewBag.cupIsOver = "是";
            ViewBag.cupIsNotOver = "否";
            return View(cupList);
        }
        public async Task<IActionResult> InsertCup(int cupId, string cupName, bool isOver)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                int num = await _iCupReposity.InsertAsync(new Cup
                {
                    CupId = cupId,
                    CupName = cupName,
                    IsOver = isOver
                });
                ViewData["num"] = num;
                return View("Insert");
            }
            catch
            {
                return View("InsertDefeated");
            }
        }
        public async Task<IActionResult> DeleteCupById(int cupId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                bool isDeleted = await _iCupReposity.DeleteByIdAsync(cupId);
                ViewData["isDelete"] = isDeleted;
                return View("Delete");
            }
            catch
            {
                return View("DeleteDefeated");
            }
        }
        public async Task<IActionResult> UpdateCup(int cupId, string cupName, bool isOver)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                if (cupName == null)
                {
                    return View("UpdateDefeated");
                }
                int num = await _iCupReposity.UpdateAsync(new Cup
                {
                    CupId = cupId,
                    CupName = cupName,
                    IsOver = isOver
                });
                ViewBag.num = num;
                return View("UpdateSuccessful");
            }
            catch
            {
                return View("UpdateDefeated");
            }
        }

        public async Task<IActionResult> CupState(int cupId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            ViewBag.cupId = cupId;
            //已经在杯赛里的队伍信息下拉内容
            List<SelectListItem> cupTeamSelectListItems = new List<SelectListItem>();
            List<CupState> cupStatesList = await _iCupStateRepository.QueryByCupId(cupId);
            List<CupStates_Teamname> cupStates_Teamname= new List<CupStates_Teamname>();
            foreach (var item in cupStatesList)
            {
                cupStates_Teamname.Add(new CupStates_Teamname
                {
                    cupState = item,
                    teamname = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == item.TeamId).ToList().First().TeamName
                });
                //硬编码式
                cupTeamSelectListItems.Add(new SelectListItem
                {
                    Value = item.TeamId.ToString(),
                    Text= SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == item.TeamId).ToList().First().TeamName
                }); ;
                
            }
            List<Team> teams =await SqlSugarHelper.Db.Queryable<Team>().ToListAsync();
            SelectList teamSelectList = new SelectList(teams,"TeamId","TeamName");

            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Text="是",Value="true"},
                new SelectListItem { Text = "否", Value = "false" }
            };
            ViewBag.teamSelectList = teamSelectList;

            ViewBag.cupTeamSelectList=new SelectList(cupTeamSelectListItems, "Value", "Text");
            ViewBag.boolSelectList = new SelectList(selectListItems, "Value", "Text");
            
            ViewBag.isOut = "是";
            ViewBag.isNotOut = "否";
            return View(cupStates_Teamname);
        }

        public async Task<IActionResult> InsertCupState(IFormCollection collection, int cupId, string boolValue)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                List<int> teamIdList = _iCupStateRepository.GetTeamId();
                if (teamIdList.Contains(Convert.ToInt32(collection["teamId"])))
                {
                    int num = await _iCupStateRepository.InsertAsync(new CupState
                    {
                        TeamId = Convert.ToInt32(collection["teamId"]),
                        Group = collection["group"],
                        GroupScore = Convert.ToInt32(collection["groupScore"]),
                        IsKnockout = Convert.ToBoolean(boolValue),
                        CupId = cupId,
                    });
                    ViewData["num"] = num;
                    return View("Insert");
                }
                else
                {
                    return View("DontHaveTeamId");
                }

            }
            catch
            {
                return View("InsertDefeated");
            }
        }
        public async Task<IActionResult> DeleteCupStateById(int teamId, int cupId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                bool isDeleted = await _iCupStateRepository.DeleteByTeamIdAndCupId(teamId, cupId);
                ViewData["isDelete"] = isDeleted;
                return View("Delete");
            }
            catch
            {
                return View("DeleteDefeated");
            }
        }
        public async Task<IActionResult> UpdateCupState(int teamId, int cupId, string group, int groupScore, string boolValue)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                int num = await _iCupStateRepository.UpdateByTeamIdAndCupId(teamId, cupId, group, groupScore, Convert.ToBoolean(boolValue));
                ViewBag.num = num;
                if (num == 0)
                {
                    return View("UpdateDefeated");
                }
                return View("UpdateSuccessful");
            }
            catch
            {
                return View("UpdateDefeated");
            }
        }
        public ActionResult League(int pageIndex = 1, int pageSize = 8)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            List<League> pagingList = PagingHelper<League>.Paging(_iLeagueRepository.Query(), pageIndex, pageSize);
            List<League> leagueList = PagingHelper<League>.OrderList(pagingList, x => x.LeagueId);
            int totalPage = PagingHelper<League>.GetTotalPage(_iLeagueRepository.Query().Count(), pageSize);
            //将数据传入razor页面
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPage = totalPage;

            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Text="是",Value="true"},
                new SelectListItem { Text = "否", Value = "false" }
            };
            ViewBag.boolSelectList = new SelectList(selectListItems, "Value", "Text");
            ViewBag.isOver = "是";
            ViewBag.isNotOver = "否";
            return View(leagueList);
        }
        public async Task<IActionResult> InsertLeague(int leagueId, string leagueName, bool isOver)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                int num = await _iLeagueRepository.InsertAsync(new League
                {
                    LeagueId = leagueId,
                    LeagueName = leagueName,
                    IsOver = isOver
                });
                ViewData["num"] = num;
                return View("Insert");
            }
            catch
            {
                return View("InsertDefeated");
            }
        }
        public async Task<IActionResult> DeleteLeagueById(int leagueId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                bool isDeleted = await _iLeagueRepository.DeleteByIdAsync(leagueId);
                ViewData["isDelete"] = isDeleted;
                return View("Delete");
            }
            catch
            {
                return View("DeleteDefeated");
            }
        }
        public async Task<IActionResult> UpdateLeagueById(League league)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                int leagueId = league.LeagueId;
                string leagueName = league.LeagueName;
                bool isOver = league.IsOver;
                int num = await _iLeagueRepository.UpdateAsync(league);
                ViewBag.num = num;
                if (num == 0)
                {
                    return View("UpdateDefeated");
                }
                return View("UpdateSuccessful");
            }
            catch
            {
                return View("UpdateDefeated");
            }
        }
        public async Task<IActionResult> LeagueScore(int leagueId, int pageIndex = 1, int pageSize = 8)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            List<LeagueScore> leagueScoreList = await SqlSugarHelper.Db.Queryable<LeagueScore>().OrderByDescending(x=>x.Score).Where(x=>x.LeagueId==leagueId).ToListAsync();
            List<LeagueScore> list = PagingHelper<LeagueScore>.Paging(leagueScoreList, pageIndex, pageSize);
            List<LeagueScore> oList = PagingHelper<LeagueScore>.OrderList(list, x => x.Score);
            int totalPage = PagingHelper<LeagueScore>.GetTotalPage(leagueScoreList.Count, pageSize);
            //将数据传入razor页面
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPage = totalPage;
            ViewBag.leagueId = leagueId;

            List<SelectListItem> leagueTeamSelectListItems = new List<SelectListItem>();
            List<LeagueScore_Teamname> leagueScore_Teamname = new List<LeagueScore_Teamname>();
            foreach (var item in leagueScoreList)
            {
                leagueScore_Teamname.Add(new LeagueScore_Teamname
                {
                    leagueScore = item,
                    teamname = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == item.TeamId).ToList().First().TeamName
                });
                leagueTeamSelectListItems.Add(new SelectListItem
                {
                    Text = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == item.TeamId).ToList().First().TeamName,
                    Value=item.TeamId.ToString()
                });
            }

            List<Team> teams = await SqlSugarHelper.Db.Queryable<Team>().ToListAsync();
            SelectList teamSelectList = new SelectList(teams, "TeamId", "TeamName");

            List<SelectListItem> selectListItems = new List<SelectListItem>()
            {
                new SelectListItem { Text="是",Value="true"},
                new SelectListItem { Text = "否", Value = "false" }
            };
            ViewBag.teamSelectList = teamSelectList;
            ViewBag.leagueTeamSelectList = new SelectList(leagueTeamSelectListItems, "Value", "Text") ;
            return View(leagueScore_Teamname);
        }
        public async Task<IActionResult> InsertLeagueScore(int leagueScoreId, int teamId, int score, int leagueId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                List<int> teamIdList = _iLeagueScoreRepository.GetTeamId();
                if (teamIdList.Contains(teamId))
                {
                    int num = await _iLeagueScoreRepository.InsertAsync(new LeagueScore
                    {
                        LeagueScoreId = leagueScoreId,
                        TeamId = teamId,
                        Score = score,
                        LeagueId = leagueId
                    });
                    ViewData["num"] = num;
                    return View("Insert");
                }
                else
                {
                    return View("DontHaveTeamId");
                }
            }
            catch
            {
                return View("InsertDefeated");
            }
        }
        public async Task<IActionResult> DeleteLeagueScore(int leagueId, int teamId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                bool isDeleted = await _iLeagueScoreRepository.DeleteLeagueScoreByTwoId(leagueId, teamId);
                ViewData["isDelete"] = isDeleted;
                return View("Delete");
            }
            catch
            {
                return View("DeleteDefeated");
            }
        }
        public async Task<IActionResult> UpdateLeagueScore(int leagueId, int teamId, int score)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                int num = await _iLeagueScoreRepository.UpdateLeagueScore(leagueId, teamId, score);
                ViewBag.num = num;
                if (num == 0)
                {
                    return View("UpdateDefeated");
                }
                return View("UpdateSuccessful");
            }
            catch
            {
                return View("UpdateDefeated");
            }
        }
        public ActionResult Match(int pageIndex = 1, int pageSize = 10
            , int lpageIndex = 1, int lpageSize = 8
            , int cpageIndex = 1, int cpageSize = 8)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }

            Match_League_Cup models = new Match_League_Cup();
            List<Match> matche1 = PagingHelper<Match>.Paging(_iMatchRepository.Query(), pageIndex, pageSize);
            List<Match> matches = matche1.OrderByDescending(x => x.MatchDate).ToList();
            int totalPage = PagingHelper<Match>.GetTotalPage(_iMatchRepository.Query().Count(), pageSize);
            models.MatchList = matches;
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPage = totalPage;

            List<League> league1 = _iLeagueRepository.Query();
            List<League> league2 = PagingHelper<League>.Paging(league1, lpageIndex, lpageSize);
            List<League> league3 = league2.OrderBy(x => x.LeagueId).ToList();
            int ltotalPage = PagingHelper<League>.GetTotalPage(league1.Count(), lpageSize);
            ViewBag.lpageIndex = lpageIndex;
            ViewBag.lpageSize = lpageSize;
            ViewBag.ltotalPage = ltotalPage;
            models.LeagueList = league3;

            List<Cup> cupList1 = _iCupReposity.Query();
            List<Cup> cupList2 = PagingHelper<Cup>.Paging(cupList1, cpageIndex, cpageSize);
            List<Cup> cupList3 = cupList2.OrderBy(x => x.CupId).ToList();
            int ctotalPage = PagingHelper<Cup>.GetTotalPage(cupList1.Count(), cpageSize);
            ViewBag.cpageIndex = cpageIndex;
            ViewBag.cpageSize = cpageSize;
            ViewBag.ctotalPage = ctotalPage;
            models.CupList = cupList3;
            ViewBag.isOver = "是";
            ViewBag.isNotOver = "否";
            return View(models);
        }
        public async Task<IActionResult> InsertMatch(IFormCollection collection)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                Console.WriteLine(collection["leagueId"] + "----" + collection["leagueId"]);
                if (Convert.ToInt32(collection["leagueId"]) != 0 && Convert.ToInt32(collection["cupId"]) != 0)
                {
                    return View("HaveTwoIdInMatch");
                }
                if (Convert.ToInt32(collection["leagueId"]) == 0 && Convert.ToInt32(collection["cupId"]) == 0)
                {
                    return View("HaveNoIdInMatch");
                }

                if (Convert.ToInt32(collection["leagueId"]) != 0)
                {
                    List<int> leagueIdList = _iMatchRepository.GetLeagueId();
                    List<int> teamIdList = _iMatchRepository.GetTeamId();
                    if (!teamIdList.Contains(Convert.ToInt32(collection["teamId1"]))
                        || !teamIdList.Contains(Convert.ToInt32(collection["teamId2"])))
                    {
                        return View("DontHaveTeamId");
                    }
                    if (leagueIdList.Contains(Convert.ToInt32(collection["leagueId"])))
                    {
                        string[] dateValue = collection["matchDate"].ToString().Split("/");
                        int num = await _iMatchRepository.InsertAsync(new Match
                        {
                            TeamId1 = Convert.ToInt32(collection["teamId1"]),
                            TeamId2 = Convert.ToInt32(collection["teamId2"]),
                            MatchDate = new DateTime(Convert.ToInt32(dateValue[0]), Convert.ToInt32(dateValue[1]), Convert.ToInt32(dateValue[2]), Convert.ToInt32(dateValue[3]), Convert.ToInt32(dateValue[4]), 30),
                            MatchPosition = collection["matchPosition"],
                            LeagueId = Convert.ToInt32(collection["leagueId"]),
                            CupId = Convert.ToInt32(collection["cupId"]),
                            IsOver = false
                        });
                        ViewData["num"] = num;
                        return View("Insert");
                    }
                    else
                    {
                        return View("DontHaveLeagueId");
                    }
                }
                if (Convert.ToInt32(collection["cupId"]) != 0)
                {
                    List<int> teamIdList = _iMatchRepository.GetTeamId();
                    List<int> cupIdList = _iMatchRepository.GetCupId();
                    if (!teamIdList.Contains(Convert.ToInt32(collection["teamId1"]))
                        || !teamIdList.Contains(Convert.ToInt32(collection["teamId2"])))
                    {
                        return View("DontHaveTeamId");
                    }
                    if (cupIdList.Contains(Convert.ToInt32(collection["cupId"]))
                        && teamIdList.Contains(Convert.ToInt32(collection["teamId1"]))
                        && teamIdList.Contains(Convert.ToInt32(collection["teamId2"]))
                        )
                    {
                        string[] dateValue = collection["matchDate"].ToString().Split("/");
                        int num = await _iMatchRepository.InsertAsync(new Match
                        {
                            TeamId1 = Convert.ToInt32(collection["teamId1"]),
                            TeamId2 = Convert.ToInt32(collection["teamId2"]),
                            MatchDate = new DateTime(Convert.ToInt32(dateValue[0]), Convert.ToInt32(dateValue[1]), Convert.ToInt32(dateValue[2]), Convert.ToInt32(dateValue[3]), Convert.ToInt32(dateValue[4]),30),
                            MatchPosition = collection["matchPosition"],
                            LeagueId = Convert.ToInt32(collection["leagueId"]),
                            CupId = Convert.ToInt32(collection["cupId"]),
                            IsOver = false
                        });
                        ViewData["num"] = num;
                        return View("Insert");
                    }
                    else
                    {
                        return View("DontHaveCupId");
                    }
                }
                return View("InsertDefeated");
            }
            catch
            {
                return View("InsertDefeated");
            }
        }
        public async Task<IActionResult> DeleteMatchById(int matchId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                bool isDeleted = await _iMatchRepository.DeleteByIdAsync(matchId);
                ViewData["isDelete"] = isDeleted;
                return View("Delete");
            }
            catch
            {
                return View("DeleteDefeated");
            }
        }
        public async Task<IActionResult> UpdateMatch(int matchId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            Match dmatch = await _iMatchRepository.FindAsync(matchId);
            ViewBag.isOver = "是";
            ViewBag.isNotOver = "否";
            return View(dmatch);
        }
        public async Task<IActionResult> UpdateMatchById(int matchId, IFormCollection collection)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                string[] dateValue = collection["matchDate"].ToString().Split("/");
                int num = await _iMatchRepository.UpdateAsync(new Match
                {
                    MatchId = matchId,
                    TeamId1 = Convert.ToInt32(collection["teamId1"]),
                    TeamId2 = Convert.ToInt32(collection["teamId2"]),
                    MatchDate = new DateTime(Convert.ToInt32(dateValue[0]), Convert.ToInt32(dateValue[1]), Convert.ToInt32(dateValue[2])),
                    MatchPosition = collection["matchPosition"],
                    LeagueId = Convert.ToInt32(collection["leagueId"]),
                    CupId = Convert.ToInt32(collection["cupId"]),
                    IsOver = false
                });
                ViewBag.num = num;
                return View("UpdateSuccessful");
            }
            catch
            {
                return View("UpdateDefeated");
            }
        }
        public async Task<IActionResult> MatchListFromLeague(int leagueId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            League league = await _iLeagueRepository.FindAsync(leagueId);
            ViewBag.isOver = "是";
            ViewBag.isNotOver = "否";
            ViewBag.leagueName = league.LeagueName;
            List<Match> matchList = SqlSugarHelper.Db.Queryable<Match>().Where(x => x.LeagueId == leagueId).ToList();
            return View(matchList);
        }
        public async Task<IActionResult> MatchListFromCup(int cupId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            Cup cup = await _iCupReposity.FindAsync(cupId);
            ViewBag.isOver = "是";
            ViewBag.isNotOver = "否";
            ViewBag.cupName = cup.CupName;
            List<Match> matchList = SqlSugarHelper.Db.Queryable<Match>().Where(x => x.CupId == cupId).ToList();
            return View(matchList);
        }
        public async Task<IActionResult> EditMatchResult(int matchId)
        {
            
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            Match match = await _iMatchRepository.FindAsync(matchId);
            Team team1 = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == match.TeamId1).First();
            Team team2 = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamId == match.TeamId2).First();
            Match_Team mt = new Match_Team();
            mt.match = match;
            mt.team1 = team1;
            mt.team2 = team2;
            ViewBag.isOver = "是";
            ViewBag.isNotOver = "否";
            if (match.IsOver == false)
            {
                return View("InsertMatchResult", mt);
            }
            else
            {
                MatchResult matchResult = SqlSugarHelper.Db.Queryable<MatchResult>().Where(x => x.MatchId == matchId).First();
                mt.matchResult = matchResult;
                return View("UpdateMatchResult", mt);
            }
        }
        public async Task<IActionResult> IMatchResult(MatchResult matchResult)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                int matchId = matchResult.MatchId;
                int team1Goal = matchResult.Team1Goal;
                int team2Goal = matchResult.Team2Goal;
                string playerIdGoal = matchResult.PlayerIdGoal;
                string playerIdAssists = matchResult.PlayerIdAssists;
                string playerIdYellowcard = matchResult.PlayerIdYellowcard;
                string playerIdRedcard = matchResult.PlayerIdRedcard;
                int num = await SqlSugarHelper.Db.Insertable(matchResult).ExecuteCommandAsync();

                int result;
                //修改各项排名数据
                if (playerIdGoal != "none")
                {
                    string[] goalCount = playerIdGoal.Split(';');
                    foreach (string id in goalCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.GoalCount += 1;
                        result= SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                        Team team = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamName == player.TeamName).ToList().First();
                        team.Goal += 1;
                        result= SqlSugarHelper.Db.Updateable(team).ExecuteCommand();
                    }
                }
                if (playerIdAssists != "none")
                {
                    string[] assistCount = playerIdAssists.Split(';');
                    foreach (string id in assistCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.AssistCount += 1;
                        result= SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                    }
                }
                if (playerIdRedcard != "none")
                {
                    string[] redCardCount = playerIdRedcard.Split(';');
                    foreach (string id in redCardCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.RedCardCount += 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                    }
                }
                if (playerIdYellowcard != "none")
                {
                    string[] yellowCardCount = playerIdYellowcard.Split(';');
                    foreach (string id in yellowCardCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.YellowCardCount += 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                    }
                }

                Match match = await _iMatchRepository.FindAsync(matchId);

                match.IsOver = true;

                var result1 = SqlSugarHelper.Db.Updateable(match).UpdateColumns(x => new { x.IsOver }).ExecuteCommand();

                ViewData["num"] = num;

                ViewBag.isOver = "是";
                ViewBag.isNotOver = "否";

                return View("IMatchResultSuc");
            }
            catch
            {
                return View("IMatchResultDef");
            }
        }
        public async Task<IActionResult> UMatchResult(MatchResult matchResult,IFormCollection collection)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }

            try
            {
                int result;
                MatchResult mr = SqlSugarHelper.Db.Queryable<MatchResult>().Where(x => x.MatchId ==matchResult.MatchId)
                    .ToList().First();

                if (mr.PlayerIdGoal != "none")
                {
                    string[] goalCount = mr.PlayerIdGoal.Split(';');
                    foreach (string id in goalCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.GoalCount -= 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                        Team team = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamName == player.TeamName).ToList().First();
                        team.Goal -= 1;
                        result = SqlSugarHelper.Db.Updateable(team).ExecuteCommand();
                    }
                }
                if (mr.PlayerIdAssists != "none")
                {
                    string[] assistCount = mr.PlayerIdAssists.Split(';');
                    foreach (string id in assistCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.AssistCount -= 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                    }
                }
                if (mr.PlayerIdRedcard != "none")
                {
                    string[] redCardCount = mr.PlayerIdRedcard.Split(';');
                    foreach (string id in redCardCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.RedCardCount -= 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                    }
                }
                if (mr.PlayerIdYellowcard != "none")
                {
                    string[] yellowCardCount = mr.PlayerIdYellowcard.Split(';');
                    foreach (string id in yellowCardCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.YellowCardCount -= 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                    }
                }

                int resultId = matchResult.ResultId;
                int matchId = matchResult.MatchId;
                int team1Goal = matchResult.Team1Goal;
                int team2Goal = matchResult.Team2Goal;
                string playerIdGoal = matchResult.PlayerIdGoal;
                string playerIdAssists = matchResult.PlayerIdAssists;
                string playerIdYellowcard = matchResult.PlayerIdYellowcard;
                string playerIdRedcard = matchResult.PlayerIdRedcard;
                int num = await SqlSugarHelper.Db.Updateable(matchResult)
                    .ExecuteCommandAsync();

                if (matchResult.PlayerIdGoal != "none")
                {
                    string[] goalCount = matchResult.PlayerIdGoal.Split(';');
                    foreach (string id in goalCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.GoalCount += 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                        Team team = SqlSugarHelper.Db.Queryable<Team>().Where(x => x.TeamName == player.TeamName).ToList().First();
                        team.Goal += 1;
                        result = SqlSugarHelper.Db.Updateable(team).ExecuteCommand();
                    }
                }
                if (matchResult.PlayerIdAssists != "none")
                {
                    string[] assistCount = matchResult.PlayerIdAssists.Split(';');
                    foreach (string id in assistCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.AssistCount += 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                    }
                }
                if (matchResult.PlayerIdRedcard != "none")
                {
                    string[] redCardCount = matchResult.PlayerIdRedcard.Split(';');
                    foreach (string id in redCardCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.RedCardCount += 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                    }
                }
                if (matchResult.PlayerIdYellowcard != "none")
                {
                    string[] yellowCardCount = matchResult.PlayerIdYellowcard.Split(';');
                    foreach (string id in yellowCardCount)
                    {
                        Player player = SqlSugarHelper.Db.Queryable<Player>().Where(x => x.PlyerId == Convert.ToInt32(id)).ToList().First();
                        player.YellowCardCount += 1;
                        result = SqlSugarHelper.Db.Updateable(player).ExecuteCommand();
                    }
                }
                ViewBag.isOver = "是";
                ViewBag.isNotOver = "否";
                ViewBag.num = num;
                return View("UpdateSuccessful");
            }
            catch
            {
                return View("UpdateDefeated");
            }
        }
        public ActionResult User(int pageIndex = 1, int pageSize = 20)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            List<User> userList1 = SqlSugarHelper.Db.Queryable<User>().ToList();
            List<User> userList2 = PagingHelper<User>.Paging(userList1, pageIndex, pageSize);
            int totalPage = PagingHelper<User>.GetTotalPage(userList1.Count(), pageSize);
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPage = totalPage;
            return View(userList2);
        }
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            try
            {
                if (userId != "manager")
                {
                    bool isDelete = await SqlSugarHelper.Db.Deleteable<User>().Where(x => x.UserId == userId).ExecuteCommandHasChangeAsync();
                    ViewData["isDelete"] = isDelete;
                    return View("Delete");
                }
                return View("DeleteDefeated");
            }
            catch
            {
                return View("DeleteDefeated");
            }
        }
        public ActionResult Comment()
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            List<Comment> comments = SqlSugarHelper.Db.Queryable<Comment>().OrderByDescending(x => x.CommentDate).ToList();
            return View(comments);
        }
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            bool isDeleted = await SqlSugarHelper.Db.Deleteable<Comment>().Where(x => x.CommentId == commentId).ExecuteCommandHasChangeAsync();
            ViewData["isDelete"] = isDeleted;
            return View("Delete");
        }
        public IActionResult Article()
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            List<Article> articleList1 = SqlSugarHelper.Db.Queryable<Article>().OrderByDescending(x => x.ArticleDate).ToList();
            return View(articleList1);
        }
        public async Task<IActionResult> DeleteArticle(string articleId)
        {
            if (Request.Cookies["UserId"] != "manager" || Request.Cookies["UserPassword"] != "manager123")
            {
                return View("HaveNoPermission");
            }
            bool isDeleted = await SqlSugarHelper.Db.Deleteable<Article>().Where(x => x.ArticleId == Convert.ToInt32(articleId)).ExecuteCommandHasChangeAsync();
            ViewData["isDelete"] = isDeleted;
            return View("Delete");
        }
    }
}
