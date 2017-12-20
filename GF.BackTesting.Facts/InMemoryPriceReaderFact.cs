using System.Linq;
using Xunit;

namespace GF.BackTesting.Facts
{
    public class InMemoryPriceReaderFact
    {

        [Fact]
        public void NoSeedPrice_GetOnlyNullNewPriceItem()
        {
            var reader = new InMemoryPriceReader();
            PriceItem price = null;
            int count = 0;

            reader.NewPrice += (sender, e) =>
            {
                price = e.NewPrice;
                count++;
            };

            reader.Start();

            Assert.Equal(1, count);
            Assert.Null(price);
        }


        [Fact]
        public void SinglePrice()
        {
            var reader = new InMemoryPriceReader();
            decimal price = 0m;

            reader.AddSeedPrice(last: 15.0m);
            reader.NewPrice += (sender, e) =>
            {
                if(e.NewPrice != null)
                price = e.NewPrice.Last;
            };

            reader.Start();

            Assert.Equal(expected: 15.0m, actual:price);
        }

        [Fact]
        public void ThreePrices()
        {
            var reader = new InMemoryPriceReader();
            decimal price = 0m;
            int count = 0;
            bool foundStopper = false;

            reader.AddSeedPrice(last: 15.0m);
            reader.AddSeedPrice(last: 16.0m);
            reader.AddSeedPrice(last: 14.0m);

            reader.NewPrice += (sender, e) =>
            {
                if (e.NewPrice != null)
                {
                    price = e.NewPrice.Last;
                    count++;
                }
                else
                {
                    foundStopper = true;
                }
                
            };
            

            reader.Start();

            Assert.Equal(3, count);
            Assert.True(foundStopper);
        }

    }
}
