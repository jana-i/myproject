using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECinema.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
        [Required]
        public string TicketName { get; set; }
        [Required]
        public string TicketImage { get; set; }
        [Required]
        public string TicketCity { get; set; }
        [Required]
        public string TicketDate { get; set; }
        [Required]
        public int TicketPrice { get; set; }

        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
        public IEnumerable<TicketInOrder> Orders { get; set; }

    }
}
