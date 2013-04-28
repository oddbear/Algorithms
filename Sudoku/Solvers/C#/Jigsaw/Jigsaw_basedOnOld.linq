<Query Kind="Program" />

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
	g = new int[81] {
		1,1,2,2,2,2,2,2,3,
		1,1,1,2,2,3,3,3,3,
		1,1,1,1,2,3,3,6,3,
		4,4,5,5,5,5,3,6,6,
		4,4,4,4,5,6,6,6,6,
		4,4,7,5,5,5,5,6,6,
		7,4,7,7,8,9,9,9,9,
		7,7,7,7,8,8,9,9,9,
		7,8,8,8,8,8,8,9,9
	};
	for(int i = 0; i < g.Length; i++) g[i] = g[i] - 1;
	map = new int[g.Length];
	for(int grp = 0; grp < columns; grp++)
	{
		int pos = 0;
		for(int i = 0; i < tot; i++)
		{
			if(g[i] == grp)
				map[grp * columns + pos++] = i;
		}
	}
	
	s = new int[g.Length];
	s[0 * 9 + 7] = 8;
	s[1 * 9 + 1] = 1;
	s[2 * 9 + 4] = 1;
	s[3 * 9 + 2] = 9;s[3 * 9 + 3] = 5;
	s[4 * 9 + 2] = 2; s[4 * 9 + 4] = 6; s[4 * 9 + 6] = 5; s[4 * 9 + 8] = 4;
	s[5 * 9 + 0] = 4; s[5 * 9 + 1] = 8; s[5 * 9 + 6] = 2; s[5 * 9 + 8] = 9;
	s[6 * 9 + 5] = 3; s[6 * 9 + 8] = 8;
	s[7 * 9 + 2] = 6; s[7 * 9 + 4] = 2;
	s[8 * 9 + 1] = 3; s[8 * 9 + 2] = 8; s[8 * 9 + 4] = 5;
	
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
		brukt |= 1 << s[local_y * columns + i];
		brukt |= 1 << s[i * columns + local_x];
		brukt |= 1 << s[map[start_index + i]];
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