using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.Helpers;
using System;

namespace PhysicianPortal.Core.Repository
{
    public class UnitOfWork : IDisposable
    {
        private MH_DWEntities context = new MH_DWEntities();
        private bool disposed = false;

        private GenericRepository<User> _User;
        private GenericRepository<Role> _Role;
        private GenericRepository<UserClaim> _UserClaim;
        private GenericRepository<ErrorLog> _ErrorLog;
        private GenericRepository<AuditTrail> _AuditTrail;
        private GenericRepository<AuditOperationType> _AuditOperationType;
        private GenericRepository<vwPatient> _Patient;
        private GenericRepository<vwPatientInsurance> _PatientInsurance;
        private GenericRepository<vwPharmacyNote> _PharmacyNote;
        private GenericRepository<vwDocument> _Document;
        private GenericRepository<vwPhysician> _Physician;
        private GenericRepository<PhysicianNote> _PhysicianNote;
        private GenericRepository<vwPhysicianNote> _vwPhysicianNote;
        private GenericRepository<Message> _Message;
        private GenericRepository<PhysicianPortalPatient> _PhysicianPortalPatient;
        private GenericRepository<vwMessage> _vwMessage;
        private GenericRepository<vwLastThreadMessage> _vwLastThreadMessage;
        private GenericRepository<vwPrescription> _vwPrescription;
        private GenericRepository<PatientDocument> _dimDoument;
        private GenericRepository<ApplicationSetting> _applicationSetting;
        private GenericRepository<spICD10Diseases_Result> _spICD10Diseases_Result;
        private GenericRepository<Disease> _Disease;
        private GenericRepository<DimDrug> _DimDrug;
        private GenericRepository<PhysicianPortalPrescription> _PhysicianPortalPrescription;
        private GenericRepository<State> _State;
        private GenericRepository<Office> _Office;
        private GenericRepository<User_Office_Relationship> _User_Office_Relationship;
        private GenericRepository<UserPhysicianRelationship> _UserPhysicianRelationship;
        private GenericRepository<vwDocument1> _Document1;
        private GenericRepository<vwRefillFaxReportLog> _vwRefillFaxReportLog;
        private GenericRepository<ReferralForm> _ReferralForm;
        private GenericRepository<ReferralFormCategory> _ReferralFormCategory;
        private GenericRepository<RefFormCardiovascular> _RefFormCardiovascular;
        private GenericRepository<PhysicianPortalPatient_ReferralForm_Relationship> _PhysicianPortalPatient_ReferralForm_Relationship;
        private GenericRepository<RefFormHivAid> _RefFormHivAid;
        private GenericRepository<vwGetPhysicianPortalPatientMRN> _vwGetPhysicianPortalPatientMRN;

