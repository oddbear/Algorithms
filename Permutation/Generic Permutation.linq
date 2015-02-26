<Query Kind="Program" />

void Main()
{
	var x = new [] { 1, 2, 3, 4 }; //, 5, 6, 7, 8 };
	
	//Get the number of answers that should be used.
	var factorial = GetFactorial(x.Length);
	
	//Store sub-result to results, must be a copy since it reference based. Therefor .ToArray();
	//Might instead be used to display result, or whatever you want to do with it.
	var results1 = new List<int[]>(factorial);
	var results2 = new List<int[]>(factorial);
	Action<int[]> func1 = (arr) => results1.Add(arr.ToArray());
	Action<int[]> func2 = (arr) => results2.Add(arr.ToArray());
	Perm1(x.ToArray(), 0, func1);
	Perm2(x.ToArray(), 0, func2);
	
	var count = 100000;
	//Test the two algorithms against each other:
	var sw = Stopwatch.StartNew();
	for(int i = 0; i < count; i++)
		Perm1(x.ToArray(), 0, null);	//This seems a little bit faster.
	sw.Stop();
	sw.Elapsed.Dump();
	sw.Restart();
	for(int i = 0; i < count; i++)
		Perm2(x.ToArray(), 0, null);	//This has nicer order of the results.
	sw.Stop();
	sw.Elapsed.Dump();
	
	//Debug:
	Tests(results1, factorial, "a");
	Tests(results2, factorial, "b");
	
	//Both results should be the same (but might be in different order).
	var r1s = results1.Select(r => string.Join(" ", r)).OrderBy(s => s);
	var r2s = results2.Select(r => string.Join(" ", r)).OrderBy(s => s);
	if(!r1s.SequenceEqual(r2s))
		Console.WriteLine("Bad 3, The two algorithms did not give the same results.");
}

private static void Tests(List<int[]> results, int factorial, string alg)
{
	if(results.Count != factorial)
		Console.WriteLine("Bad 1-{0}, Count is not the same as the factorial.", alg);
	
	if(results.Select(r => string.Join(" ", r)).Distinct().Count() != results.Count) //Will also fail if dataset is not distinct.
		Console.WriteLine("Bad 2-{0}, Same parmutation appeared multiple times.", alg);
	
	//results.Select(r => string.Join(" ", r)).Dump();
}

public static void Perm1<T>(T[] arr, int p, Action<T[]> func)
{
	if(p == arr.Length)
	{
		if(func != null)
			func(arr);
		return;
	}
	
	Perm1(arr, p + 1, func);
	for(int i = p + 1; i < arr.Length; i++)
	{
		Swap(ref arr[p], ref arr[i]);
		Perm1(arr, p + 1, func);
		Swap(ref arr[p], ref arr[i]);
	}
}

public static void Perm2<T>(T[] arr, int p, Action<T[]> func)
{
	if(p == arr.Length)
	{
		if(func != null)
			func(arr);
		return;
	}
	
	Perm2(arr, p + 1, func);
	for(int i = p + 1; i < arr.Length; i++)
	{
		Swap(ref arr[p], ref arr[i]);
		Perm2(arr, p + 1, func);
	}
	
	Rotate_Window_One_Left(arr, p);
}

private static void Rotate_Window_One_Left<T>(T[] arr, int p)
{
	var x = arr[p];
	for (int k = p + 1;  k < arr.Length;  k++)
		arr[k - 1] = arr[k];
	
	arr[arr.Length - 1] = x;
}

private static void Swap<T>(ref T a, ref T b)
{
	var t = a;
	a = b;
	b = t;
}

private static int GetFactorial(int number)
{
	//var factorial = Enumerable.Range(1, len).Aggregate((a, b) => a * b); //Sloooooow.
	var factorial = 1;
	checked {
		for(int f = 1; f < number; f++)
			factorial += f * factorial;
	}
	return factorial;
}