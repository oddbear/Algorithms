#include <iostream>
#include <iomanip>
#include <ctime>
#include "Dynamic6.h"

int main()
{
	const int x = 4;
	const int y = 3;
	const int c = x * y;
	const int f = c * c;
	const int w = 2;

	int arr[f] =   { 0, 0, 0, 0,  0, 1, 0, 0,  0, 0, 3,12,
			 3, 0, 1, 0,  0, 0,12,10,  6, 0, 0, 0,
			 0, 0, 2,12,  4, 3, 0, 0,  0, 0, 7, 0,

			 0,10, 0, 8, 12, 0, 0, 0,  7, 0, 6, 0,
			 0, 7, 5, 0,  0, 2, 0, 3,  0, 0, 4, 0,
			 0, 0, 0, 9,  0, 0, 0, 7, 11, 3, 0,10,

			 5, 0, 9,11,  7, 0, 0, 0,  2, 0, 0, 0,
			 0, 6, 0, 0,  2, 0, 1, 0,  0, 5,12, 0,
			 0,12, 0, 2,  0, 0, 0, 4,  8, 0,11, 0,

			 0, 2, 0, 0,  0, 0, 7, 8,  3,11, 0, 0,
			 0, 0, 0, 3,  9,12, 0, 0,  0, 4, 0, 2,
			10, 4, 0, 0,  0, 0, 3, 0,  0, 0, 0, 0};

	std::cout << "start" << std::endl;
	unsigned int start = clock();
	d6::find_solution(arr, x, y);
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
		std::cout << std::setw(w) << arr[i] << " ";
	}
	std::cout << std::endl;
	return 0;
}
