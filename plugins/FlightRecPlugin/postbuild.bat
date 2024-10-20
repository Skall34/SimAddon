echo copy from %1
echo current folder is
cd
if not exist ..\..\bin\plugins\FlightRecorder mkdir ..\..\bin\plugins\FlightRecorder
copy %1\*.* ..\..\bin\plugins\FlightRecorder