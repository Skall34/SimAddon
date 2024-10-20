echo copy from %1
echo current folder is
cd
if not exist ..\bin mkdir ..\bin
copy %1\*.* ..\bin