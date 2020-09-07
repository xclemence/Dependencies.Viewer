# Dependencies Viewer

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE)
[![Master][github-actions-badge]][github-actions]
[![Quality Gate Status][sonar-badge]][sonar-url]

Dependencies Viewer is a dependencies analyser for Microsoft assemblies. It's supports managed assemblies (.Net Framework and .Net Core), Native Assemblies (C++), and mix assemblies (managed and native linked with dll import or cli/c++).

It allows you to find missing assemblies or mismatch versions between referenced assembly and loaded assembly (binding redirect).

Dependencies Viewer require [**.Net Core 3.1 runtime**](https://dotnet.microsoft.com/download/dotnet-core/3.1).

<img src="doc/images/viewer-dark.png"/>

## How to analyse assembly 
- File-> Open File
- Drag and drop assembly on Dependencies Viewer main window
- Command Line 
```
"Dependencies Viewer.exe" c:\MyAssembly.dll
```

## Plugins

### Linked projects
|        Project                                        |                Build State                                | 
| ----------------------------------------------------- | --------------------------------------------------------- | 
| [**Dependencies Analyser**][analyser-url]             |      [![Build][analyser-badge]][analyser-url]             | 
| [**Dependencies Exchange**][exchange-url]             |      [![Build][exchange-badge]][exchange-url]             | 
| [**Dependencies Graph Services**][graph-service-url]  |      [![Build][graph-service-badge]][graph-service-url]   | 

### Analysers

Dependencies Viewer uses plugins to analyse an assembly. All plugins can be found in [*Dependencies Analyser*][analyser-url] project. You can change analyser from the settings view

Dependencies Viewer supports:
- Mono analyser
- Microsoft analyser

### Import-export 

Dependencies viewer allows import-export of analysis results. All code of these plugins are located in [*Dependencies Exchange*][exchange-url] project.

Now, Dependencies Viewer supports:
- Json file
- Dependencies Graph Service

### Dependencies Graph Service

To use Dependencies Graph Services, you need to configure service URL from setting screen (Import/Export part)

## Themes
You can choose your favorite theme between light and dark from the settings view.

<img src="doc/images/viewer-light.png" width="370"/>  <img src="doc/images/viewer-dark.png" width="370"/>

Light theme is selected by default.

## Build project

This project contains sub-modules. It's mandatory to download them with this repository.

```
git clone --recursive https://github.com/xclemence/Dependencies.Viewer.git
```

After, you can open sln file with Visual Studio 2019 (be careful, you need C++ and CLI/C++ support).

[github-actions]:                  https://github.com/xclemence/Dependencies.Viewer/actions
[github-actions-badge]:            https://github.com/xclemence/Dependencies.Viewer/workflows/Master/badge.svg?branch=master

[sonar-badge]:                     https://sonarcloud.io/api/project_badges/measure?project=xclemence_Dependencies.Viewer&metric=alert_status
[sonar-url]:                       https://sonarcloud.io/dashboard?id=xclemence_Dependencies.Viewer

[graph-service-url]:               https://github.com/xclemence/Dependencies-graph-services
[graph-service-badge]:             https://github.com/xclemence/Dependencies-graph-services/workflows/Build/badge.svg?branch=master

[analyser-badge]:                  https://github.com/xclemence/Dependencies.Viewer/workflows/Ms%20Build/badge.svg
[analyser-url]:                    https://github.com/xclemence/Dependencies.Viewer

[exchange-badge]:                  https://github.com/xclemence/Dependencies.Exchange/workflows/WPF%20.NET%20Core/badge.svg?branch=master
[exchange-url]:                    https://github.com/xclemence/Dependencies.Exchange
