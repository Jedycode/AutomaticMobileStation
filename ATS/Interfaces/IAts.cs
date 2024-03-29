﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticMobileStation
{
    public interface IAts
    {
        void ConnectToBillingSystem(IBillingSystem billingSystem);
        void RegisterOnPortEvent(IPort port);
        void ClearEvents();
    }
}
