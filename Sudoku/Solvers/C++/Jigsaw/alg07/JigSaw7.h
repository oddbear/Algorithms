namespace js7
{
	void finn_losning(int arr[], int arrG[]);

	struct Square
	{
		int v;
		int x, y, g;
		int px, py, pg;
		Square(int v, int x, int y, int g);
		~Square();
	};
	struct Box
	{
		Square** x,** y,** g;
		Box(Square** x, Square** y, Square** g);
		~Box();
	};
}