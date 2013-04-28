#include "FixedSize6.h"

namespace fs6
{
	int FindUsedForLocal(int n, int s[]);
	bool finn_losning(int n, int s[]);

	void finn_losning(int arr[])
	{
		finn_losning(0, arr);
	}

	bool finn_losning(int n, int s[])
	{
		for (; n < 81 && s[n] != 0; n++) ;

		if (n == 81) return true;

		int brukt = FindUsedForLocal(n, s);

		for (int i = 1; i <= 9; i++)
			if ((brukt & (1 << i)) == 0)
			{
				s[n] = i;
				if (finn_losning(n + 1, s))
					return true;
			}

		s[n] = 0;

		return false;
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
