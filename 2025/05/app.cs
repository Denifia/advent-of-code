var lines = File.ReadLines("input.txt").ToArray();

var ranges = new List<FreshItemRange>();
var items = new List<Item>();
var processing = Processing.Ranges;
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
    {
        processing = Processing.Items;
        continue;
    }

    if (processing == Processing.Ranges)
    {
        ranges.Add(new FreshItemRange(line));
        continue;
    }

    if (processing == Processing.Items)
    {
        items.Add(new Item(line));
        continue;
    }
}

var count = items.Count(item => ranges.Any(range => range.Contains(item)));
Console.WriteLine($"fresh item count: {count}");


enum Processing
{
    Ranges,
    Items
}

class FreshItemRange
{
    public long Start { get; }
    public long End { get; }

    public FreshItemRange(ReadOnlySpan<char> range)
    {
        var split = range.IndexOf("-");
        Start = long.Parse(range.Slice(0, split));
        End = long.Parse(range.Slice(split + 1));
    }

    public bool Contains(Item item)
    {
        return item.Id >= Start && item.Id <= End;
    }
}

class Item
{
    public long Id { get; }

    public Item(ReadOnlySpan<char> item)
    {
        Id = long.Parse(item);
    }
}