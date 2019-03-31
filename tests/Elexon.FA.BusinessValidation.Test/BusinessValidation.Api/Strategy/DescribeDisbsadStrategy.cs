using Elexon.FA.BusinessValidation.Api.Strategy;
using Elexon.FA.BusinessValidation.DISBSADFlow.Command;
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
    public class DescribeDisbsadStrategy : IDisposable
    {
        private Mock<IMediator> _mockMediator;

        public DescribeDisbsadStrategy()
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
        public async Task ItShouldReturnTypeOfNetbsadStrategy()
        {
            await Task.Run(() =>
            {
                DisbsadStrategy disbsadStrategy = new DisbsadStrategy(_mockMediator.Object);

                Assert.NotNull(disbsadStrategy);
                Assert.IsType<DisbsadStrategy>(disbsadStrategy);
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

                ValidatedDisbsadCommand command = new ValidatedDisbsadCommand(item);
                _mockMediator.Setup(x => x.Send(command, new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new BusinessValidationProxy()));

                IBusinessValidationStrategy disbsadStrategy = new DisbsadStrategy(_mockMediator.Object);

                Task<BusinessValidationProxy> result = disbsadStrategy.ExecuteStrategy(string.Empty, item);

                _mockMediator.Verify(
                    v => v.Send(It.IsAny<ValidatedDisbsadCommand>(), It.IsAny<CancellationToken>())
                    , Times.Exactly(1));


            });
        }
    }
}
