using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using PaymentSimple.Core.Abstractions.Repositories;
using PaymentSimple.Core.Domain.Models;
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

        public AuthorizeController(
            IRepository<Payment> paymentRepository,
            IRepository<Card> cardRepository)
        {
            _paymentRepository = paymentRepository;
            _cardRepository = cardRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentResponse>>> GetAllTransactions()
        {
            var payments = await _paymentRepository.GetAllAsync();

            var response = payments.Where(p =>
            p.Status == (int)Status.Captured)
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


        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> AuthorizePayment(AuthorizeRequest request)
        {
            if (request.Amount < 0)
                throw new Exception($"Incorrect amount {request.Amount}");

            var card = _cardRepository.GetCardByNumberAsync(request.CardHolderNumber).Result;
            if (card is null)
                throw new Exception($"Card with number {request.CardHolderNumber} doesn't exist in system");

            var expiryDate = new DateTime(request.ExpirationYear, request.ExpirationMonth + 1, 01);
            if (expiryDate < DateTime.Today)
                throw new Exception("Card has expired");

            var payment = new Payment()
            {
                Id = Guid.NewGuid(),
                PaymentAmount = request.Amount,
                PaymentCurrency = request.Currency,
                CardId = card.Id,
                OrderId = Guid.Parse(request.OrderReference),
                Status = (int)Status.Authorized
            };

            await _paymentRepository.AddAsync(payment);

            return new PaymentResponse()
            {
                Id = payment.Id,
                Status = payment.Status.GetStatusValue()
            };
        }

        [HttpPost]
        [Route("{id}/voids")]
        public async Task<ActionResult<PaymentResponse>> VoidPayment(PaymentShortRequest request)
        {
            if (request.Id.Equals(Guid.Empty))
                throw new Exception("Payment Id is null!");

            var payment = _paymentRepository.GetByIdAsync(request.Id).Result;
            if (payment is null)
                throw new Exception($"Payment with id {request.Id} doesn't exist");

            var voidedPayment = new Payment()
            {
                Id = Guid.NewGuid(),
                PaymentAmount = payment.PaymentAmount,
                PaymentCurrency = payment.PaymentCurrency,
                CardId = payment.Card.Id,
                OrderId = payment.OrderId,
                Status = (int)Status.Voided
            };

            await _paymentRepository.AddAsync(voidedPayment);

            return new PaymentResponse()
            {
                Id = payment.Id,
                Status = payment.Status.GetStatusValue()
            };
        }

        [HttpPost]
        [Route("{id}/capture")]
        public async Task<ActionResult<PaymentResponse>>CapturePayment(PaymentShortRequest request)
        {
            if (request.Id.Equals(Guid.Empty))
                throw new Exception("Payment Id is null!");

            var payment = _paymentRepository.GetByIdAsync(request.Id).Result;
            if (payment is null)
                throw new Exception($"Payment with id {request.Id} doesn't exist");

            var capturedPayment = new Payment()
            {
                Id = Guid.NewGuid(),
                PaymentAmount = payment.PaymentAmount,
                PaymentCurrency = payment.PaymentCurrency,
                CardId = payment.Card.Id,
                OrderId = payment.OrderId,
                Status = (int)Status.Captured
            };

            await _paymentRepository.AddAsync(capturedPayment);

            return new PaymentResponse()
            {
                Id = payment.Id,
                Status = payment.Status.GetStatusValue()
            };
        }
    }
}
