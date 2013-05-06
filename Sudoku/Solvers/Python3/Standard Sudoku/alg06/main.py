#[0,0,5, 1,0,9, 4,7,3,
# 0,0,9, 5,0,0, 8,0,6,
# 1,4,0, 8,0,0, 2,0,0,
#
# 4,0,0, 0,0,0, 0,6,0,
# 0,0,6, 7,0,2, 5,0,0,
# 0,8,0, 0,0,0, 0,0,1,
#
# 0,0,4, 0,0,1, 0,2,8,
# 5,0,2, 0,0,8, 6,0,0,
# 3,9,8, 2,0,7, 1,0,0]
s = [0,0,5,1,0,9,4,7,3,0,0,9,5,0,0,8,0,6,1,4,0,8,0,0,2,0,0,4,0,0,0,0,0,0,6,0,0,0,6,7,0,2,5,0,0,0,8,0,0,0,0,0,0,1,0,0,4,0,0,1,0,2,8,5,0,2,0,0,8,6,0,0,3,9,8,2,0,7,1,0,0]

def findUsedForLocal(n):
	local_x = n % 9 #Finds the first position in the y axis.
	local_y = n // 9

	start_y = local_y * 9 #Finds the first position in the x axis.

	anchor = n - local_x % 3 - (local_y % 3) * 9 #Finds the first position in the group.
	brukt = 0
	for i in range(0, 9): #Test all x, y and group values, does not mather if it also checks the value in the n position.
		brukt |= 1 << s[start_y + i] #Gather info from the y axis.
		brukt |= 1 << s[i * 9 + local_x] #Gather info from the x axis.
		brukt |= 1 << s[anchor + (i // 3) * 9 + (i % 3)] #Gather info from the group.
	return brukt #Returns all the values it cannot be. All not set is a posiblity.

def findSolution(n):
	while n < 81 and s[n] != 0: #Skips those who already has values.
		n += 1
	
	if n == 81: #solution found, if compiled with false, it will do all posible combinations.
		return True

	brukt = findUsedForLocal(n) #Find all posible values to try.

	for i in range(1, 10):            #Checks if value(i) is a posible,
		if brukt & (1 << i) == 0: # 0 means that it is not a conflict on x, y or g, and might be a posibility.
			s[n] = i #Sets i as a posbile.
			if findSolution(n + 1):
				return True

	s[n] = 0 #None of the posibles worked. Resets the value.

	return False #solution not found, yet.

findSolution(0)

print(s)
