namespace Elexon.FA.BusinessValidation.Domain.Seed
{
    /// <summary>
    /// Defines the <see cref="BusinessValidationConstants" />
    /// </summary>
    public static class BusinessValidationConstants
    {
        /// <summary>
        /// Defines the ErrorCheck
        /// </summary>
        public static readonly string ERRORCHECK = "ErrorCheck";

        /// <summary>
        /// Defines the Inbound
        /// </summary>
        public static readonly string INBOUND = "Inbound";  

        /// <summary>
        /// Defines the Processing
        /// </summary>
        public static readonly string PROCESSING = "Processing";

        /// <summary>
        /// Defines the Rejected
        /// </summary>
        public static readonly string REJECTED = "Rejected";

        /// <summary>
        /// Defines the WarningCheck
        /// </summary>
        public static readonly string WARNINGCHECK = "WarningCheck";

        /// <summary>
        /// Defines the Failed
        /// </summary>
        public static readonly string FAILED = "Failed";

        /// <summary>
        /// Defines the Success
        /// </summary>
        public static readonly string SUCCESS = "Success";

        /// <summary>
        /// Defines the Warning
        /// </summary>
        public static readonly string WARNING = "Warning";

        /// <summary>
        /// Defines the CONFIG_LONGDAY
        /// </summary>
        public static readonly string CONFIG_LONGDAY = "2018-12-25";

        /// <summary>
        /// Defines the CONFIG_SHORTDAY
        /// </summary>
        public static readonly string CONFIG_SHORTDAY = "2018-03-23";

        /// <summary>
        /// Defines the CONFIG_DATEFORMAT
        /// </summary>
        public static readonly string CONFIG_DATEFORMAT = "yyyy-MM-dd";

        /// <summary>
        /// Defines the SettlementDuration
        /// </summary>
        public static readonly int SETTLEMENTDURATION = 30;

        /// <summary>
        /// Defines the UpdateOrINS
        /// </summary>
        public static readonly string UPDATEORINS = "UpdateOrINS";

        /// <summary>
        /// Defines the AmmendmentFlagINS
        /// </summary>
        public static readonly string AMMENDMENTFLAGINS = "INS";

        /// <summary>
        /// Defines the AmmendmentFlagORI
        /// </summary>
        public static readonly string AMMENDMENTFLAGORI = "ORI";

        /// <summary>
        /// Defines the AmmendmentFlagUPD
        /// </summary>
        public static readonly string AMMENDMENTFLAGUPD = "UPD";

        /// <summary>
        /// Defines the AmmendmentFlagDEL
        /// </summary>
        public static readonly string AMMENDMENTFLAGDEL = "DEL";

        /// <summary>
        /// Defines the IsTrue
        /// </summary>
        public static readonly string ISTRUE = "TRUE";

        /// <summary>
        /// Defines the IsFalse
        /// </summary>
        public static readonly string ISFALSE = "FALSE";

        /// <summary>
        /// Defines the BoalfFile
        /// </summary>
        public static readonly string BOALFFILE = "BOALF.json";

        /// <summary>
        /// Defines the Boalf
        /// </summary>
        public static readonly string BOALF = "BOALF_";

        /// <summary>
        /// Defines the FileExtension
        /// </summary>
        public static readonly string FILEEXTENSION = ".json";

        /// <summary>
        /// Defines the dateFormate
        /// </summary>
        public static readonly string DATEFORMATE = "yyyyMMddHHmmss";

        /// <summary>
        /// Defines the BMUReferenceTableGet
        /// </summary>
        public static readonly string BMUREFERENCETABLEGET = "/BMUReferenceTableGet?effectiveFrom=";

        /// <summary>
        /// Defines the EffectiveTo
        /// </summary>
        public static readonly string EFFECTIVETO = "&effectiveTo=";

        /// <summary>
        /// Defines the BoalfIndexTableGet
        /// </summary>
        public static readonly string BOALFINDEXTABLEGET = "/BoalfIndexTableGet?bmuName=";

