<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

public const int x = 3;
public const int y = 3;

public const int columns = x * y;
public const int tot = columns * columns;

public Field[] s;
public Field[] map;

public int solutions = 0;
public const bool firstSolutionOnly = true;

void Main()
{
	JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "jsonBoard1.txt"));
	
	var sw = Stopwatch.StartNew();
	FindSolution(0);
	sw.Stop();
	Console.WriteLine("Algoritm compute time: {0}", sw.Elapsed);
	Console.WriteLine("Algoritm compute time: {0}μs", (sw.Elapsed.TotalMilliseconds * 1000).ToString("# ##0"));
	Display(s);
	Console.WriteLine("Number of solutions: {0}", solutions.ToString());
}

public bool FindSolution(int n)
{
	if (n == tot)
	{
		solutions++;
		return firstSolutionOnly;
	}
		
	if(s[n].Value != 0)
		return FindSolution(n + 1);
	else
	{
		var brukt = FindUsedForLocal(n);
		
		for (int i = 1; i <= columns; i++)
			if ((brukt & (1 << i)) == 0)
			{
				s[n].Value = i;
				if(FindSolution(n + 1))
					return true;
			}
		
		s[n].Value = 0;
		
		return false;
	}
}

public int FindUsedForLocal(int n)
{
	int local_x = n % columns;
	int local_y = n / columns;
	
	var start_index = s[n].Group * columns;
		
	var brukt = 0;
	for (int i = 0; i < columns; i++)
	{
		brukt |= 1 << s[local_y * columns + i].Value;
		brukt |= 1 << s[i * columns + local_x].Value;
		brukt |= 1 << map[start_index + i].Value;
	}
	
	return brukt;
}

public class Field
{
	public int Value { get; set; }
	public short Group { get; private set; }
	
	public Field(short Group)
	{
		this.Group = Group;
	}
}

public void Display(Field[] b)
{
	for(int i = 0; i < 81; i++)
	{
		if(i % 9 == 0) Console.WriteLine();
		Console.Write(b[i].Value);
	}
	Console.WriteLine();
}


public void JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	var json = js.Deserialize<int[][]>(File.ReadAllText(path));

	if(json[0].Length != tot || json[1].Length != tot)
		throw new FileLoadException("Wrong format in file.");
	
	s = new Field[tot];
	for(int i = 0; i < tot; i++)
	{
		s[i] = new Field((short)(json[0][i] - 1));
		s[i].Value = json[1][i];
	}
	
	map = s.OrderBy(f => f.Group).ToArray<Field>();
}