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

static void FindSolution(int[] s)
{
	var b = new Square[SIZE];
	
	var xa = new int[WIDTH];
	var ya = new int[WIDTH];
	var ga = new int[WIDTH];
	
	for (int i = 0; i < SIZE; i++)
		b[i] = new Square(i, s[i], xa, ya, ga);
	
	FindSolution(b);
	
	for (int i = 0; i < SIZE; i++)
		s[i] = b[i].v;
}

static bool FindSolution(Square[] t)
{
	int n = 0;
	for (; n < SIZE && t[n].isStatic; n++) ;

	int min = n;
	
	while (n < SIZE)
	{
		int i, brukt;
		
		if (t[n].v != EMPTY)	//On way Down:
		{
			int v = t[n].v;
			t[n].RemoveValue();
			brukt = t[n].brukt;
			i = v << 1;
		}
		else //On way Up:
		{
			brukt = t[n].GetPosibles();
			i = ONE;
		}
		
		if (brukt != ALL)
		{
			for (; i < FINAL; i <<= 1)
				if ((brukt & i) == 0)
				{
					t[n].SetValue(i);
					break;
				}
		}
		else
			i = FINAL;
		
		if (i == FINAL)
		{
			//Cleanup:
			t[n].RemoveValue();
			
			if (n == min)
				return false;
			
			//CountDown:
			n--;
			for (; n > min && t[n].isStatic; n--) ;
		}
		else
		{
			n++;
			for (; n < SIZE && t[n].isStatic; n++) ;
		}
	}
	
	return true;
}

class Square
{
	public int v;
	
	public int lx;
	public int ly;
	public int lg;
	public bool isStatic;
	public int brukt;
	
	public int[] xa;
	public int[] ya;
	public int[] ga;

	public Square(int n, int v, int[] xa, int[] ya, int[] ga)
	{
		this.lx = n % WIDTH; 
		this.ly = n / WIDTH;
		this.lg = (n / GROUPWIDTH) % GROUPWIDTH + (n / PADDING) * GROUPWIDTH;
		
		this.xa = xa;
		this.ya = ya;
		this.ga = ga;
		
		if (v != EMPTY)
		{
			isStatic = true;
			SetValue(v);
		}
	}
	
	public int GetPosibles()
	{
		brukt = xa[lx] | ya[ly] | ga[lg];
		return brukt;
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

void JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	arr = js.Deserialize<int[]>(File.ReadAllText(path));
}