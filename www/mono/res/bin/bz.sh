#!/usr/bin/bash

if [ $# -lt 2 ] ; then
	OUT=$1.bz2
else
	OUT=$2
fi

bzip2 -7 -c $1 > $OUT


