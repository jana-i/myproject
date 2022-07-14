using ECinema.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto GetShoppingCartInfo(string userId);
        bool deleteTicketFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId);
    }
}