        /// <summary>
        /// Defines the BoAcceptanceNumber
        /// </summary>
        public static readonly string BOACCEPTANCENUMBER = "&boAcceptanceNumber=";

        /// <summary>
        /// Defines the BoAcceptanceTime
        /// </summary>
        public static readonly string BOACCEPTANCETIME = "&boAcceptanceTime=";

        /// <summary>
        /// Defines the DateTimeFormat
        /// </summary>
        public static readonly string DATETIMEFORMAT = "yyyy-MM-dd HH:mm";

        /// <summary>
        /// Defines the Suffix
        /// </summary>
        public static readonly string SUFFIX = "S";

        /// <summary>
        /// Defines the StatusTable
        /// </summary>
        public static readonly string STATUSTABLE = "/StatusTable?entity=";

        /// <summary>
        /// Defines the BoalfIndexTableInsert
        /// </summary>
        public static readonly string BOALFINDEXTABLEINSERT = "/BoalfIndexTableInsert?entity=";

        /// <summary>
        /// Defines the BusinessValidation
        /// </summary>
        public static readonly string BUSINESSVALIDATION = "BusinessValidation";

        /// <summary>
        /// Defines the External
        /// </summary>
        public static readonly string EXTERNAL = "External";

        /// <summary>
        /// Defines the ExternalInterface
        /// </summary>
        public static readonly string EXTERNALINTERFACE = "ExternalInterface";

        /// <summary>
        /// Defines the UnHealthy
        /// </summary>
        public static readonly string UNHEALTHY = "UnHealthy";

        /// <summary>
        /// Defines the LongDay
        /// </summary>
        public static readonly string KEYVAULT_LONGDAY = "elxkvvlongDay";

        /// <summary>
        /// Defines the ShortDay
        /// </summary>
        public static readonly string KEYVAULT_SHORTDAY = "elxkvvshortDay";

        /// <summary>
        /// Defines the SettlementDuration
        /// </summary>
        public static readonly string KEYVAULT_SETTLEMENTDURATION = "elxkvvsettlementDuration";

        /// <summary>
        /// Defines the MinPairID
        /// </summary>
        public static readonly string KEYVAULT_MINPAIRID = "elxkvvminPairID";

        /// <summary>
        /// Defines the MaxPairID
        /// </summary>
        public static readonly string KEYVAULT_MAXPAIRID = "elxkvvmaxPairID";

        /// <summary>
        /// Defines the Url
        /// </summary>
        public static readonly string KEYVAULT_URL = "https:" + "//elxkvldvnest01.vault.azure.net/";

        /// <summary>
        /// Defines the StatusTableURL
        /// </summary>
        public static readonly string KEYVAULT_STATUSTABLEURL = "elxkvvStatusTableURL";

        /// <summary>
        /// Defines the Inbound
        /// </summary>
        public static readonly string KEYVAULT_INBOUND = "elxkvvInbound";

        /// <summary>
        /// Defines the Processing
        /// </summary>
        public static readonly string KEYVAULT_PROCESSING = "elxkvvProcessing";

        /// <summary>
        /// Defines the Rejected
        /// </summary>
        public static readonly string KEYVAULT_REJECTED = "elxkvvRejected";

        /// <summary>
        /// Defines the TableApiUrl
        /// </summary>
        public static readonly string KEYVAULT_TABLEAPIURL = "elxkvvBMUreferenceTableURL";

        /// <summary>
        /// Defines the BusinessValidationSuccessTopic
        /// </summary>
        public static readonly string KEYVAULT_BUSINESSVALIDATIONSUCCESSTOPIC = "elxkvvbusinessvalidationsuccesstopic";

        /// <summary>
        /// Defines the BusinessValidationFailureTopic
        /// </summary>
        public static readonly string KEYVAULT_BUSINESSVALIDATIONFAILURETOPIC = "elxkvvbusinessvalidationfailuretopic";

        /// <summary>
        /// Defines the ReceiveTopic
        /// </summary>
        public static readonly string KEYVAULT_RECEIVETOPIC = "elxkvvgenericfilevalidationtopicpub";

