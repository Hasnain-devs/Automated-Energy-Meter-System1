param(
    [string]$ApiUrl = "http://127.0.0.1/api/iot/readings.ashx",
    [string]$MeterId = "MTR1001",
    [string]$DeviceKey = "dev-MTR1001",
    [string]$SharedSecret = "",
    [decimal]$UnitsDelta = 0.140,
    [long]$SequenceNo = 0
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($SharedSecret)) {
    throw "SharedSecret is required. Pass -SharedSecret <secret>."
}

if ($SequenceNo -le 0) {
    $SequenceNo = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()
}

$readingAtUtc = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")

# Keep property order deterministic so the signature matches the exact body sent.
$bodyObject = [ordered]@{
    meterId      = $MeterId
    unitsDelta   = [double]$UnitsDelta
    readingAtUtc = $readingAtUtc
    sequenceNo   = $SequenceNo
}

$json = $bodyObject | ConvertTo-Json -Compress

$hmac = New-Object System.Security.Cryptography.HMACSHA256
$hmac.Key = [Text.Encoding]::UTF8.GetBytes($SharedSecret)
$hashBytes = $hmac.ComputeHash([Text.Encoding]::UTF8.GetBytes($json))
$signature = -join ($hashBytes | ForEach-Object { $_.ToString("x2") })

$headers = @{
    "X-Device-Key" = $DeviceKey
    "X-Signature"  = $signature
}

Write-Host "POST $ApiUrl"
Write-Host "Body: $json"
Write-Host "Signature: $signature"

try {
    $response = Invoke-RestMethod -Method Post -Uri $ApiUrl -Headers $headers -ContentType "application/json" -Body $json
    Write-Host "Request accepted."
    $response | ConvertTo-Json -Depth 10
}
catch {
    if ($_.Exception.Response) {
        $stream = $_.Exception.Response.GetResponseStream()
        $reader = New-Object IO.StreamReader($stream)
        $serverBody = $reader.ReadToEnd()
        Write-Host "Server rejected request:"
        Write-Host $serverBody
    }
    throw
}
