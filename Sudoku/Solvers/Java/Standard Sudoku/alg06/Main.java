package com.company;

public class Main {
    public static void main(String[] args) {
        int[] s = new int[] {
                0,0,5, 1,0,9, 4,7,3,
                0,0,9, 5,0,0, 8,0,6,
                1,4,0, 8,0,0, 2,0,0,

                4,0,0, 0,0,0, 0,6,0,
                0,0,6, 7,0,2, 5,0,0,
                0,8,0, 0,0,0, 0,0,1,

                0,0,4, 0,0,1, 0,2,8,
                5,0,2, 0,0,8, 6,0,0,
                3,9,8, 2,0,7, 1,0,0
        };

        findSolution(0, s);
        display(s);
    }

    public static boolean findSolution(int n, int[] s) {
        for(; n < 81 && s[n] != 0; n++) ;

        if (n == 81)
            return true;

        int used = findUsedForLocal(n, s);

        for (int i = 1; i <= 9; i++)
            if((used & (1 << i)) == 0) {
                s[n] = i;
                if(findSolution(n + 1, s))
                    return true;
            }

        s[n] = 0;

        return false;
    }

    public static int findUsedForLocal(int n, int[] s) {
        int local_x = n % 9;
        int local_y = n / 9;

        int start_y = local_y * 9;

        int anchor = n
            - local_x % 3
            - (local_y % 3) * 9;

        int used = 0;
        for (int i = 0; i < 9; i++) {
            used |= 1 << s[start_y + i];
            used |= 1 << s[i * 9 + local_x];
            used |= 1 << s[
                anchor
                + (i / 3) * 9
                + (i % 3)
            ];
        }

        return used;
    }

    public static void display(int[] s) {
        for(int i = 0; i < 81; i++) {
            if(i % 9 == 0)
                System.out.println();
            System.out.print(s[i]);
        }
        System.out.println();
    }
}
