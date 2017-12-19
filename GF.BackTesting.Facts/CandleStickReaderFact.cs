using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GF.BackTesting.Facts {
  public class CandleStickReaderFact {

    [Fact]
    public void NoPrice_NoCandleStick() {
      // arrange
      var p = new InMemoryPriceReader();
      var c = new CandleStickReader(timeframe: 15, priceReader: p);
      int count = 0;

      // a
      c.NewCandleStick += (sender, e) => {
        count++;
      };
      c.Start();

      // a
      Assert.Equal(0, count);
    }

    [Fact]
    public void SinglePrice_NoCandleStick() {
      // arrange
      var p = new InMemoryPriceReader();
      var c = new CandleStickReader(timeframe: 15, priceReader: p);
      int count = 0;
      CandleStickItem item = null;

      p.AddSeedPrice(12m);
      c.NewCandleStick += (sender, e) => {
        count++;
        item = e.CandleStick;
      };

      // a
      c.Start();

      // a
      Assert.Equal(1, count);
      Assert.Equal(12m, item.Open);
      Assert.Equal(12m, item.High);
      Assert.Equal(12m, item.Close);
      Assert.Equal(12m, item.Low);
      Assert.Equal(CandleStickColor.Green, 
        item.Color);

    }

    [Fact]
    public void TwoPrices() {
      // arrange
      var p = new InMemoryPriceReader();
      var c = new CandleStickReader(timeframe: 15, priceReader: p);
      int count = 0;
      CandleStickItem item = null;
      var dt1 = new DateTime(2017, 5, 5, 10, 1, 0);
      var dt2 = new DateTime(2017, 5, 5, 10, 2, 0);

      p.AddSeedPrice(dt1, 12m, 0m, 0m);
      p.AddSeedPrice(dt2, 14m, 0m, 0m);

      c.NewCandleStick += (sender, e) => {
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
      var dt0 = new DateTime(2017, 5, 5, 10, 0, 0);
      Assert.Equal(dt0, item.Date);
      Assert.Equal(CandleStickColor.Green,
        item.Color);

    }

  }
}
