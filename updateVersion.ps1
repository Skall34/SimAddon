function Update-vdproj-version([string] $ver, [string] $filename) {
	$vItems = $ver.split(".")
	$version = $vItems[0]+"."+$vItems[1]+"."+$vItems[2]
	echo "using version $version"
    $productCodePattern     = '\"ProductCode\" = \"8\:{([\d\w-]+)}\"'
    $packageCodePattern     = '\"PackageCode\" = \"8\:{([\d\w-]+)}\"'
    $productVersionPattern  = '\"ProductVersion\" = \"8\:[0-9]+(\.([0-9]+)){1,3}\"'
    $productCode            = '"ProductCode" = "8:{' + [guid]::NewGuid().ToString().ToUpper() + '}"'
    $packageCode            = '"PackageCode" = "8:{' + [guid]::NewGuid().ToString().ToUpper() + '}"'
    $productVersion         = '"ProductVersion" = "8:' + $version + '"'

    (Get-Content $filename) | ForEach-Object {
        % {$_ -replace $productCodePattern, $productCode } |
        % {$_ -replace $packageCodePattern, $packageCode } |
        % {$_ -replace $productVersionPattern, $productVersion }
    } | Set-Content $filename
}

function Update-nsis-version([string] $ver, [string] $filename) {
	$vItems = $ver.split(".")
	$version = $vItems[0]+"."+$vItems[1]+"."+$vItems[2]
	echo "using version $version"
	#rem !define VERSION "3.0.1"

    $productVersionPattern  = '\!define VERSION \"[0-9]+(\.([0-9]+)){1,3}\"'
    $productVersion         = '!define VERSION "' + $version + '"'

    (Get-Content $filename) | ForEach-Object {
        % {$_ -replace $productVersionPattern, $productVersion }
    } | Set-Content $filename
}

echo "Tag version $args"
Update-vdproj-version $args[0] "Installer/Installer.vdproj"
Update-nsis-version $args[0] "nsis/installer.nsi"