        private GenericRepository<vwReferralDocument> _vwReferralDocument;
        private GenericRepository<vwRefillFaxDocument> _vwRefillFaxDocument;
        private GenericRepository<vwWorkflow> _vwWorkflow;
        private GenericRepository<RefFormCysticFibrosi> _RefFormCysticFibrosi;
        private GenericRepository<RefFormGastroenterology> _RefFormGastroenterology;
        private GenericRepository<RefFormHepatology> _RefFormHepatology;
        private GenericRepository<RefFormGrowthHormone> _RefFormGrowthHormone;
        private GenericRepository<RefFormOsteoporosi> _RefFormOsteoporosi;
        private GenericRepository<RefFormPediatricGastoenterology> _RefFormPediatricGastoenterology;
        private GenericRepository<RefFormImmunology> _RefFormImmunology;
        private GenericRepository<RefFormRemicade> _RefFormRemicade;
        private GenericRepository<RefFormRheumatology> _RefFormRheumatology;
        private GenericRepository<RefFormTransplant> _RefFormTransplant;
        private GenericRepository<RefFormNeurology> _RefFormNeurology;
        private GenericRepository<RefFormOncology_1> _RefFormOncology_1;
        private GenericRepository<RefFormDermatology> _RefFormDermatology;
        private GenericRepository<RefFormTransplant1> _RefFormTransplant1;
        private GenericRepository<RefFormOncology> _RefFormOncology;
        private GenericRepository<RefFormOncology_2> _RefFormOncology_2;
        private GenericRepository<RefFormTCA_AIandPI> _RefFormTCA_AIandPI;
        private GenericRepository<RefFormNolaOncology> _RefFormNolaOncology;
        private GenericRepository<RefFormGeneralintake> _RefFormGeneralintake;
        private GenericRepository<RefFormBloodModifyingAgent> _RefFormBloodModifyingAgent;
        private GenericRepository<InsuranceEligibilityInfo> _InsuranceEligibilityInfo;
        private GenericRepository<PokitDok_TradingPartner> _PokitDok_TradingPartner;
        private GenericRepository<Pharmacy> _Pharmacy;
        private GenericRepository<NewRefill> _NewRefill;
        private GenericRepository<RefillFaxReportLog> _RefillFaxReportLog;
        private GenericRepository<vwDocumentsUploadLog> _vwDocumentsUploadLog;
        private GenericRepository<User1> _PatientUser;
        private GenericRepository<RefillStatu> _RefillStatu;
        private GenericRepository<PhysicianPortalPatientChecklistRelationship> _PhysicianPortalPatientChecklistRelationship;
        private GenericRepository<ReferralFormChecklistRelationship> _ReferralFormChecklistRelationship;
        private GenericRepository<spPatientChecklist_Result> _spPatientChecklist_Result;
        private GenericRepository<vwReferralForm> _vwReferralForm;
        private GenericRepository<vwNotificationAlert> _vwNotificationAlert;
        private GenericRepository<PatientQueue> _PatientQueue;
        private GenericRepository<HomePage> _HomePage;
        private GenericRepository<PatientPicture> _PatientPicture;
        private GenericRepository<PhysicianSignature> _PhysicianSignature;
        private GenericRepository<vwAuditTrail> _vwAuditTrail;
        private GenericRepository<vwPhysicianPortalFeedback> _vwPhysicianPortalFeedback;
        private GenericRepository<PhysicianPortalFeedback> _PhysicianPortalFeedback;
        private GenericRepository<FeedbackStatu> _FeedbackStatu;
        private GenericRepository<FeedbackRelated> _FeedbackRelated;



        public GenericRepository<User> UserRepository
        {
            get
            {
                return _User ??
                       (_User = new GenericRepository<User>(context));
            }
        }

        public GenericRepository<User1> PatientUserRepository
        {
            get
            {
                return _PatientUser ??
                       (_PatientUser = new GenericRepository<User1>(context));
            }
        }

        public GenericRepository<Role> RoleRepository
        {
            get
            {
                return _Role ??
                       (_Role = new GenericRepository<Role>(context));
            }
        }

        public GenericRepository<UserClaim> UserClaimRepository
        {
            get
            {
                return _UserClaim ??
                       (_UserClaim = new GenericRepository<UserClaim>(context));
            }
        }

        public GenericRepository<ErrorLog> ErrorLogRepository
        {
            get
            {
                return _ErrorLog ??
                       (_ErrorLog = new GenericRepository<ErrorLog>(context));
            }
        }

        public GenericRepository<AuditTrail> AuditTrailRepository
        {
            get
            {
                return _AuditTrail ??
                       (_AuditTrail = new GenericRepository<AuditTrail>(context));
            }
        }

        public GenericRepository<AuditOperationType> AuditOperationTypeRepository
        {
            get
            {
                return _AuditOperationType ??
                       (_AuditOperationType = new GenericRepository<AuditOperationType>(context));
            }
        }

        public GenericRepository<vwPatient> PatientsRepository
        {
            get
            {
                return _Patient ??
                       (_Patient = new GenericRepository<vwPatient>(context));
            }
        }

        public GenericRepository<vwPhysician> PhysiciansRepository
        {
            get
            {
                return _Physician ??
                       (_Physician = new GenericRepository<vwPhysician>(context));
            }
        }

        public GenericRepository<PhysicianPortalPrescription> PhysicianPortalPrescriptionRepository
        {
            get
            {
                return _PhysicianPortalPrescription ??
                       (_PhysicianPortalPrescription = new GenericRepository<PhysicianPortalPrescription>(context));
            }
        }

        public GenericRepository<vwPatientInsurance> PatientsInsuranceRepository
        {
            get
            {
                return _PatientInsurance ??
                       (_PatientInsurance = new GenericRepository<vwPatientInsurance>(context));
            }
        }

