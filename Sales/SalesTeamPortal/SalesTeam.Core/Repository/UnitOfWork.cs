using System;
using SalesTeam.Core.Data;

namespace SalesTeam.Core.Repository
{
    public class UnitOfWork : IDisposable
    {
        private MH_DWEntities context = new MH_DWEntities();
        private bool disposed = false;
        private GenericRepository<vwSalesTeam> _viewSalesTeam;
        private GenericRepository<vwSalesTeamPatient> _viewSalesTeamPatient;        //vwSalesTeamPatient
        private GenericRepository<vwSalesTeamPhysician> _viewSalesTeamPhysician;
        private GenericRepository<vwSalesTeamNote> _viewSalesTeamNote;
        private GenericRepository<vwSalesTeamQueue> _viewSalesTeamQueue;
        private GenericRepository<vwSalesTeamInfo> _viewSalesTeamInfo;
        private GenericRepository<vwPharmacyNote> _viewPharmacyNote;
        private GenericRepository<vwPatientInsurance> _vwPatientInsurance;
        private GenericRepository<vwDocument> _vwDocument;
        private GenericRepository<vwPrescription> _vwPrescription;                  //vwPrescription
        private GenericRepository<vwPhysicianAddress> _vwPhysicianAddress;
        private GenericRepository<SalesTeamNote> _SalesTeamNote;
        private GenericRepository<SalesTeamQueue> _SalesTeamQueue;
        private GenericRepository<vwPhysician> _vwPhysician;
        private GenericRepository<vwPatient> _vwPatient;                            //vwPatient
        private GenericRepository<ReferralFormCategory> _ReferralFormCategory;
        private GenericRepository<ReferralForm> _ReferralForm;
        private GenericRepository<AssociatesSalesTeam> _AssociatesSalesTeam;
        private GenericRepository<spGetPhysicianNameAndPatientcountForChart_Result> _spGetPhysicianNameAndPatientcountForChart_Result;
        private GenericRepository<spGetDrugShortNameAndPatientcountForChart_Result> _spGetDrugShortNameAndPatientcountForChart_Result;
        private GenericRepository<spDrillDownDrugShortNameAndPatientcountForChart_Result> _spDrillDownDrugShortNameAndPatientcountForChart_Result;
        private GenericRepository<spPatientActivityStatusForChart_Result> _spPatientActivityStatusForChart_Result;
        private GenericRepository<DimProcessTime> _DimProcessTime;
        private GenericRepository<Feedback> _Feedback;
        private GenericRepository<FeedbackStatu> _FeedbackStatu;
        private GenericRepository<FeedbackRelated> _FeedbackRelated;
        private GenericRepository<vwFeedback> _vwFeedback;
        private GenericRepository<spHIPAAConsent_Result> _spHIPAAConsent_Result;
        private GenericRepository<vwSalesTeamPhysicianDrugTherapySearch> _vwSalesTeamPhysicianDrugTherapySearch;
        private GenericRepository<PhysicianConsent> _PhysicianConsent;
        private GenericRepository<SalesPortalFaxLog> _FaxLog;

