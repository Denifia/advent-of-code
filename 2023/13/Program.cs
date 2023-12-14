﻿// Title

var lines = File.ReadAllLines("input.txt");

var sum = GetInputChunk(lines).Sum(SummarizeInput);


// question 1
Console.WriteLine($"Part 1 Answer: {sum}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

static IEnumerable<string[]> GetInputChunk(string[] allLines)
{
	List<string> _lines = new();
	foreach (var line in allLines)
	{
        if (line == string.Empty)
		{
            yield return _lines.ToArray();
            _lines.Clear();
        }
        else
		{
            _lines.Add(line);
        }
    }
	yield return _lines.ToArray();
}

static int SummarizeInput(string[] horizontalLines)
{
	var horizontalMirrorPoint = FindMirrorPoint(horizontalLines);
    if (horizontalMirrorPoint > 0)
	{
		return horizontalMirrorPoint * 100;
	}

	var verticalLines = new List<string>();
    for (int i = 0; i < horizontalLines[0].Length; i++)
	{
        verticalLines.Add(string.Join(string.Empty, horizontalLines.Select(x => x[i])));
    }
	var verticalMirrorPoint = FindMirrorPoint(verticalLines.ToArray());
	if (verticalMirrorPoint == 0)
	{
		throw new Exception();
	}
	return verticalMirrorPoint;
}

static int FindMirrorPoint(string[] someLines)
{
	for (int i = 1; i < someLines.Length; i++)
	{
		if (someLines[i - 1] == someLines[i] && Validate(someLines, i))
		{
			return i;
		}
	}
	return 0;
}

static bool Validate(string[] someLines, int splitIndex)
{
	var left = someLines.Take(splitIndex).Reverse().ToArray();
    var right = someLines.Skip(splitIndex).ToArray();
	var length = Math.Min(left.Length, right.Length);
	for (int i = 0; i < length; i++)
	{
		if (left[i] != right[i])
		{
            return false;
        }	
	}
	return true;
}
