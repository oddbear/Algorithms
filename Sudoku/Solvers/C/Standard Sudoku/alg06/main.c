#include <stdio.h>

typedef enum { false, true } bool;

int FindUsedForLocal(int n, int s[]);
bool find_solution(int n, int s[]);

bool find_solution(int n, int s[])
{
	for (; n < 81 && s[n] != 0; n++) ; //Skips those who already has values.
	if (n == 81) return true; //solution found, if compiled with false, it will do all posible combinations.

	int brukt = FindUsedForLocal(n, s); //Find all posible values to try.
	int i;
	for (i = 1; i <= 9; i++)
		if ((brukt & (1 << i)) == 0) //Checks if value(i) is a posible,
		{                            // 0 means that it is not a conflict on x, y or g, and might be a posibility.
			s[n] = i; //Sets i as a posbile.
			if (find_solution(n + 1, s))
				return true;
		}

	s[n] = 0; //None of the posibles worked. Resets the value.

	return false; //solution not found, yet.
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
	int i;
	for (i = 0; i < 9; i++) //Test all x, y and group values,
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

int main()
{
	int s[] = {0,0,5, 1,0,9, 4,7,3,
		   0,0,9, 5,0,0, 8,0,6,
		   1,4,0, 8,0,0, 2,0,0,

		   4,0,0, 0,0,0, 0,6,0,
		   0,0,6, 7,0,2, 5,0,0,
		   0,8,0, 0,0,0, 0,0,1,

		   0,0,4, 0,0,1, 0,2,8,
		   5,0,2, 0,0,8, 6,0,0,
		   3,9,8, 2,0,7, 1,0,0};
	if(find_solution(0, s))
	{
		printf("Solution Found!");
		int i;
		for (i = 0; i < 81; i++)
		{
			if (i % 3 == 0) printf(" ");
			if (i % 9 == 0) printf("\n");
			if (i % (3 * 9) == 0) printf("\n");
			printf("%d ", s[i]);
		}
		printf("\n");
	}
	else
		printf("No Solution\n");
}

