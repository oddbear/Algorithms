<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

int[] s;

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

void Main()
{
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "vanskelig.txt"));
	
	bool harLøsning = false;
	var sw = Stopwatch.StartNew();
	int[] t = null;
	for(int i = 0; i < 1000; i++)
	{
		t = s.Select(n => GetShifted(n)).ToArray();
		harLøsning = FindSolution(t);
		t = t.Select(n => GetUnShifted(n)).ToArray();
	}
	sw.Stop();
	Console.WriteLine("Algoritm har løsning: {0}", harLøsning);
	Console.WriteLine("Algoritm compute time: {0}", sw.Elapsed);
	Console.WriteLine("Algoritm compute time: {0}μs", (sw.Elapsed.TotalMilliseconds * 1000).ToString("# ##0"));
	Display(t);
}

static bool FindSolution(int[] s)
{
	int n = 0;
	var b = new int[SIZE];

	for (; n < SIZE && s[n] != EMPTY; n++) ;
	var min = n;

	//CountUp:
	while(n < SIZE)
	{
		int i, brukt;

		if (s[n] != EMPTY)
		{
			brukt = b[n];
			i = s[n] << 1;
		}
		else
		{
			brukt = b[n] = FindUsedForLocal(n, s);
			i = ONE;
		}

		for (; i < FINAL; i <<= 1)
			if ((brukt & i) == 0)
			{
				s[n] = i;
				break;
			}
	
		if (i == FINAL)
		{
			//Cleanup:
			s[n] = b[n] = EMPTY;

			if (n == min)
				return false;

			//CountDown:
			for (; n > min && b[n] == EMPTY; n--) ;
		}
		else
		{
			n++;
			for (; n < SIZE && s[n] != EMPTY && b[n] == EMPTY; n++) ;
		}
	}

	return true;
}

static int FindUsedForLocal(int n, int[] s)
{
	var local_x = n % WIDTH;
	var local_y = n / WIDTH;

	var start_y = local_y * WIDTH;

	var anchor = n
		- local_x % GROUPWIDTH
		- (local_y % GROUPWIDTH) * WIDTH;

	var brukt = 0;

	for (var i = 0; i < WIDTH; i++)
	{
		brukt |= s[start_y + i];
		brukt |= s[i * WIDTH + local_x];
		brukt |= s[
			anchor
			+ (i / GROUPWIDTH) * WIDTH
			+ (i % GROUPWIDTH)
		];
	}
	
	return brukt;
}

int GetShifted(int i)
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

int GetUnShifted(int i)
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
	s = js.Deserialize<int[]>(File.ReadAllText(path));
}