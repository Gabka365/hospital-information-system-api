using HIS.Api.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.PatientTests
{
    [CollectionDefinition("HIS-Api Patients Collection")]
    public class TestPatientCollection : IClassFixture<MockAuthApiFactory>
    {

    }
}
