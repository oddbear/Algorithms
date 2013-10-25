var s = [
	0,0,5, 1,0,9, 4,7,3,
	0,0,9, 5,0,0, 8,0,6,
	1,4,0, 8,0,0, 2,0,0,
	
	4,0,0, 0,0,0, 0,6,0,
	0,0,6, 7,0,2, 5,0,0,
	0,8,0, 0,0,0, 0,0,1,
	
	0,0,4, 0,0,1, 0,2,8,
	5,0,2, 0,0,8, 6,0,0,
	3,9,8, 2,0,7, 1,0,0
];

function findSolution(n, s) {
	for (; n < 81 && s[n] != 0; n++) ; //Skips those who already has values.

	if (n == 81) //solution found, if compiled with false, it will do all posible combinations.
		return true;

	var used = findUsedForLocal(n, s); //Find all posible values to try.

	for (var i = 1; i <= 9; i++) {     //Checks if value(i) is a posible,
		if ((used & (1 << i)) == 0) { // 0 means that it is not a conflict on x, y or g, and might be a posibility.
			s[n] = i; //Sets i as a posbile.
			if (findSolution(n + 1, s)) {
				return true;
			}
		}
	}

	s[n] = 0; //None of the posibles worked. Resets the value.

	return false; //solution not found, yet.
};

function findUsedForLocal(n, s) {
	var local_x = n % 9; //Finds the first position in the y axis.
	var local_y = Math.floor(n / 9);

	var start_y = local_y * 9; //Finds the first position in the x axis.

	var anchor = n - (local_x % 3) - ((local_y % 3) * 9); //Finds the first position in the group.

	var used = 0;
	for (var i = 0; i < 9; i++) { //Test all x, y and group values, does not mather if it also checks the value in the n position.
		used |= 1 << s[start_y + i]; //Gather info from the y axis.
		used |= 1 << s[i * 9 + local_x]; //Gather info from the x axis.
		used |= 1 << s[anchor + Math.floor(i / 3) * 9 + (i % 3)]; //Gather info from the group.
	}
	return used; //Returns all the values it cannot be. All not set is a posiblity.
};

function makeTable(s) {
	var table = document.createElement('table');
	
	for(var y = 0; y < 9; y++) {
		var tr = document.createElement('tr');
		table.appendChild(tr);
		
		for(var x = 0; x < 9; x++) {
			var td = document.createElement('td');
			td.innerHTML = s[y * 9 + x];
			tr.appendChild(td);
		}
	}
	
	return table;
};

function solve() {
	findSolution(0, s);
	
	var el = document.getElementById('solution');
	el.innerHTML = '';
	el.appendChild(makeTable(s));
	
	return false;
};

window.onload = function () {
	document.getElementById('solution').appendChild(makeTable(s));
};