        public GenericRepository<vwPharmacyNote> PharmacyNotesRepository
        {
            get
            {
                return _PharmacyNote ??
                       (_PharmacyNote = new GenericRepository<vwPharmacyNote>(context));
            }
        }

        public GenericRepository<vwDocument> PatientDocumentsRepository
        {
            get
            {
                return _Document ??
                       (_Document = new GenericRepository<vwDocument>(context));
            }
        }

        public GenericRepository<PhysicianNote> NotesRepository
        {
            get
            {
                return _PhysicianNote ??
                       (_PhysicianNote = new GenericRepository<PhysicianNote>(context));
            }
        }

        public GenericRepository<vwPhysicianNote> vwNotesRepository
        {
            get
            {
                return _vwPhysicianNote ??
                       (_vwPhysicianNote = new GenericRepository<vwPhysicianNote>(context));
            }
        }

        public GenericRepository<Message> MessagesRepository
        {
            get
            {
                return _Message ??
                       (_Message = new GenericRepository<Message>(context));
            }
        }

        public GenericRepository<PhysicianPortalPatient> PhysicianPortalPatientRepository
        {
            get
            {
                return _PhysicianPortalPatient ??
                       (_PhysicianPortalPatient = new GenericRepository<PhysicianPortalPatient>(context));
            }
        }

        public GenericRepository<Office> OfficeRepository
        {
            get
            {
                return _Office ??
                       (_Office = new GenericRepository<Office>(context));
            }
        }

        public GenericRepository<vwMessage> vwMessagesRepository
        {
            get
            {
                return _vwMessage ??
                       (_vwMessage = new GenericRepository<vwMessage>(context));
            }
        }

        public GenericRepository<vwLastThreadMessage> vwLastThreadMessageRepository
        {
            get
            {
                return _vwLastThreadMessage ??
                       (_vwLastThreadMessage = new GenericRepository<vwLastThreadMessage>(context));
            }
        }

        public GenericRepository<vwPrescription> PreceptionRepository
        {
            get
            {
                return _vwPrescription ??
                       (_vwPrescription = new GenericRepository<vwPrescription>(context));
            }
        }

        public GenericRepository<PatientDocument> DocumentsRepository
        {
            get
            {
                return _dimDoument ??
                       (_dimDoument = new GenericRepository<PatientDocument>(context));
            }
        }

        public GenericRepository<ApplicationSetting> ApplicationSettingRepository
        {
            get
            {
                return _applicationSetting ??
                       (_applicationSetting = new GenericRepository<ApplicationSetting>(context));
            }
        }

        public GenericRepository<DimDrug> DrugRepository
        {
            get
            {
                return _DimDrug ??
                       (_DimDrug = new GenericRepository<DimDrug>(context));
            }
        }

        public GenericRepository<spICD10Diseases_Result> spICD10Diseases_ResultRepository
        {
            get
            {
                return _spICD10Diseases_Result ??
                       (_spICD10Diseases_Result = new GenericRepository<spICD10Diseases_Result>(context));
            }
        }

        public GenericRepository<Disease> DiseaseRepository
        {
            get
            {
                return _Disease ??
                       (_Disease = new GenericRepository<Disease>(context));
            }
        }

        public GenericRepository<State> StateRepository
        {
            get
            {
                return _State ??
                       (_State = new GenericRepository<State>(context));
            }
        }

        public GenericRepository<User_Office_Relationship> User_Office_RelationshipRepository
        {
            get
            {
                return _User_Office_Relationship ??
                       (_User_Office_Relationship = new GenericRepository<User_Office_Relationship>(context));
            }
        }

        public GenericRepository<UserPhysicianRelationship> UserPhysicianRelationshippRepository
        {
            get
            {
                return _UserPhysicianRelationship ??
                       (_UserPhysicianRelationship = new GenericRepository<UserPhysicianRelationship>(context));
            }
        }

        public GenericRepository<vwDocument1> PharmacyDocumentsRepository
        {
            get
            {
                return _Document1 ??
                       (_Document1 = new GenericRepository<vwDocument1>(context));
            }
        }

        public GenericRepository<vwRefillFaxReportLog> vwRefillFaxReportLogsRepository
        {
            get
            {
                return _vwRefillFaxReportLog ??
                       (_vwRefillFaxReportLog = new GenericRepository<vwRefillFaxReportLog>(context));
            }
        }

