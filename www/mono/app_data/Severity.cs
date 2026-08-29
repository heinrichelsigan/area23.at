using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Area23.At.Mono.App_Data
{
    /// <summary>
    /// Severity 
    /// </summary>
    [Serializable]
    [DefaultValue(Severity.None)]
    public enum Severity : short
    {        
        None = 0,
        OK = 1,
        Ask = 2,
        Info = 3,
        Warn = 4,
        Error = 5
    }

}