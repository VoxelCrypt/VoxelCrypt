[CmdletBinding()]
param(
    [switch]$RemoveVolumes
)

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$composeFile = Join-Path $repoRoot 'docker-compose.yml'

if (-not (Test-Path -LiteralPath $composeFile -PathType Leaf)) {
    throw "docker-compose.yml was not found at '$composeFile'."
}

Set-Location $repoRoot

$args = @('compose', '-f', $composeFile, 'down')
if ($RemoveVolumes) {
    $args += '-v'
}

& docker @args
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}
