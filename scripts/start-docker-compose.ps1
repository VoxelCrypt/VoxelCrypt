[CmdletBinding()]
param(
    [switch]$Detached
)

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$composeFile = Join-Path $repoRoot 'docker-compose.yml'

if (-not (Test-Path -LiteralPath $composeFile -PathType Leaf)) {
    throw "docker-compose.yml was not found at '$composeFile'."
}

Set-Location $repoRoot

$args = @('compose', '-f', $composeFile, 'up', '--build')
if ($Detached) {
    $args += '-d'
}

& docker @args
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}
