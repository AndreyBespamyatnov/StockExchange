using System;

namespace StockExchange.Models
{
    public class PersonalizedUserList
    {
        public Guid UserId { get; set; }
        public string StockCode { get; set; }
        public int Price { get; set; } 
    }
}