namespace Elexon.FA.BusinessValidation.Domain.Query
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IQueryFlow{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueryFlow<T>
    {
        /// <summary>
        /// The GetListAsync
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/></param>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <returns>The <see cref="Task{List{T}}"/></returns>
        Task<List<T>> GetListAsync(string filePath, string blobName);

        /// <summary>
        /// The GetBMUParticipationAsync
        /// </summary>
        /// <param name="TimeFrom">The TimeFrom<see cref="DateTime"/></param>
        /// <param name="TimeTo">The TimeTo<see cref="DateTime"/></param>
        /// <returns>The <see cref="Task{List{ParticipantEnergyAsset}}"/></returns>
        Task<List<ParticipantEnergyAsset>> GetBmuParticipationAsync(DateTime TimeFrom, DateTime TimeTo);

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteAsync(string blobName);

        /// <summary>
        /// The GetListBoalfIndexTable
        /// </summary>
        /// <param name="bmuName">The bmuName<see cref="string"/></param>
        /// <param name="boAcceptanceNumber">The boAcceptanceNumber<see cref="string"/></param>
        /// <param name="boAcceptanceTime">The boAcceptanceTime<see cref="string"/></param>
        /// <returns>The <see cref="Task{List{BoalfIndexTable}}"/></returns>
        Task<List<BoalfIndexTable>> GetListBoalfIndexTable(string bmuName, string boAcceptanceNumber, string boAcceptanceTime);

        /// <summary>
        /// The ExistsAsync
        /// </summary>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> ExistsAsync(string blobName);
    }
}
