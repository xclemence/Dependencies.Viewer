using Dependencies.Analyser.Base.Models;
using System;

namespace Dependencies.Viewer.UnitTests.DataProviders
{
    public static class AssemblyInformationDataProvider
    {
        public static AssemblyInformation AssemblyTestV4 => new AssemblyInformation("AssemblyTest", "4.0.0.0", null)
        {
            AssemblyName = "AssemblyTest, Version=4.0.0.0, Culture=neutral, PublicKeyToken=69f1c32f803d307e",
            TargetFramework = ".NETCoreApp,Version=v3.1",
            TargetProcessor = TargetProcessor.AnyCpu,
            IsDebug = true,
            IsILOnly = true,
            IsLocalAssembly = true,
            IsNative = true,
            Creator = "xce",
            CreationDate = new DateTime(2020, 2, 2),
            HasEntryPoint = true,
            IsResolved = true,
        };
    }
}
