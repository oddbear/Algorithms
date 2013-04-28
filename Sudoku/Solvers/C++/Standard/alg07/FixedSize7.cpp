#include "FixedSize7.h"
#include <algorithm>

namespace fs7
{
	int FindUsedForLocal(int n, Square** t, Box* b);
	bool finn_losning(int n, Square** t, Box* b);

	Square* CreateSquare(int n, int s[]);

	bool SortX (Square* a, Square* b) { return ( a->x < b->x); }
	bool SortY (Square* a, Square* b) { return ( a->y < b->y); }
	bool SortG (Square* a, Square* b) { return ( a->g < b->g); }

	void finn_losning(int arr[])
	{
		const int SIZE = 81;
		Square** b = new Square*[SIZE];
		for (int i = 0; i < SIZE; i++)
			b[i] = CreateSquare(i, arr);

		Square** x = new Square*[SIZE];
		Square** y = new Square*[SIZE];
		Square** g = new Square*[SIZE];

		for (int i = 0; i < SIZE; i++)
			x[i] = y[i] = g[i] = b[i];
		
		std::sort(x, x + 81, SortX);
		std::sort(y, y + 81, SortY);
		std::sort(g, g + 81, SortG);

		Box* box = new Box(x, y, g);

		finn_losning(0, b, box);

		for (int i = 0; i < SIZE; i++)
			arr[i] = b[i]->v;

		for (int i = 0; i < SIZE; i++)
			delete b[i];
		delete[] b, x, y, g;
		delete box;
	}

	Square* CreateSquare(int n, int s[])
	{
		int lx = n % 9;
		int ly = n / 9;
		int lg =  (n / 3) % 3
				+ (n / 27) * 3;

		return new Square(s[n], lx, ly, lg);
	}

	int FindUsedForLocal(int n, Square** t, Box* b)
	{
		int px = t[n]->px;
		int py = t[n]->py;
		int pg = t[n]->pg;

		int brukt = 0;
		for (int i = 0; i < 9; i++)
		{
			brukt |= 1 << b->x[px + i]->v;
			brukt |= 1 << b->y[py + i]->v;
			brukt |= 1 << b->g[pg + i]->v;
		}

		return brukt;
	}

	bool finn_losning(int n, Square** t, Box* b)
	{
		for (; n < 81 && t[n]->v != 0; n++) ;

		if (n == 81)
			return true;

		int brukt = FindUsedForLocal(n, t, b);

		for (int i = 1; i <= 9; i++)
			if ((brukt & (1 << i)) == 0)
			{
				t[n]->v = i;
				if (finn_losning(n + 1, t, b))
					return true;
			}

		t[n]->v = 0;

		return false;
	}

	Square::Square(int v, int x, int y, int g)
	{
		this->v = v;
		this->x = x;
		this->y = y;
		this->g = g;
		this->px = x * 9;
		this->py = y * 9;
		this->pg = g * 9;
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
