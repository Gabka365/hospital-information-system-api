using System.Diagnostics.Contracts;

namespace HIS.Api
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "/api";

        public static class V1
        {
            private const string VersionBase = $"{ApiBase}/v1";

            public static class Doctors
            {
                private const string Base = $"{VersionBase}/doctors";

                public const string Get = $"{Base}/{{id:guid}}";
                public const string Create = $"{Base}";
                public const string GetAll = $"{Base}";
                public const string Update = $"{Base}/{{id:guid}}";
                public const string Delete = $"{Base}/{{id:guid}}";

                public const string Rate = $"{Base}/{{id:guid}}/ratings";
                public const string DeleteRating = $"{Base}/{{id:guid}}/ratings";
                public const string GetUserRatingsForDoctor = $"{Base}/me";
            }
        }

        public static class V2
        {
            private const string VersionBase = $"{ApiBase}/v2";

            public static class Doctors
            {
                private const string Base = $"{VersionBase}/doctors";

                public const string GetDoctorsPatients = $"{Base}/{{id:guid}}/patients";
                public const string AddPatientForDoctor = $"{Base}/add/patient/{{PatientId:guid}}/doctor/{{DoctorId:guid}}";
                public const string AddPatientForCurrentUser = $"{Base}/add/patient/{{patientId:guid}}";
                public const string DeleteDoctorPatient = $"{Base}/delete/patient/{{PatientId:guid}}/doctor/{{DoctorId:guid}}";
            }
        }



        public static class Patients
        {
            private const string Base = $"{ApiBase}/patients";

            public const string Get = $"{Base}/{{id:guid}}";
            public const string Create = $"{Base}";
            public const string GetAll = $"{Base}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";

            public const string GetPatientsDoctors = $"{Base}/{{id:guid}}/doctors";
            public const string AddDoctorForPatient = $"{Base}/add/doctor/{{DoctorId:guid}}/patient/{{PatientId:guid}}";
            public const string AddDoctorForCurrentUser = $"{Base}/add/doctor/{{doctorId:guid}}";
            public const string DeletePatientDoctor = $"{Base}/delete/doctor/{{DoctorId:guid}}/patient/{{PatientId:guid}}";
        }

        public static class Auth
        {
            private const string Base = $"{ApiBase}/auth";

            public const string Register = $"{Base}/register";
            public const string Login = $"{Base}/login";
        }
    }
}
