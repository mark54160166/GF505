using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.BackTesting
{
    public abstract class PriceReader
    {
        private List<PriceItem> prices;

        public PriceReader()
        {
            prices = new List<PriceItem>();
        }

        public event EventHandler<NewPriceEventArgs> NewPrice;

        protected void RaiseNewPrice(PriceItem item)
        {
            NewPrice?.Invoke(this, new NewPriceEventArgs(item));
        }

        public void AddSeedPrice(decimal last)
        {
            AddSeedPrice(DateTime.Now, last, 0m, 0m);
        }

        public void AddSeedPrice(DateTime date, decimal last,
                                 decimal bid, decimal offer)
        {
            var item = new PriceItem
            {
                Date = date,
                Last = last,
                Bid = bid,
                Offer = offer
            };
            prices.Add(item);
        }

        public virtual void Start()
        {
            RaiseSeedPrices();
            RaiseStopper();
        }

        protected void RaiseSeedPrices()
        {
            foreach (var p in prices)
            {
                RaiseNewPrice(p);
            }
        }

        protected void RaiseStopper()
        {
            RaiseNewPrice(null);
        }
    }
}
