#include "FixedSize10b.h"

namespace fs10b
{
	bool find_solution(int n, Square* t);
	int GetShifted(int i);
	int GetUnShifted(int i);
	
	int *Square::xa = new int[WIDTH];
	int *Square::ya = new int[WIDTH];
	int *Square::ga = new int[WIDTH];

	void find_solution(int arr[])
	{
		for (int i = 0; i < WIDTH; i++)
			Square::xa[i] = Square::ya[i] = Square::ga[i] = EMPTY;
		
		Square* b = new Square[SIZE];
		for (int i = 0; i < SIZE; i++)
			b[i].Populate(i, arr[i]);

		find_solution(0, b);

		for (int i = 0; i < SIZE; i++)
			arr[i] = GetUnShifted(b[i].v);

		delete[] b;
	}

	bool find_solution(int n, Square* t)
	{
		for (; n < SIZE && t[n].v != 0; n++) ; //Skips those who already has values.

		if (n == SIZE) //solution found, if compiled with false, it will do all posible combinations.
			return true;

		int brukt = t[n].GetPosibles(); //Find all posible values to try.

		if (brukt == ALL) //If none values is posible, this path cannot be a part of the solution.
			return false;

		for (int i = 1; i < FINAL; i <<= 1)
			if ((brukt & i) == EMPTY) //Checks if value(i) is a posible,
			{                         // EMPTY means that it is not a conflict on x, y or g, and might be a posibility.
				t[n].SetValue(i); //Sets i as a posbile.

				if (find_solution(n + 1, t)) //Tries next position.
					return true;

				t[n].RemoveValue(); //Removes i as a posbile.
			}

		return false; //solution not found, yet.
	}

	Square::Square() { };

	void Square::Populate(int n, int value)
	{
		lx = n % WIDTH; 
		ly = n / WIDTH;
		lg = (n / GROUPWIDTH) % GROUPWIDTH + (n / PADDING) * GROUPWIDTH;
				
		if (value != EMPTY)
			SetValue(GetShifted(value));
		else
			v = EMPTY;
	};

	int Square::GetPosibles()
	{
		return xa[lx] | ya[ly] | ga[lg];
	}
	
	void Square::SetValue(int value)
	{
		v = value;
		xa[lx] |= v;
		ya[ly] |= v;
		ga[lg] |= v;
	}
	
	void Square::RemoveValue()
	{
		xa[lx] ^= v;
		ya[ly] ^= v;
		ga[lg] ^= v;
		v = EMPTY;
	}

	Square::~Square() { };

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
