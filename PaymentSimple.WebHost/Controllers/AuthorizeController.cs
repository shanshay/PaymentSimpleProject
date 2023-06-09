using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using PaymentSimple.Core.Abstractions.Repositories;
using PaymentSimple.Core.Domain.Models;
using PaymentSimple.DataAccess;
using PaymentSimple.Exceptions;
using PaymentSimple.WebHost.Extensions;
using PaymentSimple.WebHost.Models;

namespace PaymentSimple.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Card> _cardRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly int pageSize = 2;

        public AuthorizeController(
            IRepository<Payment> paymentRepository,
            IRepository<Card> cardRepository,
            IRepository<Transaction> transactionRepository)
        {
            _paymentRepository = paymentRepository;
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Get all transactions
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <exception cref="IncorrectPageValueException"></exception>
        [HttpGet]
        public async Task<ActionResult<List<AuthorizeShortResponse>>> GetAllTransactions(int page = 1)
        {
            if (page <= 0)
                throw new IncorrectPageValueException(page);

            var payments = await _paymentRepository.GetAllAsync();
            
            var response = payments.Where(p =>
            p.Status == (int)Status.Captured)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new AuthorizeShortResponse()
                {
                    Amount = x.PaymentAmount,
                    Currency = x.PaymentCurrency,
                    HolderName = x.Card.CardNumber.HideCardNumber(),
                    Id = x.Id,
                    Status = x.Status.GetStatusValue()
                }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Create authorized transaction
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="IncorrectRequestAmountException"></exception>
        /// <exception cref="CardDoesntExistException"></exception>
        /// <exception cref="CardExpiredException"></exception>
        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> AuthorizePayment(AuthorizeRequest request)
        {
            if (request.Amount < 0)
                throw new IncorrectRequestAmountException(request.Amount);

            var card = await _cardRepository.GetCardByNumberAsync(request.CardHolderNumber);
            if (card is null)
                throw new CardDoesntExistException(request.CardHolderNumber);

            var expiryDate = new DateTime(request.ExpirationYear, request.ExpirationMonth + 1, 01);
            if (expiryDate < DateTime.Today)
                throw new CardExpiredException(card.CardNumber);

            var payment = new Payment()
            {
                Id = Guid.NewGuid(),
                PaymentAmount = request.Amount,
                PaymentCurrency = request.Currency,
                CardId = card.Id,
                OrderId = Guid.Parse(request.OrderReference),
                Status = (int)Status.Authorized
            };

            await _paymentRepository.CreateAsync(payment);

            var transaction = new Transaction(payment);
            await _transactionRepository.CreateAsync(transaction);

            return new TransactionResponse(payment);
        }

        /// <summary>
        /// Create voided transaction
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="PaymentDoesntExistException"></exception>
        [HttpPost]
        [Route("{id}/voids")]
        public async Task<ActionResult<TransactionResponse>> VoidPayment(TransactionShortRequest request)
        {
            if (request.Id.Equals(Guid.Empty))
                throw new PaymentIdIsNullException();

            var payment = await _paymentRepository.GetByIdAsync(request.Id);
            if (payment is null)
                throw new PaymentDoesntExistException(request.Id.ToString());

            payment.Status = (int)Status.Voided;
            await _paymentRepository.UpdateAsync(payment);

            var transaction = new Transaction(payment);
            await _transactionRepository.CreateAsync(transaction);

            return new TransactionResponse(payment);
        }

        /// <summary>
        /// Create captured transaction
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="PaymentDoesntExistException"></exception>
        [HttpPut]
        [Route("{id}/capture")]
        public async Task<ActionResult<TransactionResponse>>CapturePayment(TransactionShortRequest request)
        {
            if (request.Id.Equals(Guid.Empty))
                throw new PaymentIdIsNullException();

            var payment = await _paymentRepository.GetByIdAsync(request.Id);
            if (payment is null)
                throw new PaymentDoesntExistException(request.Id.ToString());

            payment.Status = (int)Status.Captured;
            await _paymentRepository.UpdateAsync(payment);

            var transaction = new Transaction(payment);
            await _transactionRepository.CreateAsync(transaction);

            return new TransactionResponse(payment);
        }        
    }
}
