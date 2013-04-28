<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

const int SCOLUMN = 3;
const int COLUMN = SCOLUMN * SCOLUMN;
const int SIZE = COLUMN * COLUMN;

void Main()
{
	var path = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "board1.txt");
	Input[] inputs = JsonBoardParser(path);
	
	var sw = Stopwatch.StartNew();
	
	var s = new Field[SIZE];
	foreach (var input in inputs)
	{
		var arr = input.arr;
		var len = arr.Length;
		var sum = input.sum;
		var fi = new FieldInfo(len);
		fi.sum = sum;
		var endIndex = arr.Max();
		int p = 0;
		foreach (var i in arr)
		{
			var field = new Field();
			field.isEndField = i == endIndex;
			field.fieldInfo = fi;
			fi.group[p++] = field;
			s[i] = field;
			
			if (len == 1) field.SetValue(sum);
		}
	}
	
	FindSolution(0, s);
	sw.Stop();
	
	Display(s);
	sw.Dump();
}

class FieldInfo
{
	public int sum;
	public Field[] group;
	public int tmpValue = 0;
	
	public FieldInfo(int len)
	{
		group = new Field[len];
	}
}

class Field
{
	public int value;
	public bool isEndField;
	
	public FieldInfo fieldInfo;
	
	public bool SetValue(int value)
	{
		this.value = value;
		this.fieldInfo.tmpValue += value;
		if (isEndField && this.fieldInfo.tmpValue != this.fieldInfo.sum)
		{
			RemoveValue();
			return false;
		}
		return true;
	}
	
	public void RemoveValue()
	{
		this.fieldInfo.tmpValue -= value;
		this.value = 0;
	}
}

static bool FindSolution(int n, Field[] s)
{
	for(; n < SIZE && s[n].value != 0; n++) ;
	
	if (n == SIZE)
		return true;
	
	var brukt = FindUsedForLocal(n, s);
	
	var field = s[n];
	
	for (int i = 1; i <= COLUMN; i++)
		if((brukt & (1 << i)) == 0)
		{
			if (!field.SetValue(i))
				continue;
			
			if(FindSolution(n + 1, s))
				return true;
			
			field.RemoveValue();
		}
	
	return false;
}

static int FindUsedForLocal(int n, Field[] s)
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
		brukt |= 1 << s[start_y + i].value;
		brukt |= 1 << s[i * COLUMN + local_x].value;
		brukt |= 1 << s[
			anchor
			+ (i / SCOLUMN) * COLUMN
			+ (i % SCOLUMN)
		].value;
	}
	
	return brukt;
}

static void Display(Field[] s)
{
	for(int i = 0; i < 81; i++)
	{
		if(i % 9 == 0 && i != 0) Console.WriteLine();
		Console.Write(s[i].value);
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