<#
Tested only in PSv4.
#>

function findSolution {
    param([int]$n, [int[]]$s) #$s is sendt by reference, if $s is copied, ex. $s += 0; this code will work, however Display will write out the original copy of $s.
                    # sett global variable $Global:variablename = $s, or rund display inside the if($n -eq 81) statement(also do this if the return here is false).

	for(; $n -lt 81 -and $s[$n] -ne 0; $n++) { } #Skips those who already has values.
	
	if ($n -eq 81) { return $true; } #solution found, if compiled with false, it will do all posible combinations.

	$used = findUsedForLocal $n $s; #Find all posible values to try.

	for ($i = 1; $i -le 9; $i++) { #Checks if value(i) is a posible,
		if(($used -band (1 -shl $i)) -eq 0) { #0 means that it is not a conflict on x, y or g, and might be a posibility.
			$s[$n] = $i; #Sets i as a posbile.
			if(findSolution ($n + 1) $s) {
				return $true;
            }
		}
    }
	
	$s[$n] = 0; #None of the posibles worked. Resets the value.
	
	return $false; #solution not found, yet.
}

function findUsedForLocal {
    param ([int]$n, [int[]]$s)

	$local_x = $n % 9; #Finds the first position in the y axis.
	$local_y = [math]::truncate($n / 9);
	
	$start_y = $local_y * 9; #Finds the first position in the x axis.
	
	$anchor = $n - ($local_x % 3) - (($local_y % 3) * 9); #Finds the first position in the group.

	$used = 0;
	for ($i = 0; $i -lt 9; $i++) { #Test all x, y and group values,  does not mather if it also checks the value in the n position.
		$used = $used -bor 1 -shl $s[$start_y + $i]; #Gather info from the y axis.
		$used = $used -bor 1 -shl $s[$i * 9 + $local_x]; #Gather info from the x axis.
		$used = $used -bor 1 -shl $s[$anchor + [math]::truncate($i / 3) * 9 + ($i % 3)]; #Gather info from the group.
	}
	return $used; #Returns all the values it cannot be. All not set is a posiblity.
}

function Display {
    param([int[]]$s)

	for($i = 0; $i -lt 81; $i++) {
		if (!($i % 9)) {
            Write-Host ;
        }
        Write-Host $s[$i] -NoNewLine;
	}
	Write-Host ;
}

#-------------------------------------------------------

# 0,0,5, 1,0,9, 4,7,3,
# 0,0,9, 5,0,0, 8,0,6,
# 1,4,0, 8,0,0, 2,0,0,
#
# 4,0,0, 0,0,0, 0,6,0,
# 0,0,6, 7,0,2, 5,0,0,
# 0,8,0, 0,0,0, 0,0,1,
#
# 0,0,4, 0,0,1, 0,2,8,
# 5,0,2, 0,0,8, 6,0,0,
# 3,9,8, 2,0,7, 1,0,0
$s = [int[]]@(0,0,5,1,0,9,4,7,3,0,0,9,5,0,0,8,0,6,1,4,0,8,0,0,2,0,0,4,0,0,0,0,0,0,6,0,0,0,6,7,0,2,5,0,0,0,8,0,0,0,0,0,0,1,0,0,4,0,0,1,0,2,8,5,0,2,0,0,8,6,0,0,3,9,8,2,0,7,1,0,0)

if(findSolution 0 $s ) {
    Display $s
}