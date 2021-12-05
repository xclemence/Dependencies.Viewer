﻿using System.Collections.Generic;
using System.Linq;
using Dependencies.Analyser.Base;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Models.About;

namespace Dependencies.Viewer.Wpf.Controls.Extensions;

public static class PluginModelExtensions
{
    public static PluginTypeModel PluginTypeModel(this IEnumerable<IAssemblyAnalyserFactory> analyserFactories) =>
        new ("Analysers", analyserFactories.Select(x => new PluginModel(x.Name, x.Version)).ToList());

    public static PluginTypeModel PluginTypeModel(this IEnumerable<IExportAssembly> exportServiceProviders) =>
        new ("Export", exportServiceProviders.Select(x => new PluginModel(x.Name, x.Version)).ToList());

    public static PluginTypeModel PluginTypeModel(this IEnumerable<IImportAssembly> exportServiceProviders) =>
        new ("Import", exportServiceProviders.Select(x => new PluginModel(x.Name, x.Version)).ToList());
}
