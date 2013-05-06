namespace fs10b
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

	void find_solution(int arr[]);

	class Square
	{
	private:
		int lx, ly, lg;
	public:
		int v;
		static int *xa, *ya, *ga;

		Square();
		void Populate(int n, int v);

		int GetPosibles();
		void SetValue(int value);
		void RemoveValue();

		~Square();
	};
}