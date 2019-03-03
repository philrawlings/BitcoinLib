using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinLib
{
    public enum SigHash : byte
    {
        // ALL: Applies to all inputs and all of the outputs
        ALL_IN_ALL_OUT = 0x01,
        // NONE: Applies to all inputs and none of the outputs
        ALL_IN_NO_OUT = 0x02,
        // SINGLE: Applied to all inputs but only the one output with the same index number as the signed input
        ALL_IN_SINGLE_OUT = 0x03,
        // ALL|ANYONECANPAY
        SINGLE_IN_ALL_OUT = 0x81,
        // NONE|ANYONECANPAY
        SINGLE_IN_NONE_OUT = 0x82,
        // SINGLE|ANYONECANPAY
        SINGLE_IN_SINGLE_OUT = 0x83
    }
}
