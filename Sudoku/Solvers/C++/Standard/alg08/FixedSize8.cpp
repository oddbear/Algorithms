#include "FixedSize8.h"

namespace fs8
{
	int FindUsedForLocal(int n, int s[]);
	bool finn_losning(int n, int s[]);

	void finn_losning(int arr[])
	{
		finn_losning(0, arr);
	}

	bool finn_losning(int n, int s[])
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
		int local_x = n % 9;
		int local_y = n / 9;

		int start_y = local_y * 9;

		int anchor = n
			- local_x % 3
			- (local_y % 3) * 9;

		int brukt = 0;
		for (int i = 0; i < 9; i++)
		{
			brukt |= 1 << s[start_y + i];
			brukt |= 1 << s[i * 9 + local_x];
			brukt |= 1 << s[
				anchor
				+ (i / 3) * 9
				+ (i % 3)
			];
		}

		return brukt;
	}
}
