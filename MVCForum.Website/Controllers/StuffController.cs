using System;
using System.Web.Mvc;
using MVCForum.Domain.DomainModel;
using MVCForum.Domain.Interfaces.Services;
using MVCForum.Domain.Interfaces.UnitOfWork;
using MVCForum.Website.ViewModels;
using System.Collections.Generic;
using MVCForum.Utilities;
using System.Linq;

namespace MVCForum.Website.Controllers
{
    public partial class StuffController : BaseController
    {
        private readonly IPostService _postService;
        private readonly IFavouriteService _favouriteService;
        private readonly IStuffService _stuffService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggingService"> </param>
        /// <param name="unitOfWorkManager"> </param>
        /// <param name="postService"> </param>
        /// <param name="membershipService"> </param>
        /// <param name="localizationService"></param>
        /// <param name="roleService"> </param>
        /// <param name="settingsService"> </param>
        /// <param name="favouriteService"></param>
        public StuffController(ILoggingService loggingService,
            IUnitOfWorkManager unitOfWorkManager,
            IPostService postService,
            IMembershipService membershipService,
            ILocalizationService localizationService, IRoleService roleService,
            ISettingsService settingsService, IFavouriteService favouriteService, IStuffService stuffService)
            : base(loggingService, unitOfWorkManager, membershipService, localizationService, roleService, settingsService)
        {
            _postService = postService;
            _favouriteService = favouriteService;
            _stuffService = stuffService;
        }
        
        public ActionResult DoStuff()
        {
            using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
            {
                _stuffService.ResetDailyStuff();
                unitOfWork.Commit();
            }

            using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
            {
                IList<Stuff> allStuff = _stuffService.GetAllStuff();                
                
                Stuff current = allStuff.GetRandom();
                allStuff.Remove(current);
                _stuffService.AddDailyStuff(new DailyStuff(current.Id, RandUtils.GetRandom(1, 4)));
                current = allStuff.GetRandom();                
                _stuffService.AddDailyStuff(new DailyStuff(current.Id, RandUtils.GetRandom(1, 4)));                
                unitOfWork.Commit();
            }

            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult AllStuff()
        {
            using (UnitOfWorkManager.NewUnitOfWork())
            {
                var allStuff = _stuffService.GetAllStuff();
                var stuffListModel = new AllStuffViewModel
                {
                    AllStuff = allStuff
                };

                return View(stuffListModel);
            }
        }

        [HttpPost]
        [Authorize]
        public JsonResult PurchaseStuff(Guid id)
        {
            using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
            {
                try
                {
                    var loggedOnUser = MembershipService.GetUser(LoggedOnReadOnlyUser.UserName);
                    if (_stuffService.PurchaseStuff(id, loggedOnUser))
                    {
                        unitOfWork.Commit();
                        return Json(RequestStatus.Success);
                    }                    
                }
                catch
                {
                    unitOfWork.Rollback();
                }

                return Json(RequestStatus.Failure);
            }
        }

        [HttpGet]
        public ActionResult Daily()
        {
            using (UnitOfWorkManager.NewUnitOfWork())
            {
                var loggedOnUser = MembershipService.GetUser(LoggedOnReadOnlyUser.UserName);
                var myStuff = loggedOnUser.Stuff;
                var allDailyStuff = _stuffService.GetAllDailyStuff();
                var allStuff = _stuffService.GetAllStuff();                
                var dailyStuffModel = new List<AllDailyStuffViewModel.DailyStuff>();
                foreach (var dailyStuff in allDailyStuff)
                {
                    Stuff item = allStuff.FirstOrDefault(a => a.Id == dailyStuff.Id);
                    if (item != null)
                    {
                        dailyStuffModel.Add(new AllDailyStuffViewModel.DailyStuff()
                        {
                            Stuff = item,
                            Amount = dailyStuff.Amount,
                            Owned = myStuff.Any(a => a.Id == item.Id)
                        });
                    }
                }

                var stuffListModel = new AllDailyStuffViewModel
                {
                    AllDailyStuff = dailyStuffModel,
                    MyStuff = myStuff
                };

                return View(stuffListModel);
            }
        }
    }
}
