using Dependencies.Exchange.Base.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Dependencies.Viewer.UnitTests.DataProviders
{
    [ExcludeFromCodeCoverage]
    public static class AssemblyExchangeDataProvider
    {
        public static AssemblyExchange AssemblyTestV4 => new AssemblyExchange
        {
            Name = "AssemblyTest, Version=4.0.0.0, Culture=neutral, PublicKeyToken=69f1c32f803d307e",
            ShortName = "AssemblyTest",
            Version = "4.0.0.0",
            IsDebug = true,
            IsILOnly = true,
            IsLocal = true,
            IsNative = true,
            HasEntryPoint = true,
            IsPartial = false,
            Creator = "xce",
            CreationDate = new DateTime(2020, 2, 2),
            TargetFramework = ".NETCoreApp,Version=v3.1",
            TargetProcessor = "AnyCpu",
        };

        public static AssemblyExchange AssemblyTestV2 => new AssemblyExchange
        {
            Name = "AssemblyTest, Version=2.0.0.0, Culture=neutral, PublicKeyToken=69f1c32f803d307e",
            ShortName = "AssemblyTest",
            Version = "2.0.0.0",
            IsDebug = true,
            IsILOnly = true,
            IsLocal = true,
            IsNative = false,
            HasEntryPoint = true,
            IsPartial = false,
            Creator = "xce",
            CreationDate = DateTime.Now,
            TargetFramework = ".NETCoreApp,Version=v3.1",
            TargetProcessor = "AnyCpu",
        };

        public static AssemblyExchange AnalyseBase => new AssemblyExchange
        {
            Name = "Dependencies.Analyser.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            ShortName = "Dependencies.Analyser.Base",
            Version = "1.0.0.0",
            IsDebug = true,
            IsILOnly = true,
            IsLocal = true,
            IsNative = false,
            HasEntryPoint = true,
            IsPartial = false,
            Creator = "xce",
            CreationDate = DateTime.Now,
            TargetFramework = ".NETStandard,Version=v2.0",
            TargetProcessor = "AnyCpu",
        };
    }
}
