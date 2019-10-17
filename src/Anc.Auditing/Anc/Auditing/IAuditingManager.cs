﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Auditing
{
    public interface IAuditingManager
    {
        AuditLogInfo Current { get; }

        IAuditLogSaveHandle BeginScope();
    }
}