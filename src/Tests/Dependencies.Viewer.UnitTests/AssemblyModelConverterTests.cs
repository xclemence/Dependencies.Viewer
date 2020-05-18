using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Dependencies.Viewer.UnitTests.DataProviders;
using Dependencies.Viewer.UnitTests.Extensions;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dependencies.Viewer.UnitTests
{

    [TestClass, ExcludeFromCodeCoverage]
    public class AssemblyModelConverterTests
    {
        [TestMethod]
        public void AssemblyToExchangeModel()
        {
            var baseAssembly = AssemblyModelDataProvider.AssemblyTestV4;

            var (assembly, dependencies) = baseAssembly.ToExchangeModel();

            Assert.That.DeepEqual(AssemblyExchangeDataProvider.AssemblyTestV4, assembly);
            Assert.AreEqual(0, dependencies.Count);
        }

        [TestMethod]
        public void AssemblyToExchangeModelWithReferences()
        {
            var baseAssembly = AssemblyModelDataProvider.AnalyseBase;
            var assemblyTest4 = AssemblyModelDataProvider.AssemblyTestV4;

            baseAssembly.ReferencedAssemblyNames = ImmutableList.Create(assemblyTest4.FullName);

            var testAssembly = baseAssembly.ShadowClone(new Dictionary<string, ReferenceModel>
            {
                [assemblyTest4.FullName] = assemblyTest4.CreateReferenceModel()
            });

            var (_, dependencies) = testAssembly.ToExchangeModel();

            Assert.AreEqual(1, dependencies.Count);
            Assert.AreEqual(assemblyTest4.FullName, dependencies[0].Name);
        }

        [TestMethod]
        public void AssemblyToExchangeModelReferencesTwice()
        {
            var baseAssembly = AssemblyModelDataProvider.AnalyseBase;
            var assemblyTest4 = AssemblyModelDataProvider.AssemblyTestV4;
            var assemblyTest2 = AssemblyModelDataProvider.AssemblyTestV2;

            baseAssembly.ReferencedAssemblyNames = ImmutableList.Create(assemblyTest4.FullName, assemblyTest2.FullName);

            var testAssembly = baseAssembly.ShadowClone(new Dictionary<string, ReferenceModel>
            {
                [assemblyTest4.FullName] = assemblyTest4.CreateReferenceModel(),
                [assemblyTest2.FullName] = assemblyTest4.CreateReferenceModel(assemblyTest2.FullName, assemblyTest2.Version),
            });

            var (_, dependencies) = testAssembly.ToExchangeModel();

            Assert.AreEqual(2, dependencies.Count);
        }
    }
}
