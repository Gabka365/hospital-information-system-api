using System.Diagnostics.Contracts;

namespace HIS.Api
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "/api";
        public static class Doctors
        {
            private const string Base = $"{ApiBase}/doctors";

            public const string Get = $"{Base}/{{id:guid}}";
            public const string Create = $"{Base}";
            public const string GetAll = $"{Base}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";

            public const string GetDoctorsPatients = $"{Base}/{{id:guid}}/patients";
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
        }

        public static class Auth
        {
            private const string Base = $"{ApiBase}/auth";

            public const string Register = $"{Base}/register";
            public const string Login = $"{Base}/login";
        }
    }
}
