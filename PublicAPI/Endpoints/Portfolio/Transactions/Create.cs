using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.TransactionAggregate;
using ApplicationCore.TransactionAggregate.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Portfolio.Transactions
{
    public class Create: BasePortfolioRelatedEndpoint<CreateTransactionRequest, TransactionResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IAssetTransactionService _transactionService; 

        public Create(IAuthorizationService authorizationService, IAssetTransactionService transactionService)
        {
            _authorizationService = authorizationService;
            _transactionService = transactionService;
        }
        
        [HttpPost("transactions")]
        public override async Task<ActionResult<TransactionResponse>> HandleAsync
            ([FromMultipleSource]CreateTransactionRequest request, CancellationToken cancellationToken = new CancellationToken())
        {

            if (!await IsAllowedToExecute(request.PortfolioId, _authorizationService))
            {
                return Unauthorized(NotAllowedPortfolioMessage); 
            }
            var dto = request.CreateTransactionCommand.Adapt<CreateTransactionDto>();

            try
            {
                var transaction = request.CreateTransactionCommand.TransactionType switch
                {
                    "withdrawToCash" => await _transactionService.CreateWithdrawToCashTransaction(dto),
                    "withdrawToOutside" => await _transactionService.Fake(),
                    "moveToFund" => await _transactionService.Fake(),
                    "addValue" => await _transactionService.CreateAddValueTransaction(dto),
                    _ => throw new InvalidOperationException()
                };

                var response = new TransactionResponse(transaction);
                return Ok(response);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest($"Invalid operation {invalidOperationException.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}