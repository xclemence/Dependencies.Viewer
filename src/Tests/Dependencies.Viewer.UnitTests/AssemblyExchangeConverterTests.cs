using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dependencies.Exchange.Base.Models;
using Dependencies.Viewer.UnitTests.DataProviders;
using Dependencies.Viewer.UnitTests.Extensions;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dependencies.Viewer.UnitTests
{

    [TestClass, ExcludeFromCodeCoverage]
    public class AssemblyExchangeConverterTests
    {
        [TestMethod]
        public void ExchangeToAssemblyModel()
        {
            var assembly = AssemblyExchangeDataProvider.AssemblyTestV4();

            var dependencies = Array.Empty<AssemblyExchange>();

            var result = assembly.ToAssemblyModel(dependencies);

            Assert.That.DeepEqual(AssemblyModelDataProvider.AssemblyTestV4, result);
        }

        [TestMethod]
        public void ExchangeToAssemblyWithLink()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase();

            var dependencies = new[] {
                AssemblyExchangeDataProvider.AssemblyTestV4(),
            };

            assembly.AssembliesReferenced.AddRange(dependencies.Select(x => x.Name));

            var result = assembly.ToAssemblyModel(dependencies);

            Assert.AreEqual(1, result.ReferencedAssemblyNames.Count);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4().Name, result.References[0].AssemblyFullName);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4().Version, result.References[0].AssemblyVersion);
        }

        [TestMethod]
        public void ExchangeToAssemblyismatchAndTwoVesions()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase();

            var dependencies = new[] {
                AssemblyExchangeDataProvider.AssemblyTestV4(),
                AssemblyExchangeDataProvider.AssemblyTestV2(),
            };

            assembly.AssembliesReferenced.Add(AssemblyExchangeDataProvider.AssemblyTestV2().Name);

            var result = assembly.ToAssemblyModel(dependencies);

            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Name, result.References[0].AssemblyFullName);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Version, result.References[0].AssemblyVersion);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4().Version, result.References[0].LoadedAssembly.Version);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4().Name, result.References[0].LoadedAssembly.FullName);
        }

        [TestMethod]
        public void ExchangeToAssemblyNoDependency()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase();

            var partialAssembly = AssemblyExchangeDataProvider.AssemblyTestV2(true);

            var dependencies = new[] {
                partialAssembly
            };

            assembly.AssembliesReferenced.Add(AssemblyExchangeDataProvider.AssemblyTestV2().Name);

            var result = assembly.ToAssemblyModel(dependencies);

            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Name, result.References[0].AssemblyFullName);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Version, result.References[0].AssemblyVersion);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Version, result.References[0].LoadedAssembly.Version);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Name, result.References[0].LoadedAssembly.FullName);
            Assert.AreEqual(false, result.References[0].LoadedAssembly.IsResolved);
        }

        [TestMethod]
        public void ExchangeToInformationMissingVersion()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase();

            var dependencies = new[] {
                AssemblyExchangeDataProvider.AssemblyTestV4(),
            };

            assembly.AssembliesReferenced.Add(AssemblyExchangeDataProvider.AssemblyTestV2().Name);

            var result = assembly.ToAssemblyModel(dependencies);

            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Name, result.References[0].AssemblyFullName);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Version, result.References[0].AssemblyVersion);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4().Version, result.References[0].LoadedAssembly.Version);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV4().Name, result.References[0].LoadedAssembly.FullName);
            Assert.AreEqual(true, result.References[0].LoadedAssembly.IsResolved);
        }

        [TestMethod]
        public void ExchangeToInformationMissingReference()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase();

            var dependencies = Array.Empty<AssemblyExchange>();

            assembly.AssembliesReferenced.Add(AssemblyExchangeDataProvider.AssemblyTestV2().Name);

            var result = assembly.ToAssemblyModel(dependencies);

            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Name, result.References[0].AssemblyFullName);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Version, result.References[0].AssemblyVersion);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Version, result.References[0].LoadedAssembly.Version);
            Assert.AreEqual(AssemblyExchangeDataProvider.AssemblyTestV2().Name, result.References[0].LoadedAssembly.FullName);
            Assert.AreEqual(false, result.References[0].LoadedAssembly.IsResolved);
        }

        [TestMethod]
        public void ExchangeToAssemblyTwoSameVersion()
        {
            var assembly = AssemblyExchangeDataProvider.AnalyseBase();

            var dependencies = new[] {
                AssemblyExchangeDataProvider.AssemblyTestV4(),
                AssemblyExchangeDataProvider.AssemblyTestV4(),
            };

            assembly.AssembliesReferenced.AddRange(dependencies.Select(x => x.Name));

            Assert.ThrowsException<ArgumentException>(() => assembly.ToAssemblyModel(dependencies));
        }
    }
}
