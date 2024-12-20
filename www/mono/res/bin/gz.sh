#!/usr/bin/bash

if [ $# -lt 2 ] ; then
	OUT=$1.gz
else
	OUT=$2
fi

gzip -7 -c $1 > $OUT

