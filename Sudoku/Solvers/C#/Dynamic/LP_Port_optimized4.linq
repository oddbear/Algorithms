<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

public const int x = 4;
public const int y = 3;

public static int columns = x * y;
public static int tot = columns * columns;

public int[] s;

public int solutions = 0;
public const bool firstSolutionOnly = true;

void Main()
{
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "jsonBoard_4x3_1.txt"));
	
	var sw = Stopwatch.StartNew();
	FindSolution(0);
	sw.Stop();
	Console.WriteLine("Algoritm compute time: {0}", sw.Elapsed);
	Display();
	Console.WriteLine("Number of solutions: {0}", solutions.ToString());
}

public bool FindSolution(int n)
{
	if (n == tot)
	{
		solutions++;
		return firstSolutionOnly;
	}
		
	if(s[n] != 0)
		return FindSolution(n + 1);
	else
	{
		var brukt = FindUsedForLocal(n);
		
		for (int i = 1; i <= columns; i++)
			if ((brukt & (1 << i)) == 0)
			{
				s[n] = i;
				if(FindSolution(n + 1))
					return true;
			}
		
		s[n] = 0;
		
		return false;
	}
}

public int FindUsedForLocal(int n)
{
	var local_x = n % columns;
	var local_y = n / columns;
		
	var start_y = local_y * columns;
	
	var anchor = n
		- local_x % x
		- (local_y % y) * columns;

	var brukt = 0;
	for (int i = 0; i < columns; i++)
	{
		brukt |= 1 << s[start_y + i];
		brukt |= 1 << s[i * columns + local_x];
		brukt |= 1 << s[
			anchor
			+ (i / x) * columns
			+ (i % x)
		];
	}
	
	return brukt;
}

public void Display()
{
	for(int i = 0; i < tot; i++)
	{
		if(i % (columns * y) == 0 && i != 0)
		{
			Console.WriteLine();
			for(int p = 0; p < columns * 3; p++)
				Console.Write('-');
			Console.WriteLine();
		}
		else if(i % columns == 0 && i != 0)
		{
			Console.WriteLine();
		}
		else if(i % x == 0 && i != 0)
		{
			Console.Write('|');
		}
		Console.Write(s[i] < 10 ? "  " + s[i].ToString() : " " + s[i].ToString());
	}
	Console.WriteLine();
}

public void JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	s = js.Deserialize<int[]>(File.ReadAllText(path));
}