<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

static int x = 4, y = 5, size = x * y;

void Main()
{
	var path = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "kakuro5x4.txt");
	var fields = JsonBoardParser(path);
	FindSolution(0, fields);
	
	Display(fields);
}

static bool FindSolution(int n, Field[] f)
{
	for (; n < size && !f[n].clear; n++) ;
	
	if (n == size)
		return true;
	
	for (int i = 1; i <= 9; i++)
	{
		f[n].value = i;
		
		if (!IsValidX(n, f)) continue;
		if (!IsValidY(n, f)) continue;
		
		if (FindSolution(n + 1, f))
			return true;
	}
	f[n].value = 0;
	
	return false;
}

static bool IsValidX(int n, Field[] f)
{
	int ty = (n / x) * x;
	int ny = ((n + 1) / x) * x;
	
	if (ty != ny || !f[n + 1].clear)
	{
		int sum = 0;
		
		int j = n;
		var used = new bool[9];
		for (; f[j].clear; j--)
		{
			var val = f[j].value;
			sum += val;
			if (used[val - 1]) return false;
			else used[val -1] = true;
		}
		return f[j].right == sum;
	}
	
	return true;
}

static bool IsValidY(int n, Field[] f)
{
	int nn = n + x;
	if (nn > size || !f[n + x].clear)
	{
		int sum = 0;
		
		int j = n;
		var used = new bool[9];
		for (; f[j].clear; j -= x)
		{
			var val = f[j].value;
			sum += val;
			if (used[val - 1]) return false;
			else used[val -1] = true;
		}
		return f[j].down == sum;
	}
	
	return true;
}

static void Display(Field[] f)
{
	var matrix = new string[y,x];
	for(int i = 0; i < size; i++)
	{
		int lx = i % x;
		int ly = (i / x);
		if (f[i].right != 0 || f[i].down != 0)
			matrix[ly, lx] = string.Format(
				@"{0}\{1}",
				f[i].down == 0 ? "¯¯" : f[i].down.ToString(),
				f[i].right == 0 ? "__" : f[i].right.ToString()
				);
		else if (f[i].value != 0)
			matrix[ly, lx] = f[i].value.ToString();
		else
			matrix[ly, lx] = "[===]";
	}
	matrix.Dump();
}

class Field
{
	public bool empty { get; set; }
	public bool clear { get; set; }
	public int right { get; set; }
	public int down { get; set; }
	public int value { get; set; }
}

static Field[] JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	return js.Deserialize<Field[]>(File.ReadAllText(path));
}