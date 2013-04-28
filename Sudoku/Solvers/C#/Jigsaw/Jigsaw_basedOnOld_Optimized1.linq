<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

public const int x = 3;
public const int y = 3;

public const int columns = x * y;
public const int tot = columns * columns;

public int[] s;
public int[] g;
public int[] map;

public int solutions = 0;
public const bool firstSolutionOnly = true;

void Main()
{
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "jsonBoard1.txt"));
	
	var sw = Stopwatch.StartNew();
	FindSolution(0);
	sw.Stop();
	Console.WriteLine("Algoritm compute time: {0}", sw.Elapsed);
	Display(s);
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
	int local_x = n % columns;
	int local_y = n / columns;
	
	var start_index = g[n] * columns;
		
	var brukt = 0;
	for (int i = 0; i < columns; i++)
	{
		var iMap = map[start_index + i];
		brukt |= 1 << s[iMap];
		brukt |= 1 << s[local_y * columns + i];
		brukt |= 1 << s[i * columns + local_x];
	}
	
	return brukt;
}

public void Display(int[] b)
{
	for(int i = 0; i < 81; i++)
	{
		if(i % 9 == 0) Console.WriteLine();
		Console.Write(b[i]);
	}
	Console.WriteLine();
}
	
public void JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	var json = js.Deserialize<int[][]>(File.ReadAllText(path));

	s = json[1];
	g = json[0];
	
	if(g.Length != tot || s.Length != tot)
		throw new FileLoadException("Wrong format in file.");
	
	for(int i = 0; i < tot; i++) g[i] = g[i] - 1;
	
	map = new int[tot];
	var groups = new int[columns];
	for(int i = 0; i < tot; i++)
	{
		var grp = g[i];
		map[grp * columns + groups[grp]++] = i;
	}
}