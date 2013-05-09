#include "JigSaw7.h"
#include <algorithm>

namespace js7
{
	int FindUsedForLocal(int n, Square** t, Box* b);
	bool find_solution(int n, Square** t, Box* b);

	Square* CreateSquare(int n, int s[], int arrG[]);

	bool SortX (Square* a, Square* b) { return ( a->x < b->x); }
	bool SortY (Square* a, Square* b) { return ( a->y < b->y); }
	bool SortG (Square* a, Square* b) { return ( a->g < b->g); }

	void find_solution(int arr[], int arrG[])
	{
		const int SIZE = 81;
		Square** b = new Square*[SIZE]; //Creates new array of squares.
		for (int i = 0; i < SIZE; i++)	//Creates each Square
			b[i] = CreateSquare(i, arr, arrG);

		Square** x = new Square*[SIZE]; //Arrays for sorted Squares by x.
		Square** y = new Square*[SIZE]; //Arrays for sorted Squares by y.
		Square** g = new Square*[SIZE]; //Arrays for sorted Squares by g.

		for (int i = 0; i < SIZE; i++)
			x[i] = y[i] = g[i] = b[i]; //Copy the references from b array to x, y and g.
		
		std::sort(x, x + 81, SortX); //Makes x array sorted by x.
		std::sort(y, y + 81, SortY); //Makes x array sorted by y.
		std::sort(g, g + 81, SortG); //Makes x array sorted by g.

		Box* box = new Box(x, y, g); //Box is the array of Squares sorted and "boxed" together.

		find_solution(0, b, box);

		for (int i = 0; i < SIZE; i++)	//Set solution from bit stream to 10base values.
			arr[i] = b[i]->v;

		for (int i = 0; i < SIZE; i++)
			delete b[i]; //Cleanup each Square.
		delete[] b, x, y, g; //Cleanup arrays.
		delete box;
	}

	Square* CreateSquare(int n, int s[], int arrG[])
	{
		int lx = n % 9; //Pre calulate x index.
		int ly = n / 9;	//Pre calulate y index.
		int lg = arrG[n]; //Set/copy the group index from the grouping array.

		return new Square(s[n], lx, ly, lg); //Returns new square.
	}

	int FindUsedForLocal(int n, Square** t, Box* b)
	{
		int px = t[n]->px; //Gets the start index of x.
		int py = t[n]->py; //Gets the start index of y.
		int pg = t[n]->pg; //Gets the start index of g.

		int brukt = 0;
		for (int i = 0; i < 9; i++)
		{
			brukt |= 1 << b->x[px + i]->v; //Get the already used by other squares on the x axis.
			brukt |= 1 << b->y[py + i]->v; //Get the already used by other squares on the y axis.
			brukt |= 1 << b->g[pg + i]->v; //Get the already used by other squares on the g axis.
		}

		return brukt; //Returns all the values it cannot be. All not set is a posiblity.
	}

	bool find_solution(int n, Square** t, Box* b)
	{
		for (; n < 81 && t[n]->v != 0; n++) ; //Skips those who already has values.

		if (n == 81) //solution found, if compiled with false, it will do all posible combinations.
			return true;

		int brukt = FindUsedForLocal(n, t, b); //Find all posible values to try.

		for (int i = 1; i <= 9; i++)
			if ((brukt & (1 << i)) == 0) //Checks if value(i) is a posible,
			{                            // 0 means that it is not a conflict on x, y or g, and might be a posibility.
				t[n]->v = i; //Sets i as a posbile.
				if (find_solution(n + 1, t, b)) //Tries next position.
					return true; //if solution is found, don't do more work.
			}

		t[n]->v = 0; //None of the posibles worked. Resets the value.

		return false; //solution not found, yet.
	}

	Square::Square(int v, int x, int y, int g)
	{
		this->v = v; //Sets the value.
		this->x = x; //Sets the x index.
		this->y = y; //Sets the y index.
		this->g = g; //Sets the g index.
		this->px = x * 9; //Sets the start index of x.
		this->py = y * 9; //Sets the start index of y.
		this->pg = g * 9; //Sets the start index of g.
	};
	Square::~Square() { };

	Box::Box(Square** x, Square** y, Square** g)
	{
		this->x = x;
		this->y = y;
		this->g = g;
	};

	Box::~Box() { };
}
