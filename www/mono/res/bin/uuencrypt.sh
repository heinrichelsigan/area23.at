#!/usr/bin/bash

if [ $# -lt 3 ] ; then
        echo "$0 failed to execute with too few arguments"
        exit 1;
fi

UU_CMD=/usr/bin/uuenview
FILE_PLAIN=$1
FILE_UU_NAME=$2
FILE_UU_OUT=$3

# echo "$0: executing $UU_CMD $FILE_PLAIN $FILE_UU_NAME > $FILE_UU_OUT" >> /tmp/mono.txt 2>&1
echo "$0: executing $UU_CMD $FILE_PLAIN $FILE_UU_NAME > $FILE_UU_OUT"
if [ -f $FILE_PLAIN ] ; then

        ${UU_CMD} -u $FILE_PLAIN $FILE_UU_NAME > $FILE_UU_OUT

        if [ -f $FILE_UU_OUT ] ; then
                echo "$0 Success writing to $FILE_UU_OUT " 2>&1
                exit 0;
        fi
        echo "$0 Error writing to $FILE_UU_OUT " 1>&2
        exit 2;
fi

echo "$0 Error reading from file $FILE_PLAIN "  1>&2
exit 2;
