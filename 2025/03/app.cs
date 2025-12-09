var totalOfLargestVoltages = File.ReadLines("input.txt")
    .ConvertToBatteryBanks()
    .SumLargestVoltageForEachBank();

Console.WriteLine($"Total of largest voltages: {totalOfLargestVoltages}");

public class BatteryBank
{
    private readonly Battery[] _batteries;

    public BatteryBank(string input)
    {
        _batteries = new Battery[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            _batteries[i] = new Battery(input.AsSpan(i, 1));
        }
    }

    public int CalculateLargestVoltage()
    {
        var largestVoltage = 0;

        for (int first = 0; first < _batteries.Length - 1; first++)
        for (int second = first + 1; second < _batteries.Length; second++)
        {
            var combinedVoltage = _batteries[first].Voltage * 10 + _batteries[second].Voltage;
            largestVoltage = Math.Max(largestVoltage, combinedVoltage);
        }

        return largestVoltage;
    }
}

public record Battery
{
    public int Voltage { get; init; }
        
    public Battery(ReadOnlySpan<char> c)
    {
        Voltage = int.Parse(c);
    }
}

public static class HelperExtensions
{
    public static IEnumerable<BatteryBank> ConvertToBatteryBanks(this IEnumerable<string> lines) => lines.Select(line => new BatteryBank(line));

    public static int SumLargestVoltageForEachBank(this IEnumerable<BatteryBank> banks) => banks.Sum(bank => bank.CalculateLargestVoltage());
}