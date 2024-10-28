using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AccountService.Data.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Domain
{
    public class Account
    {
        [BsonId]
        public string Name { get; set; }

        public DateTimeOffset LastSeen { get; set; }

        public List<IncomeItem> Incomes { get; set; }

        public List<ExpenseItem> Expenses { get; set; }

        [Required]
        public Saving Saving { get; set; }

        [StringLength(20000, MinimumLength = 0)]
        public string Note { get; set; }
    }
}
