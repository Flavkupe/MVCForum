using System;
using System.Collections.Generic;
using MVCForum.Utilities;

namespace MVCForum.Domain.DomainModel
{
    public partial class Stuff : Entity
    {
        private string displayName;

        public Stuff()
        {
            Id = GuidComb.GenerateComb();
        }

        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }        

        public string Description { get; set; }

        public string Image { get; set; }

        public int? Cost { get; set; }

        public string DisplayName
        {
            get { return displayName ?? Name; }
            set { displayName = value; }
        }

        public virtual IList<MembershipUser> Users { get; set; }
    }
    public partial class DailyStuff : Entity
    {
        public DailyStuff()
        {
        }

        public DailyStuff(Guid stuffId, int amount = 1)
        {
            Id = stuffId;
            Amount = amount;
        }

        public Guid Id { get; set; }

        public int Amount { get; set; }
    }
}
