# Introduction

**BV2BIDS** is a command line tool that creates the EEG-BIDS folder hierarchy from BVCD files.

# Getting Started

To build and deploy, all you need is to download and install the [.NET Core SDK 3.0.101](https://dotnet.microsoft.com/download/dotnet-core/3.0).  
To start coding, use your preferred editor. You may want to choose one of [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) editions, including the free Community edition.

## Usage

Supported projects targeting:
- .NET Core App 3.0+
- .NET Standard 2.1+

Supported language versions:
- C# 8.0+  
  Note that all projects use [Nullable Reference Types](https://docs.microsoft.com/dotnet/csharp/nullable-references).
  
## Build & Run

You should be able to build **BV2BIDS** for any of [OS versions supported by .NET Core](https://github.com/dotnet/core/blob/master/release-notes/3.0/3.0-supported-os.md).

1. Build:
   ```
   dotnet build sln/BVTools.sln
   ```  
   or
   ```
   dotnet build src/FileFormats/src/FileFormats.BrainVisionToBidsConverterCLI/FileFormats.BrainVisionToBidsConverterCLI.csproj
   ```
1. Run:
   ```
   dotnet run --project src/FileFormats/src/FileFormats.BrainVisionToBidsConverterCLI/FileFormats.BrainVisionToBidsConverterCLI.csproj
   ```
1. Publish for _Windows x64_:
   ```
   dotnet publish src/FileFormats/src/FileFormats.BrainVisionToBidsConverterCLI/FileFormats.BrainVisionToBidsConverterCLI.csproj -r win-x64 -c "Release" --self-contained=true -p:PublishSingleFile=true -p:ProductId=BrainVisionToBidsConverterCLI
   ```

# Download

If you just want to use the **BV2BIDS** tool without building it, you can download it directly from [Brain Products](https://www.brainproducts.com/downloads.php?kid=40#dlukat_226) web page.  
The tool is digitally signed with Brain Products SHA256 certificate in order to guarantee that the code has not been altered or corrupted.
