﻿// Haunted Wasteland

var lines = File.ReadAllLines("input.txt");
var (instructions, map) = ParseInput(lines);
var repeatingInstructions = RepeatingInstructions(instructions);

var count = 0;
var row = "AAA";
foreach (var instruction in repeatingInstructions)
{
    count++;
    row = map[row][instruction];
    if (row == "ZZZ")
    {
        break;
    }
}

// question 
Console.WriteLine($"Answer: {count}");


(int[], Dictionary<string, string[]>) ParseInput(string[] lines)
{
    var instructions = lines[0].Select(x => x == 'R' ? 1 : 0).ToArray();

    var letterMap = lines.Skip(2).ToDictionary(
        keySelector: line => line[..3],
        elementSelector: line => new string[] { line.Substring(7, 3), line.Substring(12, 3) });

    //(int, int)[] map = new (int, int)[lines.Length - 2];

    return (instructions, letterMap);
}

IEnumerable<int> RepeatingInstructions(int[] instructions)
{
    var index = 0;
    while (true)
    {
        yield return instructions[index];
        index = (index + 1) % instructions.Length;
    }
}