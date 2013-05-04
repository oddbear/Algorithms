#include "FixedSize9.h"

namespace fs9
{
	const int GROUPWIDTH = 3;
	const int WIDTH = 9;
	const int SIZE = 81;

	int FindUsedForLocal(int n, int s[]);
	bool finn_losning(int n, int s[]);
	int GetShifted(int i);
	int GetUnShifted(int i);
	
	void finn_losning(int arr[])
	{
		for (int i = 0; i < SIZE; i++)
			arr[i] = GetShifted(arr[i]);

		finn_losning(0, arr);

		for (int i = 0; i < SIZE; i++)
			arr[i] = GetUnShifted(arr[i]);
	}

	bool finn_losning(int n, int s[])
	{
		int b[SIZE];
		for(int i = 0; i < SIZE; i++)
			b[i] = EMPTY;

		for (; n < SIZE && s[n] != EMPTY; n++) ;
		int min = n;

		while(n < SIZE)
		{
			int i, brukt;

			if (s[n] != EMPTY)
			{
				brukt = b[n];
				i = s[n] << 1;
			}
			else
			{
				brukt = b[n] = FindUsedForLocal(n, s);
				i = ONE;
			}

			for (; i < FINAL; i <<= 1)
				if ((brukt & i) == 0)
				{
					s[n] = i;
					break;
				}
		
			if (i == FINAL)
			{
				s[n] = b[n] = EMPTY;

				if (n == min)
					return false;

				for (; n > min && b[n] == EMPTY; n--) ;
			}
			else
			{
				n++;
				for (; n < SIZE && s[n] != EMPTY && b[n] == EMPTY; n++) ;
			}
		}

		return true;
	}

	int FindUsedForLocal(int n, int s[])
	{
		int local_x = n % WIDTH;
		int local_y = n / WIDTH;

		int start_y = local_y * WIDTH;

		int anchor = n
			- local_x % GROUPWIDTH
			- (local_y % GROUPWIDTH) * WIDTH;

		int brukt = 0;

		for (int i = 0; i < WIDTH; i++)
		{
			brukt |= s[start_y + i];
			brukt |= s[i * WIDTH + local_x];
			brukt |= s[
				anchor
				+ (i / GROUPWIDTH) * WIDTH
				+ (i % GROUPWIDTH)
			];
		}

		return brukt;
	}

	int GetShifted(int i)
	{
		switch (i)
		{
			case 1:  return ONE;
			case 2:  return TWO;
			case 3:  return THREE;
			case 4:  return FOUR;
			case 5:  return FIVE;
			case 6:  return SIX;
			case 7:  return SEVEN;
			case 8:  return EIGHT;
			case 9:  return NINE;
			case 10: return FINAL;
			default: return EMPTY;
		}
	}

	int GetUnShifted(int i)
	{
		switch (i)
		{
			case ONE:   return 1;
			case TWO:   return 2;
			case THREE: return 3;
			case FOUR:  return 4;
			case FIVE:  return 5;
			case SIX:   return 6;
			case SEVEN: return 7;
			case EIGHT: return 8;
			case NINE:  return 9;
			case FINAL: return 10;
			default:    return 0;
		}
	}
}