        public GenericRepository<vwSalesTeam> vwSalesTeamsRepository
        {
            get
            {
                return _viewSalesTeam ??
                       (_viewSalesTeam = new GenericRepository<vwSalesTeam>(context));
            }
        }
        public GenericRepository<vwSalesTeamPatient> vwSalesTeamPatientsRepository
        {
            get
            {
                return _viewSalesTeamPatient ??
                       (_viewSalesTeamPatient = new GenericRepository<vwSalesTeamPatient>(context));
            }
        }
        public GenericRepository<vwSalesTeamPhysician> vwSalesTeamPhysiciansRepository
        {
            get
            {
                return _viewSalesTeamPhysician ??
                       (_viewSalesTeamPhysician = new GenericRepository<vwSalesTeamPhysician>(context));
            }
        }
        public GenericRepository<vwSalesTeamNote> vwSalesTeamNotesRepository
        {
            get
            {
                return _viewSalesTeamNote ??
                       (_viewSalesTeamNote = new GenericRepository<vwSalesTeamNote>(context));
            }
        }
        public GenericRepository<vwSalesTeamQueue> vwSalesTeamQueuesRepository
        {
            get
            {
                return _viewSalesTeamQueue ??
                       (_viewSalesTeamQueue = new GenericRepository<vwSalesTeamQueue>(context));
            }
        }
        public GenericRepository<vwSalesTeamInfo> vwSalesTeamInfosRepository
        {
            get
            {
                return _viewSalesTeamInfo ??
                       (_viewSalesTeamInfo = new GenericRepository<vwSalesTeamInfo>(context));
            }
        }
        public GenericRepository<vwPharmacyNote> vwPharmacyNotesRepository
        {
            get
            {
                return _viewPharmacyNote ??
                       (_viewPharmacyNote = new GenericRepository<vwPharmacyNote>(context));
            }
        }
        public GenericRepository<vwPatientInsurance> vwPatientInsurancesRepository
        {
            get
            {
                return _vwPatientInsurance ??
                       (_vwPatientInsurance = new GenericRepository<vwPatientInsurance>(context));
            }
        }
        public GenericRepository<vwDocument> vwDocumentRepository
        {
            get
            {
                return _vwDocument ??
                       (_vwDocument = new GenericRepository<vwDocument>(context));
            }
        }
        public GenericRepository<vwPrescription> vwPrescriptionRepository
        {
            get
            {
                return _vwPrescription ??
                       (_vwPrescription = new GenericRepository<vwPrescription>(context));
            }
        }
        public GenericRepository<vwPhysicianAddress> vwPhysicianAddressRepository
        {
            get
            {
                return _vwPhysicianAddress ??
                       (_vwPhysicianAddress = new GenericRepository<vwPhysicianAddress>(context));
            }
        }
        public GenericRepository<SalesTeamNote> SalesTeamNotesRepository
        {
            get
            {
                return _SalesTeamNote ??
                       (_SalesTeamNote = new GenericRepository<SalesTeamNote>(context));
            }
        }
        public GenericRepository<SalesTeamQueue> SalesTeamQueuesRepository
        {
            get
            {
                return _SalesTeamQueue ??
                       (_SalesTeamQueue = new GenericRepository<SalesTeamQueue>(context));
            }
        }
        public GenericRepository<vwPhysician> vwPhysiciansRepository
        {
            get
            {
                return _vwPhysician ??
                       (_vwPhysician = new GenericRepository<vwPhysician>(context));
            }
        }
        public GenericRepository<vwPatient> vwPatientRepository
        {
            get
            {
                return _vwPatient ??
                       (_vwPatient = new GenericRepository<vwPatient>(context));
            }
        }
        public GenericRepository<ReferralFormCategory> ReferralFormCategoryRepository
        {
            get
            {
                return _ReferralFormCategory ??
                       (_ReferralFormCategory = new GenericRepository<ReferralFormCategory>(context));
            }
        }
        public GenericRepository<ReferralForm> ReferralFormRepository
        {
            get
            {
                return _ReferralForm ??
                       (_ReferralForm = new GenericRepository<ReferralForm>(context));
            }
        }
        public GenericRepository<AssociatesSalesTeam> AssociatesSalesTeamRepository
        {
            get
            {
                return _AssociatesSalesTeam ??
                       (_AssociatesSalesTeam = new GenericRepository<AssociatesSalesTeam>(context));
            }
        }
        public GenericRepository<spGetPhysicianNameAndPatientcountForChart_Result> spGetPhysicianNameAndPatientcountForChart_ResultRepository
        {
            get
            {
                return _spGetPhysicianNameAndPatientcountForChart_Result ??
                       (_spGetPhysicianNameAndPatientcountForChart_Result = new GenericRepository<spGetPhysicianNameAndPatientcountForChart_Result>(context));
            }
        }
        public GenericRepository<spGetDrugShortNameAndPatientcountForChart_Result> spGetDrugShortNameAndPatientcountForChart_ResultRepository
        {
            get
            {
                return _spGetDrugShortNameAndPatientcountForChart_Result ??
                       (_spGetDrugShortNameAndPatientcountForChart_Result = new GenericRepository<spGetDrugShortNameAndPatientcountForChart_Result>(context));
            }
        }
        public GenericRepository<spDrillDownDrugShortNameAndPatientcountForChart_Result> spDrillDownDrugShortNameAndPatientcountForChart_ResultRepository
        {
            get
            {
                return _spDrillDownDrugShortNameAndPatientcountForChart_Result ??
                       (_spDrillDownDrugShortNameAndPatientcountForChart_Result = new GenericRepository<spDrillDownDrugShortNameAndPatientcountForChart_Result>(context));
            }
        }
        public GenericRepository<spPatientActivityStatusForChart_Result> spPatientActivityStatusForChart_ResultRepository
        {
            get
            {
                return _spPatientActivityStatusForChart_Result ??
                       (_spPatientActivityStatusForChart_Result = new GenericRepository<spPatientActivityStatusForChart_Result>(context));
            }
        }
        public GenericRepository<DimProcessTime> DimProcessTimesRepository
        {
            get
            {
                return _DimProcessTime ??
                       (_DimProcessTime = new GenericRepository<DimProcessTime>(context));
            }
        }
        public GenericRepository<Feedback> FeedbacksRepository
        {
            get
            {
                return _Feedback ??
                       (_Feedback = new GenericRepository<Feedback>(context));
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
        public GenericRepository<vwFeedback> vwFeedbackRepository
        {
            get
            {
                return _vwFeedback ??
                       (_vwFeedback = new GenericRepository<vwFeedback>(context));
            }
        }
        public GenericRepository<spHIPAAConsent_Result> spHIPAAConsent_ResultRepository
        {
            get
            {
                return _spHIPAAConsent_Result ??
                       (_spHIPAAConsent_Result = new GenericRepository<spHIPAAConsent_Result>(context));
            }
        }
        public GenericRepository<vwSalesTeamPhysicianDrugTherapySearch> SalesTeamPhysicianDrugTherapySearchRepository
        {
            get
            {
                return _vwSalesTeamPhysicianDrugTherapySearch ??
                       (_vwSalesTeamPhysicianDrugTherapySearch = new GenericRepository<vwSalesTeamPhysicianDrugTherapySearch>(context));
            }
        }

        public GenericRepository<PhysicianConsent> PhysicianConsentRepository
        {
            get
            {
                return _PhysicianConsent ??
                       (_PhysicianConsent= new GenericRepository<PhysicianConsent>(context));
            }
        }

        public GenericRepository<SalesPortalFaxLog> SalesPortalFaxLogRepository
        {
            get
            {
                return _FaxLog ??
                       (_FaxLog = new GenericRepository<SalesPortalFaxLog>(context));
            }
        }













        public void Save()
        {
            context.SaveChanges();
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
