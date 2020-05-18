using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.UnitTests.DataProviders
{
    public static class AssemblyModelDataProvider
    {
        public static AssemblyModel AssemblyTestV4 => new AssemblyModel(new Dictionary<string, ReferenceModel>())
        {
            Name = "AssemblyTest",
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
            ReferencedAssemblyNames = ImmutableList.Create<string>()
        };

        public static AssemblyModel AssemblyTestV2 => new AssemblyModel(new Dictionary<string, ReferenceModel>())
        {
            Name = "AssemblyTest",
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
            ReferencedAssemblyNames = ImmutableList.Create<string>()
        };

        public static AssemblyModel AnalyseBase => new AssemblyModel(new Dictionary<string, ReferenceModel>())
        {
            Name = "Dependencies.Analyser.Base",
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
            ReferencedAssemblyNames = ImmutableList.Create<string>()
        };

        public static ReferenceModel CreateReferenceModel(this AssemblyModel assembly, string assemblyFullName = null, string assemblyVersion = null) => new ReferenceModel
        {
            LoadedAssembly = assembly,
            AssemblyFullName = assemblyFullName ?? assembly.FullName,
            AssemblyVersion = assemblyVersion ?? assembly.Version
        };
    }
}
