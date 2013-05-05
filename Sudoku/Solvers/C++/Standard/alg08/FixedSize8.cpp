/*
 * A version that does not use recursion.
 */

#include "FixedSize8.h"

namespace fs8
{
	int FindUsedForLocal(int n, int s[]);
	bool find_solution(int n, int s[]);

	void find_solution(int arr[])
	{
		find_solution(0, arr);
	}

	bool find_solution(int n, int s[])
	{
		const int MAX = 81;
		int b[MAX];
		for(int i = 0; i < MAX; i++)
			b[i] = 0;

		for (; n < MAX && s[n] != 0; n++) ;
		int min = n;
	
		while(n < MAX)
		{
			int i, brukt;

			if (s[n] != 0)
			{
				brukt = b[n];
				i = s[n] + 1;
			}
			else
			{
				brukt = b[n] = FindUsedForLocal(n, s);
				i = 1;
			}

			for (; i <= 9; i++)
				if((brukt & (1 << i)) == 0)
				{
					s[n] = i;
					break;
				}
		
			if (i == 10)
			{
				s[n] = b[n] = 0;
				if (n == min)
					return false;
				for (; n > min && b[n] == 0; n--) ;
			}
			else
			{
				n++;
				for (; n < MAX && s[n] != 0 && b[n] == 0; n++) ;
			}
		}

		return true;
	}

	int FindUsedForLocal(int n, int s[])
	{
		int local_x = n % 9; //Finds the first position in the y axis.
		int local_y = n / 9;

		int start_y = local_y * 9; //Finds the first position in the x axis.

		int anchor = n //Finds the first position in the group.
			- local_x % 3
			- (local_y % 3) * 9;

		int brukt = 0;
		for (int i = 0; i < 9; i++) //Test all x, y and group values,
		{				            // does not mather if it also checks the value in the n position.
			brukt |= 1 << s[start_y + i]; //Gather info from the y axis.
			brukt |= 1 << s[i * 9 + local_x];	//Gather info from the x axis.
			brukt |= 1 << s[ //Gather info from the group.
				anchor
				+ (i / 3) * 9
				+ (i % 3)
			];
		}

		return brukt; //Returns all the values it cannot be. All not set is a posiblity.
	}
}
