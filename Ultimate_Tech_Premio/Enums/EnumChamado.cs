using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Ultimate_Tech_Premio.Enum
{
    public enum EnumChamado
    {
        [Description("ABERTO")]
        ABERTO,
        [Description("FECHADO")]
        FECHADO,
        [Description("PENDENTE")]
        PENDENTE
    }
}