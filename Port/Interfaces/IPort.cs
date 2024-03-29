﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomaticMobileStation
{
    public interface IPort
    {
        PortState GetState { get; }

        void PlugToTerminal(object sender, EventArgs args);
        void UnPlugFromTerminal(object sender, EventArgs args);

        event EventHandler<Request> OnOutgoingRequest;
        event EventHandler<Request> OnIncomingRequest;

        event EventHandler<Response> OnOutgoingResponse;

        event EventHandler<Response> OnRequestWasCompleted;
        event EventHandler<Response> OnTerminateRequest;

        event EventHandler<Request> OnUnplaged;

        void OutgoingRequest(object sender, Request request);
        void IncomingRequest(Request e);

        void OutgoingResponse(object sender, Response response);

        void TerminateCall(object sender, Response response);
        void CallWasCompleted(Response response);
    }
}
