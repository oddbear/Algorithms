Testet 10 times over 1 board(3x3) in Visual Studio 2012 on Windows 8 64bit:

The board will then look like:
[[
0,0,0, 0,0,0, 0,8,0,
0,1,0, 0,0,0, 0,0,0,
0,0,0, 0,1,0, 0,0,0,

0,0,9, 5,0,0, 0,0,0,
0,0,2, 0,6,0, 5,0,4,
4,8,0, 0,0,0, 2,0,9,

0,0,0, 0,0,3, 0,0,8,
0,0,6, 0,2,0, 0,0,0,
0,3,8, 0,5,0, 0,0,0
],[
0,0,1, 1,1,1, 1,1,2,
0,0,0, 1,1,2, 2,2,2,
0,0,0, 0,1,2, 2,5,2,

3,3,4, 4,4,4, 2,5,5,
3,3,3, 3,4,5, 5,5,5,
3,3,6, 4,4,4, 4,5,5,

6,3,6, 6,7,8, 8,8,8,
6,6,6, 6,7,7, 8,8,8,
6,7,7, 7,7,7, 7,8,8
]]

The first array is for "painting borders" of the group, and the second is the values on the board.

Boards(runtime in seconds):
js6: 2.76235
js7: 2.57333
js10: 1.37418



