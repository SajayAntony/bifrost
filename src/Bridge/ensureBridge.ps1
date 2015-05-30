$baseAddress = "http://localhost:8080"

Write-Host

function checkBridge {
	try { 
		$response = Invoke-WebRequest $baseAddress
		Write-Host Status code from the bridge :  $response.StatusCode
		return $true
	} catch {
		Write-Debug $_.Exception
	}
	Write-Warning "Could not find bridge at $baseAddress"
	return $false;
}

$result= checkBridge;

if(!$result)
{
	Write-Host Launching bridge.exe.
	$bridgePath = Join-Path $PSScriptRoot bridge.exe
	Start-Process $bridgePath
	$result = checkBridge;
}

if($result){
	Write-Host Invoking test command Bridge.Commands.WhoAmI on the bridge.
	Invoke-RestMethod http://localhost:8080/resource/WhoAmI -Method PUT -Body "{name:'Bridge.Commands.WhoAmI'}" -ContentType application/json
	exit 0
}

exit -1;