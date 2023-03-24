// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Management.Automation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniSharp.Extensions.JsonRpc;
using Microsoft.PowerShell.EditorServices.Services.Symbols;
using Microsoft.PowerShell.EditorServices.Services.PowerShell;
using Microsoft.PowerShell.EditorServices.Services;
using Microsoft.Extensions.Logging;
using Microsoft.PowerShell.EditorServices.Services.TextDocument;
namespace Microsoft.PowerShell.EditorServices.Handlers
{
    [Serial, Method("powerShell/renameSymbol")]
    internal interface IRenameSymbolHandler : IJsonRpcRequestHandler<RenameSymbolParams, RenameSymbolResult> { }

    internal class RenameSymbolParams : IRequest<RenameSymbolResult>
    {
        public string FileName { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
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

        private readonly ILogger _logger;
        private readonly WorkspaceService _workspaceService;

        public RenameSymbolHandler(IInternalPowerShellExecutionService executionService,
        ILoggerFactory loggerFactory,
            WorkspaceService workspaceService) {
            _logger = loggerFactory.CreateLogger<RenameSymbolHandler>();
            _workspaceService = workspaceService;
            _executionService = executionService;
            }

        public async Task<RenameSymbolResult> Handle(RenameSymbolParams request, CancellationToken cancellationToken)
        {
            if (!_workspaceService.TryGetFile(request.FileName, out ScriptFile scriptFile))
            {
                _logger.LogDebug("Failed to open file!");
                return null;
            }
            //scriptFile.ScriptAst.FindAll()
            IEnumerable<SymbolReference> occurrences = SymbolsService.FindOccurrencesInFile(
                scriptFile,
                request.Line+1,
                request.Column+1);
            _logger.LogDebug("Testing");
            // TODO: Refactor to not rerun the function definition every time.
            PSCommand psCommand = new();
            psCommand
                .AddScript("Return 'hello world'")
                .AddStatement();
            IReadOnlyList<string> result = await _executionService.ExecutePSCommandAsync<string>(psCommand, cancellationToken).ConfigureAwait(false);
            return new RenameSymbolResult
            {
                Text = "YOYOYO"
            };
        }
    }
}
