using System.Collections.Generic;
using Xunit;

namespace GildedRose.Tests;

public class GildedRoseTests
{
    [Fact]
    public void NormalItem_DecreasesQualityByOne()
    {
        var item = CreateItem("Elixir of the Mongoose", 5, 7);

        UpdateQuality(item);

        Assert.Equal(4, item.Quality);
        Assert.Equal(4, item.SellIn);
    }

    [Fact]
    public void NormalItem_AfterSellByDate_DecreasesQualityByTwo()
    {
        var item = CreateItem("Elixir of the Mongoose", 0, 7);

        UpdateQuality(item);

        Assert.Equal(5, item.Quality);
        Assert.Equal(-1, item.SellIn);
    }

    [Fact]
    public void NormalItem_QualityCannotBecomeNegative()
    {
        var item = CreateItem("Elixir of the Mongoose", 5, 0);

        UpdateQuality(item);

        Assert.Equal(0, item.Quality);
    }

    [Fact]
    public void ConjuredItem_DecreasesQualityByTwo()
    {
        var item = CreateItem("Conjured Mana Cake", 3, 6);

        UpdateQuality(item);

        Assert.Equal(4, item.Quality);
        Assert.Equal(2, item.SellIn);
    }

    [Fact]
    public void ConjuredItem_AfterSellByDate_DecreasesQualityByFour()
    {
        var item = CreateItem("Conjured Mana Cake", 0, 6);

        UpdateQuality(item);

        Assert.Equal(2, item.Quality);
        Assert.Equal(-1, item.SellIn);
    }

    [Fact]
    public void ConjuredItem_QualityCannotBecomeNegative()
    {
        var item = CreateItem("Conjured Mana Cake", 0, 2);

        UpdateQuality(item);

        Assert.Equal(0, item.Quality);
    }

    [Fact]
    public void AgedBrie_IncreasesQualityByOne()
    {
        var item = CreateItem("Aged Brie", 5, 10);

        UpdateQuality(item);

        Assert.Equal(11, item.Quality);
        Assert.Equal(4, item.SellIn);
    }

    [Fact]
    public void AgedBrie_AfterSellByDate_IncreasesQualityByTwo()
    {
        var item = CreateItem("Aged Brie", 0, 10);

        UpdateQuality(item);

        Assert.Equal(12, item.Quality);
        Assert.Equal(-1, item.SellIn);
    }

    [Fact]
    public void Quality_CannotExceedFifty()
    {
        var item = CreateItem("Aged Brie", 5, 50);

        UpdateQuality(item);

        Assert.Equal(50, item.Quality);
    }

    [Fact]
    public void BackstagePass_MoreThanTenDays_IncreasesQualityByOne()
    {
        var item = CreateItem(
            "Backstage passes to a TAFKAL80ETC concert",
            15,
            20);

        UpdateQuality(item);

        Assert.Equal(21, item.Quality);
        Assert.Equal(14, item.SellIn);
    }

    [Fact]
    public void BackstagePass_TenDaysOrLess_IncreasesQualityByTwo()
    {
        var item = CreateItem(
            "Backstage passes to a TAFKAL80ETC concert",
            10,
            20);

        UpdateQuality(item);

        Assert.Equal(22, item.Quality);
        Assert.Equal(9, item.SellIn);
    }

    [Fact]
    public void BackstagePass_FiveDaysOrLess_IncreasesQualityByThree()
    {
        var item = CreateItem(
            "Backstage passes to a TAFKAL80ETC concert",
            5,
            20);

        UpdateQuality(item);

        Assert.Equal(23, item.Quality);
        Assert.Equal(4, item.SellIn);
    }

    [Fact]
    public void BackstagePass_AfterConcert_QualityBecomesZero()
    {
        var item = CreateItem(
            "Backstage passes to a TAFKAL80ETC concert",
            0,
            20);

        UpdateQuality(item);

        Assert.Equal(0, item.Quality);
        Assert.Equal(-1, item.SellIn);
    }

    [Fact]
    public void Sulfuras_DoesNotChange()
    {
        var item = CreateItem(
            "Sulfuras, Hand of Ragnaros",
            0,
            80);

        UpdateQuality(item);

        Assert.Equal(80, item.Quality);
        Assert.Equal(0, item.SellIn);
    }

    private static Item CreateItem(
        string name,
        int sellIn,
        int quality)
    {
        return new Item
        {
            Name = name,
            SellIn = sellIn,
            Quality = quality
        };
    }

    private static void UpdateQuality(Item item)
    {
        var program = new Program
        {
            Items = new List<Item> { item }
        };

        program.UpdateQuality();
    }
}