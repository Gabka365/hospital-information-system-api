using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Requests.Patients;
using HIS.Contracts.Responses;
using HIS.Contracts.Responses.Doctors;
using HIS.Contracts.Responses.Patients;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface IHisApi
    {
        #region Doctors

        [Get(ApiEndpoints.V1.Doctors.Get)]
        Task<DoctorResponse> GetDoctorAsync(string id);

        [Get(ApiEndpoints.V1.Doctors.GetAll)]
        Task<DoctorsResponse> GetDoctorsAsync(GetAllDoctorsRequest request);

        [Post(ApiEndpoints.V1.Doctors.Create)]
        Task<DoctorResponse> CreateDoctorAsync(CreateDoctorRequest request);

        [Put(ApiEndpoints.V1.Doctors.Update)]
        Task<DoctorResponse> UpdateDoctorAsync(string id, UpdateDoctorRequest request);

        [Delete(ApiEndpoints.V1.Doctors.Delete)]
        Task<bool> DeleteDoctorAsync(string id);

        #endregion


        #region Patients

        [Get(ApiEndpoints.Patients.Get)]
        Task<PatientResponse> GetPaientAsync(string id);

        [Get(ApiEndpoints.Patients.GetAll)]
        Task<PatientsResponse> GetPatientsAsync(GetAllPatientsRequest request);

        [Post(ApiEndpoints.Patients.Create)]
        Task<PatientResponse> CreatePatientAsync(CreatePatientRequest request);

        [Put(ApiEndpoints.Patients.Update)]
        Task<PatientResponse> UpdatePatientAsync(string id, UpdatePatientRequest request);

        [Delete(ApiEndpoints.Patients.Delete)]
        Task<bool> DeletePatientAsync(string id);

        #endregion
    }
}
