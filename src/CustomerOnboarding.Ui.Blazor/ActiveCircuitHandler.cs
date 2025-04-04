﻿using Csla.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace CustomerOnboarding.Ui.Blazor
{
    public class ActiveCircuitHandler(ActiveCircuitState state) : CircuitHandler
    {
        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            state.CircuitExists = true;
            return base.OnCircuitOpenedAsync(circuit, cancellationToken);
        }

        public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            state.CircuitExists = false;
            return base.OnCircuitClosedAsync(circuit, cancellationToken);
        }
    }
}
