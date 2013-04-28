<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

int[] s;

const bool firstSolutionOnly = true;

const int allUsed = 1022;

void Main()
{
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "vanskelig.txt"));
	
	bool harLøsning = false;
	var sw = Stopwatch.StartNew();
	int[] t = null;
	for(int i = 0; i < 1000; i++)
	{
		t = s.ToArray();
		harLøsning = FindSolution(t);
	}
	sw.Stop();
	Console.WriteLine("Algoritm har løsning: {0}", harLøsning);
	Console.WriteLine("Algoritm compute time: {0}", sw.Elapsed);
	Console.WriteLine("Algoritm compute time: {0}μs", (sw.Elapsed.TotalMilliseconds * 1000).ToString("# ##0"));
	Display(t);
	
}

static bool FindSolution(int[] s)
{
	var b = new int[81];
	int n = 0, i = -1, brukt = 0;
	
	//CountUp:
	while (n < 81)
	{
		for (; n < 81 && s[n] != 0 && b[n] == 0; n++) ;
		
		if (n == 81) return true;

		if (s[n] != 0)
		{
			brukt = b[n];
			i = s[n] + 1;
		}
		else
		{
			brukt = b[n] = FindUsedForLocal(n, s);
			i = 1;
		}
		
		for (; i <= 9; i++)
			if((brukt & (1 << i)) == 0)
			{
				s[n] = i;
				break;
			}
		
		if (i == 10)
		{
			//Cleanup:
			s[n] = b[n] = 0;
			for (; n > 0 && b[n] == 0; n--) ;
		}
		else
			n++;
	}
	
	return false;
}

static int FindUsedForLocal(int n, int[] s)
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

static void Display(int[] s)
{
	for(int i = 0; i < 81; i++)
	{
		if(i % 9 == 0) Console.WriteLine();
		Console.Write(s[i]);
	}
	Console.WriteLine();
}

void JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	s = js.Deserialize<int[]>(File.ReadAllText(path));
}