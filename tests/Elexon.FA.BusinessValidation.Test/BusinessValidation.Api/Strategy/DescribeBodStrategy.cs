using Elexon.FA.BusinessValidation.Api.Strategy;
using Elexon.FA.BusinessValidation.BODFlow.Command;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.Core.IntegrationMessage;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Api.Strategy
{
    public class DescribeBodStrategy : IDisposable
    {
        private Mock<IMediator> _mockMediator;

        public DescribeBodStrategy()
        {
            _mockMediator = new Mock<IMediator>();
        }
        /// <summary>
        /// Dispose unused resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// The Clean up
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            _mockMediator = null;
        }
        [Fact]
        public async Task ItShouldReturnTypeOfBodStrategy()
        {
            await Task.Run(() =>
            {
                BodStrategy bodStrategy = new BodStrategy(_mockMediator.Object);

                Assert.NotNull(bodStrategy);
                Assert.IsType<BodStrategy>(bodStrategy);
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

                ValidatedBodCommand command = new ValidatedBodCommand(item);
                _mockMediator.Setup(x => x.Send(command, new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new BusinessValidationProxy()));

                IBusinessValidationStrategy bodStrategy = new BodStrategy(_mockMediator.Object);

                Task<BusinessValidationProxy> result = bodStrategy.ExecuteStrategy(string.Empty, item);

                _mockMediator.Verify(
                    v => v.Send(It.IsAny<ValidatedBodCommand>(), It.IsAny<CancellationToken>())
                    , Times.Exactly(1));


            });
        }
    }
}
