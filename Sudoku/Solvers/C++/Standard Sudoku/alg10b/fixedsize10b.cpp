#include "fixedsize10b.h"

namespace fs10b
{
	bool find_solution(int n, Square* t);
	int GetShifted(int i);
	int GetUnShifted(int i);
	
	int *Square::xa = new int[WIDTH];	//Shared static x-axis
	int *Square::ya = new int[WIDTH];	//Shared static y-axis
	int *Square::ga = new int[WIDTH];	//Shared static group.

	void find_solution(int arr[])
	{
		for (int i = 0; i < WIDTH; i++) //Reset all values in shared static array.
			Square::xa[i] = Square::ya[i] = Square::ga[i] = EMPTY;
		
		Square* b = new Square[SIZE];
		for (int i = 0; i < SIZE; i++) //Convert from value array to Square array.
			b[i].Populate(i, arr[i]); //Populate the array.

		find_solution(0, b);

		for (int i = 0; i < SIZE; i++)	//Set solution from bit stream to 10base values.
			arr[i] = GetUnShifted(b[i].v);

		delete[] b; //Cleanup array.
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

	Square::Square() { };	//Constructor not used here, se Populate.

	void Square::Populate(int n, int value) //Used like a constructor, any other way when not a reference?
	{
		lx = n % WIDTH; //Pre calulate x index.
		ly = n / WIDTH;	//Pre calulate y index.
		lg = (n / GROUPWIDTH) % GROUPWIDTH + (n / PADDING) * GROUPWIDTH; //Pre calulate group index.
				
		if (value != EMPTY) //If Square has a value. Set the shifted value.
			SetValue(GetShifted(value));
		else
			v = EMPTY;
	};

	int Square::GetPosibles()
	{
		return xa[lx] | ya[ly] | ga[lg]; //Combine all used on x, y, and g. Thos who are not set, is posibles.
	}
	
	void Square::SetValue(int value)
	{
		v = value;		//Sets the value
		xa[lx] |= v;	//Set value as used for every one on x axis.
		ya[ly] |= v;	//Set value as used for every one on y axis.
		ga[lg] |= v;	//Set value as used for every one in group.
	}
	
	void Square::RemoveValue()
	{
		xa[lx] ^= v;	//Remove value as used for every one in x axis.
		ya[ly] ^= v;	//Remove value as used for every one in y axis.
		ga[lg] ^= v;	//Remove value as used for every one in group.
		v = EMPTY;		//Remove value, sets it to empty.
	}

	Square::~Square() { };	//Destructure not used.

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
