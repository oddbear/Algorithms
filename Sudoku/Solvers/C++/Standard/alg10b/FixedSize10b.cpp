#include "FixedSize10b.h"

namespace fs10b
{
	bool finn_losning(int n, Square* t);
	int GetShifted(int i);
	int GetUnShifted(int i);
	
	int *Square::xa = new int[WIDTH];
	int *Square::ya = new int[WIDTH];
	int *Square::ga = new int[WIDTH];

	void finn_losning(int arr[])
	{
		for (int i = 0; i < WIDTH; i++)
			Square::xa[i] = Square::ya[i] = Square::ga[i] = EMPTY;
		
		Square* b = new Square[SIZE];
		for (int i = 0; i < SIZE; i++)
			b[i].Populate(i, arr[i]);

		finn_losning(0, b);

		for (int i = 0; i < SIZE; i++)
			arr[i] = GetUnShifted(b[i].v);

		delete[] b;
	}

	bool finn_losning(int n, Square* t)
	{
		for (; n < SIZE && t[n].v != 0; n++) ;

		if (n == SIZE)
			return true;

		int brukt = t[n].GetPosibles();

		if (brukt == ALL)
			return false;

		for (int i = 1; i < FINAL; i <<= 1)
			if ((brukt & i) == EMPTY)
			{
				t[n].SetValue(i);

				if (finn_losning(n + 1, t))
					return true;

				t[n].RemoveValue();
			}

		return false;
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