        public GenericRepository<ReferralForm> ReferralFormsRepository
        {
            get
            {
                return _ReferralForm ??
                       (_ReferralForm = new GenericRepository<ReferralForm>(context));
            }
        }

        public GenericRepository<ReferralFormCategory> ReferralFormCategoriesRepository
        {
            get
            {
                return _ReferralFormCategory ??
                       (_ReferralFormCategory = new GenericRepository<ReferralFormCategory>(context));
            }
        }

        public GenericRepository<RefFormCardiovascular> RefFormCardiovascularesRepository
        {
            get
            {
                return _RefFormCardiovascular ??
                       (_RefFormCardiovascular = new GenericRepository<RefFormCardiovascular>(context));
            }
        }

        public GenericRepository<PhysicianPortalPatient_ReferralForm_Relationship> PhysicianPortalPatient_ReferralForm_RelationshipsRepository
        {
            get
            {
                return _PhysicianPortalPatient_ReferralForm_Relationship ??
                       (_PhysicianPortalPatient_ReferralForm_Relationship = new GenericRepository<PhysicianPortalPatient_ReferralForm_Relationship>(context));
            }
        }

        public GenericRepository<RefFormHivAid> RefFormHivAidsRepository
        {
            get
            {
                return _RefFormHivAid ??
                       (_RefFormHivAid = new GenericRepository<RefFormHivAid>(context));
            }
        }

        public GenericRepository<vwGetPhysicianPortalPatientMRN> vwGetPhysicianPortalPatientMRNRepository
        {
            get
            {
                return _vwGetPhysicianPortalPatientMRN ??
                       (_vwGetPhysicianPortalPatientMRN = new GenericRepository<vwGetPhysicianPortalPatientMRN>(context));
            }
        }

        public GenericRepository<vwReferralDocument> vwReferralDocumentRepository
        {
            get
            {
                return _vwReferralDocument ??
                       (_vwReferralDocument = new GenericRepository<vwReferralDocument>(context));
            }
        }

        public GenericRepository<vwRefillFaxDocument> vwRefillFaxDocumentRepository
        {
            get
            {
                return _vwRefillFaxDocument ??
                       (_vwRefillFaxDocument = new GenericRepository<vwRefillFaxDocument>(context));
            }
        }


        public GenericRepository<vwWorkflow> WorkflowsRepository
        {
            get
            {
                return _vwWorkflow ??
                       (_vwWorkflow = new GenericRepository<vwWorkflow>(context));
            }
        }

        public GenericRepository<RefFormCysticFibrosi> RefFormCysticFibrosisRepository
        {
            get
            {
                return _RefFormCysticFibrosi ??
                       (_RefFormCysticFibrosi = new GenericRepository<RefFormCysticFibrosi>(context));
            }
        }

        public GenericRepository<RefFormGastroenterology> RefFormGastroenterologyRepository
        {
            get
            {
                return _RefFormGastroenterology ??
                       (_RefFormGastroenterology = new GenericRepository<RefFormGastroenterology>(context));
            }
        }

        public GenericRepository<RefFormHepatology> RefFormHepatologyRepository
        {
            get
            {
                return _RefFormHepatology ??
                       (_RefFormHepatology = new GenericRepository<RefFormHepatology>(context));
            }
        }

        public GenericRepository<RefFormGrowthHormone> RefFormGrowthHormoneRepository
        {
            get
            {
                return _RefFormGrowthHormone ??
                       (_RefFormGrowthHormone = new GenericRepository<RefFormGrowthHormone>(context));
            }
        }

        public GenericRepository<RefFormOsteoporosi> RefFormOsteoporosiRepository
        {
            get
            {
                return _RefFormOsteoporosi ??
                       (_RefFormOsteoporosi = new GenericRepository<RefFormOsteoporosi>(context));
            }
        }

        public GenericRepository<RefFormPediatricGastoenterology> RefFormPediatricGastoenterology
        {
            get
            {
                return _RefFormPediatricGastoenterology ??
                       (_RefFormPediatricGastoenterology = new GenericRepository<RefFormPediatricGastoenterology>(context));
            }
        }

        public GenericRepository<RefFormImmunology> RefFormImmunology
        {
            get
            {
                return _RefFormImmunology ??
                       (_RefFormImmunology = new GenericRepository<RefFormImmunology>(context));
            }
        }

