using System;
using System.Collections.Generic;
using MVCForum.Domain.DomainModel;

namespace MVCForum.Website.ViewModels
{
    public class AllStuffViewModel
    {
        public IList<Domain.DomainModel.Stuff> AllStuff { get; set; }
    }

    public class AllDailyStuffViewModel
    {
        public class DailyStuff
        {
            public Domain.DomainModel.Stuff Stuff { get; set; }
            public int Amount { get; set; }
            public bool Owned { get; set; }
        }

        public IList<AllDailyStuffViewModel.DailyStuff> AllDailyStuff { get; set; }

        public IList<Domain.DomainModel.Stuff> MyStuff { get; set; }
    }
}