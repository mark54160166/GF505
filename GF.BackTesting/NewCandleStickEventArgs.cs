using System;

namespace GF.BackTesting {
  public class NewCandleStickEventArgs : EventArgs {

    public NewCandleStickEventArgs(CandleStickItem candleStick) {
      CandleStick = candleStick;
    }

    public CandleStickItem CandleStick { get; }
  }
}