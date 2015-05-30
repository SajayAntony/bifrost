$baseAddress = "http://localhost:8080"

function checkBridge {
	try { 
		$response = Invoke-WebRequest $baseAddress
		Write-Host Status code from the bridge :  $response.StatusCode
		exit 0
	} catch {
		Write-Debug $_.Exception
	}
	Write-Host Could not find bridge at $baseAddress
	return $false;
}

$result= checkBridge;

if(!$result)
{
	Write-Host Launching bridge.exe.
	$bridgePath = Join-Path $PSScriptRoot bridge.exe
	Start-Process $bridgePath
	$result = checkBridge;
	Write-Host Check bridge result : $result
}