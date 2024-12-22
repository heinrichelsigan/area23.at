#!/usr/bin/bash
## huuencode helper 
# 
if [ $# -lt 3 ] ; then 
    echo "$0 failed to execute with too few arguments" 
    exit 2;
fi

UU_CMD=/usr/bin/uuenview
FILE_PLAIN=$1
FILE_UU_NAME=$2
FILE_UU_OUT=$3

if [ -f $UU_CMD ] ; then
    if [ -f $FILE_PLAIN ] ; then 
        FILE_UU_NAME=$2
    else
        /usr/bin/echo "$0 Error reading from file $FILE_PLAIN "  1>&2
        exit 1;
    fi
else 
     /usr/bin/echo "$0 uuencode uuenview cmd not found in $UU_CMD"  1>&2
    exit 2;
fi

# /usr/bin/echo "$0: executing $UU_CMD $FILE_PLAIN $FILE_UU_NAME > $FILE_UU_OUT"
${UU_CMD} -u $FILE_PLAIN $FILE_UU_NAME > $FILE_UU_OUT
   
if [ -f $FILE_UU_OUT ] ; then 
    /usr/bin/cat $FILE_UU_OUT  
else 
    /usr/bin/echo "$0 Error writing to $FILE_UU_OUT " 1>&2 
    exit 3;
fi

exit 0;