        /// <summary>
        /// Defines the SubscriptionName
        /// </summary>
        public static readonly string KEYVAULT_SUBSCRIPTIONNAME = "elxkvvbusinessvalidationcompletedsubscription";

        /// <summary>
        /// Defines the ServiceBusConnectionString
        /// </summary>
        public static readonly string KEYVAULT_SERVICEBUSCONNECTIONSTRING = "elxkvvServiceBusConnectionString01";

        /// <summary>
        /// Defines the StorageAccount
        /// </summary>
        public static readonly string KEYVAULT_STORAGEACCOUNT = "elxkvvStorageLrsh02Name";

        /// <summary>
        /// Defines the StorageKey
        /// </summary>
        public static readonly string KEYVAULT_STORAGEKEY = "elxkvvStorageLrsh02Key1";

        /// <summary>
        /// Defines the ContainerName
        /// </summary>
        public static readonly string KEYVAULT_CONTAINERNAME = "elxkvvblobcontainername01";

        /// <summary>
        /// Defines the ClinetId
        /// </summary>
        public static readonly string KEYVAULT_CLINETID = "5946723b-d786-4adc-86f8-e421dc9d42e9";

        /// <summary>
        /// Defines the Secrete
        /// </summary>
        public static readonly string KEYVAULT_SECRETE = "%qUFnSo&e)($DL}3c{o_+%z-48FG3{6CVT7O7f|n$0+^g1:o$z!$RTI|_.c42";

        /// <summary>
        /// Defines the ApplicationInsightsKey
        /// </summary>
        public static readonly string KEYVAULT_APPLICATIONINSIGHTSKEY = "elxkvvapplicationInsightsInstrumentationkey01";

        /// <summary>
        /// Defines the ErrorTopicName
        /// </summary>
        public static readonly string KEYVAULT_ERRORTOPICNAME = "errorTopicName";

        public static readonly int LOGGINGLEVEL = 6;

        /// <summary>
        /// Defines the InvalidSettlementDayErrorMessage
        /// </summary>
        public static readonly string MSG_INVALIDSETTLEMENTDAY = "Invalid Settlement Day";

        /// <summary>
        /// Defines the InvalidSettlementPeriodErrorMessage
        /// </summary>
        public static readonly string MSG_INVALIDSETTLEMENTPERIOD = "Invalid Settlement Period";

        /// <summary>
        /// Defines the InvalidBMUErrorMessage
        /// </summary>
        public static readonly string MSG_INVALIDBMU = "Invalid BMU Unit";

        /// <summary>
        /// Defines the TimeFromIsGreaterThanTimeToErrorMessage
        /// </summary>
        public static readonly string MSG_TIMEFROMGREATERTHANTIMETO = "Time From is greater than Time To";

        /// <summary>
        /// Defines the DuplicateWaringMessage
        /// </summary>
        public static readonly string MSG_DUPLICATE = "Duplicate Record";

        /// <summary>
        /// Defines the InValidPairIdWaringMessage
        /// </summary>
        public static readonly string MSG_INVALIDPAIRID = "Invalid Pair Id";

        /// <summary>
        /// Defines the PairIdAreNotConsecutiveWaringMessage
        /// </summary>
        public static readonly string MSG_PAIRIDNOTCONSECUTIVE = "Pair are not consecutive for particular BMU Reference";

        /// <summary>
        /// Defines the PairIdAndLevelDifferentSignWaringMessage
        /// </summary>
        public static readonly string MSG_PAIRIDANDLEVELDIFFERENTSIGN = "PairId, LevelFrom and LevelTo have different sign";

        /// <summary>
        /// Defines the INFO_MSG_BUSINESSVALIDATIONPROCESS_STARTED
        /// </summary>
        public static readonly string MSG_INFO_BUSINESSVALIDATIONPROCESS_STARTED = "BusinessValidation process started...";

        /// <summary>
        /// Defines the ERROR_MSG_INVALIDSETTLEMENTPERIOD
        /// </summary>
        public static readonly string MSG_ERROR_INVALIDSETTLEMENTPERIOD = "Settlement Period not in range of Associated Settlement Day";

