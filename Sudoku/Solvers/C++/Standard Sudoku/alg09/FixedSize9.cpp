/*
 * A version that does not use recursion.
 */

#include "FixedSize9.h"

namespace fs9
{
	const int GROUPWIDTH = 3;
	const int WIDTH = 9;
	const int SIZE = 81;

	int FindUsedForLocal(int n, int s[]);
	bool find_solution(int n, int s[]);
	int GetShifted(int i);
	int GetUnShifted(int i);
	
	void find_solution(int arr[])
	{
		for (int i = 0; i < SIZE; i++)
			arr[i] = GetShifted(arr[i]); //Fill array with values in bit shifted base 2.

		find_solution(0, arr);

		for (int i = 0; i < SIZE; i++)
			arr[i] = GetUnShifted(arr[i]); //Fill array with solution in base 10.
	}

	bool find_solution(int n, int s[])
	{
		int b[SIZE]; //Array to store posibles.
		for(int i = 0; i < SIZE; i++)
			b[i] = EMPTY;

		for (; n < SIZE && s[n] != EMPTY; n++) ; //Skips all the first positions with values, finds the first empty.
		int min = n; //First position without a value.

		while(n < SIZE)
		{
			int i, brukt;

			if (s[n] != EMPTY) //Resumes states of posibles at n, and tries next.
			{
				brukt = b[n];
				i = s[n] << 1;
			}
			else //Finds all posibles for the position.
			{
				brukt = b[n] = FindUsedForLocal(n, s);
				i = ONE;
			}

			for (; i < FINAL; i <<= 1)
				if ((brukt & i) == 0) //Fins and tries first posible value.
				{
					s[n] = i; //Sets and tries a posible value.
					break;
				}
		
			if (i == FINAL) //All posibles tested.
			{
				s[n] = b[n] = EMPTY; //Clears the values.

				if (n == min)	//No solution found.
					return false; //It has gone throught all posiblities and not found any solution.

				for (; n > min && b[n] == EMPTY; n--) ; //Skips all n's that is not set by code.
			}
			else
			{
				n++; //Gets ready to try posibles at the next position.
				for (; n < SIZE && s[n] != EMPTY && b[n] == EMPTY; n++) ; //Skips all n's that is not set by code.
			}
		}

		return true;	//Solution found.
	}

	int FindUsedForLocal(int n, int s[])
	{
		int local_x = n % WIDTH; //Finds the first position in the y axis.
		int local_y = n / WIDTH;

		int start_y = local_y * WIDTH; //Finds the first position in the x axis.

		int anchor = n //Finds the first position in the group.
			- local_x % GROUPWIDTH
			- (local_y % GROUPWIDTH) * WIDTH;

		int brukt = 0;

		for (int i = 0; i < WIDTH; i++) //Test all x, y and group values,
		{				                // does not mather if it also checks the value in the n position.
			brukt |= s[start_y + i]; //Gather info from the y axis.
			brukt |= s[i * WIDTH + local_x];	//Gather info from the x axis.
			brukt |= s[ //Gather info from the group.
				anchor
				+ (i / GROUPWIDTH) * WIDTH
				+ (i % GROUPWIDTH)
			];
		}

		return brukt; //Returns all the values it cannot be. All not set is a posiblity.
	}

	int GetShifted(int i)
	{
		switch (i)
		{
			case 1:  return ONE;			//00 0000 0001
			case 2:  return TWO;			//00 0000 0010
			case 3:  return THREE;			//00 0000 0100
			case 4:  return FOUR;			//00 0000 1000
			case 5:  return FIVE;			//00 0001 0000
			case 6:  return SIX;			//00 0010 0000
			case 7:  return SEVEN;			//00 0100 0000
			case 8:  return EIGHT;			//00 1000 0000
			case 9:  return NINE;			//01 0000 0000
			case 10: return FINAL;			//10 0000 0000
			default: return EMPTY;			//00 0000 0000
		}
	}

	int GetUnShifted(int i)
	{
		switch (i)
		{
			case ONE:   return 1;			//00 0000 0001
			case TWO:   return 2;			//00 0000 0010
			case THREE: return 3;			//00 0000 0100
			case FOUR:  return 4;			//00 0000 1000
			case FIVE:  return 5;			//00 0001 0000
			case SIX:   return 6;			//00 0010 0000
			case SEVEN: return 7;			//00 0100 0000
			case EIGHT: return 8;			//00 1000 0000
			case NINE:  return 9;			//01 0000 0000
			case FINAL: return 10;			//10 0000 0000
			default:    return 0;			//00 0000 0000
		}
	}
}
