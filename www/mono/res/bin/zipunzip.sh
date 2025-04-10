#!/usr/bin/bash -x
# arguments: 
#   $1 ... zip/unzip type
#   $2 ... infile, where to process operation
#   $3 ... outfile as result of processing op
# zipunzip provides a unix shell script wrapper for zipping and unzipping following formats:
#   7z bzip2 gzip lzma xz zip
if [ $# -lt 3 ] ; then
    echo "$0 to fiew arguments $*, Usage: $0 zipop infile outfile" 1>&2
    exit $#
fi

ZIPOP=$1
IN=$2   
OUT=$3
NEXIST=0

if [ -f $IN ] ; then
    UU_NAME=$(ls -a $IN);
else
    echo "$0 infile $2 doesn't exists or apache2 mod mono has no access to it, Usage: $0 zipop infile outfile" 1>&2
    exit -1;
fi

#
#
## case esac options
#
case "$ZIPOP" in
    "bz" ) 
        /usr/bin/bzip2 -6 -c $IN > $OUT ;;  
    "bz2" )
        /usr/bin/bzip2 -6 -c $IN > $OUT ;;  
    "gz" )
        /usr/bin/gzip -6 -c $IN > $OUT ;;
    "uuen" )
        /usr/bin/uuenview $IN $UU_NAME > $OUT ;;
    "uuencode" )
        /usr/bin/uuencode $IN $UU_NAME > $OUT ;;
    "7z" ) 
        /usr/bin/7z a $OUT $IN 
        ;;
    "z7" )
        /usr/bin/7z a $OUT $IN 
        ;;
    "zip" )
        /usr/bin/zip -r $OUT $IN 
        ;;
    "bunzip" )
        /usr/bin/bzip2 -c -d $IN > $OUT
        ;;
    "bunzip" )
        /usr/bin/bzip2 -c -d $IN > $OUT
        ;;
    "gunzip" )
        /usr/bin/gzip -c -d $IN > $OUT ;;
    "uudecode" )
        /usr/bin/uudeview $IN -o $OUT
        ;;
    "uude" )
        /usr/bin/cat $IN | /usr/bin/uudecode > $OUT
        ;;
    "unzip7" )
        /usr/bin/7z -so x $IN > $OUT ;;
    "7unzip" )
        /usr/bin/7z -so x $IN > $OUT ;;
    "unzip" )
        /usr/bin/unzip -p $IN > $OUT ;;
    * )
        /usr/bin/echo "$0 invalid argument zipop $1, Usage: $0 zipop infile outfile" 1>&2
        ;;
esac


if [ -f $OUT ] ; then   
    echo > /dev/null
else
    /usr7bin/echo "$0 couldn't create outfile $OUT during operation $ZIPOP processing input file $IN " 1>&2
    exit -2;
fi

exit 0;