        public GenericRepository<RefFormRemicade> RefFormRemicade
        {
            get
            {
                return _RefFormRemicade ??
                       (_RefFormRemicade = new GenericRepository<RefFormRemicade>(context));
            }
        }

        public GenericRepository<RefFormRheumatology> RefFormRheumatology
        {
            get
            {
                return _RefFormRheumatology ??
                       (_RefFormRheumatology = new GenericRepository<RefFormRheumatology>(context));
            }
        }

        public GenericRepository<RefFormTransplant> RefFormTransplant
        {
            get
            {
                return _RefFormTransplant ??
                       (_RefFormTransplant = new GenericRepository<RefFormTransplant>(context));
            }
        }

        public GenericRepository<RefFormNeurology> RefFormNeurology
        {
            get
            {
                return _RefFormNeurology ??
                       (_RefFormNeurology = new GenericRepository<RefFormNeurology>(context));
            }
        }

        public GenericRepository<RefFormOncology_1> RefFormOncology_1
        {
            get
            {
                return _RefFormOncology_1 ??
                       (_RefFormOncology_1 = new GenericRepository<RefFormOncology_1>(context));
            }
        }

        public GenericRepository<RefFormDermatology> RefFormDermatology
        {
            get
            {
                return _RefFormDermatology ??
                       (_RefFormDermatology = new GenericRepository<RefFormDermatology>(context));
            }
        }

        public GenericRepository<RefFormTransplant1> RefFormTransplant1
        {
            get
            {
                return _RefFormTransplant1 ??
                       (_RefFormTransplant1 = new GenericRepository<RefFormTransplant1>(context));
            }
        }

        public GenericRepository<RefFormOncology> RefFormOncology
        {
            get
            {
                return _RefFormOncology ??
                       (_RefFormOncology = new GenericRepository<RefFormOncology>(context));
            }
        }

        public GenericRepository<RefFormOncology_2> RefFormOncology_2
        {
            get
            {
                return _RefFormOncology_2 ??
                       (_RefFormOncology_2 = new GenericRepository<RefFormOncology_2>(context));
            }
        }

        public GenericRepository<RefFormTCA_AIandPI> RefFormTCA_AIandPIRepository
        {
            get
            {
                return _RefFormTCA_AIandPI ??
                       (_RefFormTCA_AIandPI = new GenericRepository<RefFormTCA_AIandPI>(context));
            }
        }

        public GenericRepository<RefFormNolaOncology> RefFormNolaOncology
        {
            get
            {
                return _RefFormNolaOncology ??
                       (_RefFormNolaOncology = new GenericRepository<RefFormNolaOncology>(context));
            }
        }

        public GenericRepository<RefFormGeneralintake> RefFormGeneralintake
        {
            get
            {
                return _RefFormGeneralintake ??
                       (_RefFormGeneralintake = new GenericRepository<RefFormGeneralintake>(context));
            }
        }

        public GenericRepository<RefFormBloodModifyingAgent> RefFormBloodModifyingAgent
        {
            get
            {
                return _RefFormBloodModifyingAgent ??
                       (_RefFormBloodModifyingAgent = new GenericRepository<RefFormBloodModifyingAgent>(context));
            }
        }

        public GenericRepository<Pharmacy> PharmacyRepository
        {
            get
            {
                return _Pharmacy ??
                       (_Pharmacy = new GenericRepository<Pharmacy>(context));
            }
        }

        public GenericRepository<InsuranceEligibilityInfo> InsuranceEligibilityInfoRepository
        {
            get
            {
                return _InsuranceEligibilityInfo ??
                       (_InsuranceEligibilityInfo = new GenericRepository<InsuranceEligibilityInfo>(context));
            }
        }

        public GenericRepository<PokitDok_TradingPartner> PokitDok_TradingPartnerRepository
        {
            get
            {
                return _PokitDok_TradingPartner ??
                       (_PokitDok_TradingPartner = new GenericRepository<PokitDok_TradingPartner>(context));
            }
        }

        public GenericRepository<NewRefill> NewRefillRepository
        {
            get
            {
                return _NewRefill ??
                       (_NewRefill = new GenericRepository<NewRefill>(context));
            }
        }


