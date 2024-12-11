#!/usr/bin/bash

if [ $# -lt 2 ] ; then
        echo "$0 failed to execute with too few arguments" 1>&2
        exit 1;
fi

UUDV_CMD=/usr/bin/uudeview
UUD_CMD=/usr/bin/uudecode
FILE_UU_IN=$1
FILE_OUT=$2

if [ $# -gt 2 ] ; then
  FILE_OUT=$3
fi

echo "$0: executing $UUD_CMD $FILE_UU_IN -o $FILE_OUT "
if [ -f $FILE_UU_IN ] ; then

        cd /var/www/net/res/uu

        cat  $FILE_UU_IN | ${UUD_CMD}  -o $FILE_OUT
        
        ${UUDV_CMD} -i "$FILE_UU_IN"  -o $FILE_OUT

        if [ -f $FILE_OUT ] ; then
                echo "$0 Success writing to $FILE_OUT"
                exit 0;
        fi
        echo "$0 Error writing to $FILE_OUT " 1>&2
        exit 2;
fi

echo "$0 Error reading from file $FILE_UU_IN " 1>%2
exit 2;
