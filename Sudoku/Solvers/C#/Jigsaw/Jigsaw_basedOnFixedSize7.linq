<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

int[] sarr;
int[] garr;

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
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "jsonBoard1.txt"));
	
	sarr = sarr.Select(n => GetShifted(n)).ToArray();
	var sw = Stopwatch.StartNew();
	int[] s = null;
	for(int i = 0; i < 10; i++)
	{
		s = sarr.ToArray();
		int[] g = garr.ToArray();
		FindSolution(s, g);
	}
	sw.Stop();
	s = s.Select(n => GetUnShifted(n)).ToArray();
	Console.WriteLine("Algoritm compute time: {0}", sw.Elapsed);
	Console.WriteLine("Algoritm compute time: {0}μs", (sw.Elapsed.TotalMilliseconds * 1000).ToString("# ##0"));
	Display(s);
}

static void FindSolution(int[] s, int[] g)
{
	var b = new Square[SIZE];
	
	var xa = new int[WIDTH];
	var ya = new int[WIDTH];
	var ga = new int[WIDTH];
	
	for (int i = 0; i < SIZE; i++)
		b[i] = new Square(i, s[i], xa, ya, ga, g[i]);
	
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
	private int[] xa, ya, ga;

	public Square(int n, int v, int[] xa, int[] ya, int[] ga, int lg)
	{
		this.lx = n % WIDTH; 
		this.ly = n / WIDTH;
		this.lg = lg;
		
		this.xa = xa;
		this.ya = ya;
		this.ga = ga;
		
		if (v != EMPTY)
			SetValue(v);
	}
	
	public int GetPosibles()
	{
		return xa[lx] | ya[ly] | ga[lg];
	}
	
	public void SetValue(int value)
	{
		v = value;
		xa[lx] |= v;
		ya[ly] |= v;
		ga[lg] |= v;
	}
	
	public void RemoveValue()
	{
		xa[lx] ^= v;
		ya[ly] ^= v;
		ga[lg] ^= v;
		v = EMPTY;
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


public void JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	var json = js.Deserialize<int[][]>(File.ReadAllText(path));

	sarr = json[1];
	garr = json[0];
	
	if(garr.Length != SIZE || sarr.Length != SIZE)
		throw new FileLoadException("Wrong format in file.");
	
	for(int i = 0; i < SIZE; i++)
		garr[i] = garr[i] - 1;
}