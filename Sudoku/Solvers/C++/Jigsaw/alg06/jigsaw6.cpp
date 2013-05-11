#include "jigsaw6.h"

namespace js6
{
	int LWIDTH;
	int LHEIGHT;
	int COLUMNS;
	int SIZE;

	int FindUsedForLocal(int n, int s[], int g[], int map[]);
	bool find_solution(int n, int s[], int g[], int map[]);
	int* CreateArray(int size);

	void find_solution(int arr[], int arrG[], int x, int y)
	{
		LWIDTH = x;
		LHEIGHT = y;
		COLUMNS = LWIDTH * LHEIGHT;
		SIZE = COLUMNS * COLUMNS;

		int* map = CreateArray(SIZE);
		int* groups = CreateArray(SIZE); //SIZE?, COLUMNS should be OK.

		for (int i = 0; i < SIZE; i++) //Makes the mapping array, that maps the groups together.
		{
			int grp = arrG[i];
			map[grp * COLUMNS + groups[grp]++] = i; //"Partitions" the groups by the width, and increment the others in the group to spread them over the "partition".
			//Ex: group = 2, columns = 9(default sudoku), groups[] increments by the group.   First: 2 * 9 + 0,   Next: 2 * 9 + 1... Last in group: 2 * 9 + 8, then: 3 * 9 + 0... etc.
		}
		delete[] groups; //Clears the groups array.
		find_solution(0, arr, arrG, map);
		delete[] map; //Clears the map array.
	}

	bool find_solution(int n, int s[], int g[], int map[])
	{
		for (; n < SIZE && s[n] != 0; n++) ; //Skips those who already has values.

		if (n == SIZE) return true; //solution found, if compiled with false, it will do all posible combinations.

		int brukt = FindUsedForLocal(n, s, g, map); //Find all posible values to try.

		for (int i = 1; i <= COLUMNS; i++)
			if ((brukt & (1 << i)) == 0) //Checks if value(i) is a posible,
			{                            // 0 means that it is not a conflict on x, y or g, and might be a posibility.
				s[n] = i; //Sets i as a posbile.
				if (find_solution(n + 1, s, g, map)) //Tries next position.
					return true; //if solution is found, don't do more work.
			}

		s[n] = 0; //None of the posibles worked. Resets the value.

		return false; //solution not found, yet.
	}

	int FindUsedForLocal(int n, int s[], int g[], int map[])
	{
		int local_x = n % COLUMNS; //Finds the first position in the y axis.
		int local_y = n / COLUMNS;
		
		int start_index = g[n] * COLUMNS; //Finds the position of the first value in the group.
		
		int brukt = 0;
		for (int i = 0; i < COLUMNS; i++)
		{
			int iMap = map[start_index + i]; //Find a position in the group, based on the group mapping.
			brukt |= 1 << s[iMap]; //Gather info from the group.
			brukt |= 1 << s[local_y * COLUMNS + i]; //Gather info from the y axis.
			brukt |= 1 << s[i * COLUMNS + local_x];	//Gather info from the x axis.
		}

		return brukt; //Returns all the values it cannot be. All not set is a posiblity.
	}

	int* CreateArray(int size) //Creates an array with empty values.
	{
		int* arr = new int[size];
		for (int i = 0; i < size; i++)
			arr[i] = 0;
		return arr;
	}
}
