#include "JigSaw6.h"

namespace js6
{
	int LWIDTH;
	int LHEIGHT;
	int COLUMNS;
	int SIZE;

	int FindUsedForLocal(int n, int s[], int g[], int map[]);
	bool finn_losning(int n, int s[], int g[], int map[]);
	int* CreateArray(int size);

	void finn_losning(int arr[], int arrG[], int x, int y)
	{
		LWIDTH = x;
		LHEIGHT = y;
		COLUMNS = LWIDTH * LHEIGHT;
		SIZE = COLUMNS * COLUMNS;

		int* map = CreateArray(SIZE);
		int* groups = CreateArray(SIZE);

		for (int i = 0; i < SIZE; i++)
		{
			int grp = arrG[i];
			map[grp * COLUMNS + groups[grp]++] = i;
		}
		delete[] groups;
		finn_losning(0, arr, arrG, map);
		delete[] map;
	}

	bool finn_losning(int n, int s[], int g[], int map[])
	{
		for (; n < SIZE && s[n] != 0; n++) ;

		if (n == SIZE) return true;

		int brukt = FindUsedForLocal(n, s, g, map);

		for (int i = 1; i <= COLUMNS; i++)
			if ((brukt & (1 << i)) == 0)
			{
				s[n] = i;
				if (finn_losning(n + 1, s, g, map))
					return true;
			}

		s[n] = 0;

		return false;
	}

	int FindUsedForLocal(int n, int s[], int g[], int map[])
	{
		int local_x = n % COLUMNS;
		int local_y = n / COLUMNS;
		
		int start_index = g[n] * COLUMNS;
		
		int brukt = 0;
		for (int i = 0; i < COLUMNS; i++)
		{
			int iMap = map[start_index + i];
			brukt |= 1 << s[iMap];
			brukt |= 1 << s[local_y * COLUMNS + i];
			brukt |= 1 << s[i * COLUMNS + local_x];
		}

		return brukt;
	}

	int* CreateArray(int size)
	{
		int* arr = new int[size];
		for (int i = 0; i < size; i++)
			arr[i] = 0;
		return arr;
	}
}
