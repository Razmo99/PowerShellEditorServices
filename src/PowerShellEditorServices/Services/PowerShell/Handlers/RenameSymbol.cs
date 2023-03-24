// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniSharp.Extensions.JsonRpc;
using Microsoft.PowerShell.EditorServices.Services.PowerShell;

namespace Microsoft.PowerShell.EditorServices.Handlers
{
    [Serial, Method("powerShell/renameSymbol")]
    internal interface IRenameSymbolHandler : IJsonRpcRequestHandler<RenameSymbolParams, RenameSymbolResult> { }

    internal class RenameSymbolParams : IRequest<RenameSymbolResult>
    {
        public bool FileName { get; set; }
        public bool Line { get; set; }
        public bool Column { get; set; }
        public bool WantsTextChanges { get; set; }
        public bool ApplyTextChanges { get; set; } = true;
        public string RenameTo { get; set; }
    }

    internal class RenameSymbolResult
    {
        public string Text { get; set; }
    }

    internal class RenameSymbolHandler : IRenameSymbolHandler
    {
        private readonly IInternalPowerShellExecutionService _executionService;

        public RenameSymbolHandler(IInternalPowerShellExecutionService executionService) => _executionService = executionService;

        public async Task<RenameSymbolResult> Handle(RenameSymbolParams request, CancellationToken cancellationToken)
        {

            // TODO: Refactor to not rerun the function definition every time.
            PSCommand psCommand = new();
            psCommand
                .AddScript("Return 'hello world'")
                .AddStatement();
            System.Collections.Generic.IReadOnlyList<string> result = await _executionService.ExecutePSCommandAsync<string>(psCommand, cancellationToken).ConfigureAwait(false);
            return new RenameSymbolResult
            {
                Text = result[0]
            };
        }
    }
}
