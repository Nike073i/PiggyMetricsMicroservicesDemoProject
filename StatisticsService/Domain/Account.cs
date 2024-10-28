using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Domain
{
    public class Account
    {
        [Required]
        public List<Item> Incomes { get; set; }

        [Required]
        public List<Item> Expenses { get; set; }

        [Required]
        public Saving Saving { get; set; }
    }
}
