using System;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;
using Xunit;

namespace PaymentContext.Tests
{
    public class StudentTests
    {
        private readonly Name _name;
        private readonly Document _document;
        private readonly Email _email;
        private readonly Address _address;
        private readonly Student _student;
        private readonly Subscription _subscription;
        public StudentTests()
        {
            _name = new Name("Pedro", "Pinheiro");
            _document = new Document("12358745220", EDocumentType.CPF);
            _email = new Email("pedro@email.com");
            _address = new Address("Rua 1", "1234", "Bairro Legal", "Araguaina", "TO", "BR", "33213487");
            _student = new Student(_name, _document, _email);
            _subscription = new Subscription(null);
        }
        [Fact]
        public void Student_ShouldReturnErrorWhen_HadActiveSubscription()
        {
            var payment = new PayPalPayment("1234213589076472", DateTime.Now, DateTime.Now.AddDays(2), 10, 10, "Pedro Pinheiro", _document, _address, _email);
            _subscription.AddPayment(payment);

            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.True(_student.Invalid);
        }

        [Fact]
        public void Student_ShouldReturnErrorWhen_SubscriptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);

            Assert.True(_student.Invalid);
        }

        [Fact]
        public void Student_ShouldReturnSuccessWhen_HadNoActiveSubscription()
        {
            var payment = new PayPalPayment("1234213589076472", DateTime.Now, DateTime.Now.AddDays(2), 10, 10, "Pedro Pinheiro", _document, _address, _email);
            _subscription.AddPayment(payment);
            _student.AddSubscription(_subscription);

            Assert.True(_student.Valid);
        }
    }
}
