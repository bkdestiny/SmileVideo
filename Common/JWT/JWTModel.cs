﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.JWT
{
    public record JWTModel(string name,string rule,string ipAddress,Guid avatar);
}
