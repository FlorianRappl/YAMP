Function Publish-Library($name) {
    nuget pack YAMP.$name\YAMP.$name.csproj -Prop Configuration=Release
    nuget push *.nupkg
    rm *.nupkg
}

Function Publish-Plugin($name) {
    nuget update YAMP.$name\YAMP.$name.csproj
    Publish-Library $name
}

Publish-Library Core
Publish-Plugin Physics
Publish-Plugin Sensors
Publish-Plugin Io
Publish-Plugin Sets