@echo off

if "%1"=="" goto :usage
if "%2"=="" goto :usage
if "%3"=="" goto :usage

set ZipOp=%1
set IN=%2
set OUT=%3 

:Select
goto %ZipOp%

	:bz
	echo "bzip2.exe -c %IN%  > %OUT% "
	bzip2.exe -c %2  > %3
	goto CaseEnd

	:bz2
	echo "bzip2.exe -c %IN%  > %OUT% "
	bzip2.exe -c %2  > %3
	goto CaseEnd

	:gz
	echo "gzip.exe -c %IN%  > %OUT% "
	gzip.exe -c %2  > %3
	goto CaseEnd

	:7z
	echo "7z -t7z -spf -ssc -sccWIN a %OUT% %IN%"
	7z a %OUT% %IN%
	goto CaseEnd

	:z7
	echo "7z -t7z  -spf -ssc -sccWIN a %OUT% %IN%"
	7z a %OUT% %IN%
	goto CaseEnd

	:zip
	echo "7z  -tzip a %OUT% %IN%"
	7z -tzip a %OUT% %IN%
	goto CaseEnd


	:bunzip
	echo "bzip2 -c -d %IN% > %OUT% "
	bzip2.exe -c -d %IN% > %OUT%
	REM bzip2.exe -c %2  > %3
	goto CaseEnd

	:gunzip
	echo "gzip -c -d %IN% > %OUT% "
	gzip.exe -c -d %IN%  > %OUT%
	goto CaseEnd

	:7unzip
	echo "7z -so x %IN% > %OUT% "
	7z -so x %IN% > %OUT%
	goto CaseEnd

	:unzip7
	echo "7z -so x %IN% > %OUT% "
	7z -so x %IN% > %OUT%

	:unzip
	echo "7z -so -tzip x %IN% > %OUT% "
	7z -tzip x %IN% > %OUT%
	goto CaseEnd


	:?
	echo ?
	goto CaseEnd

	:usage
		echo "Usage: zipunzip.bat zipop infile outfile"
		exit 1


:CaseEnd