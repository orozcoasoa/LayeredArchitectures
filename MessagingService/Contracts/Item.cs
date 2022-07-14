﻿namespace MessagingService.Contracts
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + ", Name: " + (Name ?? "") + ", Price: " + Price.ToString();
        }
    }
}
