@echo off

echo %* | bzip2.exe -c -d > bunzip.txt
