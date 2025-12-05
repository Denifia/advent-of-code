using System.Collections.Immutable;

var dial = new Dial(50);
var password = new Password();
var instructions = File.ReadAllLines("input.txt")
                       .Select(line => new Instruction(line))
                       .ToImmutableArray();

foreach (var instruction in instructions)
{
    var timesPointedAtZero = dial.Rotate(instruction);
    password.IncrementBy(timesPointedAtZero);
}

Console.WriteLine($"Password: {password.Value}");

class Dial(int value)
{
    private const int _minValue = 0;
    private const int _maxValue = 99;

    /// <summary>
    /// The current value of the dial that the arror is pointing to.
    /// </summary>
    public int Value { get; private set; } = value;

    /// <summary>
    /// Rotates the dial following the provided instruction.
    /// </summary>
    /// <returns>The number of times zero was pointed at by the arrow.</returns>
    public int Rotate(Instruction instruction) => Rotate(instruction.Direction, instruction.Value);

    /// <summary>
    /// Rotates the dial in the specified direction by the specified value.
    /// </summary>
    /// <returns>The number of times zero was pointed at by the arrow.</returns>
    public int Rotate(Direction direction, int value)
    {
        int timesPointingAtZero = 0;
        
        for (int i = 0; i < value; i++)
        {
            Value = direction switch
            {
                Direction.Left => Value == _minValue ? _maxValue : Value - 1,
                Direction.Right => Value == _maxValue ? _minValue : Value + 1,
                _ => Value,
            };

            if (Value == 0)
            {
                timesPointingAtZero++;
            }
        }

        return timesPointingAtZero;
    }
}

class Instruction
{
    public Instruction(string instruction)
    {
        Direction = instruction.AsSpan()[0] switch
        {
            'L' => Direction = Direction.Left,
            'R' => Direction = Direction.Right,
            _ => Direction = Direction.None,
        };

        Value = int.Parse(instruction[1..]);
    }

    public Direction Direction { get; init; }
    public int Value { get; init; }
}

enum Direction 
{
    None,
    Left,
    Right,
}

class Password
{
    public int Value { get; private set; }

    public void IncrementIfZeroValue(int value)
    {
        if (value == 0)
        {
            Value++;
        }
    }

    public void IncrementBy(int value)
    {
        Value += value;
    }
}