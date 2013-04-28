<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Extensions.dll</Reference>
</Query>

const int SCOLUMN = 3;
const int COLUMN = SCOLUMN * SCOLUMN;
const int SIZE = COLUMN * COLUMN;
const int BOARDCOUNT = 5;
const int TOTALSIZE = SIZE * BOARDCOUNT - COLUMN * (BOARDCOUNT - 1);

void Main()
{
	var groups = JsonBoardParser(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "board1.txt"));
	
	var fields = new Field[TOTALSIZE];
	var boards = new Board[BOARDCOUNT];
	for (int i = 0; i < BOARDCOUNT; i++)
		boards[i] = new Board();
	
	int p = 0;
	foreach (var group in groups)
	for (int i = 0; i < COLUMN; i++)
	{
		var field = new Field();
		field.value = group.arr[i];
		foreach (var mapping in group.boards)
		{
			var board = boards[mapping.id];
			var bfi = new BoardFieldInfo(field, board, mapping.id, mapping.group, i);
			board.view[bfi.n] = bfi;
			field.boards.Add(bfi);
		}
		fields[p++] = field;
	}
	
	FindSolution(0, fields);
	
	Display(fields);
}

static bool FindSolution(int n, Field[] s)
{
	for(; n < TOTALSIZE && s[n].value != 0; n++) ;
	
	if (n == TOTALSIZE)
		return true;
	
	var field = s[n];
	
	var brukt = 0;
	foreach (var board in field.boards)
		brukt |= board.FindUsedForLocal();
	
	for (int i = 1; i <= 9; i++)
		if((brukt & (1 << i)) == 0)
		{
			field.value = i;
			if(FindSolution(n + 1, s))
				return true;
		}
	
	field.value = 0;
	
	return false;
}

class Board
{
	public BoardFieldInfo[] view = new BoardFieldInfo[SIZE];
}

class BoardFieldInfo
{
	public Board board;
	public Field field;
	public int n, x, y, g, b;
	private int a, sy;
	
	public BoardFieldInfo(Field field, Board board, int b, int g, int i)
	{
		this.board = board;
		this.field = field;
		this.a = (g % SCOLUMN) * SCOLUMN + (g / SCOLUMN) * (SCOLUMN * COLUMN);
		this.n = (i % SCOLUMN) + (i / SCOLUMN) * COLUMN + a;
		this.x = n % 9;
		this.y = n / 9;
		this.g = g;
		this.sy = (n / 9) * 9;
		this.b = b;
	}
	
	public int FindUsedForLocal()
	{
		var brukt = 0;
		for (int i = 0; i < 9; i++)
		{
			brukt |= 1 << board.view[sy + i].field.value;
			brukt |= 1 << board.view[i * 9 + x].field.value;
			brukt |= 1 << board.view[a + (i % 3) + (i / 3) * 9].field.value;
		}
		
		return brukt;
	}
}

class Field
{
	public List<BoardFieldInfo> boards = new List<BoardFieldInfo>();
	public int value;
}

static void Display(Field[] s)
{
	var matrix = new string[21, 21];
	for (int y = 0; y < 21; y++)
	for (int x = 0; x < 21; x++)
		matrix[y, x] = string.Empty;
	
	var bad = new Dictionary<int, Tuple<int, int>>();
	bad.Add(0, new Tuple<int, int>( 0,  0));
	bad.Add(1, new Tuple<int, int>( 0, 12));
	bad.Add(2, new Tuple<int, int>( 6,  6));
	bad.Add(3, new Tuple<int, int>(12,  0));
	bad.Add(4, new Tuple<int, int>(12, 12));
	
	for (int i = 0; i < TOTALSIZE; i++)
	{
		var field = s[i];
		var board = field.boards.First();
		
		var bay = bad[board.b].Item1;
		var bax = bad[board.b].Item2;
		
		matrix[bay + board.y, bax + board.x] = field.value.ToString();
	}
	
	matrix.Dump();
}

static Group[] JsonBoardParser(string path)
{
	var js = new System.Web.Script.Serialization.JavaScriptSerializer();
	return js.Deserialize<Group[]>(File.ReadAllText(path));
}

class Group
{
	public Mapping[] boards { get; set; }
	public int[] arr { get; set; }
	
	public class Mapping
	{
		public int id { get; set; }
		public int group { get; set; }
	}
}