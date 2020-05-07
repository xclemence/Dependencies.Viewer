using Dependencies.Exchange.Base.Models;
using Dependencies.Viewer.UnitTests.DataProviders;
using Dependencies.Viewer.UnitTests.Extensions;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dependencies.Viewer.UnitTests
{

    [TestClass, ExcludeFromCodeCoverage]
    public class AssemblyExchangeConverterTests
    {
        [TestMethod]
        public void ExchangeToInformation()
        {
            var assembly = AssemblyExchangeDataProvider.AssemblyTestV4;

            var dependencies = Array.Empty<AssemblyExchange>();

            var result = assembly.ToInformationModel(dependencies);

            Assert.That.DeepEqual(AssemblyInformationDataProvider.AssemblyTestV4, result);
        }

        [TestMethod]
        public void ExchangeToInformationWithLink()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase;

            var dependencies = new[] {
                AssemblyExchangeDataProvider.AssemblyTestV4,
            };

            assembly.AssembliesReferenced.AddRange(dependencies.Select(x => x.Name));

            var result = assembly.ToInformationModel(dependencies);

            Assert.AreEqual(1, result.Links.Count);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4.Name, result.Links[0].LinkFullName);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4.Version, result.Links[0].LinkVersion);
        }

        [TestMethod]
        public void ExchangeToInformationMismatchAndTwoVesions()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase;

            var dependencies = new[] {
                AssemblyExchangeDataProvider.AssemblyTestV4,
                AssemblyExchangeDataProvider.AssemblyTestV2,
            };

            assembly.AssembliesReferenced.Add(AssemblyExchangeDataProvider.AssemblyTestV2.Name);

            var result = assembly.ToInformationModel(dependencies);

            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2.Name, result.Links[0].LinkFullName);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2.Version, result.Links[0].LinkVersion);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4.Version, result.Links[0].Assembly.LoadedVersion);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4.Name, result.Links[0].Assembly.FullName);
        }

        [TestMethod]
        public void ExchangeToInformationNoDependency()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase;

            var partialAssembly = AssemblyExchangeDataProvider.AssemblyTestV2;
            partialAssembly.IsPartial = true;

            var dependencies = new [] {
                partialAssembly
            };

            assembly.AssembliesReferenced.Add(AssemblyExchangeDataProvider.AssemblyTestV2.Name);

            var result = assembly.ToInformationModel(dependencies);

            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2.Name, result.Links[0].LinkFullName);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2.Version, result.Links[0].LinkVersion);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2.Version, result.Links[0].Assembly.LoadedVersion);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2.Name, result.Links[0].Assembly.FullName);
            Assert.AreEqual(false, result.Links[0].Assembly.IsResolved);
        }

        [TestMethod]
        public void ExchangeToInformationMissingVersion()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase;

            var partialAssembly = AssemblyExchangeDataProvider.AssemblyTestV2;
            partialAssembly.IsPartial = true;

            var dependencies = new[] {
                AssemblyExchangeDataProvider.AssemblyTestV4,
            };

            assembly.AssembliesReferenced.Add(AssemblyExchangeDataProvider.AssemblyTestV2.Name);

            var result = assembly.ToInformationModel(dependencies);
        }

        [TestMethod]
        public void ExchangeToInformationTwoSameVersion()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase;

            var dependencies = new[] {
                AssemblyExchangeDataProvider.AssemblyTestV4,
                AssemblyExchangeDataProvider.AssemblyTestV4,
            };

            assembly.AssembliesReferenced.AddRange(dependencies.Select(x => x.Name));

            Assert.ThrowsException<ArgumentException>(() => assembly.ToInformationModel(dependencies));
        }
    }
}
