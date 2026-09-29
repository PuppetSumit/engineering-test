using System;
using System.Collections.Generic;

namespace GildedRose.Console;

public class Program
{
    public IList<Item> Items = new List<Item>();

    private const string AgedBrie = "Aged Brie";
    private const string Sulfuras = "Sulfuras, Hand of Ragnaros";
    private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
    private const string ConjuredPrefix = "Conjured ";

    static void Main(string[] args)
    {
        System.Console.WriteLine("OMGHAI!");

        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 },
                new Item { Name = AgedBrie, SellIn = 2, Quality = 0 },
                new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 },
                new Item { Name = Sulfuras, SellIn = 0, Quality = 80 },
                new Item
                {
                    Name = BackstagePasses,
                    SellIn = 15,
                    Quality = 20
                },
                new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 }
            }
        };

        app.UpdateQuality();

        System.Console.ReadKey();
    }

    public void UpdateQuality()
    {
        foreach (var item in Items)
        {
            // Sulfuras is legendary and never changes.
            if (IsSulfuras(item))
            {
                continue;
            }

            UpdateQuality(item);

            item.SellIn--;

            // Quality changes again after the sell-by date.
            if (item.SellIn < 0)
            {
                UpdateQualityAfterSellByDate(item);
            }
        }
    }

    private static void UpdateQuality(Item item)
    {
        if (IsBackstagePass(item))
        {
            UpdateBackstagePassQuality(item);
            return;
        }

        if (IsAgedBrie(item))
        {
            IncreaseQuality(item);
            return;
        }

        DecreaseQuality(item, GetDegradationRate(item));
    }

    private static void UpdateQualityAfterSellByDate(Item item)
    {
        if (IsBackstagePass(item))
        {
            // Backstage passes have no value after the concert.
            item.Quality = 0;
            return;
        }

        if (IsAgedBrie(item))
        {
            // Aged Brie continues to improve after its sell-by date.
            IncreaseQuality(item);
            return;
        }

        // Normal and Conjured items degrade twice as fast after expiry.
        DecreaseQuality(item, GetDegradationRate(item));
    }

    private static void UpdateBackstagePassQuality(Item item)
    {
        IncreaseQuality(item);

        if (item.SellIn < 11)
        {
            IncreaseQuality(item);
        }

        if (item.SellIn < 6)
        {
            IncreaseQuality(item);
        }
    }

    private static int GetDegradationRate(Item item)
    {
        return IsConjured(item) ? 2 : 1;
    }

    private static void IncreaseQuality(Item item, int amount = 1)
    {
        item.Quality = Math.Min(50, item.Quality + amount);
    }

    private static void DecreaseQuality(Item item, int amount)
    {
        item.Quality = Math.Max(0, item.Quality - amount);
    }

    private static bool IsAgedBrie(Item item)
    {
        return item.Name == AgedBrie;
    }

    private static bool IsSulfuras(Item item)
    {
        return item.Name == Sulfuras;
    }

    private static bool IsBackstagePass(Item item)
    {
        return item.Name == BackstagePasses;
    }

    private static bool IsConjured(Item item)
    {
        return item.Name.StartsWith(
            ConjuredPrefix,
            StringComparison.OrdinalIgnoreCase);
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}