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
	echo "bzip2.exe -c %IN%  outpipe %OUT% "
	bzip2.exe -c %2  > %3

	goto CaseEnd

	:bz2
	echo "bzip2.exe -c %IN%  outpipe %OUT% "
	bzip2.exe -c %2  > %3

	goto CaseEnd

	:gz
	echo "gzip.exe -c %IN%  outpipe %OUT% "
	gzip.exe -c %2  > %3

	goto CaseEnd

	:7z
	echo "7z a %OUT% %IN%"
	7z a %OUT% %IN%

	goto CaseEnd

	:z7
	echo "7z a %OUT% %IN%"
	7z a %OUT% %IN%

	goto CaseEnd

	:zip
	echo zip
	goto CaseEnd


	:bunzip
	echo "bzip2 -c -d %IN% outpipe %OUT% "
	bzip2.exe -c -d %IN% > %OUT%
	REM bzip2.exe -c %2  > %3

	goto CaseEnd

	:gunzip
	echo "gzip -c -d %IN% outpipe %OUT% "
	gzip.exe -c -d %IN%  > %OUT%

	goto CaseEnd

	:7unzip
	echo "7z -so x %IN% outpipe %OUT% "
	7z -so x %IN% > %OUT%

	goto CaseEnd

	:unzip7
	echo "7z -so x %IN% outpipe %OUT% "
	7z -so x %IN% > %OUT%

	goto CaseEnd

	:unzip
	echo unzip
	goto CaseEnd


	:?
	echo ?
	goto CaseEnd

	:usage
		echo "Usage: zipunzip.bat zipop infile outfile"
		exit 1


:CaseEnd