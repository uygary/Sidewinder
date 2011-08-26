pushd ..\releases\v%1\source

del NuGet\Sidewinder\lib\net40\delete.me
copy Sidewinder\bin\debug\sidewinder.exe NuGet\Sidewinder\lib\net40
copy Sidewinder\bin\debug\sidewinder.pdb NuGet\Sidewinder\lib\net40
copy Sidewinder\bin\debug\NuGet.Core.dll NuGet\Sidewinder\lib\net40
copy Sidewinder\bin\debug\Ionic.Zip.dll NuGet\Sidewinder\lib\net40
copy Sidewinder\bin\debug\Fluent.IO.dll NuGet\Sidewinder\lib\net40
copy Sidewinder\bin\debug\Args.dll NuGet\Sidewinder\lib\net40

popd
pushd ..\
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Sidewinder\Sidewinder.nuspec -version %1 -OutputDirectory Releases\v%1 
popd

