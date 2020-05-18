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
    public class ReferenceModelExtensionsTests
    {
        [TestMethod]
        public void AssemblyToExchangeModel()
        {
            var baseAssembly = AssemblyModelDataProvider.AssemblyTestV4;

            var (assembly, dependencies) = baseAssembly.ToExchangeModel();

            Assert.That.DeepEqual(AssemblyExchangeDataProvider.AssemblyTestV4, assembly);
            Assert.AreEqual(0, dependencies.Count);
        }
    }
}
