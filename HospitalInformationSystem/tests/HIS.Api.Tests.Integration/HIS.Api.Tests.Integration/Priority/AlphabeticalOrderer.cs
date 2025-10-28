using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace HIS.Api.Tests.Integration.Priority
{
    public class AlphabeticalOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
        IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var ordered = new List<TTestCase>();

            var priority = new Dictionary<string, int>
            {
                { "CreateDoctorTests", 1 },
                { "SecondTestClass", 2 },
                { "ThirdTestClass", 3 }
            };

            foreach (var testCase in testCases
                .OrderBy(tc =>
                {
                    var className = tc.TestMethod.TestClass.Class.Name;
                    return priority.TryGetValue(className, out var p) ? p : 999;
                }))
            {
                ordered.Add(testCase);
            }

            return ordered;
        }
    }
}
