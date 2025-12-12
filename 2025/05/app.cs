var lines = File.ReadLines("test-input.txt").ToArray();

var ranges = new List<FreshItemRange>();
var processing = Processing.Ranges;
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
    {
        processing = Processing.Items;
        break;
    }

    if (processing == Processing.Ranges)
    {
        ranges.Add(new FreshItemRange(line));
        continue;
    }
}

var finalRanges = new List<FreshItemRange>(ranges);
foreach (var range in ranges)
{
    if (finalRanges.Count == 0)
    {
        finalRanges.Add(range);
    }

    var tempRange = new FreshItemRange(range.Start, range.End);
    var removedRanges = new List<int>();
    for (int i = 0; i < finalRanges.Count; i++)
    {
        if (finalRanges[i] == range)
        {
            // don't check yo self
            continue;
        }

        if (TryMerge(range, finalRanges[i], out var newRange))
        {
            tempRange = newRange;
            removedRanges.Add(i);
        }
    }

    removedRanges.Reverse();
    foreach (var rangeIndex in removedRanges)
    {
        finalRanges.RemoveAt(rangeIndex);
    }

    finalRanges.Add(tempRange);
}

Console.WriteLine($"fresh item count: {finalRanges.Sum(range => range.Count)}");


static bool TryMerge(FreshItemRange first, FreshItemRange second, out FreshItemRange newRange)
{
    newRange = new FreshItemRange("0-0");

    if (first.Contains(second.Start) && !first.Contains(second.End))
    {
        // layout
        // 1: |----|
        // 2:   |-----|

        newRange = new(first.Start, second.End);
        return true;
    }

    if (!first.Contains(second.Start) && first.Contains(second.End))
    {
        // layout
        // 1:   |----|
        // 2: |----|

        newRange = new(second.Start, first.End);
        return true;
    }

    if (first.Contains(second.Start) && first.Contains(second.End))
    {
        // layout
        // 1: |------|
        // 2:   |--|

        newRange = new(first.Start, first.End);
        return true;
    }

    if (second.Contains(first.Start) && second.Contains(first.End))
    {
        // layout
        // 1:   |--|
        // 2: |------|

        newRange = new(second.Start, second.End);
        return true;
    }

    return false;
}

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

    public FreshItemRange(long start, long end)
    {
        Start = start;
        End = end;
    }

    public bool Contains(Item item) => Contains(item.Id);

    public bool Contains(long id)
    {
        return id >= Start && id <= End;
    }

    public IEnumerable<long> GetItemIds()
    {
        for (long id = Start; id <= End; id++)
        {
            yield return id;
        }
    }

    public long Count => End - Start;
}

class Item
{
    public long Id { get; }

    public Item(ReadOnlySpan<char> item)
    {
        Id = long.Parse(item);
    }
}