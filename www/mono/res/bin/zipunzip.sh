#!/usr/bin/bash
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
    OUT=$3
else
    echo "$0 infile $2 doesn't exists or apache2 mod mono has no access to it, Usage: $0 zipop infile outfile"
    exit -1;
fi

if [ "$ZIPOP" = "bz" ] ; then
    /usr/bin/echo "/usr/bin/bzip2 -6 -c $IN outpipe $OUT "
    /usr/bin/bzip2 -6 -c $IN > $OUT
fi

if [ "$ZIPOP" = "bz2" ] ; then
    /usr/bin/echo "/usr/bin/bzip2 -6 -c $IN outpipe $OUT "
    /usr/bin/bzip2 -6 -c $IN > $OUT
fi

if [ "$ZIPOP" = "gz" ] ; then
    echo "/usr/bin/gzip -6 -c $IN outpipe $OUT "
    /usr/bin/gzip -6 -c $IN > $OUT
fi

if [ "$ZIPOP" = "7z" ] ; then        
     echo "/usr/bin/7z a $OUT $IN ":
    /usr/bin/7z a $OUT $IN      
fi

if [ "$ZIPOP" = "z7" ] ; then
    echo "/usr/bin/7z a $OUT $IN ";
    /usr/bin/7z a $OUT $IN      
fi
if [ "$ZIPOP" = "zip" ] ; then
    echo "/usr/bin/zip -r $OUT $IN ";
    /usr/bin/zip -r $OUT $IN 
fi

if [ "$ZIPOP" = "gunzip" ] ; then
    echo "/usr/bin/gzip -c -d $IN outpipe $OUT "
    /usr/bin/gzip -c -d $IN > $OUT
fi

if [ "$ZIPOP" = "bunzip" ] ; then
    echo "/usr/bin/bzip2 -c -d $IN outpipe $OUT "
    /usr/bin/bzip2 -c -d $IN > $OUT
fi

if [ "$ZIPOP" = "unzip7" ] ; then
    echo "/usr/bin/7z -so x $IN outpipe $OUT "
    /usr/bin/7z -so x $IN > $OUT
fi

if [ "$ZIPOP" = "7unzip" ] ; then
    echo "/usr/bin/7z -so x $IN outpipe $OUT "
    /usr/bin/7z -so x $IN > $OUT
fi

if [ "$ZIPOP" = "unzip" ] ; then
    echo "/usr/bin/unzip -p $IN outpipe $OUT "
    /usr/bin/unzip -p $IN > $OUT
fi

#
#case $ZIPOP in
#    "bz" )
#        echp "/usr/bin/bzip2 -6 -c $IN > $OUT"
#        /usr/bin/bzip2 -6 -c $IN > $OUT
#        ;;  
#    "bz2" )
#        echp "/usr/bin/bzip2 -6 -c $IN > $OUT"
#        /usr/bin/bzip2 -6 -c $IN > $OUT
#        ;;  
#    "gz" )
#        echo "/usr/bin/gzip -6 -c $IN > $OUT"
#        /usr/bin/gzip -6 -c $IN > $OUT
#        ;;  
#    "7z" )
#        echo "/usr/bin/7z a $OUT $IN " 
#        /usr/bin/7z a $OUT $IN 
#        ;;
#    "z7" )
#        echo "/usr/bin/7z a $OUT $IN " 
#        /usr/bin/7z a $OUT $IN 
#        ;;
#    "zip" )
#        echo "/usr/bin/zip -r $OUT $IN"
#        /usr/bin/zip -r $OUT $IN 
#        ;;
#    "gunzip" )
#        echo "/usr/bin/gzip -c -d $IN > $OUT"
#        /usr/bin/gzip -c -d $IN > $OUT
#        ;;
#    "bunzip" )
#        echo "/usr/bin/bzip2 -c -d $IN > $OUT "
#        /usr/bin/bzip2 -c -d $IN > $OUT
#        ;;
#    "unzip7" )
#        echo "/usr/bin/7z -so x $IN > $OUT "
#        /usr/bin/7z -so x $IN > $OUT
#        ;;
#    "7unzip" )
#        echo "/usr/bin/7z -so x $IN > $OUT "
#        /usr/bin/7z -so x $IN > $OUT
#        ;;
#    "unzip" )
#        echo "/usr/bin/unzip -p $IN > $OUT"
#        /usr/bin/unzip -p $IN > $OUT
#        ;;
#    * )
#        # Code to be executed if variable doesn't match any pattern
#        echo "$0 invalid argument zipop $1, Usage: $0 zipop infile outfile" 1>&2
#        ;;
#esac
#
#

if [ -f $OUT ] ; then   
    OUTXISTS=1
else
    echo "$0 couldn't create outfile $OUT during operation $ZIPOP processing input file $IN "
    exit -2;
fi

exit 0;


