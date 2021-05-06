using System;
using System.Collections.Generic;
using System.Linq;
using Flunt.Validations;
using PaymentContext.Shared.Entites;

namespace PaymentContext.Domain.Entities
{
    public class Subscription : Entity
    {
        private readonly IList<Payment> _payments;
        public Subscription(DateTime? expireDate)
        {
            CreateDate = DateTime.Now;
            LastUpdateDate = DateTime.Now;
            ExpireDate = expireDate;
            Active = true;
            _payments = new List<Payment>();
        }
        public bool Active { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdateDate { get; private set; }
        public DateTime? ExpireDate { get; private set; }
        public IReadOnlyCollection<Payment> Payments { get { return _payments.ToArray(); } }

        public void AddPayment(Payment payment)
        {
            AddNotifications(new Contract()
                .Requires()
                .IsGreaterOrEqualsThan(DateTime.Now, payment.PaidDate, "Subscription.Payments", "A data do pagamento deve ser futura")
            );

            _payments.Add(payment);
        }

        public void Activate() {
            Active = true;
            LastUpdateDate = DateTime.Now;
        }
        public void Deactivate() {
            Active = false;
            LastUpdateDate = DateTime.Now;
        }
    }
}