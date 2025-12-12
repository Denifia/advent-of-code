using System;

var lines = File.ReadLines("input.txt").ToArray();

var ranges = new List<FreshItemRange?>();
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

var atLeastOneMerge = true;
while (atLeastOneMerge == true)
{
    atLeastOneMerge = false;

    for (int outerIndex = 0; outerIndex < ranges.Count; outerIndex++)
    {
        if (ranges[outerIndex] == null)
        {
            continue;
        }

        for (int innerIndex = 0; innerIndex < ranges.Count; innerIndex++)
        {
            if (innerIndex == outerIndex)
            {
                continue;
            }

            if (TryMerge(ranges[outerIndex]!, ranges[innerIndex]!, out var newRange))
            {
                atLeastOneMerge = true;
                ranges[outerIndex] = newRange;
                ranges[innerIndex] = null;
            }
        }
    }
}

Console.WriteLine($"fresh item count: {ranges.Sum(range => range?.Count)}");


static bool TryMerge(FreshItemRange? first, FreshItemRange? second, out FreshItemRange? newRange)
{
    newRange = null;
    if (first == null || second == null)
    {
        return false;
    }

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

    public long Count => End - Start + 1;
}

class Item
{
    public long Id { get; }

    public Item(ReadOnlySpan<char> item)
    {
        Id = long.Parse(item);
    }
}