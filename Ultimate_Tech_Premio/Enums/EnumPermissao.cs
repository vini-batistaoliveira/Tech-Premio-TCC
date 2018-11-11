using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Ultimate_Tech_Premio.Enum
{
    public enum EnumPermissao
    {
        [Description("ADM")]
        ADM,
        [Description("TEC")]
        TEC,
        [Description("USER")]
        USER
    }
}