        /// <summary>
        /// Defines the ERROR_MSG_NODATAFOUND
        /// </summary>
        public static readonly string MSG_ERROR_NODATAFOUND = "No Data Found in the file";

        /// <summary>
        /// Defines the WARNING_MSG_DUPLICATEDATA
        /// </summary>
        public static readonly string MSG_WARNING_DUPLICATEDATA = "Duplicate Settlement Period Data";

        /// <summary>
        /// Defines the IsFileContainsOnlyOneSettlementDayErrorMessage
        /// </summary>
        public static readonly string MSG_FILECONTAINSMULTIPLE_SETTLEMENTDAY = "File contains more than one settlement Day";

        /// <summary>
        /// Defines the IsFileContainsOnlyOneSettlementPeriodErrorMessage
        /// </summary>
        public static readonly string MSG_FILECONTAINSMULTIPLE_SETTLEMENTPERIOD = "File contains more than one settlement Period";

        /// <summary>
        /// Defines the IsFileAlreadyExistsOrNotErrorMessage
        /// </summary>
        public static readonly string MSG_FILEALREADYEXISTSORNOT = "FPN file is already exists for the particular settlement day and settlement period";

        /// <summary>
        /// Defines the IsToTimeGreaterThanFromTime
        /// </summary>
        public static readonly string MSG_TOTIMEGREATERTHANFROMTIME = "Time From is greater than Time To";

        /// <summary>
        /// Defines the IsStartTimeThereForSameBmUnitRecord
        /// </summary>
        public static readonly string MSG_STARTTIMETHEREFORSAMEBMUNITRECORD = "Start Time is not there for a particular BMUnit";

        /// <summary>
        /// Defines the IsEndTimeThereForSameBmUnitRecord
        /// </summary>
        public static readonly string MSG_SENDTIMETHEREFORSAMEBMUNITRECORD = "To Time is not there for a particular BMUnit";

        /// <summary>
        /// Defines the IsTimeFromOfPreviousRecordNotEqualToTimetoOfNextRecord
        /// </summary>
        public static readonly string MSG_TIMEFROMPREVIOUSRECORDNOTEQUALTOTIMETONEXTRECORD = "From Time and To Time should maintain continuity across the settlement period for same BMU unit";

        /// <summary>
        /// Defines the IsTimeFromOfAndTimetoOfOneRecordOverlapsWithNextRecord
        /// </summary>
        public static readonly string MSG_TIMEFROMANDTIMETOOFRECORDOVERLAPSWITHNEXTRECORD = "From Time and To Time of one record should not overlap with next record of same BMUnit";

        /// <summary>
        /// Defines the MessageIsOnWait
        /// </summary>
        public static readonly string MSG_ISONWAIT = "Message is onWait state";

        /// <summary>
        /// Defines the BusinessValidationStarted
        /// </summary>
        public static readonly string MSG_BUSINESSVALIDATIONSTARTED = "BusinessValidation process started...";

        /// <summary>
        /// Defines the BODUploadedToProcessing
        /// </summary>
        public static readonly string MSG_BODUPLOADEDTOPROCESSING = "BOD File updloaded to processing folder...";

        /// <summary>
        /// Defines the BODRecordUploadedToRejected
        /// </summary>
        public static readonly string MSG_BODRECORDUPLOADEDTOREJECTED = "BOD File with invalid record updloaded to rejected folder...";

        /// <summary>
        /// Defines the BODFileUploadedToRejected
        /// </summary>
        public static readonly string MSG_BODFILEUPLOADEDTOREJECTED = "BOD File updloaded to rejected folder...";

        /// <summary>
        /// Defines the FPNUploadedToProcessing
        /// </summary>
        public static readonly string MSG_FPNUPLOADEDTOPROCESSING = "FPN File updloaded to processing folder...";

        /// <summary>
        /// Defines the FPNRecordUploadedToRejected
        /// </summary>
        public static readonly string MSG_FPNRECORDUPLOADEDTOREJECTED = "FPN File with invalid record updloaded to rejected folder...";

