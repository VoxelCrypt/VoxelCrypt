[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = 'High')]
param(
    [string]$RootPath = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
)

$targetNames = @('bin', 'obj')

if (-not (Test-Path -LiteralPath $RootPath -PathType Container)) {
    throw "Root path '$RootPath' does not exist or is not a directory."
}

$directories = Get-ChildItem -LiteralPath $RootPath -Directory -Recurse -Force |
    Where-Object { $targetNames -contains $_.Name } |
    Sort-Object { $_.FullName.Length } -Descending

if (-not $directories) {
    Write-Host "No build artifact folders (bin/obj) found under '$RootPath'."
    return
}

Write-Host "Found $($directories.Count) build artifact folder(s) under '$RootPath'."

foreach ($directory in $directories) {
    if ($PSCmdlet.ShouldProcess($directory.FullName, 'Remove build artifact directory')) {
        Remove-Item -LiteralPath $directory.FullName -Recurse -Force -ErrorAction Stop
        Write-Host "Deleted: $($directory.FullName)"
    }
}
