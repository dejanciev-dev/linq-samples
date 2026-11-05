namespace LinqSamples.Tests;

public class CartSampleTests
{
    [Fact] public void Subtotal() => Assert.Equal(116m, CartSamples.Subtotal());
    [Fact] public void Item_count() => Assert.Equal(21, CartSamples.ItemCount());
    [Fact] public void Per_category() => Assert.Equal(["home:30.00", "media:51.00", "office:35.00"], CartSamples.PerCategory());
    [Fact] public void Biggest_line() => Assert.Equal("lamp", CartSamples.BiggestLine());
    [Fact] public void Media_discount() => Assert.Equal(5.1m, CartSamples.MediaDiscount());
    [Fact] public void Buy_three_get_one() => Assert.Equal(109m, CartSamples.BuyThreeGetOne());
    [Fact] public void Free_shipping() => Assert.True(CartSamples.FreeShipping());
    [Fact] public void Cheapest() => Assert.Equal(["lamp", "disc", "pen"], CartSamples.CheapestPerCategory());
    [Fact] public void Bulk() => Assert.Equal(["pad", "pen"], CartSamples.BulkLines());
    [Fact] public void Units() => Assert.Equal(21, CartSamples.UnitCount());
    [Fact] public void Error_rate() => Assert.Equal(50.0, LogSamples.ErrorRate());
    [Fact] public void Levels_per_source() => Assert.Equal(["api:ERROR/INFO", "db:ERROR/WARN"], LogSamples.LevelsPerSource());
}
