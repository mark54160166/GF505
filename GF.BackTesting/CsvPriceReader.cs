using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.BackTesting {
  public class CsvPriceReader : PriceReader {
    private string fileName;

    public CsvPriceReader(string fileName) {
      this.fileName = fileName;
    }

    public override void Start() {
      base.Start();

      // read csv file line-by-line.
      using (var reader = new StreamReader(fileName)) {
        string s;

        reader.ReadLine();
        while ((s = reader.ReadLine()) != null) {
          var data = s.Split(new char[] { ',', ' ' },
             StringSplitOptions.RemoveEmptyEntries);

          if (data.Length < 5) continue;

          var p = new PriceItem {
            Date = DateTime.Parse(data[0]),
            Last = decimal.Parse(data[1]),
            Bid = decimal.Parse(data[3]),
            Offer = decimal.Parse(data[4]),
          };
          RaiseNewPrice(p);
        }
      }
    }
  }
}
