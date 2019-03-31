namespace Elexon.FA.BusinessValidation.API
{
    using Elexon.FA.Core.Services;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="Program" />
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class.
        /// </summary>
        Program()
        {
        }

        /// <summary>
        /// The Main
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public static async Task Main(string[] args)
        {
            await ServiceHost.Create<Startup>(args)
                    .Build()
                    .Run();
        }
    }
}
