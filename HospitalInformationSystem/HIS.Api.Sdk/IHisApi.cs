using HIS.Contracts.Responses.Doctors;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Sdk
{
    public interface IHisApi
    {
        [Get(ApiEndpoints.V1.Doctors.Get)]
        Task<DoctorResponse> GetDoctorAsync(string id);
    }
}
