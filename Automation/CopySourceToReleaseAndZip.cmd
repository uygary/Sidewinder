pushd ..\
msbuild.exe Automation\msbuild\ReleaseSource.proj /p:pProjectFolder=%cd%;pSolutionFile=Sidewinder.sln;pReleaseNumber=%1 /t:ExecuteCopyAndZip
ren Releases\v%1\Source_v%1.zip Sidewinder_Source_v%1.zip
popd

