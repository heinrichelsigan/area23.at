#!/usr/bin/bash

# BC=$(which bc);
# echo 2 + 2 | $($BC)
# echo "$0 started $(date)" >> /tmp/dotnet.tmp

if [ $# -gt 0 ] ; then
        # echo "$0 args: $@"  >> /tmp/dotnet.tmp
        if [ -f /usr/bin/bc ] ; then
                echo "$@" | /usr/bin/bc
                # echo "$@" | /usr/bin/bc >> /tmp/dotnet.tmp
        fi
fi
