#include "FixedSize10.h"

namespace fs10
{
	bool finn_losning(int n, Square** t);
	int GetShifted(int i);
	int GetUnShifted(int i);

	void finn_losning(int arr[])
	{
		int* xa = new int[WIDTH];
		int* ya = new int[WIDTH];
		int* ga = new int[WIDTH];

		for (int i = 0; i < WIDTH; i++)
			xa[i] = ya[i] = ga[i] = EMPTY;

		Square** b = new Square*[SIZE];
		for (int i = 0; i < SIZE; i++)
			b[i] = new Square(i, arr[i], xa, ya, ga);

		finn_losning(0, b);

		for (int i = 0; i < SIZE; i++)
			arr[i] = GetUnShifted(b[i]->v);

		for (int i = 0; i < SIZE; i++)
			delete b[i];
		delete[] b, xa, ya, ga;
	}

	bool finn_losning(int n, Square** t)
	{
		for (; n < SIZE && t[n]->v != 0; n++) ;

		if (n == SIZE)
			return true;

		int brukt = t[n]->GetPosibles();

		if (brukt == ALL)
			return false;

		for (int i = 1; i < FINAL; i <<= 1)
			if ((brukt & i) == EMPTY)
			{
				t[n]->SetValue(i);

				if (finn_losning(n + 1, t))
					return true;

				t[n]->RemoveValue();
			}

		return false;
	}

	Square::Square(int n, int v, int* xa, int* ya, int* ga)
	{
		this->lx = n % WIDTH; 
		this->ly = n / WIDTH;
		this->lg = (n / GROUPWIDTH) % GROUPWIDTH + (n / PADDING) * GROUPWIDTH;
		
		this->xa = xa;
		this->ya = ya;
		this->ga = ga;
		
		if (v != EMPTY)
			SetValue(GetShifted(v));
		else
			this->v = EMPTY;
	};

	int Square::GetPosibles()
	{
		return this->xa[this->lx] | this->ya[this->ly] | this->ga[this->lg];
	}
	
	void Square::SetValue(int value)
	{
		this->v = value;
		this->xa[this->lx] |= this->v;
		this->ya[this->ly] |= this->v;
		this->ga[this->lg] |= this->v;
	}
	
	void Square::RemoveValue()
	{
		this->xa[this->lx] ^= this->v;
		this->ya[this->ly] ^= this->v;
		this->ga[this->lg] ^= this->v;
		this->v = EMPTY;
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
