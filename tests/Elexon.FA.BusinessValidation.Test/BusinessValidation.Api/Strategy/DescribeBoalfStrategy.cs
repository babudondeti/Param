using Elexon.FA.BusinessValidation.Api.Strategy;
using Elexon.FA.BusinessValidation.BOALFFlow.Command;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.Core.IntegrationMessage;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Api.Strategy
{
    public class DescribeBoalfStrategy
    {
        private readonly Mock<IMediator> _mockMediator;

        public DescribeBoalfStrategy()
        {
            _mockMediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task ItShouldReturnTypeOfBoalfStrategy()
        {
            await Task.Run(() =>
            {
                BoalfStrategy boalfStrategy = new BoalfStrategy(_mockMediator.Object);

                Assert.NotNull(boalfStrategy);
                Assert.IsType<BoalfStrategy>(boalfStrategy);
            });
        }

        [Fact]
        public async Task ItShouldCallMediatorSendMethodExactlyOnceWhenExecuteStrategyCalled()
        {
            await Task.Run(() =>
            {
                Item item = new Item()
                {
                    ItemPath = "Inbound/path",
                    ItemId = "location"
                };

                ValidatedBoalfCommand command = new ValidatedBoalfCommand(item);
                _mockMediator.Setup(x => x.Send(command, new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new BusinessValidationProxy()));

                IBusinessValidationStrategy boalfStrategy = new BoalfStrategy(_mockMediator.Object);

                Task<BusinessValidationProxy> result = boalfStrategy.ExecuteStrategy(string.Empty, item);

                _mockMediator.Verify(
                    v => v.Send(It.IsAny<ValidatedBoalfCommand>(), It.IsAny<CancellationToken>())
                    , Times.Exactly(1));


            });
        }
    }
}
