#!/bin/bash
#
# Usage #1 (set static color): SetDayLight ws://HOST:PORT red green blue white
# Usage #2 (set fading): SetDayLight ws://HOST:PORT redFrom greenFrom blueFrom whiteFrom redTo greenTo blueTo whiteTo
# Usage #3 (set fading): SetDayLight ws://HOST:PORT redFrom greenFrom blueFrom whiteFrom redTo greenTo blueTo whiteTo fadeSteps pauseFadeSteps[msec]
#
# echo 'export LC_NUMERIC="en_US.UTF-8"' >>~/.bashrc
#

WSTA=/usr/local/bin/wsta

if (( $# == 5 )) ; then
    echo "static"
    host=$1
    msg='{'"\"r\":$2,\"g\":$3,\"b\":$4,\"w\":$5"'}'
    $WSTA $host $msg
elif (( $# >= 9 )) ; then
    echo "fade with autoscale"

    steps=50
    delay=1000

    if (( $# == 11 )) ; then
        echo "fade with parameters"
        steps=${10}
        delay=${11}

        echo "Delay: $delay"
    else
        echo "fade with autoscale"
    fi

    r0=$2
    r1=$6
    rdiff=`expr $r1 - $r0`
    rr=`awk  'BEGIN { rounded = sprintf("%.3f", '${rdiff}'/'${steps}'); print rounded }'`
    rsteps=$(printf "%.3f" $rr)

    g0=$3
    g1=$7
    gdiff=`expr $g1 - $g0`
    gg=`awk  'BEGIN { rounded = sprintf("%.3f", '${gdiff}'/'${steps}'); print rounded }'`
    gsteps=$(printf "%.3f" $gg)

    b0=$4
    b1=$8
    bdiff=`expr $b1 - $b0`
    bb=`awk  'BEGIN { rounded = sprintf("%.3f", '${bdiff}'/'${steps}'); print rounded }'`
    bsteps=$(printf "%.3f" $bb)

    w0=$5
    w1=$9
    wdiff=`expr $w1 - $w0`
    ww=`awk  'BEGIN { rounded = sprintf("%.3f", '${wdiff}'/'${steps}'); print rounded }'`
    wsteps=$(printf "%.3f" $ww)

    echo "Steps: $rsteps   $gsteps   $bsteps   $wsteps"

    host=$1

    COUNTER=0
    MAXSTEPS=$steps
    let MAXSTEPS=MAXSTEPS
    msecsToWait=`awk 'BEGIN {print sprintf("%0.3f", ('${delay}'/'1000'))}'`
    while [  $COUNTER -lt $MAXSTEPS ]; do

        r0=`awk 'BEGIN {print sprintf("%d", ('${r0}'+'${rsteps}'))}'`
        g0=`awk 'BEGIN {print sprintf("%d", ('${g0}'+'${gsteps}'))}'`
        b0=`awk 'BEGIN {print sprintf("%d", ('${b0}'+'${bsteps}'))}'`
        w0=`awk 'BEGIN {print sprintf("%d", ('${w0}'+'${wsteps}'))}'`

        msg='{'"\"r\":$r0,\"g\":$g0,\"b\":$b0,\"w\":$w0"'}'

        echo "MSG: $msg"

        $WSTA $host $msg

        sleep ${msecsToWait}

        let COUNTER=COUNTER+1
    done
else
    echo "help"
fi