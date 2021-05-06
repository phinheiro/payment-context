using System;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;
using Xunit;

namespace PaymentContext.Tests.Handlers
{
    public class SubscriptionHandlerTests
    {
        [Fact]
        public void Document_ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand
            {
                FirstName = "Bruce",
                LastName = "Wayne",
                Document = "99999999999",
                Email = "hello@balta.io2",
                BarCode = "123456789",
                BoletoNumber = "2315465132",
                PaymentNumber = "134652133135",
                PaidDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddMonths(1),
                Total = 60,
                TotalPaid = 60,
                Payer = "Wayne Enterprises",
                PayerDocument = "12345678911",
                PayerDocumentType = EDocumentType.CPF,
                PayerEmail = "batman@dc.com",
                Street = "Rua 1",
                Number = "123",
                Neighborhood = "Bairro 2",
                City = "Gotham City",
                State = "New Jersey",
                Country = "United States",
                ZipCode = "00501",
            };

            handler.Handle(command);

            Assert.False(handler.Valid);
        }

        [Fact]
        public void Document_ShouldReturnErrorEmailExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand
            {
                FirstName = "Bruce",
                LastName = "Wayne",
                Document = "99999999991",
                Email = "hello@balta.io",
                BarCode = "123456789",
                BoletoNumber = "2315465132",
                PaymentNumber = "134652133135",
                PaidDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddMonths(1),
                Total = 60,
                TotalPaid = 60,
                Payer = "Wayne Enterprises",
                PayerDocument = "12345678911",
                PayerDocumentType = EDocumentType.CPF,
                PayerEmail = "batman@dc.com",
                Street = "Rua 1",
                Number = "123",
                Neighborhood = "Bairro 2",
                City = "Gotham City",
                State = "New Jersey",
                Country = "United States",
                ZipCode = "00501",
            };

            handler.Handle(command);

            Assert.False(handler.Valid);
        }

        [Fact]
        public void Document_ShouldReturnSuccessWhenDocumentAndEmailDoesNotExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand
            {
                FirstName = "Bruce",
                LastName = "Wayne",
                Document = "99999999991",
                Email = "hello@balta.io2",
                BarCode = "123456789",
                BoletoNumber = "2315465132",
                PaymentNumber = "134652133135",
                PaidDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddMonths(1),
                Total = 60,
                TotalPaid = 60,
                Payer = "Wayne Enterprises",
                PayerDocument = "12345678911",
                PayerDocumentType = EDocumentType.CPF,
                PayerEmail = "batman@dc.com",
                Street = "Rua 1",
                Number = "123",
                Neighborhood = "Bairro 2",
                City = "Gotham City",
                State = "New Jersey",
                Country = "United States",
                ZipCode = "00501",
            };

            handler.Handle(command);

            Assert.True(handler.Valid);
        }
    }
}