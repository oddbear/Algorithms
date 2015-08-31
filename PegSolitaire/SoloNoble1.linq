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
	var count = b.Count(ch => ch == P.Set);

	//Start position:
	var start = w * h / 2 - 2; //must be -2, +2, -w or +w
	var move = new Fot(start, start + 1, start + 2);
	Move(b, move, count);
	//solutions.Dump();
}

[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
static bool IsValidBorder(Fot fot)
{
	if (fot.From < 0 || fot.From >= w * h
	 || fot.To   < 0 || fot.To   >= w * h)
		return false;

	return fot.IsRight || fot.IsLeft
		? fot.From / w == fot.To / w
		: fot.From % w == fot.To % w;
}

[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
static bool IsValidMove(P[] b, Fot fot)
{
	return b[fot.From] == P.Set
		&& b[fot.Over] == P.Set
		&& b[fot.To]   == P.Unset;
}

[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
static Fot[] CreatePossibles(Fot fot)
{
	int f = fot.From, o = fot.Over, t = fot.To;

	//Max 11 possible next moves:
	//All possible moves by change:
	int i = 0;
	var arr = new Fot[11];

	//Over (now empty), but two will always be empty.
	if (fot.IsUp || fot.IsDown)
	{
		arr[i++] = new Fot(from: o - 2, over: o - 1, to: o);
		arr[i++] = new Fot(from: o + 2, over: o + 1, to: o);
	}
	else //fot.IsLeft || fot.IsRight
	{
		arr[i++] = new Fot(from: o - w - w, over: o - w, to: o);
		arr[i++] = new Fot(from: o + w + w, over: o + w, to: o);
	}
	
	//Skipping impossible moves (ex if ← then → cannot be next move):
	if (!fot.IsLeft)
	{
		arr[i++] = new Fot(from: f - 2, over: f - 1, to: f); //From (now empty), except jumping back
		arr[i++] = new Fot(from: t, over: t + 1, to: t + 2); //To (now set), keep moving //TODO: is this correct in inverse?
		arr[i++] = new Fot(from: t + 1, over: t, to: t - 1); //New moves possible, can now jump over this, but the old over position vil always be empty.
	}
	if (!fot.IsRight)
	{
		arr[i++] = new Fot(from: f + 2, over: f + 1, to: f);
		arr[i++] = new Fot(from: t, over: t - 1, to: t - 2);
		arr[i++] = new Fot(from: t - 1, over: t, to: t + 1);
	}
	if (!fot.IsUp)
	{
		arr[i++] = new Fot(from: f - w - w, over: f - w, to: f);
		arr[i++] = new Fot(from: t, over: t + w, to: t + w + w);
		arr[i++] = new Fot(from: t + w, over: t, to: t - w);
	}
	if (!fot.IsDown)
	{
		arr[i++] = new Fot(from: f + w + w, over: f + w, to: f);
		arr[i++] = new Fot(from: t, over: t - w, to: t - w - w);
		arr[i++] = new Fot(from: t - w, over: t, to: t + w);
	}
	
	return arr;
}

//static int solutions = 0;

static bool Move(P[] b, Fot move, int count)
{
	MakeMove(b, move);
	if (--count == 1)
	{
		//solutions++; RollbackMove(b, move); return false;
		return DisplayAndRollbackMove(b, count, move);
	}

	var possibles = CreatePossibles(move);
	foreach(var fot in possibles)
	{
		if (IsValidBorder(fot) && IsValidMove(b, fot) && Move(b, fot, count))
			return DisplayAndRollbackMove(b, count, move);
	}
	
	RollbackMove(b, move);
	return false;
}
[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
static void MakeMove(P[] b, Fot fot)
{
	b[fot.From] = b[fot.Over] = P.Unset;
	b[fot.To] = P.Set;
}
[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
static void RollbackMove(P[] b, Fot fot)
{
	b[fot.From] = b[fot.Over] = P.Set;
	b[fot.To] = P.Unset;
}

class Fot
{
	public int From { get; }
	public int Over { get; }
	public int To { get; }
	
	public bool IsRight => From + 2 == To; //→
	public bool IsLeft => From - 2 == To; //←
	public bool IsUp => From - w - w == To; //↑
	public bool IsDown => From + w + w == To; //↓

	public Fot(int from, int over, int to)
	{
		From = from;
		Over = over;
		To = to;
	}
}

static bool DisplayAndRollbackMove(P[] b, int nr, Fot move)
{
	Console.WriteLine("Pegs: {0}", nr);
	for (int y = 0; y < h; y++)
	{
		for (int x = 0; x < w; x++)
		{
			var p = b[y * w + x];
			Console.Write(p == P.Set ? 'o' : p == P.Unset ? 'x' : '_');
		}
		Console.WriteLine();
	}
	Console.WriteLine();
	RollbackMove(b, move);
	return true;
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
		.Select(ch => ch == '.' ? P.Unset
					: ch == 'o' ? P.Set
					: P.Null
		).ToArray();
}