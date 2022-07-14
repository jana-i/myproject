using ECinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECinema.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public ECinemaApplicationUser User { get; set; }

        public virtual ICollection<TicketInOrder> Tickets { get; set; }
    }
}
