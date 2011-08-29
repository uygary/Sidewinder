pushd ..\releases\v%1\source

del NuGet\Sidewinder\lib\net40\delete.me
copy Sidewinder\bin\debug\sidewinder.exe NuGet\Sidewinder\lib\net40
copy Sidewinder\bin\debug\sidewinder.pdb NuGet\Sidewinder\lib\net40

del NuGet\Sidewinder.Core\lib\net40\delete.me
copy Sidewinder\bin\debug\sidewinder.core.dll NuGet\Sidewinder\lib\net40
copy Sidewinder\bin\debug\sidewinder.core.pdb NuGet\Sidewinder\lib\net40

popd
pushd ..\
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Sidewinder\Sidewinder.nuspec -version %1 -OutputDirectory Releases\v%1 
Automation\NuGet\NuGet.exe pack Releases\v%1\source\NuGet\Sidewinder.Core\Sidewinder.Core.nuspec -version %1 -OutputDirectory Releases\v%1 
popd

