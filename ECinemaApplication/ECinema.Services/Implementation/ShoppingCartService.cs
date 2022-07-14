using ECinema.Domain.DomainModels;
using ECinema.Domain.DTO;
using ECinema.Repository.Interface;
using ECinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECinema.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IUserRepository _userRepository;
        

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> ticketInOrderRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
        }

        public bool deleteTicketFromShoppingCart(string userId, Guid id)
        {
            if(!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.Cart;

                var itemToDelete = userShoppingCart.TicketInShoppingCarts.Where(z => z.TicketId.Equals(id)).FirstOrDefault();

                userShoppingCart.TicketInShoppingCarts.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }

        public ShoppingCartDto GetShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.Cart;

            var AllTickets = userShoppingCart.TicketInShoppingCarts.ToList();

            var allTicketPrice = AllTickets.Select(z => new
            {
                TicketPrice = z.Ticket.TicketPrice,
                Quantity = z.Quantity
            }).ToList();

            var totalPrice = 0;

            foreach (var item in allTicketPrice)
            {
                totalPrice += item.Quantity * item.TicketPrice;
            }

           
            ShoppingCartDto shoppingCartDtoItem = new ShoppingCartDto
            {
                TicketInShoppingCarts = userShoppingCart.TicketInShoppingCarts.ToList(),
                TotalPrice = totalPrice
            };

            return shoppingCartDtoItem;
        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.Cart;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepository.Insert(order);

                List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

                var result = userShoppingCart.TicketInShoppingCarts.Select(z => new TicketInOrder
                {
                    TicketId = z.Ticket.Id,
                    SelectedTicket = z.Ticket,
                    OrderId = order.Id,
                    UserOrder = order
                }).ToList();

                ticketInOrders.AddRange(result);

                foreach (var item in ticketInOrders)
                {
                    this._ticketInOrderRepository.Insert(item);
                }


                loggedInUser.Cart.TicketInShoppingCarts.Clear();

                this._userRepository.Update(loggedInUser);

                return true;
            }
            return false;

        }
    }
}
