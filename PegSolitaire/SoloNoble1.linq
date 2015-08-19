<Query Kind="Program" />

//Peg solitaire (or Solo Noble)
const int w = 7, h = 7;

void Main()
{
	var str = "  ooo  "
			+ "  ooo  "
			+ "ooooooo"
			+ "ooo.ooo"
			+ "ooooooo"
			+ "  ooo  "
			+ "  ooo  ";
	
	var b = Create(str);
	var count = Count(str);

	//Start position:
	var start = w * h / 2 - 2; //must be -2, +2, -w or +w
	var move = new Fot(start, start + 2);
	Move(b, move, count);
}

static bool IsValidBorder(Fot fot)
{
	if (fot.From < 0 || fot.From >= w * h
	 || fot.To   < 0 || fot.To   >= w * h)
		return false;

	return fot.IsRight() || fot.IsLeft()
		? fot.From / w == fot.To / w
		: fot.From % w == fot.To % w;
}

static bool IsValidMove(P[] b, Fot fot)
{
	return b[fot.From] == P.Set
		&& b[fot.Over] == P.Set
		&& b[fot.To] == P.Unset;
}

static Fot[] CreatePossibles(Fot fot)
{
	int f = fot.From, o = fot.Over, t = fot.To;

	//Max 11 possible next moves:
	//All possible moves by change:
	int i = 0;
	var arr = new Fot[11];
	if (!fot.IsLeft())
	{
		arr[i++] = new Fot(from: f - 2, over: f - 1, to: f); //From (now empty), except jumping back
		arr[i++] = new Fot(from: t, over: t + 1, to: t + 2); //To (now set), keep moving //TODO: is this correct in inverse?
		arr[i++] = new Fot(from: t + 1, over: t, to: t - 1); //New moves possible, can now jump over this, but the old over position vil always be empty.
	}
	if (!fot.IsRight())
	{
		arr[i++] = new Fot(from: f + 2, over: f + 1, to: f);
		arr[i++] = new Fot(from: t, over: t - 1, to: t - 2);
		arr[i++] = new Fot(from: t - 1, over: t, to: t + 1);
	}
	if (!fot.IsUp())
	{
		arr[i++] = new Fot(from: f - w - w, over: f - w, to: f);
		arr[i++] = new Fot(from: t, over: t + w, to: t + w + w);
		arr[i++] = new Fot(from: t + w, over: t, to: t - w);
	}
	if (!fot.IsDown())
	{
		arr[i++] = new Fot(from: f + w + w, over: f + w, to: f);
		arr[i++] = new Fot(from: t, over: t - w, to: t - w - w);
		arr[i++] = new Fot(from: t - w, over: t, to: t + w);
	}

	//Over (now empty), but two will always be empty.
	if (fot.IsUp() || fot.IsDown())
	{
		arr[i++] = new Fot(from: o - 2, over: o - 1, to: o);
		arr[i++] = new Fot(from: o + 2, over: o + 1, to: o);
	}
	else //fot.IsLeft() || fot.IsRight()
	{
		arr[i++] = new Fot(from: o - w - w, over: o - w, to: o);
		arr[i++] = new Fot(from: o + w + w, over: o + w, to: o);
	}
	
	return arr;
}
bool Move(P[] b, Fot move, int count)
{
	if (count == 2)	//ca. 14s.
	{
		MakeMove(b, move);
		for (int y = 0; y < h; y++)
		{
			for (int x = 0; x < w; x++)
			{
				var p = b[y * w + x];
				Console.Write(p == P.Set ? 'o' : p == P.Unset ? '.' : ' ');
			}
			Console.WriteLine();
		}
		return true;
	}

//	if (!IsValidBorder(move) || !IsValidMove(b, move))
//		throw new Exception("!");
	
	MakeMove(b, move);
	var possibles = CreatePossibles(move);
	for (int i = 0; i < 11; i++)
	{
		var fot = possibles[i];
		if (IsValidBorder(fot) && IsValidMove(b, fot))
		{
			if(Move(b, fot, count - 1))
				return true;
			//fot.DisplayVector(w);
		}
	}
	RollbackMove(b, move);
	
	return false;
}
void MakeMove(P[] b, Fot fot)
{
	b[fot.From] = b[fot.Over] = P.Unset;
	b[fot.To] = P.Set;
}
void RollbackMove(P[] b, Fot fot)
{
	b[fot.From] = b[fot.Over] = P.Set;
	b[fot.To] = P.Unset;
}

class Fot
{
	public int From { get; private set; }
	public int Over { get; private set; }
	public int To { get; private set; }

	public Fot(int from, int to)
	{
		From = from;
		Over = from + (to - from) / 2;
		To = to;
	}
	public Fot(int from, int over, int to)
	{
		From = from;
		Over = over;
		To = to;
	}
	public bool IsRight()
	{
		return From + 2 == To; //→
	}
	public bool IsLeft()
	{
		return From - 2 == To; //←
	}
	public bool IsUp()
	{
		return From - w - w == To; //↑
	}
	public bool IsDown()
	{
		return From + w + w == To; //↓
	}
	public void DisplayVector(int _w)
	{
		Console.WriteLine("{0}{1},{2}{3} -> {4}{5}",
			(char)('a' + (From / _w)), From % _w,
			(char)('a' + (Over / _w)), Over % _w,
			(char)('a' + (To / _w)), To % _w
		);
	}
}

enum P
{
	Unset,
	Set,
	Null
}

//Board setup:
static P[] Create(string str)
{
	return str
		.ToCharArray()
		.Select(ch =>
			ch == '.' ? P.Unset :
			ch == 'o' ? P.Set :
			P.Null
		)
		.ToArray();
}
static int Count(string str)
{
	return str
		.ToCharArray()
		.Count(ch => ch == 'o');
}