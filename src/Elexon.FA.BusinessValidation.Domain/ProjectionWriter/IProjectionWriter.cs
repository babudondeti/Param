namespace Elexon.FA.BusinessValidation.Domain.ProjectionWriter
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IProjectionWriter" />
    /// </summary>
    public interface IProjectionWriter
    {
        /// <summary>
        /// The UpdateFileStatus
        /// </summary>
        /// <param name="statusTableEntity">The statusTableEntity<see cref="StatusTableEntity"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task UpdateFileStatus(StatusTableEntity statusTableEntity);

        /// <summary>
        /// The UpLoadFile
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task UpLoadFile(object value, string path);

        /// <summary>
        /// The UpLoadFileBoalf
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task UpLoadFileBoalf(object value, string path);

        /// <summary>
        /// The InsertBoalfIndex
        /// </summary>
        /// <param name="boalfIndexTable">The boalfIndexTable<see cref="object"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task InsertBoalfIndex(object boalfIndexTable);
        Task DeleteBlob(string blobName);
    }
}
