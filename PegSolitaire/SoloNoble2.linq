<Query Kind="Program">
  <Namespace>System.Net</Namespace>
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
	s.M(c, count);
	sCount.Dump();
}

static int sCount = 0;

public class Peg
{
	public bool Value;

	public Peg[] U = new Peg[2];
	public Peg[] D = new Peg[2];
	public Peg[] L = new Peg[2];
	public Peg[] R = new Peg[2];

	public bool CanML => L[1] != null && L[0] != null && Value && L[0].Value && !L[1].Value;
	public bool CanMR => R[1] != null && R[0] != null && Value && R[0].Value && !R[1].Value;
	public bool CanMU => U[1] != null && U[0] != null && Value && U[0].Value && !U[1].Value;
	public bool CanMD => D[1] != null && D[0] != null && Value && D[0].Value && !D[1].Value;

	public bool M(Peg v2, int count)
	{
		Peg v1 = null; //Over, v2 = To

		if (U[1] == v2 && U[0].Value) v1 = U[0];
		else if (D[1] == v2 && D[0].Value) v1 = D[0];
		else if (L[1] == v2 && L[0].Value) v1 = L[0];
		else if (R[1] == v2 && R[0].Value) v1 = R[0];
		//		else
		//			throw new InvalidOperationException("1");
		//
//		if (v2.Value || !v1.Value || !Value)
//			throw new InvalidOperationException("2");
		
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

		//Can move in over:
		if (v1.U[0] == this || v1.U[0] == v2)
		{
			if (v1.L[1] != null && v1.L[0].Value && v1.L[1].Value && v1.L[1].M(v1, count)) return true;
			if (v1.R[1] != null && v1.R[0].Value && v1.R[1].Value && v1.R[1].M(v1, count)) return true;
		}
		else //if (v1.L[0] == this || v1.L[0] == v2)
		{
			if (v1.U[1] != null && v1.U[0].Value && v1.U[1].Value && v1.U[1].M(v1, count)) return true;
			if (v1.D[1] != null && v1.D[0].Value && v1.D[1].Value && v1.D[1].M(v1, count)) return true;
		}

		//Can move in here:
		if (U[1] != null && U[1] != v2 && U[1].Value && U[0].Value && U[1].M(this, count)) return true;
		if (D[1] != null && D[1] != v2 && D[1].Value && D[0].Value && D[1].M(this, count)) return true;
		if (L[1] != null && L[1] != v2 && L[1].Value && L[0].Value && L[1].M(this, count)) return true;
		if (R[1] != null && R[1] != v2 && R[1].Value && R[0].Value && R[1].M(this, count)) return true;

		//Can move from to:
		if (v2.CanMD && v2.M(v2.D[1], count)) return true;
		if (v2.CanMU && v2.M(v2.U[1], count)) return true;
		if (v2.CanML && v2.M(v2.L[1], count)) return true;
		if (v2.CanMR && v2.M(v2.R[1], count)) return true;

		//Can move over to:
		if(v2.U[0] != null && v2.U[0].CanMD && v2.U[0].M(v2.D[0], count)) return true;
		if(v2.D[0] != null && v2.D[0].CanMU && v2.D[0].M(v2.U[0], count)) return true;
		if(v2.L[0] != null && v2.L[0].CanMR && v2.L[0].M(v2.R[0], count)) return true;
		if(v2.R[0] != null && v2.R[0].CanML && v2.R[0].M(v2.L[0], count)) return true;

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
		}
	}
	return b;
}