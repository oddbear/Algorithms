/*
 * A version that does support dynamic sizes of the board.
 */

#include "Dynamic6.h"

using namespace std;

namespace d6
{
	int LWIDTH;
	int LHEIGHT;
	int COLUMNS;
	int SIZE;

	int FindUsedForLocal(int n, int s[]);
	bool find_solution(int n, int s[]);

	void find_solution(int arr[], int x, int y)
	{
		LWIDTH = x;
		LHEIGHT = y;
		COLUMNS = LWIDTH * LHEIGHT;
		SIZE = COLUMNS * COLUMNS;
		find_solution(0, arr);
	}

	bool find_solution(int n, int s[])
	{
		for (; n < SIZE && s[n] != 0; n++) ; //Skips those who already has values.

		if (n == SIZE) return true; //solution found, if compiled with false, it will do all posible combinations.

		int brukt = FindUsedForLocal(n, s); //Find all posible values to try.

		for (int i = 1; i <= COLUMNS; i++)
			if ((brukt & (1 << i)) == 0) //Checks if value(i) is a posible,
			{                            // 0 means that it is not a conflict on x, y or g, and might be a posibility.
				s[n] = i; //Sets i as a posbile.
				if (find_solution(n + 1, s)) //Tries next position.
					return true; //if solution is found, don't do more work.
			}

		s[n] = 0; //None of the posibles worked. Resets the value.

		return false; //solution not found, yet.
	}

	int FindUsedForLocal(int n, int s[])
	{
		int local_x = n % COLUMNS; //Finds the first position in the y axis.
		int local_y = n / COLUMNS;

		int start_y = local_y * COLUMNS; //Finds the first position in the x axis.

		int anchor = n //Finds the first position in the group.
			- local_x % LWIDTH
			- (local_y % LHEIGHT) * COLUMNS;

		int brukt = 0;
		for (int i = 0; i < COLUMNS; i++) //Test all x, y and group values,
		{				  // does not mather if it also checks the value in the n position.
			brukt |= 1 << s[start_y + i]; //Gather info from the y axis.
			brukt |= 1 << s[i * COLUMNS + local_x];	//Gather info from the x axis.
			brukt |= 1 << s[ //Gather info from the group.
				anchor
				+ (i / LWIDTH) * COLUMNS
				+ (i % LWIDTH)
			];
		}

		return brukt; //Returns all the values it cannot be. All not set is a posiblity.
	}
}
