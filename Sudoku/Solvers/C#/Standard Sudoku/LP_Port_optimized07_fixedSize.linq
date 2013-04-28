<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

public int[] arr;

public const bool firstSolutionOnly = true;

void Main()
{
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "vanskelig.txt"));

	var sw = Stopwatch.StartNew();
	int[] s = null;
	for(int i = 0; i < 1000; i++)
	{
		s = arr.ToArray();
		FindSolution(s);
	}
	sw.Stop();
	Console.WriteLine("Algoritm compute time: {0}", sw.Elapsed);
	Console.WriteLine("Algoritm compute time: {0}μs", (sw.Elapsed.TotalMilliseconds * 1000).ToString("# ##0"));
	Display(s);
}

public static void FindSolution(int[] s)
{
	const int SIZE = 81;
	var b = new Square[SIZE];

	for (int i = 0; i < SIZE; i++)
		b[i] = CreateSquare(i, s);

	var x = b.OrderBy(sq => sq.x).ToArray();
	var y = b.OrderBy(sq => sq.y).ToArray();
	var g = b.OrderBy(sq => sq.g).ToArray();

	var box = new Box(x, y, g);

	FindSolution(0, b, box);

	for (int i = 0; i < SIZE; i++)
		s[i] = b[i].v;
}

private static bool FindSolution(int n, Square[] t, Box b)
{
	for (; n < 81 && t[n].v != 0; n++) ;

	if (n == 81)
		return firstSolutionOnly;

	var brukt = FindUsedForLocal(n, t, b);

	for (int i = 1; i <= 9; i++)
		if ((brukt & (1 << i)) == 0)
		{
			t[n].v = i;
			if (FindSolution(n + 1, t, b))
				return true;
		}

	t[n].v = 0;

	return false;
}

private static int FindUsedForLocal(int n, Square[] t, Box b)
{
	var px = t[n].px;
	var py = t[n].py;
	var pg = t[n].pg;

	var brukt = 0;
	for (int i = 0; i < 9; i++)
	{
		brukt |= 1 << b.x[px + i].v;
		brukt |= 1 << b.y[py + i].v;
		brukt |= 1 << b.g[pg + i].v;
	}

	return brukt;
}

private static Square CreateSquare(int n, int[] s)
{
	var lx = n % 9;
	var ly = n / 9;
	
	var lg =  (n / 3) % 3
			+ (n / 27) * 3;

	return new Square(s[n], lx, ly, lg);
}

public class Square
{
	public int v;
	public int x;
	public int y;
	public int g;
	public int px;
	public int py;
	public int pg;

	public Square(int v, int x, int y, int g)
	{
		this.v = v;
		this.x = x;
		this.y = y;
		this.g = g;
		this.px = x * 9;
		this.py = y * 9;
		this.pg = g * 9;
	}
}

private struct Box
{
	public Square[] x, y, g;

	public Box(Square[] x, Square[] y, Square[] g)
	{
		this.x = x;
		this.y = y;
		this.g = g;
	}
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
	arr = js.Deserialize<int[]>(File.ReadAllText(path));
}