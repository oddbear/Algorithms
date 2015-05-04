<Query Kind="Program" />

/*
 * My Second Nonogram solver.
 * This algorithm works. It's easy. Faster than trying every combination.
 * This is also a lot faster than the first. 15x15 in 2seconds. Can be optimized more.
 * Still not good enough for "worst cases" with * solutions.
 * Should be good enough to find first solution if any.
 */

void Main()
{
	//Picture:
	//X###
	//####
	//XXXX
	//XX#X
	/*
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
	*/
	//Picture:
	//####XX######XX#
	//####X#XXXXXX#X#
	//####X##X###X#X#
	//##XX#XX#####X##
	//##X#####XX#XX##
	//XXXX#X######X##
	//XXXXXX###XXXXX#
	//XXXX##XX#X#X#X#
	//XXXX###XXXXXXX#
	//#XX#####X#XXX##
	//########X######
	//########X######
	//###X###X#####X#
	//###X###X#X#X#X#
	//XXXX##XXX#XX#XX
	var pX = new int[][] {
		new [] {4,1},
		new [] {5,1},
		new [] {7,1},
		new [] {1,4,3},
		new [] {3,1},
	
		new [] {1,1,2},
		new [] {1,1,1,1},
		new [] {2,2,3},
		new [] {1,1,4,1},
		new [] {1,1,3,1},
	
		new [] {1,1,2,1},
		new [] {2,1,4,2},
		new [] {1,4,2},
		new [] {3,3,3},
		new [] {1}
	};
	
	var pY = new int[][] {
		new [] {2,2},
		new [] {1,6,1},
		new [] {1,1,1,1},
		new [] {2,2,1},
		new [] {1,2,2},
	
		new [] {4,1,1},
		new [] {6,5},
		new [] {4,2,1,1,1},
		new [] {4,7},
		new [] {2,1,3},
	
		new [] {1},
		new [] {1},
		new [] {1,1,1},
		new [] {1,1,1,1,1},
		new [] {4,3,2,2}
	};
	/*
	var pX = new int[][] {
		new [] {1}, new [] {1}, new [] {1}, new [] {1}, new [] {1},
		new [] {1}, new [] {1}, new [] {1}, new [] {1}, new [] {1},
		new [] {1}, new [] {1}, new [] {1}, new [] {1}, new [] {1}
	};
	
	var pY = new int[][] {
		new [] {1}, new [] {1}, new [] {1}, new [] {1}, new [] {1},
		new [] {1}, new [] {1}, new [] {1}, new [] {1}, new [] {1},
		new [] {1}, new [] {1}, new [] {1}, new [] {1}, new [] {1}
	};
	*/
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
	
	Solve(b, ppX, 0, ppY);
}

static void Solve(bool[,] b, List<bool[]>[] xL, int xPos, List<bool[]>[] yL)
{
	if(xPos == xL.Length)
	{
		DisplayB(b);
		return;
	}
	
	var lines = xL[xPos];
	foreach(var line in lines)
	{
		for(int y = 0; y < yL.Length; y++) //Copy down on axis.
			b[y, xPos] = line[y];
		
		//Reduse all Ys to just those posible:
		var newYl = new List<bool[]>[yL.Length];
		for(int y  = 0; y < yL.Length; y++)
		{
			//A new redused list of possible Ys:
			newYl[y] = new List<bool[]>();
			
			var yLines = yL[y]; //List<bool[]>
			for(int i = 0; i < yLines.Count; i++) //"Unknown" size.
			{
				var yLine = yLines[i];
				if(line[y] == yLine[xPos])
					newYl[y].Add(yLine);
			}
		}
		
		if(newYl.All(l => l.Count() != 0)) //If single Y is empty, this is not the right path. Do a rollback.
			Solve(b, xL, xPos + 1, newYl);
	}
}

//Displays the whole board:
static void DisplayB(bool[,] b)
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
	Console.WriteLine();
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