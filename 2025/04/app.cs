var lines = File.ReadLines("input.txt").ToArray();
var lineLength = lines[0].Length;
var totalLines = lines.Length;

var positions = new List<Position>();

for (int row = 0; row < totalLines; row++)
{
    for (int column = 0; column < lineLength; column++)
    {
        if (lines[row][column] != '@')
            continue;

        var rangeStart = Math.Max(0, column - 1);
        var range = new Range(rangeStart, Math.Min(lineLength, column + 2));
        var rowAbove = row > 0 ? lines[row - 1].AsSpan(range) : new ReadOnlySpan<char>();
        var rowBelow = row < totalLines - 1 ? lines[row + 1].AsSpan(range) : new ReadOnlySpan<char>();
        var currentRow = lines[row].AsSpan(range);

        positions.Add(new Position(rowAbove, currentRow, rowBelow, indexInCurrentRow: column - rangeStart));
    }
}

var forliftMovableRolls = positions.Count(position => position.PaperRolls < 4);

Console.WriteLine($"Forklift movable rolls: {forliftMovableRolls}");

class Position
{
    public Position(
        ReadOnlySpan<char> rowAbove,
        ReadOnlySpan<char> currentRow,
        ReadOnlySpan<char> rowBelow,
        int indexInCurrentRow)
    {
        var paperRolls = rowAbove.Count('@')
            + rowBelow.Count('@');

        for (int i = 0; i < currentRow.Length; i++)
        {
            if (i == indexInCurrentRow)
            {
                continue;
            }

            if (currentRow[i] == '@')
            {
                paperRolls++;
            }
        }

        PaperRolls = paperRolls;
    }

    public int PaperRolls { get; }
}

