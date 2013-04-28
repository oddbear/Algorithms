<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

const int SCOLUMN = 3;
const int COLUMN = SCOLUMN * SCOLUMN;
const int SIZE = COLUMN * COLUMN;

static Field[] fields = null;
static Dictionary<int, Field> IndexOfIntrest = null;

void Main()
{
	var path = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "board1.txt");
	fields = JsonBoardParser(path);
	
	var sw = Stopwatch.StartNew();
	IndexOfIntrest = new Dictionary<int, Field>();
	foreach (var field in fields)
		IndexOfIntrest.Add(field.arr.Max(), field);
	
	var s = new int[SIZE];
	
	FindSolution(0, s);
	sw.Stop();
	
	Display(s);
	sw.Dump();
}

public static bool FindSolution(int n, int[] s)
{
	if (n == SIZE)
		return true;
	
	var brukt = FindUsedForLocal(n, s);
	
	for (int i = 1; i <= COLUMN; i++)
		if((brukt & (1 << i)) == 0)
		{
			s[n] = i;
			
			if(IndexOfIntrest.ContainsKey(n) && !IndexOfIntrest[n].IsLegal(s))
				continue;
			
			if(FindSolution(n + 1, s))
				return true;
		}
	
	s[n] = 0;
	
	return false;
}

public static int FindUsedForLocal(int n, int[] s)
{
	var local_x = n % COLUMN;
	var local_y = n / COLUMN;
	
	var start_y = local_y * COLUMN;
	
	var anchor = n
		- local_x % SCOLUMN
		- (local_y % SCOLUMN) * COLUMN;
		
	var brukt = 0;
	for (int i = 0; i < COLUMN; i++)
	{
		brukt |= 1 << s[start_y + i];
		brukt |= 1 << s[i * COLUMN + local_x];
		brukt |= 1 << s[
			anchor
			+ (i / SCOLUMN) * COLUMN
			+ (i % SCOLUMN)
		];
	}
	
	return brukt;
}

class Field
{
	public int sum { get; set; }
	public int[] arr { get; set; }
	
	public bool IsLegal(int[] s)
	{
		int localSum = 0;
		
		for (int i = 0; i < arr.Length; i++)
			localSum += s[arr[i]];
		
		return sum == localSum;
	}
}

public static void Display(int[] s)
{
	for(int i = 0; i < 81; i++)
	{
		if(i % 9 == 0 && i != 0) Console.WriteLine();
		Console.Write(s[i]);
	}
	Console.WriteLine();
}

static Field[] JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	return js.Deserialize<Field[]>(File.ReadAllText(path));
}