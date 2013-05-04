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
		for (; n < SIZE && s[n] != 0; n++) ;

		if (n == SIZE) return true;

		int brukt = FindUsedForLocal(n, s);

		for (int i = 1; i <= COLUMNS; i++)
			if ((brukt & (1 << i)) == 0)
			{
				s[n] = i;
				if (find_solution(n + 1, s))
					return true;
			}

		s[n] = 0;

		return false;
	}

	int FindUsedForLocal(int n, int s[])
	{
		int local_x = n % COLUMNS;
		int local_y = n / COLUMNS;

		int start_y = local_y * COLUMNS;

		int anchor = n
			- local_x % LWIDTH
			- (local_y % LHEIGHT) * COLUMNS;

		int brukt = 0;
		for (int i = 0; i < COLUMNS; i++)
		{
			brukt |= 1 << s[start_y + i];
			brukt |= 1 << s[i * COLUMNS + local_x];
			brukt |= 1 << s[
				anchor
				+ (i / LWIDTH) * COLUMNS
				+ (i % LWIDTH)
			];
		}

		return brukt;
	}
}
