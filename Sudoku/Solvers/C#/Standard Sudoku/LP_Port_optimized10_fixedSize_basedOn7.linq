<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
  <Namespace>System.Runtime.CompilerServices</Namespace>
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
const int ALL	= FINAL - 1;

const int GROUPWIDTH = 3;
const int WIDTH = 9;
const int PADDING = WIDTH * GROUPWIDTH;
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

[MethodImpl(MethodImplOptions.AggressiveInlining)]
static void FindSolution(int[] s)
{
	var b = new Square[SIZE];
	
	Square.xa = new int[WIDTH];
	Square.ya = new int[WIDTH];
	Square.ga = new int[WIDTH];
		
	for (int i = 0; i < SIZE; i++)
		b[i] = new Square(i, s[i]);
	
	FindSolution(0, b);
	
	for (int i = 0; i < SIZE; i++)
		s[i] = b[i].v;
}

static bool FindSolution(int n, Square[] t)
{
	for (; n < SIZE && t[n].v != EMPTY; n++) ;

	if (n == SIZE)
		return firstSolutionOnly;
	
	var brukt = t[n].GetPosibles();
	
	if (brukt == ALL)
		return false;
	
	for (int i = 1; i < FINAL; i <<= 1)
		if ((brukt & i) == EMPTY)
		{
			t[n].SetValue(i);
			
			if (FindSolution(n + 1, t))
				return true;
			
			t[n].RemoveValue();
		}

	return false;
}

class Square
{
	public int v;
	
	private int lx, ly, lg;
	
	public static int[] xa, ya, ga;

	public Square(int n, int v)
	{
		this.lx = n % WIDTH; 
		this.ly = n / WIDTH;
		this.lg = (n / GROUPWIDTH) % GROUPWIDTH + (n / PADDING) * GROUPWIDTH;
		
		if (v != EMPTY)
			SetValue(v);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetPosibles()
	{
		return xa[lx] | ya[ly] | ga[lg];
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetValue(int value)
	{
		v = value;
		xa[lx] |= v;
		ya[ly] |= v;
		ga[lg] |= v;
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void RemoveValue()
	{
		xa[lx] ^= v;
		ya[ly] ^= v;
		ga[lg] ^= v;
		v = EMPTY;
	}
}

[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

[MethodImpl(MethodImplOptions.AggressiveInlining)]
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