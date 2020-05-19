using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Dependencies.Viewer.UnitTests.DataProviders;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dependencies.Viewer.UnitTests
{

    [TestClass, ExcludeFromCodeCoverage]
    public class ReferenceModelExtensionsTests
    {
        [TestMethod]
        public void GetPath()
        {
            var baseAssembly = AssemblyModelDataProvider.AnalyseBase;
            var assemblyTest1 = AssemblyModelDataProvider.AssemblyTest1;
            var assemblyTest2 = AssemblyModelDataProvider.AssemblyTest2;
            var assemblyTest3 = AssemblyModelDataProvider.AssemblyTest3;

            baseAssembly.ReferencedAssemblyNames = ImmutableList.Create(assemblyTest3.FullName);
            assemblyTest3.ReferencedAssemblyNames = ImmutableList.Create(assemblyTest2.FullName);
            assemblyTest2.ReferencedAssemblyNames = ImmutableList.Create(assemblyTest1.FullName);

            var referenceProvider = new Dictionary<string, ReferenceModel>();

            baseAssembly = baseAssembly.ShadowClone(referenceProvider);
            assemblyTest1 = assemblyTest1.ShadowClone(referenceProvider);
            assemblyTest2 = assemblyTest2.ShadowClone(referenceProvider);
            assemblyTest3 = assemblyTest3.ShadowClone(referenceProvider);

            referenceProvider.Add(baseAssembly.FullName, baseAssembly.CreateReferenceModel());
            referenceProvider.Add(assemblyTest1.FullName, assemblyTest1.CreateReferenceModel());
            referenceProvider.Add(assemblyTest2.FullName, assemblyTest2.CreateReferenceModel());
            referenceProvider.Add(assemblyTest3.FullName, assemblyTest3.CreateReferenceModel());


            var result = referenceProvider[assemblyTest1.FullName].GetAssemblyParentPath(baseAssembly);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(assemblyTest2.FullName, result[0].Assembly.FullName);

            Assert.AreEqual(1, result[0].Parents.Count);
            Assert.AreEqual(assemblyTest3.FullName, result[0].Parents[0].Assembly.FullName);

            Assert.AreEqual(1, result[0].Parents[0].Parents.Count);
            Assert.AreEqual(baseAssembly.FullName, result[0].Parents[0].Parents[0].Assembly.FullName);
        }

        [TestMethod]
        public void GetPathFactorizePart()
        {
            var baseAssembly = AssemblyModelDataProvider.AnalyseBase;
            var assemblyTest1 = AssemblyModelDataProvider.AssemblyTest1;
            var assemblyTest2 = AssemblyModelDataProvider.AssemblyTest2;
            var assemblyTest3 = AssemblyModelDataProvider.AssemblyTest3;

            baseAssembly.ReferencedAssemblyNames = ImmutableList.Create(assemblyTest1.FullName, assemblyTest3.FullName);
            assemblyTest1.ReferencedAssemblyNames = ImmutableList.Create(assemblyTest2.FullName);
            assemblyTest3.ReferencedAssemblyNames = ImmutableList.Create(assemblyTest1.FullName);

            var referenceProvider = new Dictionary<string, ReferenceModel>();

            baseAssembly = baseAssembly.ShadowClone(referenceProvider);
            assemblyTest1 = assemblyTest1.ShadowClone(referenceProvider);
            assemblyTest2 = assemblyTest2.ShadowClone(referenceProvider);
            assemblyTest3 = assemblyTest3.ShadowClone(referenceProvider);

            referenceProvider.Add(baseAssembly.FullName, baseAssembly.CreateReferenceModel());
            referenceProvider.Add(assemblyTest1.FullName, assemblyTest1.CreateReferenceModel());
            referenceProvider.Add(assemblyTest2.FullName, assemblyTest2.CreateReferenceModel());
            referenceProvider.Add(assemblyTest3.FullName, assemblyTest3.CreateReferenceModel());


            var result = referenceProvider[assemblyTest2.FullName].GetAssemblyParentPath(baseAssembly);

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(2, result[0].Parents.Count);

        }

        [TestMethod]
        public void GetPathNoLink()
        {
            var baseAssembly = AssemblyModelDataProvider.AnalyseBase;
            var assembltTest1 = AssemblyModelDataProvider.AssemblyTest1;
            var assembltTest2 = AssemblyModelDataProvider.AssemblyTest2;

            baseAssembly.ReferencedAssemblyNames = ImmutableList.Create(assembltTest1.FullName);

            var referenceProvider = new Dictionary<string, ReferenceModel>();

            baseAssembly = baseAssembly.ShadowClone(referenceProvider);
            assembltTest1 = assembltTest1.ShadowClone(referenceProvider);
            assembltTest2 = assembltTest2.ShadowClone(referenceProvider);

            referenceProvider.Add(baseAssembly.FullName, baseAssembly.CreateReferenceModel());
            referenceProvider.Add(assembltTest1.FullName, assembltTest1.CreateReferenceModel());
            referenceProvider.Add(assembltTest2.FullName, assembltTest2.CreateReferenceModel());

            var result = referenceProvider[assembltTest2.FullName].GetAssemblyParentPath(baseAssembly);

            Assert.AreEqual(0, result.Count);
        }


        [TestMethod]
        public void GetPathDUplicateReference()
        {
            var baseAssembly = AssemblyModelDataProvider.AnalyseBase;
            var assembltTest1 = AssemblyModelDataProvider.AssemblyTest1;

            baseAssembly.ReferencedAssemblyNames = ImmutableList.Create(assembltTest1.FullName, assembltTest1.FullName);

            var referenceProvider = new Dictionary<string, ReferenceModel>();

            baseAssembly = baseAssembly.ShadowClone(referenceProvider);
            assembltTest1 = assembltTest1.ShadowClone(referenceProvider);

            referenceProvider.Add(baseAssembly.FullName, baseAssembly.CreateReferenceModel());
            referenceProvider.Add(assembltTest1.FullName, assembltTest1.CreateReferenceModel());

            var result = referenceProvider[assembltTest1.FullName].GetAssemblyParentPath(baseAssembly);

            Assert.AreEqual(1, result.Count);
        }
    }
}
