<#
.SYNOPSIS
    Create a new project from the Coding Standard Template.

.PARAMETER ProjectName
    Project name (A-Z, a-z, 0-9, _). Used as namespace, folder, solution name.

.PARAMETER OutputPath
    Target directory (default = parent of template folder).

.PARAMETER TaskId
    Task ID for docs (default = "TP001").

.EXAMPLE
    .\New-Project.ps1 -ProjectName "RLSR061" -TaskId "RLSR061-001"
    .\New-Project.ps1 -ProjectName "OmniChannel" -OutputPath "E:\Projects" -TaskId "OC-001"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$ProjectName,

    [Parameter(Mandatory = $false)]
    [string]$OutputPath = "",

    [Parameter(Mandatory = $false)]
    [string]$TaskId = "TP001"
)

$ErrorActionPreference = "Stop"
$TemplatePath = Join-Path $PSScriptRoot "template"
$Placeholder = "__PROJECT_NAME__"
$TaskPlaceholder = "__TASK_ID__"
$DatePlaceholder = "__DATE__"
$Today = Get-Date -Format "d MMMM yyyy"

if ([string]::IsNullOrEmpty($OutputPath)) {
    $OutputPath = Split-Path $PSScriptRoot -Parent
}

$TargetPath = Join-Path $OutputPath $ProjectName

# --- Validation ---
if (-not (Test-Path $TemplatePath)) {
    Write-Host "[ERROR] Template folder not found: $TemplatePath" -ForegroundColor Red
    exit 1
}

if (Test-Path $TargetPath) {
    Write-Host "[ERROR] Target folder already exists: $TargetPath" -ForegroundColor Red
    exit 1
}

if ($ProjectName -match '[^a-zA-Z0-9_]') {
    Write-Host '[ERROR] ProjectName must contain only A-Z, a-z, 0-9, _' -ForegroundColor Red
    exit 1
}

# --- Header ---
Write-Host ''
Write-Host '=============================================' -ForegroundColor Cyan
Write-Host '  New Project from Coding Standard Template  ' -ForegroundColor Cyan
Write-Host '=============================================' -ForegroundColor Cyan
Write-Host ''
Write-Host "  Project Name : $ProjectName" -ForegroundColor Green
Write-Host "  Task ID      : $TaskId" -ForegroundColor Green
Write-Host "  Output       : $TargetPath" -ForegroundColor Green
Write-Host ''

# --- Step 1: Copy template ---
Write-Host '[Step 1/4] Copying template...' -ForegroundColor Yellow
Copy-Item -Path $TemplatePath -Destination $TargetPath -Recurse -Force
Write-Host "  [OK] Copied to $TargetPath" -ForegroundColor Green

# --- Step 2: Rename files ---
Write-Host '[Step 2/4] Renaming files...' -ForegroundColor Yellow

$filesToRename = Get-ChildItem -Path $TargetPath -Recurse -File |
    Where-Object { $_.Name -match $Placeholder }

foreach ($file in $filesToRename) {
    $newName = $file.Name -replace $Placeholder, $ProjectName
    Rename-Item -Path $file.FullName -NewName $newName
    Write-Host "  $($file.Name) -> $newName" -ForegroundColor DarkGray
}

# Rename directories (deepest first)
$dirsToRename = Get-ChildItem -Path $TargetPath -Recurse -Directory |
    Where-Object { $_.Name -match $Placeholder } |
    Sort-Object { $_.FullName.Length } -Descending

foreach ($dir in $dirsToRename) {
    $newName = $dir.Name -replace $Placeholder, $ProjectName
    Rename-Item -Path $dir.FullName -NewName $newName
    Write-Host "  $($dir.Name) -> $newName" -ForegroundColor DarkGray
}

Write-Host '  [OK] Files renamed' -ForegroundColor Green

# --- Step 3: Replace content in files ---
Write-Host '[Step 3/4] Replacing placeholders...' -ForegroundColor Yellow

$textExtensions = @('.cs', '.csproj', '.slnx', '.sln', '.json', '.md', '.xml', '.config', '.props', '.targets')

$allFiles = Get-ChildItem -Path $TargetPath -Recurse -File |
    Where-Object { $textExtensions -contains $_.Extension }

$replaceCount = 0
foreach ($file in $allFiles) {
    $content = Get-Content -Path $file.FullName -Raw -ErrorAction SilentlyContinue
    if ($null -ne $content) {
        $newContent = $content `
            -replace [regex]::Escape($Placeholder), $ProjectName `
            -replace [regex]::Escape($TaskPlaceholder), $TaskId `
            -replace [regex]::Escape($DatePlaceholder), $Today

        if ($content -ne $newContent) {
            Set-Content -Path $file.FullName -Value $newContent -NoNewline
            $replaceCount++
        }
    }
}

Write-Host "  [OK] Replaced placeholders in $replaceCount files" -ForegroundColor Green

# --- Step 4: Summary ---
Write-Host '[Step 4/4] Verifying...' -ForegroundColor Yellow

$srcCount = @(Get-ChildItem -Path (Join-Path $TargetPath 'src') -Recurse -File -ErrorAction SilentlyContinue).Count
$testCount = @(Get-ChildItem -Path (Join-Path $TargetPath 'tests') -Recurse -File -ErrorAction SilentlyContinue).Count
$docCount = @(Get-ChildItem -Path (Join-Path $TargetPath 'docs') -Recurse -File -ErrorAction SilentlyContinue).Count

Write-Host ''
Write-Host '=============================================' -ForegroundColor Green
Write-Host '  Project Created Successfully!              ' -ForegroundColor Green
Write-Host '=============================================' -ForegroundColor Green
Write-Host ''
Write-Host "  Location     : $TargetPath" -ForegroundColor White
Write-Host "  Source files : $srcCount" -ForegroundColor White
Write-Host "  Test files   : $testCount" -ForegroundColor White
Write-Host "  Doc files    : $docCount" -ForegroundColor White
Write-Host ''
Write-Host '  Next Steps:' -ForegroundColor Cyan
$cd = '  1. cd "' + $TargetPath + '"'
$build = '  2. dotnet build ' + $ProjectName + '.slnx'
Write-Host $cd -ForegroundColor White
Write-Host $build -ForegroundColor White
Write-Host '  3. dotnet test' -ForegroundColor White
Write-Host '  4. Open docs/README.md for pipeline flow' -ForegroundColor White
Write-Host ''
