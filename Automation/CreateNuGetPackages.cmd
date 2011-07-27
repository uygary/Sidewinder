pushd ..\releases\v%1\source

rem del NuGet\Templates\Wolfpack.HealthCheck\lib\net40\delete.me
rem copy Wolfpack.Agent\bin\debug\Wolfpack.Core.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net40
rem copy Wolfpack.Agent\bin\debug\Wolfpack.Core.Interfaces.dll NuGet\Templates\Wolfpack.HealthCheck\lib\net40

popd
pushd ..\
rem Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Templates\Wolfpack.Contrib.BuildAnalytics\Wolfpack.Contrib.BuildAnalytics.nuspec -version %1 -OutputDirectory Releases\v%1 
popd

