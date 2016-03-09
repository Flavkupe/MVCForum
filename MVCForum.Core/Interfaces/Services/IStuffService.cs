using System;
using System.Collections.Generic;
using System.Reflection;
using MVCForum.Domain.DomainModel;

namespace MVCForum.Domain.Interfaces.Services
{
    public partial interface IStuffService
    {
        /// <summary>
        /// Get all Badges enabled in the applications
        /// </summary>
        /// <returns></returns>
        IList<Stuff> GetAllStuff();

        /// <summary>
        /// Delete a stuff
        /// </summary>
        /// <param name="stuff"></param>
        void Delete(Stuff stuff);

        /// <summary>
        /// Get a stuff by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Stuff GetStuff(string name);

        Stuff Get(Guid id);

        /// <summary>
        /// All badges
        /// </summary>
        /// <returns></returns>
        IEnumerable<Stuff> GetAll();

        Stuff Add(Stuff newStuff);

        DailyStuff AddDailyStuff(DailyStuff dailyStuff);

        IEnumerable<DailyStuff> GetAllDailyStuff();

        void ResetDailyStuff();

        bool PurchaseStuff(Guid stuffId, MembershipUser user);
    }
}
