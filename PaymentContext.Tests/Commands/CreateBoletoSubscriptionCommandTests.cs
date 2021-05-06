using PaymentContext.Domain.Commands;
using Xunit;

namespace PaymentContext.Tests.Commands
{
    public class CreateBoletoSubscriptionCommandTests
    {
        [Fact]
        public void Document_ShouldReturnErrorWhenNameIsInvalid()
        {
            var command = new CreateBoletoSubscriptionCommand
            {
                FirstName = ""
            };

            command.Validate();
            Assert.False(command.Valid);
        }
    }
}