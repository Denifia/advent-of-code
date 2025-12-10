using System.Text;

var lines = File.ReadLines("input.txt").ToArray();
var lineLength = lines[0].Length;
var totalPaperRollsRemoved = 0;

while (true)
{
    //Console.WriteLine(string.Join(Environment.NewLine, lines));
    //Console.WriteLine();
    var positions = GetPositions(lines);
    var paperRollsRemoved = positions.Count(position => position.TryRemovePaperRolls());

    if (paperRollsRemoved == 0) 
        break;

    totalPaperRollsRemoved += paperRollsRemoved;
    lines = GetUpdatedLines(positions);
}

Console.WriteLine($"Total paper rolls removed: {totalPaperRollsRemoved}");

List<Position> GetPositions(string[] lines)
{
    var totalLines = lines.Length;
    var positions = new List<Position>();

    for (int row = 0; row < totalLines; row++)
    {
        for (int column = 0; column < lineLength; column++)
        {
            var rangeStart = Math.Max(0, column - 1);
            var range = new Range(rangeStart, Math.Min(lineLength, column + 2));
            var rowAbove = row > 0 ? lines[row - 1].AsSpan(range) : new ReadOnlySpan<char>();
            var rowBelow = row < totalLines - 1 ? lines[row + 1].AsSpan(range) : new ReadOnlySpan<char>();
            var currentRow = lines[row].AsSpan(range);

            positions.Add(new Position(rowAbove, currentRow, rowBelow, indexInCurrentRow: column - rangeStart));
        }
    }

    return positions;
}

string[] GetUpdatedLines(List<Position> positions)
{
    var updatedLines = new List<string>();
    var sb = new StringBuilder();
    for (int i = 0; i < positions.Count; i++)
    {
        sb.Append(positions[i].Print());

        if (sb.Length % lineLength == 0)
        {
            updatedLines.Add(sb.ToString());
            sb.Clear();
        }
    }

    return updatedLines.ToArray();
}

class Position
{
    public Position(
        ReadOnlySpan<char> rowAbove,
        ReadOnlySpan<char> currentRow,
        ReadOnlySpan<char> rowBelow,
        int indexInCurrentRow)
    {
        _symbol = currentRow[indexInCurrentRow];

        if (_symbol == '.')
            return;

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

    private char _symbol;

    public bool TryRemovePaperRolls()
    {
        var newSymbol = PaperRolls switch
        {
            < 4 => '.',
            _ => '@'
        };

        if (newSymbol != _symbol && newSymbol == '.')
        {
            _symbol = newSymbol;
            return true;
        }

        return false;
    }

    public char Print() => _symbol;
}

