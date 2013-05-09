namespace js10
{
	const int EMPTY	=   0;			//00 0000 0000
	const int ONE	=   1;			//00 0000 0001
	const int TWO	=   2;			//00 0000 0010
	const int THREE	=   4;			//00 0000 0100
	const int FOUR	=   8;			//00 0000 1000
	const int FIVE	=  16;			//00 0001 0000
	const int SIX	=  32;			//00 0010 0000
	const int SEVEN	=  64;			//00 0100 0000
	const int EIGHT	= 128;			//00 1000 0000
	const int NINE	= 256;			//01 0000 0000
	const int FINAL	= 512;			//10 0000 0000
	const int ALL	= FINAL - 1;	//01 1111 1111

	const int GROUPWIDTH = 3;
	const int WIDTH = 9;
	const int PADDING = WIDTH * GROUPWIDTH;
	const int SIZE = 81;

	void find_solution(int sarr[], int garr[]);

	class Square
	{
	private:
		int lx, ly, lg;	//Index in shared static arrays.
		int *xa, *ya, *ga;	//Shared non-static values.
	public:
		int v;

		Square();
		void Populate(int n, int v, int* xa, int* ya, int* ga, int lg);

		int GetPosibles();
		void SetValue(int value);
		void RemoveValue();

		~Square();
	};
}