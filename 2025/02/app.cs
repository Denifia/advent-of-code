using System.Collections.Immutable;

var ranges = File.ReadAllText("input.txt")
                       .Split(',')
                       .Select(range => new Range(range))
                       .ToImmutableArray();

long sum = 0;
foreach (var range in ranges)
{
    sum += range.SumInvalidItems();
}

Console.WriteLine($"Invalid ID sum: {sum}");


class Range
{
    public long Start { get; }
    public long End { get; }

    public Range(string range)
    {
        var r = range.AsSpan();
        var split = r.IndexOf('-');
        Start = long.Parse(r[..split]);
        End = long.Parse(r[(split + 1)..]);
    }

    public long SumInvalidItems()
    {
        long sum = 0;
        for (long i = Start; i <= End; i++)
        {
            var number = i.ToString().AsSpan();

            if (CheckItem(number) is Item.Invalid)
            {
                sum += i;
            }
        }
        return sum;
    }

    private static Item CheckItem(ReadOnlySpan<char> number)
    {
        // work backwards from the largest possible divides
        double length = number.Length;
        var halfLength = length / 2;
        for (var i = (int)Math.Round(halfLength, MidpointRounding.ToZero); i > 0; i--)
        {
            var allSegmentsMatch = true;
            if (length % i == 0)
            {
                // found a divisible pattern length
                var pattern = number[..i];
                var nextSegmentStart = i;

                while (nextSegmentStart < number.Length)
                {
                    if (!number.Slice(nextSegmentStart, i).SequenceEqual(pattern))
                    {
                        allSegmentsMatch = false;
                        break;
                    }

                    nextSegmentStart += i;
                }

                if (allSegmentsMatch)
                {
                    //Console.WriteLine($"invalid: {number}");

                    // if all segments match, the item is invalid
                    return Item.Invalid;
                }
            }
        }

        return Item.Valid;
    }
}

enum Item
{
    Valid,
    Invalid
}