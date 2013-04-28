<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

public int[] s;

public const bool firstSolutionOnly = true;

void Main()
{
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "vanskelig.txt"));

	var sw = Stopwatch.StartNew();
	int[] t = null;
	for(int i = 0; i < 1000; i++)
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
	for(; n < 81 && s[n] != 0; n++) ;
	
	if (n == 81)
		return firstSolutionOnly;
	
	var brukt = FindUsedForLocal(n, s);
	
	for (int i = 1; i <= 9; i++)
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
	var local_x = n % 9;
	var local_y = n / 9;
	
	var start_y = local_y * 9;
	
	var anchor = n
		- local_x % 3
		- (local_y % 3) * 9;
		
	var brukt = 0;
	for (int i = 0; i < 9; i++)
	{
		brukt |= 1 << s[start_y + i];
		brukt |= 1 << s[i * 9 + local_x];
		brukt |= 1 << s[
			anchor
			+ (i / 3) * 9
			+ (i % 3)
		];
	}
	
	return brukt;
}

public static void Display(int[] s)
{
	for(int i = 0; i < 81; i++)
	{
		if(i % 9 == 0) Console.WriteLine();
		Console.Write(s[i]);
	}
	Console.WriteLine();
}

public void JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	s = js.Deserialize<int[]>(File.ReadAllText(path));
}