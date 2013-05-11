#include <iostream>
#include <iomanip>
#include <ctime>
#include "jigsaw6.h"

int main()
{
	const int x = 3;
	const int y = 3;
	const int c = x * y;
	const int f = c * c;

	int arr[f] =   {0,0,0, 0,0,0, 0,8,0,
			0,1,0, 0,0,0, 0,0,0,
			0,0,0, 0,1,0, 0,0,0,

			0,0,9, 5,0,0, 0,0,0,
			0,0,2, 0,6,0, 5,0,4,
			4,8,0, 0,0,0, 2,0,9,

			0,0,0, 0,0,3, 0,0,8,
			0,0,6, 0,2,0, 0,0,0,
			0,3,8, 0,5,0, 0,0,0};

	int arrG[f] =  {0,0,1, 1,1,1, 1,1,2,
			0,0,0, 1,1,2, 2,2,2,
			0,0,0, 0,1,2, 2,5,2,

			3,3,4, 4,4,4, 2,5,5,
			3,3,3, 3,4,5, 5,5,5,
			3,3,6, 4,4,4, 4,5,5,

			6,3,6, 6,7,8, 8,8,8,
			6,6,6, 6,7,7, 8,8,8,
			6,7,7, 7,7,7, 7,8,8};

	std::cout << "start" << std::endl;
	unsigned int start = clock();
	js6::find_solution(arr, arrG, x, y);
	unsigned int stop = clock() - start;
	std::cout << "end" << std::endl;
	std::cout << "Ran for: " << stop << std::endl;
	for (int i = 0; i < f; i++)
	{
		if (i % x == 0 )
			std::cout << " ";
		if (i % c == 0)
			std::cout << std::endl;
		if (i % (c * y) == 0)
			std::cout << std::endl;
		std::cout << arr[i] << " ";
	}
	std::cout << std::endl;
	return 0;
}