        public GenericRepository<RefillFaxReportLog> RefillFaxReportLogRepository
        {
            get
            {
                return _RefillFaxReportLog ??
                       (_RefillFaxReportLog = new GenericRepository<RefillFaxReportLog>(context));
            }
        }

        public GenericRepository<RefillStatu> RefillStatusRepository
        {
            get
            {
                return _RefillStatu ??
                       (_RefillStatu = new GenericRepository<RefillStatu>(context));
            }
        }

        public GenericRepository<PhysicianPortalPatientChecklistRelationship> PhysicianPortalPatientChecklistRelationshipRepository
        {
            get
            {
                return _PhysicianPortalPatientChecklistRelationship ??
                       (_PhysicianPortalPatientChecklistRelationship = new GenericRepository<PhysicianPortalPatientChecklistRelationship>(context));
            }
        }

        public GenericRepository<ReferralFormChecklistRelationship> ReferralFormChecklistRelationshipRepository
        {
            get
            {
                return _ReferralFormChecklistRelationship ??
                       (_ReferralFormChecklistRelationship = new GenericRepository<ReferralFormChecklistRelationship>(context));
            }
        }

        public GenericRepository<spPatientChecklist_Result> spPatientChecklist_ResultRepository
        {
            get
            {
                return _spPatientChecklist_Result ??
                       (_spPatientChecklist_Result = new GenericRepository<spPatientChecklist_Result>(context));
            }
        }

        public GenericRepository<vwReferralForm> vwReferralFormRepository
        {
            get
            {
                return _vwReferralForm ??
                       (_vwReferralForm = new GenericRepository<vwReferralForm>(context));
            }
        }

        public GenericRepository<vwNotificationAlert> vwNotificationAlertRepository
        {
            get
            {
                return _vwNotificationAlert ??
                       (_vwNotificationAlert = new GenericRepository<vwNotificationAlert>(context));
            }
        }

        public GenericRepository<PatientQueue> PatientQueuesRepository
        {
            get
            {
                return _PatientQueue ??
                       (_PatientQueue = new GenericRepository<PatientQueue>(context));
            }
        }

        public GenericRepository<HomePage> HomePagesRepository
        {
            get
            {
                return _HomePage ??
                       (_HomePage = new GenericRepository<HomePage>(context));
            }
        }

        public GenericRepository<PatientPicture> PatientPictureRepository
        {
            get
            {
                return _PatientPicture ??
                       (_PatientPicture = new GenericRepository<PatientPicture>(context));
            }
        }

        public GenericRepository<PhysicianSignature> PhysicianSignatureRepository
        {
            get
            {
                return _PhysicianSignature ??
                       (_PhysicianSignature = new GenericRepository<PhysicianSignature>(context));
            }
        }

        public GenericRepository<vwAuditTrail> vwAuditTrailRepository
        {
            get
            {
                return _vwAuditTrail ??
                       (_vwAuditTrail = new GenericRepository<vwAuditTrail>(context));
            }
        }

        public GenericRepository<vwDocumentsUploadLog> vwDocumentsUploadLogRepository
        {
            get
            {
                return _vwDocumentsUploadLog ??
                       (_vwDocumentsUploadLog = new GenericRepository<vwDocumentsUploadLog>(context));
            }
        }

        public GenericRepository<vwPhysicianPortalFeedback> vwPhysicianPortalFeedback
        {
            get
            {
                return _vwPhysicianPortalFeedback ??
                       (_vwPhysicianPortalFeedback = new GenericRepository<vwPhysicianPortalFeedback>(context));
            }
        }
        public GenericRepository<PhysicianPortalFeedback> PhysicianPortalFeedbackRepository
        {
            get
            {
                return _PhysicianPortalFeedback ??
                       (_PhysicianPortalFeedback = new GenericRepository<PhysicianPortalFeedback>(context));
            }
        }

        public GenericRepository<FeedbackStatu> FeedbackStatusRepository
        {
            get
            {
                return _FeedbackStatu ??
                       (_FeedbackStatu = new GenericRepository<FeedbackStatu>(context));
            }
        }

        public GenericRepository<FeedbackRelated> FeedbackRelatedRepository
        {
            get
            {
                return _FeedbackRelated ??
                       (_FeedbackRelated = new GenericRepository<FeedbackRelated>(context));
            }
        }





        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.InsertLog("Exception thrown in Method Save of UnitOfWork. " + e.Message, e);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
