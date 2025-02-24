#!/usr/bin/bash
## uu helper
#
if [ $# -lt 3 ] ; then
    echo "$0 failed to execute with too few arguments" 1>&2
    echo "Usage: $0 [uudecode|uuencode] infilepath [uufilename] outfilename " 1>&2
    echo "Usage: $0 uuencode infilepath uufilename outfilename " 1>&2
    echo "Usage: $0 uudecode infilepath outfilename " 1>&2
    exit 2;
fi

UUENV_CMD=/usr/bin/uuenview
UUENC_CMD=/usr/bin/uuencode
UUDEV_CMD=/usr/bin/uudeview
UUDEC_CMD=/usr/bin/uudecode

if [ $# -lt 3 ] ; then
    echo "$0 failed to execute with too few arguments" 1>&2
    echo "Usage: $0 [uudecode|uuencode] infilepath [uufilename] outfilename " 1>&2
    echo "Usage: $0 uuencode infilepath uufilename outfilename " 1>&2
    echo "Usage: $0 uudecode infilepath outfilename " 1>&2
    exit 3;
fi

UU_KIND=$1
FILE_IN=$2
FILE_UU_NAME=$3

if [ "$UU_KIND" = "uuencode" ] || [ "$UU_KIND" = "uue" ]  ;  then
    
    if [ $# -gt 3 ] ; then        
        FILE_OUT=$4
    else 
        FILE_OUT=$3
    fi

    if [ -f $UUENC_CMD ] ; then
        if [ -f $FILE_IN ] ; then
            echo "$0 executing:  $UUENC_CMD -u $FILE_IN $FILE_UU_NAME -o $FILE_OUT"
            ${UUENC_CMD} $FILE_IN $FILE_UU_NAME > $FILE_OUT
        else
            /usr/bin/echo "$0 Error reading from file $FILE_IN "  1>&2
            exit 1;
        fi
    else
         /usr/bin/echo "$0 uuencode cmd $UUENC_CMD not found."  1>&2
         if [ -f $UUENV_CMD ] ; then
            echo "$0 executing:  UUENV_CMD -u $FILE_IN  -o $FILE_OUT"
            ${UUENV_CMD}  -u  $FILE_IN   > $FILE_OUT
         else
            /usr/bin/echo "$0 uuenview cmd $UUENV_CMD not found."  1>&2
            exit 2;
        fi
    fi

    if [ -f $FILE_OUT ] ; then        
        echo "$0 Success writing to $FILE_OUT"
        exit 0;
    else
        /usr/bin/echo "$0 Error writing to $FILE_OUT " 1>&2
        exit 3;
    fi
fi


if [ "$UU_KIND" = "uudecode" ] || [ "$UU_KIND" = "uude" ] || [ "$UU_KIND" = "uud" ] ; then

    if [ $# -lt 4 ] ; then
        FILE_OUT=$3
    fi

    if [ $# -gt 3 ] ; then
      FILE_OUT=$4
      FILE_UU_NAME=$3
    fi

    if [ -f $FILE_IN ] ; then

        if [ -f $UUDEC_CMD ] ; then

            echo "$0: executing $UUDEC_CMD $FILE_IN -o $FILE_OUT "
        
            cd /var/www/net/res/uu

            cat  $FILE_IN | ${UUDEC_CMD}  -o $FILE_OUT
            if [ -f $FILE_OUT ] ; then
                echo "$0 Success writing to $FILE_OUT"
                exit 0;
            fi
        fi
        if [ -f $UUDEV_CMD ] ; then
            
            ${UUDEV_CMD} -i "$FILE_IN"  -o $FILE_OUT        
            if [ -f $FILE_OUT ] ; then
                echo "$0 Success writing to $FILE_OUT"
                exit 0;
            fi
        fi
    fi

        
    /usr/bin/echo "$0 Error executing $* " 1>&2
    exit 3;   

fi
