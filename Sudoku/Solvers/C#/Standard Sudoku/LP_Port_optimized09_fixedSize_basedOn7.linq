<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

public int[] arr;

const int EMPTY	=   0;
const int ONE	=   1;
const int TWO	=   2;
const int THREE	=   4;
const int FOUR	=   8;
const int FIVE	=  16;
const int SIX	=  32;
const int SEVEN	=  64;
const int EIGHT	= 128;
const int NINE	= 256;
const int FINAL	= 512;

const int GROUPWIDTH = 3;
const int WIDTH = 9;
const int SIZE = 81;

public const bool firstSolutionOnly = true;

void Main()
{
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "vanskelig.txt"));

	arr = arr.Select(n => GetShifted(n)).ToArray();
	var sw = Stopwatch.StartNew();
	int[] s = null;
	for(int i = 0; i < 1000; i++)
	{
		s = arr.ToArray();
		FindSolution(s);
	}
	sw.Stop();
	s = s.Select(n => GetUnShifted(n)).ToArray();
	Console.WriteLine("Algoritm compute time: {0}", sw.Elapsed);
	Console.WriteLine("Algoritm compute time: {0}μs", (sw.Elapsed.TotalMilliseconds * 1000).ToString("# ##0"));
	Display(s);
}

static void FindSolution(int[] s)
{
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

static bool FindSolution(int n, Square[] t, Box b)
{
	for (; n < SIZE && t[n].v != EMPTY; n++) ;

	if (n == SIZE)
		return firstSolutionOnly;

	var brukt = FindUsedForLocal(n, t, b);

	for (int i = 1; i < FINAL; i <<= 1)
		if ((brukt & i) == EMPTY)
		{
			t[n].v = i;
			if (FindSolution(n + 1, t, b))
				return true;
		}

	t[n].v = EMPTY;

	return false;
}

static int FindUsedForLocal(int n, Square[] t, Box b)
{
	var px = t[n].px;
	var py = t[n].py;
	var pg = t[n].pg;

	var brukt = EMPTY;
	for (int i = 0; i < 9; i++)
	{
		brukt |= b.x[px + i].v;
		brukt |= b.y[py + i].v;
		brukt |= b.g[pg + i].v;
	}

	return brukt;
}

static Square CreateSquare(int n, int[] s)
{
	var lx = n % 9;
	var ly = n / 9;
	var lg =  (n / 3) % 3
			+ (n / 27) * 3;

	return new Square(s[n], lx, ly, lg);
}

class Square
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

struct Box
{
	public Square[] x, y, g;

	public Box(Square[] x, Square[] y, Square[] g)
	{
		this.x = x;
		this.y = y;
		this.g = g;
	}
}

static int GetShifted(int i)
{
	switch (i)
	{
		case 1:  return ONE;
		case 2:  return TWO;
		case 3:  return THREE;
		case 4:  return FOUR;
		case 5:  return FIVE;
		case 6:  return SIX;
		case 7:  return SEVEN;
		case 8:  return EIGHT;
		case 9:  return NINE;
		case 10: return FINAL;
		default: return EMPTY;
	}
}

static int GetUnShifted(int i)
{
	switch (i)
	{
		case ONE:   return 1;
		case TWO:   return 2;
		case THREE: return 3;
		case FOUR:  return 4;
		case FIVE:  return 5;
		case SIX:   return 6;
		case SEVEN: return 7;
		case EIGHT: return 8;
		case NINE:  return 9;
		case FINAL: return 10;
		default:    return 0;
	}
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
	arr = js.Deserialize<int[]>(File.ReadAllText(path));
}