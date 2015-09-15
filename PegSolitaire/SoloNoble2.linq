<Query Kind="Program">
  <Namespace>System.Net</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

void Main()
{
	SolutionsCount = 0;

	var config = Config();
	
	int count = config.Item1;
	Peg center = config.Item2, start = center.L[1];
	
	start.Move(center, count);
	
	Console.WriteLine("Solutions found: {0}", SolutionsCount);
}

static int SolutionsCount;

[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static bool MoveToEmpty(Peg from, Peg over, Peg to, int count)
{
	return from != null
		&& from.Value && over.Value
		&& from.Move(to, count);
}

[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static bool MoveFromSet(Peg from, Peg over, Peg to, int count)
{
	return to != null
		&& over.Value && !to.Value
		&& from.Move(to, count);
}

[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static bool MoveOverSet(Peg from, Peg to, int count)
{
	return to != null && from != null
		&& from.Value && !to.Value
		&& from.Move(to, count);
}

public class Peg
{
	public bool Value;
	public bool IsCorner;

	public Peg[] U = new Peg[2]; //Up
	public Peg[] D = new Peg[2]; //Down
	public Peg[] L = new Peg[2]; //Left
	public Peg[] R = new Peg[2]; //Right
	
	public bool Move(Peg to, int count)
	{
		Peg over = null; //Over, to = To

		     if (U[1] == to) over = U[0];
		else if (D[1] == to) over = D[0];
		else if (L[1] == to) over = L[0];
		else 				 over = R[0];

		//Make:
		Value = false; over.Value = false; to.Value = true;
		if (--count == 1)
		{
			SolutionsCount++;
			Value = true; over.Value = true; to.Value = false;
			return false; //If false, try every combination, if true, stop on first solution.
		}

		//Signal:
		if (!this.IsCorner) //Not possible to jump over corners.
		{
			//Can move in over:
			if (over.U[0] == this || over.U[0] == to)
			{
				if (MoveToEmpty(over.L[1], over.L[0], over, count)) return true;
				if (MoveToEmpty(over.R[1], over.R[0], over, count)) return true;
			}
			else
			{
				if (MoveToEmpty(over.U[1], over.U[0], over, count)) return true;
				if (MoveToEmpty(over.D[1], over.D[0], over, count)) return true;
			}
		}

		//Can move in here:
		if (U[1] != to && MoveToEmpty(U[1], U[0], this, count)) return true;
		if (D[1] != to && MoveToEmpty(D[1], D[0], this, count)) return true;
		if (L[1] != to && MoveToEmpty(L[1], L[0], this, count)) return true;
		if (R[1] != to && MoveToEmpty(R[1], R[0], this, count)) return true;

		//Can move over to:
		if (!to.IsCorner)
		{
			if (to.U[0] != over && MoveOverSet(to.U[0], to.D[0], count)) return true;
			if (to.D[0] != over && MoveOverSet(to.D[0], to.U[0], count)) return true;
			if (to.L[0] != over && MoveOverSet(to.L[0], to.R[0], count)) return true;
			if (to.R[0] != over && MoveOverSet(to.R[0], to.L[0], count)) return true;
		}

		//Can move from to:
		if (to.D[1] != this && MoveFromSet(to, to.D[0], to.D[1], count)) return true;
		if (to.U[1] != this && MoveFromSet(to, to.U[0], to.U[1], count)) return true;
		if (to.L[1] != this && MoveFromSet(to, to.L[0], to.L[1], count)) return true;
		if (to.R[1] != this && MoveFromSet(to, to.R[0], to.R[1], count)) return true;

		//Rolback:
		Value = true; over.Value = true; to.Value = false;
		
		return false;
	}
}

public Tuple<int, Peg> Config()
{
	const int h = 7, w = 7;
	
	//Create:
	var board = (new[] { 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0 })
		.Select(val => val != 0 ? new Peg { Value = val == 1 } : null)
		.ToArray();

	//Map: Left, Right, Up, Down
	for (int y = 0; y < h; y++)
	{
		for (int x = 0; x < w; x++)
		{
			var obj = board[y * w + x];
			if (obj == null) continue;

			if (x > 0) obj.L[0] = board[y * w + x - 1];
			if (x > 1) obj.L[1] = board[y * w + x - 2];

			if (x < w - 1) obj.R[0] = board[y * w + x + 1];
			if (x < w - 2) obj.R[1] = board[y * w + x + 2];

			if (y > 0) obj.U[0] = board[y * w + x - w];
			if (y > 1) obj.U[1] = board[y * w + x - (2 * w)];

			if (y < h - 1) obj.D[0] = board[y * w + x + w];
			if (y < h - 2) obj.D[1] = board[y * w + x + (2 * w)];

			var sides = 0;
			if (obj.U[0] != null) sides++;
			if (obj.D[0] != null) sides++;
			if (obj.L[0] != null) sides++;
			if (obj.R[0] != null) sides++;
			if(sides == 2)
				obj.IsCorner = true;
		}
	}

	var count = board.Count(p => p != null && p.Value);
	var center = board[h * w / 2];
	
	return Tuple.Create(count, center);
}