        /// <summary>
        /// Defines the FPNFileUploadedToRejected
        /// </summary>
        public static readonly string MSG_FPNFILEUPLOADEDTOREJECTED = "FPN File updloaded to rejected folder...";

        /// <summary>
        /// Defines the AmendmentFlagIsNotORIOrINSOrDELOrUPD
        /// </summary>
        public static readonly string BOALF_MSG_AMENDMENTFLAGNOTHAVEORI_OR_INS_OR_DEL_OR_UPD = "Amendment flag is not having INS OR ORI OR UPD OR DEL";

        /// <summary>
        /// Defines the DifferentDeemedBidOfferFlagorSoFlagorAmmendmentFlagorStorFlag
        /// </summary>
        public static readonly string BOALF_MSG_DIFFERENTDEEMEDBIDOFFERFLAG_OR_SOFLAG_OR_AMMENDMENTFLAG_OR_STORFLAG =
            "Records with same BMU unit Id,BO Acceptance Number and BO Acceptance Time have different Deemed Bid Offer flag or SO Flag or Ammendment Flag or Stor Flag.";

        /// <summary>
        /// Defines the DifferentTimeToOfPreviousRecordAndTimeFromOfNextRecord
        /// </summary>
        public static readonly string BOALF_MSG_DIFFERENTTIMETOOFPREVIOUSRECORDANDTIMEFROMOFNEXTRECORD = "Time to of previous record not equal to the time from of next record(continued acceptance)";

        /// <summary>
        /// Defines the IsFromTimeAndToTimehaveOnlyFourHourDifference
        /// </summary>
        public static readonly string BOALF_MSG_FROMTIMEANDTOTIMEHAVEONLYFOURHOURDIFFERENCE = "Difference between timefrom and time to is greater than four hours";

        /// <summary>
        /// Defines the DifferentAcceptanceTime
        /// </summary>
        public static readonly string BOALF_MSG_DIFFERENTACCEPTANCETIME = "The records of the same BM Unit, BO Acceptance Number should have same BO Acceptance Time";

        /// <summary>
        /// Defines the AmmendmentFlagORIOrINS
        /// </summary>
        public static readonly string BOALF_MSG_AMMENDMENTFLAG_ORI_OR_INS = "For This BM Unit,BO Acceptance Number,BO Acceptance Time with Ammendment Flag ORI or INS there is already record exists.";

        /// <summary>
        /// Defines the AmmendmentFlagUPDOrDEL
        /// </summary>
        public static readonly string BOALF_MSG_AMMENDMENTFLAG_UPD_OR_DEL = "For This BM Unit,BO Acceptance Number,BO Acceptance Time with Ammendment Flag UPD or DEL there is NO record exists.";

        /// <summary>
        /// Defines the DeemedBidOfferFlagorSoFlagorStorFlagShouldHaveOnlyTrueOrFalse
        /// </summary>
        public static readonly string BOALF_MSG_DEEMEDBIDOFFERFLAG_OR_SOFLAG_OR_STORFLAGSHOULDBETRUEORFALSE =
            "Deemed Bid Offer flag or SO flag or Ammend flag or Stor Flag shoul have only True or False values";

        /// <summary>
        /// Defines the SuccessMessage
        /// </summary>
        public static readonly string BOALF_MSG_SUCCESSMESSAGE = "Boalf File updloaded to processing folder...";

        /// <summary>
        /// Defines the FailureMessage
        /// </summary>
        public static readonly string BOALF_MSG_FAILUREMESSAGE = "Boalf File with invalid record updloaded to rejected folder...";

        /// <summary>
        /// Defines the Bod
        /// </summary>
        public static readonly string FLOWS_BOD = "BOD";

        /// <summary>
        /// Defines the Boalf
        /// </summary>
        public static readonly string FLOWS_BOALF = "BOALF";

        /// <summary>
        /// Defines the Disbsad
        /// </summary>
        public static readonly string FLOWS_DISBSAD = "DISBSAD";

