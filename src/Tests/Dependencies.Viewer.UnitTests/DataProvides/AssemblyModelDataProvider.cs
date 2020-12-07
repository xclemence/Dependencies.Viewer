using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.UnitTests.DataProviders
{
    public static class AssemblyModelDataProvider
    {
        public static AssemblyModel AssemblyTestV4 => new ("AssemblyTest", ImmutableList.Create<string>(), new Dictionary<string, ReferenceModel>())
        {
            Version = "4.0.0.0",
            AssemblyName = "AssemblyTest, Version=4.0.0.0, Culture=neutral, PublicKeyToken=69f1c32f803d307e",
            TargetFramework = ".NETCoreApp,Version=v3.1",
            TargetProcessor = "AnyCpu",
            IsDebug = true,
            IsILOnly = true,
            IsLocalAssembly = true,
            IsNative = true,
            Creator = "xce",
            CreationDate = new DateTime(2020, 2, 2),
            HasEntryPoint = true,
            IsResolved = true,
        };

        public static AssemblyModel AssemblyTestV2 => new ("AssemblyTest", ImmutableList.Create<string>(), new Dictionary<string, ReferenceModel>())
        {
            Version = "2.0.0.0",
            AssemblyName = "AssemblyTest, Version=2.0.0.0, Culture=neutral, PublicKeyToken=69f1c32f803d307e",
            TargetFramework = ".NETCoreApp,Version=v3.1",
            TargetProcessor = "AnyCpu",
            IsDebug = true,
            IsILOnly = true,
            IsLocalAssembly = true,
            IsNative = true,
            Creator = "xce",
            CreationDate = new DateTime(2020, 2, 2),
            HasEntryPoint = true,
            IsResolved = true,
        };

        public static AssemblyModel AnalyseBase() => AnalyseBase(ImmutableList.Create<string>());
        public static AssemblyModel AnalyseBase(IImmutableList<string> referencedAssemblyNames) 
            => new ("Dependencies.Analyser.Base", referencedAssemblyNames, new Dictionary<string, ReferenceModel>())
        {
            Version = "1.0.0.0",
            AssemblyName = "Dependencies.Analyser.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            TargetFramework = ".NETCoreApp,Version=v3.1",
            TargetProcessor = "AnyCpu",
            IsDebug = true,
            IsILOnly = true,
            IsLocalAssembly = true,
            IsNative = true,
            Creator = "xce",
            CreationDate = new DateTime(2020, 2, 3),
            HasEntryPoint = true,
            IsResolved = true,
        };

        public static AssemblyModel AssemblyTest1 => new ("Assembly.Test1", ImmutableList.Create<string>(), new Dictionary<string, ReferenceModel>())
        {
            Version = "4.0.0.0",
            AssemblyName = "Assembly.Test1, Version=4.0.0.0, Culture=neutral, PublicKeyToken=69f1c32f803d307e",
            TargetFramework = ".NETCoreApp,Version=v3.1",
            TargetProcessor = "AnyCpu",
            IsDebug = true,
            IsILOnly = true,
            IsLocalAssembly = true,
            IsNative = true,
            Creator = "xce",
            CreationDate = new DateTime(2020, 2, 2),
            HasEntryPoint = true,
            IsResolved = true,
        };

        public static AssemblyModel AssemblyTest2 => new ("Assembly.Test2", ImmutableList.Create<string>(), new Dictionary<string, ReferenceModel>())
        {
            Version = "4.0.0.0",
            AssemblyName = "Assembly.Test2, Version=4.0.0.0, Culture=neutral, PublicKeyToken=69f1c32f803d307e",
            TargetFramework = ".NETCoreApp,Version=v3.1",
            TargetProcessor = "AnyCpu",
            IsDebug = true,
            IsILOnly = true,
            IsLocalAssembly = true,
            IsNative = true,
            Creator = "xce",
            CreationDate = new DateTime(2020, 2, 2),
            HasEntryPoint = true,
            IsResolved = true,
        };

        public static AssemblyModel AssemblyTest3 => new AssemblyModel("Assembly.Test3", ImmutableList.Create<string>(), new Dictionary<string, ReferenceModel>())
        {
            Version = "4.0.0.0",
            AssemblyName = "Assembly.Test3, Version=4.0.0.0, Culture=neutral, PublicKeyToken=69f1c32f803d307e",
            TargetFramework = ".NETCoreApp,Version=v3.1",
            TargetProcessor = "AnyCpu",
            IsDebug = true,
            IsILOnly = true,
            IsLocalAssembly = true,
            IsNative = true,
            Creator = "xce",
            CreationDate = new DateTime(2020, 2, 2),
            HasEntryPoint = true,
            IsResolved = true,
        };

        public static ReferenceModel CreateReferenceModel(this AssemblyModel assembly, string assemblyFullName = null, string assemblyVersion = null) => 
            new (assemblyFullName ?? assembly.FullName, assembly)
        {
            AssemblyVersion = assemblyVersion ?? assembly.Version
        };
    }
}
