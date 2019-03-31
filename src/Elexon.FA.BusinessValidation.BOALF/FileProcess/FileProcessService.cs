namespace Elexon.FA.BusinessValidation.BOALFlow.FileProcess
{
    using Elexon.FA.BusinessValidation.BOALFlow.Model;
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
    using Elexon.FA.BusinessValidation.Domain.Query;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.KeyVault.Abstractions;
    using Elexon.FA.Core.Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FileProcessService" />
    /// </summary>
    public class FileProcessService : IFileProcessService
    {
        /// <summary>
        /// Defines the _coreKeyVaultClient
        /// </summary>
        private readonly ICoreKeyVaultClient coreKeyVaultClient;

        /// <summary>
        /// Defines the _query
        /// </summary>
        private readonly IQueryFlow<Boalf> query;

        /// <summary>
        /// Defines the _writer
        /// </summary>
        private readonly IProjectionWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessService"/> class.
        /// </summary>
        /// <param name="query">The query<see cref="IQueryFlow{Boalf}"/></param>
        /// <param name="writer">The writer<see cref="IProjectionWriter"/></param>
        /// <param name="applicationBuilder">The applicationBuilder<see cref="IApplicationBuilder"/></param>
        public FileProcessService(IQueryFlow<Boalf> query, IProjectionWriter writer, IApplicationBuilder applicationBuilder)
        {
            this.query = query;
            this.writer = writer;
            coreKeyVaultClient = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreKeyVaultClient>();
        }

        /// <summary>
        /// The FileProcess
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task FileProcess(Aggregate<Boalf> aggregate)
        {
            try
            {
                if (aggregate!=null && aggregate.ValidFlow.Count > 0)
                {
                    await ProcessFileForValidRecords(aggregate);
                }

                if (aggregate != null && aggregate.InValidFlow.Count > 0)
                {
                    await ProcessFileForInValidRecords(aggregate);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The ProcessFileForValidRecords
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ProcessFileForValidRecords(Aggregate<Boalf> aggregate)
        {
            var boalfModels = GetBoalfModelsWithSp(aggregate.ValidFlow, aggregate.SettlementDuration);
            try
            {
                if (boalfModels != null && boalfModels.Count > 0)
                {
                    aggregate.SettlementPeriodsForFile.AddRange(boalfModels.SelectMany(g => g.Settlements.Select(s => s.SettlementPeriod)).Distinct().ToList());
                    foreach (var settlementPeriod in aggregate.SettlementPeriodsForFile)
                    {
                        var settlements = boalfModels.SelectMany(g => g.Settlements).Distinct().ToList();
                        var settlement = settlements.Where(g => g.SettlementPeriod == settlementPeriod);
                        string path =await GetPath(settlement.FirstOrDefault().SettlementDay, settlementPeriod);
                        var ExistingboalfList =await GetExistingBlobData(path);
                        await UpdateOrInsertAcceptance(aggregate, ExistingboalfList, path);
                        aggregate.ValidPath.Add(path);
                    }
                    await RemoveAcceptancesForSettlementPeriodUpdateNotReceived(boalfModels, aggregate);
                    await InsertIntoBoalfIndexTable(boalfModels);
                    await writer.UpdateFileStatus(Common.GetStatusOfBusinessValidation(aggregate.ValidPath.FirstOrDefault()));
                    Log.Information(BusinessValidationConstants.LOGMESSAGES_BOALFSUCCESSMESSAGE);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The ProcessFileForInValidRecords
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ProcessFileForInValidRecords(Aggregate<Boalf> aggregate)
        {
            try
            {
                string rejected = await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_REJECTED);
                string path = Common.GetRejectedFolderPath(rejected);
                var newPath = BusinessValidationConstants.BOALF + DateTime.Now.ToString(BusinessValidationConstants.DATEFORMATE) + BusinessValidationConstants.FILEEXTENSION;
                await UploadAcceptance(aggregate.InValidFlow, path.Replace(BusinessValidationConstants.BOALFFILE, newPath,StringComparison.CurrentCultureIgnoreCase));
                aggregate.InValidPath.Add(path);
                await writer.UpdateFileStatus(Common.GetStatusOfRejectedFile(aggregate.InValidPath.FirstOrDefault(), BusinessValidationConstants.WARNING));
                Log.Information(BusinessValidationConstants.LOGMESSAGES_BOALFFAILUREMESSAGE);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The RemoveAcceptancesForSettlementPeriodUpdateNotReceived
        /// </summary>
        /// <param name="boalfModels">The boalfModels<see cref="List{BoalfModel}"/></param>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task RemoveAcceptancesForSettlementPeriodUpdateNotReceived(List<BoalfModel> boalfModels, Aggregate<Boalf> aggregate)
        {
            var boalfGroups = boalfModels.GroupBy(g => new { g.BmuName, g.BidOfferAcceptanceNumber, g.AcceptanceTime });

            try
            {
                foreach (var boalfGroup in boalfGroups)
                {
                    await RemoveAcceptance(boalfModels, aggregate);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The GetPath
        /// </summary>
        /// <param name="settlement">The settlement<see cref="DateTime"/></param>
        /// <param name="settlementPeriod">The settlementPeriod<see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        private async Task<string> GetPath(DateTime settlement, int settlementPeriod)
        {
            try
            {
                string outbound =await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_PROCESSING);
                var path = Common.GetProcessingFolderPath(outbound, settlement, settlementPeriod.ToString());
                return path;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The GetExistingBlobData
        /// </summary>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="List{Boalf}"/></returns>
        private async Task<List<Boalf>> GetExistingBlobData(string path)
        {
            return await query.GetListAsync(path, BusinessValidationConstants.FLOWS_BOALF);
        }

        /// <summary>
        /// The UpdateOrInsertAcceptance
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        /// <param name="boalfList">The boalfList<see cref="List{Boalf}"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task UpdateOrInsertAcceptance(Aggregate<Boalf> aggregate, List<Boalf> boalfList, string path)
        {
            try
            {
                if (boalfList != null && boalfList.Count > 0)
                {
                    var RemoveBoalfs = (from boalf in aggregate.ValidFlow
                                        join blbBoalf in boalfList on boalf.BmuName equals blbBoalf.BmuName
                                        where boalf.BmuName == blbBoalf.BmuName 
                                            && boalf.BidOfferAcceptanceNumber == blbBoalf.BidOfferAcceptanceNumber 
                                            && boalf.AcceptanceTime == blbBoalf.AcceptanceTime
                                        select blbBoalf).Distinct().ToList();

                    await CreateVersionOfExistingFile(boalfList, path);
                    boalfList = boalfList.Except(RemoveBoalfs).ToList();
                    boalfList.AddRange(aggregate.ValidFlow);
                    await UploadAcceptance(boalfList, path);
                }
                else
                {
                    await UploadAcceptance(aggregate.ValidFlow, path);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The RemoveAcceptance
        /// </summary>
        /// <param name="boalfModels">The boalfModels<see cref="List{BoalfModel}"/></param>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task RemoveAcceptance(List<BoalfModel> boalfModels, Aggregate<Boalf> aggregate)
        {
            var settlementPeriodsToRemoveAcceptance = GetSettlementPeriodSToRemoveAcceptance(boalfModels, aggregate);
            try
            {
                if (settlementPeriodsToRemoveAcceptance != null && settlementPeriodsToRemoveAcceptance.Count > 0)
                {
                    foreach (var settlementPeriod in settlementPeriodsToRemoveAcceptance)
                    {
                        string path = await GetPath(aggregate.ValidFlow.FirstOrDefault().TimeTo, settlementPeriod);
                        var ExistingboalfList = await GetExistingBlobData(path);
                        await RemoveAcceptances(boalfModels, path, ExistingboalfList);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        private async Task RemoveAcceptances(List<BoalfModel> boalfModels, string path, List<Boalf> ExistingboalfList)
        {
            if (ExistingboalfList != null && ExistingboalfList.Count > 0)
            {
                var blbBoalfData = (from boalf in boalfModels
                                 join blbBoalf in ExistingboalfList on boalf.BmuName equals blbBoalf.BmuName
                                 where boalf.BmuName == blbBoalf.BmuName
                                     && boalf.BidOfferAcceptanceNumber == blbBoalf.BidOfferAcceptanceNumber
                                     && boalf.AcceptanceTime == blbBoalf.AcceptanceTime
                                     && (boalf.AmendmentFlag.ToUpper() == BusinessValidationConstants.AMMENDMENTFLAGDEL.ToUpper()) select blbBoalf).Distinct().ToList();
                var boalfAcceptanceToRemove = blbBoalfData.Where(a => a.AmendmentFlag.ToUpperInvariant() == BusinessValidationConstants.AMMENDMENTFLAGUPD.ToUpperInvariant()).ToList();
                
                await CreateVersionofExistingFileAndUploadValidAcceptance(ExistingboalfList, boalfAcceptanceToRemove, path);
            }
        }

        /// <summary>
        /// The CreateVersionofExistingFileAndUploadValidAcceptance
        /// </summary>
        /// <param name="ExistingboalfList">The ExistingboalfList<see cref="List{Boalf}"/></param>
        /// <param name="boalfAcceptanceToRemove">The boalfAcceptanceToRemove<see cref="List{Boalf}"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task CreateVersionofExistingFileAndUploadValidAcceptance(List<Boalf> ExistingboalfList, IEnumerable<Boalf> boalfAcceptanceToRemove, string path)
        {
            await CreateVersionOfExistingFile(ExistingboalfList, path);
            var boalfList = ExistingboalfList.Except(boalfAcceptanceToRemove).ToList();
            if (boalfList.Count > 0)
            {
                await UploadAcceptance(boalfList, path);
            }
            else
            {
                await writer.DeleteBlob(path);
            }
        }

        /// <summary>
        /// The GetSettlementPeriodSToRemoveAcceptance
        /// </summary>
        /// <param name="boalfModels">The boalfModels<see cref="List{BoalfModel}"/></param>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="List{int}"/></returns>
        private static List<int> GetSettlementPeriodSToRemoveAcceptance(List<BoalfModel> boalfModels, Aggregate<Boalf> aggregate)
        {
            List<int> settlementPeriodsForAcceptance = new List<int>();
            try
            {
                var settlementPeriods = boalfModels.SelectMany(g => g.Settlements.Select(s => s.SettlementPeriod)).Distinct().ToList();

                var boalfData = (from boalf in aggregate.UpdateorInsFlow
                                 where boalfModels.Any(g => g.BmuName == boalf.PartitionKey
                                     && g.BidOfferAcceptanceNumber.ToString() == boalf.BidOfferAcceptanceNumber
                                     && g.AcceptanceTime == boalf.AcceptanceTime
                                     && (g.AmendmentFlag.ToUpperInvariant() == BusinessValidationConstants.AMMENDMENTFLAGDEL.ToUpperInvariant()))
                                 select boalf).ToList();
                var existingSettlementPeriods = boalfData.Where(p => p.AmendmentFlag.ToUpperInvariant() == BusinessValidationConstants.AMMENDMENTFLAGUPD.ToUpperInvariant())
                    .Select(a => a.SettlementPeriods.Split(','));

                if (existingSettlementPeriods.FirstOrDefault() != null)
                {
                    settlementPeriodsForAcceptance = existingSettlementPeriods.FirstOrDefault().Select(int.Parse).ToList();
                    settlementPeriodsForAcceptance = settlementPeriodsForAcceptance.Except(settlementPeriods).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
            return settlementPeriodsForAcceptance;
        }

        /// <summary>
        /// The CreateVersionOfExistingFile
        /// </summary>
        /// <param name="boalfList">The boalfList<see cref="List{Boalf}"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private Task CreateVersionOfExistingFile(List<Boalf> boalfList, string path)
        {
            try
            {
                var newPath = BusinessValidationConstants.BOALF + DateTime.Now.ToString(BusinessValidationConstants.DATEFORMATE) + BusinessValidationConstants.FILEEXTENSION;
                return writer.UpLoadFile(boalfList, path.Replace(BusinessValidationConstants.BOALFFILE, newPath,StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The UploadMergedOrValidAcceptance
        /// </summary>
        /// <param name="value">The value<see cref="List{Boalf}"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task UploadAcceptance(List<Boalf> value, string path)
        {
            try
            {
                if (value != null && value.Count > 0)
                {
                    await writer.UpLoadFileBoalf(value, path);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The InsertIntoBoalfIndexTable
        /// </summary>
        /// <param name="boalfModels">The boalfModels<see cref="List{BoalfModel}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task InsertIntoBoalfIndexTable(List<BoalfModel> boalfModels)
        {
            try
            {
                if (boalfModels != null && boalfModels.Count > 0)
                {
                    await writer.InsertBoalfIndex(GetBoalfIndexTable(boalfModels));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// The GetBoalfIndexTable
        /// </summary>
        /// <param name="boalfModels">The boalfModels<see cref="List{BoalfModel}"/></param>
        /// <returns>The <see cref="List{BoalfIndexTable}"/></returns>
        private static List<BoalfIndexTable> GetBoalfIndexTable(List<BoalfModel> boalfModels)
        {
            var boalfGroups = boalfModels.GroupBy(g => new { g.BmuName, g.BidOfferAcceptanceNumber, g.AcceptanceTime });
            List<BoalfIndexTable> lstBoalfIndexTable = new List<BoalfIndexTable>();
            try
            {
                foreach (var boalfGroup in boalfGroups)
                {
                    BoalfIndexTable boalfIndexTable = new BoalfIndexTable();
                    boalfIndexTable.PartitionKey = boalfGroup.FirstOrDefault().BmuName;
                    boalfIndexTable.BmuName = boalfGroup.FirstOrDefault().BmuName;
                    boalfIndexTable.BidOfferAcceptanceNumber = boalfGroup.FirstOrDefault().BidOfferAcceptanceNumber.ToString();
                    boalfIndexTable.AcceptanceTime = boalfGroup.FirstOrDefault().AcceptanceTime;
                    boalfIndexTable.AmendmentFlag = boalfGroup.FirstOrDefault().AmendmentFlag;
                    boalfIndexTable.DeemedBidOfferFlag = boalfGroup.FirstOrDefault().DeemedBidOfferFlag;
                    boalfIndexTable.SoFlag = boalfGroup.FirstOrDefault().SoFlag;
                    boalfIndexTable.StorFlag = boalfGroup.FirstOrDefault().StorFlag;
                    var settlementPeriods = boalfModels.SelectMany(g => g.Settlements.Select(s => s.SettlementPeriod)).Distinct().ToList();
                    if (settlementPeriods.Count > 0)
                    {
                        boalfIndexTable.SettlementPeriods = String.Join(",", settlementPeriods);
                    }
                    lstBoalfIndexTable.Add(boalfIndexTable);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
            return lstBoalfIndexTable;
        }

        /// <summary>
        /// The GetBoalfModelsWithSp
        /// </summary>
        /// <param name="boalfs">The boalfs<see cref="List{Boalf}"/></param>
        /// <param name="SettlementDuration">The SettlementDuration<see cref="int"/></param>
        /// <returns>The <see cref="List{BoalfModel}"/></returns>
        private List<BoalfModel> GetBoalfModelsWithSp(List<Boalf> boalfs, int SettlementDuration)
        {
            List<BoalfModel> models = new List<BoalfModel>();
            try
            {
                foreach (var item in boalfs)
                {
                    models.Add(new BoalfModel(SettlementDuration)
                    {
                        BmuName = item.BmuName,
                        BidOfferAcceptanceNumber = item.BidOfferAcceptanceNumber,
                        AcceptanceTime = item.AcceptanceTime,
                        DeemedBidOfferFlag = item.DeemedBidOfferFlag,
                        SoFlag = item.SoFlag,
                        TimeFrom = item.TimeFrom,
                        TimeTo = item.TimeTo,
                        BidOfferLevelFrom = item.BidOfferLevelFrom,
                        BidOfferLevelTo = item.BidOfferLevelTo,
                        AmendmentFlag = item.AmendmentFlag,
                        StorFlag = item.StorFlag
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
            return models;
        }
    }
}
