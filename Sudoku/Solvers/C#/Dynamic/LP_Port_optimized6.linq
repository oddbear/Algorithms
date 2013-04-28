<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

public const int x = 4;
public const int y = 3;

public static int columns = x * y;
public static int tot = columns * columns;

public int[] s;

public const bool firstSolutionOnly = true;

void Main()
{
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "jsonBoard_4x3_1.txt"));

	var sw = Stopwatch.StartNew();
	int[] t = null;
	for(int i = 0; i < 1; i++)
	{
		t = s.ToArray();
		FindSolution(0, t);
	}
	sw.Stop();
	Console.WriteLine("Algoritm compute time: {0}", sw.Elapsed);
	Console.WriteLine("Algoritm compute time: {0}μs", (sw.Elapsed.TotalMilliseconds * 1000).ToString("# ##0"));
	Display(t);
}

public static bool FindSolution(int n, int[] s)
{
	for(; n < tot && s[n] != 0; n++) ;
	
	if (n == tot)
		return firstSolutionOnly;
	
	var brukt = FindUsedForLocal(n, s);
	
	for (int i = 1; i <= columns; i++)
		if((brukt & (1 << i)) == 0)
		{
			s[n] = i;
			if(FindSolution(n + 1, s))
				return true;
		}
	
	s[n] = 0;
	
	return false;
}

public static int FindUsedForLocal(int n, int[] s)
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


public void Display(int[] s)
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