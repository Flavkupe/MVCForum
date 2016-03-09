using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVCForum.Domain.DomainModel;
using MVCForum.Domain.DomainModel.Activity;
using MVCForum.Domain.DomainModel.Attributes;
using MVCForum.Domain.Events;
using MVCForum.Domain.Interfaces;
using MVCForum.Domain.Interfaces.Badges;
using MVCForum.Domain.Interfaces.Services;
using MVCForum.Services.Data.Context;
using MVCForum.Utilities;

namespace MVCForum.Services
{
    public partial class StuffService : IStuffService
    {
        private readonly ILocalizationService _localizationService;
        private readonly IMembershipUserPointsService _membershipUserPointsService;
        private readonly ILoggingService _loggingService;
        private readonly IReflectionService _reflectionService;
        private readonly MVCForumContext _context;

        public const int BadgeCheckIntervalMinutes = 10;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggingService"> </param>
        /// <param name="localizationService"> </param>
        /// <param name="membershipUserPointsService"></param>
        /// <param name="reflectionService"></param>
        /// <param name="context"></param>
        public StuffService(ILoggingService loggingService, ILocalizationService localizationService,
            IMembershipUserPointsService membershipUserPointsService, IReflectionService reflectionService, IMVCForumContext context)
        {
            _loggingService = loggingService;
            _localizationService = localizationService;
            _membershipUserPointsService = membershipUserPointsService;
            _reflectionService = reflectionService;
            _context = context as MVCForumContext;
        }        
        
        public bool PurchaseStuff(Guid stuffId, MembershipUser user)
        {            
            int currency = _membershipUserPointsService.UserCurrency(user);

            if (user.Stuff.Any(a => a.Id == stuffId))
            {
                // duplicate
                return false;
            }

            var dbDailyStuff = GetAllDailyStuff();

            var dbStuff = _context.Stuff.FirstOrDefault(a => a.Id == stuffId);
            if (dbStuff == null)
            {
                return false;
            }
            
            DailyStuff item = dbDailyStuff.FirstOrDefault(a => a.Id == stuffId);
            if (item == null || item.Amount <= 0 || currency < dbStuff.Cost)
            {
                return false;
            }

            user.Stuff.Add(dbStuff);
            item.Amount--;
            user.Currency -= dbStuff.Cost ?? 0;            
            return true;
        }

        public IList<Stuff> GetAllStuff()
        {
            return GetAll().ToList();
        }

        public Stuff GetStuff(string name)
        {
            return _context.Stuff.FirstOrDefault(x => x.Name == name);
        }

        public Stuff Get(Guid id)
        {
            return _context.Stuff.FirstOrDefault(badge => badge.Id == id);
        }

        public IEnumerable<Stuff> GetAll()
        {
            return _context.Stuff.ToList();
        }

        public Stuff Add(Stuff newStuff)
        {
            return _context.Stuff.Add(newStuff);
        }

        public void Delete(Stuff stuff)
        {            
            _context.Stuff.Remove(stuff);
        }

        public DailyStuff AddDailyStuff(DailyStuff dailyStuff)
        {            
            return _context.DailyStuff.Add(dailyStuff);
        }

        public IEnumerable<DailyStuff> GetAllDailyStuff()
        {
            return _context.DailyStuff.ToList();
        }

        public void ResetDailyStuff()
        {
            foreach (var dailyStuff in GetAllDailyStuff())
            {
                _context.DailyStuff.Remove(dailyStuff);
            }
        }
    }

    public class StuffAttributeNotFoundException : Exception
    {
    }
}

