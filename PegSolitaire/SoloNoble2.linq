<Query Kind="Program">
  <Namespace>System.Net</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

//TODO: Cleanup. But still faster than first one.
void Main()
{
	int h = 7, w = 7;
	var b = Config(h, w);
	
	var count = b.Count(p => p != null && p.Value);
	var c = b[h * w / 2];
	var s = c.L[1];
	sCount = 0;
	s.Move(c, count);
	sCount.Dump();
}

static int sCount = 0;

[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static bool MTE(Peg f, Peg o, Peg t, int count) //MoToEmp
{
	return f != null //NCor
		&& f.Value && o.Value
		&& f.Move(t, count);
}

[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static bool MFS(Peg f, Peg o, Peg t, int count) //MoFrSet
{
	return t != null
		&& o.Value && !t.Value
		&& f.Move(t, count);
}

[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static bool MOS(Peg f, Peg o, Peg t, int count) //MoOvSet
{
	return t != null && f != null
		&& f.Value && !t.Value
		&& f.Move(t, count);
}

public class Peg
{
	public bool Value;

	public bool IsCorner;

	public Peg[] U = new Peg[2]; //Up
	public Peg[] D = new Peg[2]; //Down
	public Peg[] L = new Peg[2]; //Left
	public Peg[] R = new Peg[2]; //Right
	
	public bool Move(Peg v2, int count)
	{
		Peg v1 = null; //Over, v2 = To

		if (U[1] == v2) v1 = U[0];
		else if (D[1] == v2) v1 = D[0];
		else if (L[1] == v2) v1 = L[0];
		else if (R[1] == v2) v1 = R[0];

		//Make:
		Value = false; v1.Value = false; v2.Value = true;
		if (--count == 1)
		{
			sCount++; //Should log solutions found?
					  //Rolback:
			Value = true; v1.Value = true; v2.Value = false;
			return false; //Should return on first solution
		}

		//Signal:
		if (!this.IsCorner) //Not possible to jump over corners.
		{
			//Can move in over:
			if (v1.U[0] == this || v1.U[0] == v2)
			{
				if (MTE(v1.L[1], v1.L[0], v1, count)) return true;
				if (MTE(v1.R[1], v1.R[0], v1, count)) return true;
			}
			else
			{
				if (MTE(v1.U[1], v1.U[0], v1, count)) return true;
				if (MTE(v1.D[1], v1.D[0], v1, count)) return true;
			}
		}

		//Can move in here:
		if (U[1] != v2 && MTE(U[1], U[0], this, count)) return true;
		if (D[1] != v2 && MTE(D[1], D[0], this, count)) return true;
		if (L[1] != v2 && MTE(L[1], L[0], this, count)) return true;
		if (R[1] != v2 && MTE(R[1], R[0], this, count)) return true;

		//Can move from to:
		if (v2.D[1] != this && MFS(v2, v2.D[0], v2.D[1], count)) return true;
		if (v2.U[1] != this && MFS(v2, v2.U[0], v2.U[1], count)) return true;
		if (v2.L[1] != this && MFS(v2, v2.L[0], v2.L[1], count)) return true;
		if (v2.R[1] != this && MFS(v2, v2.R[0], v2.R[1], count)) return true;

		//Can move over to:
		if (!v2.IsCorner)
		{
			if (v2.U[0] != v1 && MOS(v2.U[0], v2, v2.D[0], count)) return true;
			if (v2.D[0] != v1 && MOS(v2.D[0], v2, v2.U[0], count)) return true;
			if (v2.L[0] != v1 && MOS(v2.L[0], v2, v2.R[0], count)) return true;
			if (v2.R[0] != v1 && MOS(v2.R[0], v2, v2.L[0], count)) return true;
		}

		//Rolback:
		Value = true; v1.Value = true; v2.Value = false;
		return false;
	}
}

public Peg[] Config(int h, int w)
{
	//Create:
	var b = (new[] { 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0 })
		.Select(val => val != 0 ? new Peg { Value = val == 1 } : null)
		.ToArray();

	//Map:
	for (int y = 0; y < h; y++)
	{
		for (int x = 0; x < w; x++)
		{
			var obj = b[y * w + x];
			if (obj == null) continue;

			if (x > 0) obj.L[0] = b[y * w + x - 1];
			if (x > 1) obj.L[1] = b[y * w + x - 2];

			if (x < w - 1) obj.R[0] = b[y * w + x + 1];
			if (x < w - 2) obj.R[1] = b[y * w + x + 2];

			if (y > 0) obj.U[0] = b[y * w + x - w];
			if (y > 1) obj.U[1] = b[y * w + x - (2 * w)];

			if (y < h - 1) obj.D[0] = b[y * w + x + w];
			if (y < h - 2) obj.D[1] = b[y * w + x + (2 * w)];

			var sides = 0;
			if (obj.U[0] != null) sides++;
			if (obj.D[0] != null) sides++;
			if (obj.L[0] != null) sides++;
			if (obj.R[0] != null) sides++;
			if(sides == 2)
				obj.IsCorner = true;
		}
	}

	return b;
}