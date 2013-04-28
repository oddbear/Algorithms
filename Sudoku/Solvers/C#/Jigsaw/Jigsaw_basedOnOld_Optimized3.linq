<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

public const int x = 3;
public const int y = 3;

public const int columns = x * y;
public const int tot = columns * columns;

public Field[] s;

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
	var current = s[n];
	var brukt = 0;
	
	for(int i = 0; i < current.fieldsGroup.Length; i++)
		brukt |= 1 << current.fieldsGroup[i].Value;
	for(int i = 0; i < current.xGroup.Length; i++)
		brukt |= 1 << current.xGroup[i].Value;
	for(int i = 0; i < current.yGroup.Length; i++)
		brukt |= 1 << current.yGroup[i].Value;
	
	return brukt;
}

public class Field
{
	public int Value { get; set; }
	public short Group { get; private set; }
	
	public Field[] fieldsGroup { get; set; }
	public Field[] xGroup { get; set; }
	public Field[] yGroup { get; set; }
	
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
	
	for(int i = 0; i < tot; i++)
	{
		var current = s[i];
		current.fieldsGroup = s.Where(f => f.Group == current.Group).Where(f => f != current).ToArray();
		
		var xgroup = new List<Field>();
		var xStart = i - (i % columns);
		for(int cx = xStart; cx < xStart + columns; cx++)
			xgroup.Add(s[cx]);
		current.xGroup = xgroup.Where(f => f != current).ToArray();
		
		
		var ygroup = new List<Field>();
		var yStart = i % columns;
		for(int cx = 0; cx < columns; cx++)
			ygroup.Add(s[cx * columns + yStart]);
		current.yGroup = ygroup.Where(f => f != current).ToArray();
	}
}