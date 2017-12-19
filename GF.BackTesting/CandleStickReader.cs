using System;

namespace GF.BackTesting {
  public class CandleStickReader {  

    public CandleStickReader(int timeframe, PriceReader priceReader) {
      Timeframe = timeframe;
      PriceReader = priceReader ?? throw new ArgumentNullException();

      priceReader.NewPrice += PriceReader_NewPrice;
    }

    private void PriceReader_NewPrice(object sender, NewPriceEventArgs e) {
      var item = new CandleStickItem {
        Open = e.Last,
        High = e.Last,
        Close = e.Last,
        Low = e.Last,
        Color = CandleStickColor.Green
      };

      var e2 = new NewCandleStickEventArgs(item);
      NewCandleStick?.Invoke(this, e2);
    }

    public int Timeframe { get; }
    public PriceReader PriceReader { get; }

    public event EventHandler<NewCandleStickEventArgs> NewCandleStick;

    public void Start() {
      PriceReader.Start();
    }
  }
}