        /// <summary>
        /// Defines the Netbsad
        /// </summary>
        public static readonly string FLOWS_NETBSAD = "NETBSAD";

        /// <summary>
        /// Defines the Fpn
        /// </summary>
        public static readonly string FLOWS_FPN = "FPN";

        /// <summary>
        /// Defines the INFO_MSG_FILEUPLOADEDTOREJECTEDFOLDER
        /// </summary>
        public static readonly string NETBSAD_INFO_MSG_FILEUPLOADEDTOREJECTEDFOLDER = "NETBSAD File updloaded to rejected folder...";

        /// <summary>
        /// Defines the INFO_MSG_FILEUPLOADEDTOPROCESSINGFOLDER
        /// </summary>
        public static readonly string NETBSAD_INFO_MSG_FILEUPLOADEDTOPROCESSINGFOLDER = "NETBSAD File updloaded to processing folder...";

        /// <summary>
        /// Defines the INFO_MSG_INVALIDRECORDUPLOADEDTOREJECTEDFOLDER
        /// </summary>
        public static readonly string NETBSAD_INFO_MSG_INVALIDRECORDUPLOADEDTOREJECTEDFOLDER = "NETBSAD File with invalid record updloaded to rejected folder...";

        /// <summary>
        /// Defines the INFO_MSG_FILEUPLOADEDTOREJECTEDFOLDER
        /// </summary>
        public static readonly string DISBSAD_INFO_MSG_FILEUPLOADEDTOREJECTEDFOLDER = "DISBSAD File updloaded to rejected folder...";

        /// <summary>
        /// Defines the INFO_MSG_FILEUPLOADEDTOPROCESSINGFOLDER
        /// </summary>
        public static readonly string DISBSAD_INFO_MSG_FILEUPLOADEDTOPROCESSINGFOLDER = "DISBSAD File updloaded to processing folder...";

        /// <summary>
        /// Defines the INFO_MSG_INVALIDRECORDUPLOADEDTOREJECTEDFOLDER
        /// </summary>
        public static readonly string DISBSAD_INFO_MSG_INVALIDRECORDUPLOADEDTOREJECTEDFOLDER = "DISBSAD File with invalid record updloaded to rejected folder...";

        /// <summary>
        /// Defines the WARNING_MSG_INVALIDSOFANDSTORFLAG
        /// </summary>
        public static readonly string DISBSAD_WARNING_MSG_INVALIDSOFANDSTORFLAG = "Invalid SOF or STOR flag";

        /// <summary>
        /// Defines the BoalfSuccessMessage
        /// </summary>
        public static readonly string LOGMESSAGES_BOALFSUCCESSMESSAGE = "BOALF File updloaded to processing folder...";

        /// <summary>
        /// Defines the BoalfFailureMessage
        /// </summary>
        public static readonly string LOGMESSAGES_BOALFFAILUREMESSAGE = "BOALF File with invalid record updloaded to rejected folder...";

        public static readonly int Range_48 = 48;

        public static readonly int Range_46 = 46;

        public static readonly int Range_50 = 50;

        public static readonly int BOALF_FOURHOURDIFFERENCE = 4;
        public static readonly int SETTLEMENTPERIOD_CALCULATION_RANGE_60 = 60;
        public static readonly int SETTLEMENTPERIOD_CALCULATION_RANGE_30 = 30;

        public static readonly int Two = 2;
        public static readonly int Three = 3;

        public static readonly int Six = 6;
        public static readonly int Seven = 7;
        public static readonly int SettlementPeriods = 48;
        /// <summary>elxkvvStorageLrsh02Key1
        /// Defines the KeyVaultUri
        /// </summary>
        public static readonly string KeyVaultUri = "KeyVaultUri";

        /// <summary>
        /// Defines the KeyVaultClientId
        /// </summary>
        public static readonly string KeyVaultClientId = "ClientId";

        /// <summary>
        /// Defines the KeyVaultClientSecret
        /// </summary>
        public static readonly string KeyVaultClientSecret = "Secret";

        public static readonly string ErrorTopicNameKeyName = "elxsbglobalerrortopic";


    }
}
