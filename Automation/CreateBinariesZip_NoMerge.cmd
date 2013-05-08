pushd ..\
rem msbuild.exe automation\msbuild\ReleaseBinariesZip.proj /p:pProjectFolder=%cd%;pReleaseNumber=%1;pTargetProject=Wolfpack.Agent /t:Zip
rem ren Releases\v%1\Binaries_v%1.zip Wolfpack_Binaries_v%1.zip
rem msbuild.exe automation\msbuild\ReleaseBinariesZip.proj /p:pProjectFolder=%cd%;pReleaseNumber=%1;pTargetProject=Wolfpack.AppStats.Demo /t:Zip
rem ren Releases\v%1\Binaries_v%1.zip AppStatsDemoClient_Binaries_v%1.zip
popd