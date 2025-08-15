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

            public const string Rate = $"{Base}/{{id:guid}}/ratings";
            public const string DeleteRating = $"{Base}/{{id:guid}}/ratings";
        }

        public static class Ratings
        {
            private const string Base = $"{ApiBase}/ratings";

            public const string GetUserRatings = $"{Base}/me";

            public const string DeleteRatings = $"{Base}/{{id:guid}}/delete";
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
    }
}
