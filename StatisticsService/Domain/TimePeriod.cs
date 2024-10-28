using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsService.Domain
{
    public enum TimePeriod
    {
        YEAR = 0,
        QUARTER = 1,
        MONTH = 2,
        DAY = 3,
        HOUR = 4
    }

    //public class TimePeriod
    //{
    //    public static readonly TimePeriod YEAR = new TimePeriod(365.2425d);
    //    public static readonly TimePeriod QUARTER = new TimePeriod(91.3106d);
    //    public static readonly TimePeriod MONTH = new TimePeriod(30.4368d);
    //    public static readonly TimePeriod DAY = new TimePeriod(1d);
    //    public static readonly TimePeriod HOUR = new TimePeriod(0.0416d);

    //    private double _baseRatio;

    //    public TimePeriod(double baseRatio)
    //    {
    //        _baseRatio = baseRatio;
    //    }

    //    public decimal GetBaseRatio()
    //    {
    //        return new decimal(_baseRatio);
    //    }

    //    public static TimePeriod GetBase()
    //    {
    //        return DAY;
    //    }
    //}
}
