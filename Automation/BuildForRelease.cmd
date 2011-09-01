pushd ..\
msbuild.exe Automation\msbuild\ReleaseSource.proj /p:pProjectFolder=%cd%;pSolutionFile=Sidewinder.sln;pReleaseNumber=%1
popd
