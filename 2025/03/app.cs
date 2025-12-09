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

    const int _maxLength = 12;
    public string CalculateLargestVoltage()
    {
        var currentIndex = 0;
        var remainingLength = _maxLength;
        var voltages = new List<int>();

        while (remainingLength > 0)
        {
            var workingLength = _batteries.Length - currentIndex - remainingLength + 1;

            var selectedBatteries = _batteries.AsSpan(currentIndex, workingLength);
            var foundIndex = FindFirstLargestVoltageInRange(selectedBatteries);

            voltages.Add(_batteries[currentIndex + foundIndex].Voltage);
            currentIndex = currentIndex + foundIndex + 1;
            remainingLength--;
        }

        var result = string.Join("", voltages);
        //Console.WriteLine(result);
        return result;
    }

    private int FindFirstLargestVoltageInRange(ReadOnlySpan<Battery> batteries)
    {
        var largestVoltage = 0;
        var firstLargestBatteryIndex = 0;

        for (int i = 0; i < batteries.Length; i++)
        {
            if (batteries[i].Voltage > largestVoltage)
            {
                largestVoltage = batteries[i].Voltage;
                firstLargestBatteryIndex = i;
            }
        }

        return firstLargestBatteryIndex;
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

    public static long SumLargestVoltageForEachBank(this IEnumerable<BatteryBank> banks) => banks.Sum(bank => long.Parse(bank.CalculateLargestVoltage()));
}