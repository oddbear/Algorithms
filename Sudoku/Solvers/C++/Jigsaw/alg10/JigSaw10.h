namespace js10
{
	const int EMPTY	=   0;
	const int ONE	=   1;
	const int TWO	=   2;
	const int THREE	=   4;
	const int FOUR	=   8;
	const int FIVE	=  16;
	const int SIX	=  32;
	const int SEVEN	=  64;
	const int EIGHT	= 128;
	const int NINE	= 256;
	const int FINAL	= 512;
	const int ALL	= FINAL - 1;

	const int GROUPWIDTH = 3;
	const int WIDTH = 9;
	const int PADDING = WIDTH * GROUPWIDTH;
	const int SIZE = 81;

	void finn_losning(int sarr[], int garr[]);

	class Square
	{
	private:
		int lx, ly, lg;
		int *xa, *ya, *ga;
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