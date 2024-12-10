#!/usr/bin/bash

# BC=$(which bc);
# echo 2 + 2 | $($BC)
# echo "$0 started $(date)" >> /tmp/dotnet.tmp
if [ $# -lt 2 ] ; then
        echo "$0 failed to execute with too few arguments"
        exit 1;
fi

UUD_CMD=/usr/bin/uudecode
FILE_UU_IN=$1
FILE_OUT=$2
if [ $# -gt 2 ] ; then
    FILE_OUT=$3
fi

echo "$0: executing $UUD_CMD $FILE_UU_IN -o $FILE_OUT " >> /tmp/mono.txt 2>&1
echo "$0: executing $UUD_CMD $FILE_UU_IN -o $FILE_OUT "
if [ -f $FILE_UU_IN ] ; then
        
        cat $FILE_UU_IN  | ${UUD_CMD} -o $FILE_OUT 

        if [ -f $$FILE_OUT ] ; then
                echo "$0 Success writing to $FILE_OUT " >> /tmp/mono.txt 2>&1
                echo "$0 Success writing to $FILE_OUT"
                exit 0;
        fi
        echo "$0 Error writing to $FILE_OUT " >> /tmp/mono.txt 2>&1
        echo "$0 Error writing to $FILE_OUT " 1>&2
        exit 2;
fi

echo "$0 Error reading from file $FILE_UU_IN "  >> /tmp/mono.txt 2>&1
echo "$0 Error reading from file $FILE_UU_IN "  1>&2
exit 2;
