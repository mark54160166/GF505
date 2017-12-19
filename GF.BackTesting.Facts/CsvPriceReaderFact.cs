using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using Xunit.Abstractions;

namespace GF.BackTesting.Facts {
  public class CsvPriceReaderFact : IDisposable {
    private const string StockFileName1 = "stock1.csv";

    public CsvPriceReaderFact(ITestOutputHelper testOutput) {
      PrepareCsvFiles();
      TestOutput = testOutput;
    }

    public ITestOutputHelper TestOutput { get; }

    private static void PrepareCsvFiles() {
      // verbatim string
      string s = @"Date, Last, Delta, Bids, Offers
2017-10-06T10:05:00, 15.00, 0.00, 15.00, 15.50
2017-10-06T10:10:00, 16.00, 0.00, 16.00, 16.50
2017-10-06T10:15:00, 17.00, 0.00, 17.00, 17.50
2017-10-06T10:20:00, 16.00, 0.00, 16.00, 16.50";

      File.WriteAllText(StockFileName1, s);
    }

    [Fact]
    public void BasicUsage () {
      var reader = new CsvPriceReader("stock1.csv");
      decimal price = 0m;
      int count = 0;

      reader.NewPrice += (sender, e) => {
        TestOutput.WriteLine($"{e.Date:s} {e.Last,10:n2} {e.Bid,10:n2} {e.Offer,10:n2}");
        price = e.Last;
        count++;
      };

      reader.Start();

      Assert.Equal(4, count);
      Assert.Equal(16m, price);
    }

    public void Dispose() {
      File.Delete(StockFileName1);
    }
  }
}
