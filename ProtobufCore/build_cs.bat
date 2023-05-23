@echo off

set "PROTOC_EXE=%cd%\protoc.exe"
set "WORK_DIR=%cd%\proto"
set "CS_OUT_PATH=%cd%\Target"
::if not exist %CS_OUT_PATH% md %CS_OUT_PATH%

echo "==>>buildStart"
for /f "delims=" %%i in ('dir /b proto "proto/*.proto"') do (
    echo build file:%%%i
    %PROTOC_EXE% --proto_path="%WORK_DIR%" --csharp_out="%CS_OUT_PATH%" "%WORK_DIR%\%%i"
)
echo "==>>build finish"
echo "==>>copy cs"

copy %cd%\Target\ ..\Protobuf\

pause