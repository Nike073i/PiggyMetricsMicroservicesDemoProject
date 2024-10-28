using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace StatisticsService.Domain
{
    public class ExchangeRatesContainer
    {
        [JsonIgnore]
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

        public Currency Base { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }

        public override string ToString()
        {
            return $"RateList{{date={Date}, base={Base}, rates={string.Join(',', Rates.Select(x => $"{x.Key} {x.Value}").ToArray())}}}";
        }
    }
}
