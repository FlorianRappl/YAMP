$spec = "Nuget\YAMP.nuspec"

Function Update-Content($target, $source) {
    $dest = "Nuget\lib\$target"
    New-Item $dest -type directory -Force
    Copy-Item "$source\bin\Release\YAMP.dll" $dest
    Copy-Item "$source\bin\Release\YAMP.xml" $dest
}

Function Update-Version($file) {
    $ver = (Get-Item $file).VersionInfo.FileVersion
    $repl = "<version>$ver</version>"
    (Get-Content $spec) | 
        Foreach-Object { $_ -replace "<version>(.*)</version>", $repl } | 
        Set-Content $spec
    return $ver
}

Function Publish-Package($ver) {
    $file = "Nuget\AngleSharp.$ver.nupkg"
    nuget pack $spec -OutputDirectory "Nuget"
    nuget push $file
    return $LastExitCode
}

Update-Content "net35" "YAMP"
Update-Content "portable-windows8+net4+windowsphone8+wpa" "YAMP"

$version = Update-Version "Nuget\lib\net35\YAMP.dll"
$success = Publish-Package $version