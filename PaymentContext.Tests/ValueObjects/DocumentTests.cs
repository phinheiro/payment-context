using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;
using Xunit;

namespace PaymentContext.Tests.ValueObjects
{
    public class DocumentTests
    {
        [Fact]
        public void Document_ShouldReturnErrorWhenCnpjIsInvalid()
        {
            var doc = new Document("123", EDocumentType.CNPJ);
            Assert.True(doc.Invalid);
        }

        [Fact]
        public void Document_ShouldReturnSuccessWhenCnpjIsValid()
        {
            var doc = new Document("34110568000150", EDocumentType.CNPJ);
            Assert.True(doc.Valid);
        }

        [Fact]
        public void Document_ShouldReturnErrorWhenCpfIsInvalid()
        {
            var doc = new Document("312", EDocumentType.CPF);
            Assert.True(doc.Invalid);
        }

        [Fact]
        public void Document_ShouldReturnSuccessWhenCpfIsValid()
        {
            var doc = new Document("34110568000", EDocumentType.CPF);
            Assert.True(doc.Valid);
        }
    }
}