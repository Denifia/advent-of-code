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

            // skip odd length numbers
            if (number.Length % 2 == 1)
            {
                continue;
            }

            int halfLength = number.Length / 2;
            if (number[..halfLength].SequenceEqual(number[halfLength..]))
            {
                sum += i;
            }
        }
        return sum;
    }
}