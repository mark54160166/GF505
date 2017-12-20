using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GF.BackTesting.Facts
{
    public class CandleStickReaderFact
    {

        [Fact]
        public void NoPrice_NoCandleStick()
        {
            // arrange
            var p = new InMemoryPriceReader();
            var c = new CandleStickReader(timeframe: 15, priceReader: p);
            int count = 0;

            c.NewCandleStick += (sender, e) =>
            {
                count++;
            };
            c.Start();

            Assert.Equal(0, count);
        }

        [Fact]
        public void SinglePrice_OneCandleStick()
        {
            // arrange
            var p = new InMemoryPriceReader();
            var c = new CandleStickReader(timeframe: 15, priceReader: p);
            int count = 0;
            CandleStickItem item = null;

            p.AddSeedPrice(12m);
            c.NewCandleStick += (sender, e) =>
            {
                count++;
                item = e.CandleStick;
            };

            c.Start();

            Assert.Equal(1, count);
            Assert.Equal(12m, item.Open);
            Assert.Equal(12m, item.High);
            Assert.Equal(12m, item.Close);
            Assert.Equal(12m, item.Low);
            Assert.Equal(CandleStickColor.Green, item.Color);

        }

        [Fact]
        public void TwoPrices()
        {
            // arrange
            var p = new InMemoryPriceReader();
            var c = new CandleStickReader(timeframe: 15, priceReader: p);
            int count = 0;
            CandleStickItem item = null;
            var dt1 = new DateTime(2017, 5, 5, 10, 1, 0);
            var dt2 = new DateTime(2017, 5, 5, 10, 2, 0);

            p.AddSeedPrice(dt1, 12m, 0m, 0m);
            p.AddSeedPrice(dt2, 14m, 0m, 0m);

            c.NewCandleStick += (sender, e) =>
            {
                count++;
                item = e.CandleStick;
            };

            // a
            c.Start();

            // a
            Assert.Equal(1, count);
            Assert.Equal(12m, item.Low);
            Assert.Equal(12m, item.Open);
            Assert.Equal(14m, item.High);
            Assert.Equal(14m, item.Close);
            Assert.Equal(CandleStickColor.Green,
              item.Color);

        }

        [Fact]
        public void TwoCandleSticks()
        {
            // arrange
            var p = new InMemoryPriceReader();
            var c = new CandleStickReader(timeframe: 15, priceReader: p);
            int count = 0;
            List<CandleStickItem> items = new List<CandleStickItem>();
            var dt1 = new DateTime(2017, 5, 5, 10, 5, 0);
            var dt2 = new DateTime(2017, 5, 5, 10, 10, 0);
            var dt3 = new DateTime(2017, 5, 5, 10, 15, 0);
            var dt4 = new DateTime(2017, 5, 5, 10, 20, 0);

            p.AddSeedPrice(dt1, 12m, 0m, 0m);
            p.AddSeedPrice(dt2, 14m, 0m, 0m);
            p.AddSeedPrice(dt3, 16m, 0m, 0m);
            p.AddSeedPrice(dt4, 15m, 0m, 0m);

            c.NewCandleStick += (sender, e) =>
            {
                count++;
                items.Add(e.CandleStick);
            };

            // a
            c.Start();

            // a
            Assert.Equal(2, count);
            Assert.Equal(12m, items[0].Low);
            Assert.Equal(12m, items[0].Open);
            Assert.Equal(14m, items[0].High);
            Assert.Equal(14m, items[0].Close);

            Assert.Equal(16m, items[1].Open);
            Assert.Equal(15m, items[1].Low);
            Assert.Equal(16m, items[1].High);
            Assert.Equal(15m, items[1].Close);

            Assert.Equal(new DateTime(2017, 5, 5, 10, 0, 0), items[0].Date);
            Assert.Equal(new DateTime(2017, 5, 5, 10, 15, 0), items[1].Date);

        }

        [Fact]
        public void ThreeCandleSticks()
        {
            // arrange
            var p = new InMemoryPriceReader();
            var c = new CandleStickReader(timeframe: 15, priceReader: p);
            int count = 0;
            List<CandleStickItem> items = new List<CandleStickItem>();
            var dt1 = new DateTime(2017, 5, 5, 10, 5, 0);
            var dt2 = new DateTime(2017, 5, 5, 10, 10, 0);
            var dt3 = new DateTime(2017, 5, 5, 10, 15, 0);
            var dt4 = new DateTime(2017, 5, 5, 10, 20, 0);
            var dt5 = new DateTime(2017, 5, 5, 10, 40, 0);
            var dt6 = new DateTime(2017, 5, 5, 10, 41, 0);
            var dt7 = new DateTime(2017, 5, 5, 10, 42, 0);

            p.AddSeedPrice(dt1, 12m, 0m, 0m);
            p.AddSeedPrice(dt2, 14m, 0m, 0m);
            p.AddSeedPrice(dt3, 16m, 0m, 0m);
            p.AddSeedPrice(dt4, 15m, 0m, 0m);
            p.AddSeedPrice(dt5, 20m, 0m, 0m);
            p.AddSeedPrice(dt6, 23m, 0m, 0m);
            p.AddSeedPrice(dt7, 22m, 0m, 0m);

            c.NewCandleStick += (sender, e) =>
            {
                count++;
                items.Add(e.CandleStick);
            };

            // a
            c.Start();

            // a
            Assert.Equal(3, count);
            Assert.Equal(12m, items[0].Low);
            Assert.Equal(12m, items[0].Open);
            Assert.Equal(14m, items[0].High);
            Assert.Equal(14m, items[0].Close);

            Assert.Equal(16m, items[1].Open);
            Assert.Equal(15m, items[1].Low);
            Assert.Equal(16m, items[1].High);
            Assert.Equal(15m, items[1].Close);

            Assert.Equal(20m, items[2].Open);
            Assert.Equal(20m, items[2].Low);
            Assert.Equal(23m, items[2].High);
            Assert.Equal(22m, items[2].Close);

            Assert.Equal(new DateTime(2017, 5, 5, 10, 0, 0), items[0].Date);
            Assert.Equal(new DateTime(2017, 5, 5, 10, 15, 0), items[1].Date);
            Assert.Equal(new DateTime(2017, 5, 5, 10, 30, 0), items[2].Date);
        }
    }
}
