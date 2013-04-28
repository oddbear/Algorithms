<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

const int GROUPWIDTH = 3;
const int WIDTH = GROUPWIDTH * GROUPWIDTH;
const int PADDING = WIDTH * GROUPWIDTH;
const int SIZE = WIDTH * WIDTH;

void Main()
{
	var path = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "board1.txt");
	Input[] inputs = JsonBoardParser(path);
	
	var sw = Stopwatch.StartNew();
	
	var xa = new int[WIDTH];
	var ya = new int[WIDTH];
	var ga = new int[WIDTH];
	var s = new Square[SIZE];
	
	foreach (var input in inputs)
	{
		var arr = input.arr;
		int len = arr.Length,
		    sum = input.sum,
			endIndex = arr.Max();
		
		var summer = new int[1] { sum };
		foreach (var i in arr)
		{
			var isEndField = i == endIndex;
			var field = new Square(i, 0, xa, ya, ga, isEndField, summer);
			s[i] = field;
			
			if (len == 1) field.SetValue(sum);
		}
	}
	FindSolution(0, s);
	sw.Stop();
		
	Display(s);
	sw.Dump();
}

class Square
{
	public int v;
	public bool isEndField;
	
	private int[] summer;
	private int lx, ly, lg;
	private int[] xa, ya, ga;

	public Square(int n, int v, int[] xa, int[] ya, int[] ga, bool isEndField, int[] summer)
	{
		this.isEndField = isEndField;
		this.summer = summer;
	
		this.lx = n % WIDTH; 
		this.ly = n / WIDTH;
		this.lg = (n / GROUPWIDTH) % GROUPWIDTH + (n / PADDING) * GROUPWIDTH;
		
		this.xa = xa;
		this.ya = ya;
		this.ga = ga;
		
		if (v != 0)
			SetValue(v);
	}
	
	public bool CheckSum()
	{
		return summer[0] == 0;
	}
	
	public int GetPosibles()
	{
		return xa[lx] | ya[ly] | ga[lg];
	}
	
	public void SetValue(int value)
	{
		v = value;
		summer[0] -= value;
		
		var shifted = 1 << value;
		xa[lx] |= shifted;
		ya[ly] |= shifted;
		ga[lg] |= shifted;
	}
	
	public void RemoveValue()
	{
		var shifted = 1 << v;
		xa[lx] ^= shifted;
		ya[ly] ^= shifted;
		ga[lg] ^= shifted;
		
		summer[0] += v;
		v = 0;
	}
}


static bool FindSolution(int n, Square[] t)
{
	for (; n < SIZE && t[n].v != 0; n++) ;

	if (n == SIZE)
		return true;
	
	var brukt = t[n].GetPosibles();
	
	for (int i = 1; i <= WIDTH; i++)
		if((brukt & (1 << i)) == 0)
		{
			t[n].SetValue(i);
			
			if (!t[n].isEndField || t[n].CheckSum())
				if (FindSolution(n + 1, t))
					return true;
			
			t[n].RemoveValue();
		}

	return false;
}

static void Display(Square[] s)
{
	for(int i = 0; i < 81; i++)
	{
		if(i % 9 == 0 && i != 0) Console.WriteLine();
		Console.Write(s[i].v);
	}
	Console.WriteLine();
}

static Input[] JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	return js.Deserialize<Input[]>(File.ReadAllText(path));
}

class Input
{
	public int sum { get; set; }
	public int[] arr { get; set; }
}