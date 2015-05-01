<Query Kind="Program" />

/*
 * My first Nonogram solver.
 * This algorithm works. It's easy. Faster than trying every combination.
 * But still, it's not fast enough. Will work well for small boards, but larger ones (will not scale well).
 */

void Main()
{
	//Picture:
	//X###
	//####
	//XXXX
	//XX#X
	var pX = new int[][] {	//PatternX:
		new [] { 1, 2 },	//x0 -> y
		new [] { 2 },		//x1 -> y
		new [] { 1 },		//x2 -> y
		new [] { 2 }		//x3 -> y
		, new int[] {}, new [] { 1 }
	};
	
	var pY = new int[][] {	//PatternY:
		new [] { 1 },		//y0 -> x
		new int[] { },		//y1 -> x
		new [] { 4 },		//y2 -> x
		new [] { 2, 1, 1 }	//y3 -> x
	};
	
	var ppX = new List<bool[]>[pX.Length];
	var ppY = new List<bool[]>[pY.Length];
	
	//Find all possible permutations of every x and y's:
	for(int i = 0; i < pX.Length; i++)
	{
		var lines = new List<bool[]>();
		MakeLines(pX[i], pY.Length, 0, 0, lines, new bool[pY.Length]);
		ppX[i] = lines;
	}
	
	for(int i = 0; i < pY.Length; i++)
	{
		var lines = new List<bool[]>();
		MakeLines(pY[i], pX.Length, 0, 0, lines, new bool[pX.Length]);
		ppY[i] = lines;
	}
	
	var b = new bool[pY.Length, pX.Length];
	
	//Permute all x's:
	PermuteX(b, ppX, 0, ppY);
}

void CheckYs(bool[,] b, List<bool[]>[] yL, int yPos, int l)
{
	//All Xs and Ys are correct, display the board:
	if(yPos == yL.Length)
	{
		DisplayB(b);
		Console.WriteLine();
		return;
	}
	
	var lines = yL[yPos];
	foreach(var line in lines)
	{
		for(int x = 0; x < l; x++)
		{
			if(b[yPos, x] != line[x])
				return;
		}
		CheckYs(b, yL, yPos + 1, l);
	}
}

void PermuteX(bool[,] b, List<bool[]>[] xL, int xPos, List<bool[]>[] yL)
{
	if(xPos == xL.Length)
	{
		//Hit a new permutation of Xs. Check if it is correct with the Ys:
		CheckYs(b, yL, 0, xL.Length);
		return;
	}
	
	var lines = xL[xPos];
	foreach(var line in lines)
	{
		for(int y = 0; y < yL.Length; y++)
			b[y, xPos] = line[y];
		PermuteX(b, xL, xPos + 1, yL);
	}
}

//Displays the whole board:
void DisplayB(bool[,] b)
{
	var w = b.GetLength(0);
	var h = b.GetLength(1);
	for(int y = 0; y < w; y++)
	{
		for(int x = 0; x < h; x++)
		{
			Console.Write(b[y,x] ? "X" : "#");
		}
		Console.WriteLine();
	}
}

//Display a single line:
void DisplayLine(bool[] line)
{
	Console.WriteLine("|{0}|", string.Concat(line.Select(f => f ? "X" : "#")));
}

void MakeLines(int[] pattern, int size, int pp, int pr, List<bool[]> results, bool[] worker)
{
	//End of line:
	if(pp > pattern.Length)
		return;
	
	//Possible line:
	if(pp == pattern.Length)
	{
		results.Add(worker.ToArray());
		return;
	}
	
	//Possible Sub-Line:
	var n = pattern[pp];
	for(int i = 0; i < size - n + 1; i++)
	{
		//Easy first... Swap first to last later?
		for(int j = 0; j < n; j++)
			worker[pr + i + j] = true;
		
		MakeLines(pattern, size - i - n - 1, pp + 1, pr + n + i + 1, results, worker);
		
		for(int j = 0; j < n; j++)
			worker[pr + i + j] = false; //reset.
	}
}