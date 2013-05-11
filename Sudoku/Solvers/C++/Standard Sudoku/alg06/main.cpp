#include <iostream>
#include <iomanip>
#include <ctime>
#include "fixedsize6.h"

int main()
{
	const int x = 3;
	const int y = 3;
	const int c = x * y;
	const int f = c * c;

	int arr[f] =   {9,1,0, 4,0,0, 0,3,0,
			0,2,5, 6,0,3, 0,0,0,
			0,0,8, 0,0,0, 0,0,0,

			0,0,0, 0,2,0, 0,0,9,
			0,4,2, 0,0,0, 8,5,0,
			5,0,0, 0,7,0, 0,0,0,

			0,0,0, 0,0,0, 9,0,0,
			0,0,0, 3,0,1, 4,8,0,
			0,8,0, 0,0,5, 0,6,1};

	std::cout << "start" << std::endl;
	unsigned int start = clock();
	fs6::find_solution(arr);
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
