using System;
using Domain.Common;

namespace API.Application.ViewModels
{
    /**
    Response view model for order results.
    **/
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public int TotalQuantity { get; set; }

        public OrderStatus Status { get; set; }

        public Price price { get; set; }
    }
}
