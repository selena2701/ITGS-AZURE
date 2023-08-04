using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Models
{
    public class Statistic
    {
        private int statisticId;
        private DateTime statisticDate;
        private int sales, profit;

        public Statistic()
        {
        }

        public Statistic(int statisticId, DateTime statisticDate, int sales, int profit)
        {
            this.statisticId = statisticId;
            this.statisticDate = statisticDate;
            this.sales = sales;
            this.profit = profit;
        }

        public int StatisticId { get => statisticId; set => statisticId = value; }
        public DateTime StatisticDate { get => statisticDate; set => statisticDate = value; }
        public int Sales { get => sales; set => sales = value; }
        public int Profit { get => profit; set => profit = value; }
    }
}
