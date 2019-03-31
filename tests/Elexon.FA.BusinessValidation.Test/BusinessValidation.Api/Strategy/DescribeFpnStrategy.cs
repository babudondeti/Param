using Elexon.FA.BusinessValidation.Api.Strategy;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.FPNFlow.Command;
using Elexon.FA.Core.IntegrationMessage;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Api.Strategy
{
    public class DescribeFpnStrategy : IDisposable
    {
        private Mock<IMediator> _mockMediator;

        public DescribeFpnStrategy()
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
        public async Task ItShouldReturnTypeOfFpnStrategy()
        {
            await Task.Run(() =>
            {
                FpnStrategy fpnStrategy = new FpnStrategy(_mockMediator.Object);

                Assert.NotNull(fpnStrategy);
                Assert.IsType<FpnStrategy>(fpnStrategy);
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

                ValidatedFpnCommand command = new ValidatedFpnCommand(item);
                _mockMediator.Setup(x => x.Send(command, new System.Threading.CancellationToken()))
                .Returns(Task.FromResult(new BusinessValidationProxy()));

                IBusinessValidationStrategy fpnStrategy = new FpnStrategy(_mockMediator.Object);

                Task<BusinessValidationProxy> result = fpnStrategy.ExecuteStrategy(string.Empty, item);

                _mockMediator.Verify(
                    v => v.Send(It.IsAny<ValidatedFpnCommand>(), It.IsAny<CancellationToken>())
                    , Times.Exactly(1));


            });
        }
    }
}
