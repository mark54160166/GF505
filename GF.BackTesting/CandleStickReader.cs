using System;

namespace GF.BackTesting
{
    public class CandleStickReader
    {
        public PriceReader PriceReader { get; }
        public int Timeframe { get; }

        public event EventHandler<NewCandleStickEventArgs> NewCandleStick;
        private CandleStickItem item;
        private double previousCandleIndex = -1;

        public CandleStickReader(int timeframe, PriceReader priceReader)
        {
            Timeframe = timeframe;
            PriceReader = priceReader ?? throw new ArgumentNullException();

            priceReader.NewPrice += PriceReader_NewPrice;
            item = null;
        }

        private void PriceReader_NewPrice(object sender, NewPriceEventArgs e)
        {
            // stopper
            if (e.NewPrice == null)
            {
                if (item != null)
                {
                    var e2 = new NewCandleStickEventArgs(item);
                    NewCandleStick?.Invoke(this, e2);
                }
                return;
            }

            double candleIndex = Math.Floor(e.NewPrice.Date.Minute / (double)Timeframe);
            // new timeframe block?
            if (candleIndex != previousCandleIndex)
            {
                if (item != null)
                {
                    // raise new candle stick (except first time, because item is null)
                    var e2 = new NewCandleStickEventArgs(item);
                    NewCandleStick?.Invoke(this, e2);
                }

                // create new candle stick
                item = new CandleStickItem();
                item.Open = item.Close = 0m;
                item.High = decimal.MinValue;
                item.Low = decimal.MaxValue;

                item.Date = (e.NewPrice.Date).Date.AddHours(e.NewPrice.Date.Hour).AddMinutes(candleIndex * Timeframe);
            }

            // adjust candle stick

            if (item.Open == 0) item.Open = e.NewPrice.Last;
            if (e.NewPrice.Last > item.High) item.High = e.NewPrice.Last;
            if (e.NewPrice.Last < item.Low) item.Low = e.NewPrice.Last;
            item.Close = e.NewPrice.Last;

            previousCandleIndex = candleIndex;
        }

        public void Start()
        {
            PriceReader.Start();
        }
    }
}