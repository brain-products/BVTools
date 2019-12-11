using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using BrainVision.Lab.FileFormats.Properties;

namespace BrainVision.Lab.FileFormats
{
    public static class Program
    {
        const int ReturnValueOnSuccess = 0;
        const int ReturnValueOnFailure = -1;

        public static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (TryParseCommandLine(args, out string? brainVisionHeaderFilePath, out string? destinationFolderPath, out CustomizationInfo? info, out bool helpRequested, out CommandLineParser.Option? helpTopic, out bool versionRequested))
                {
                    if (helpRequested)
                    {
                        if (helpTopic.HasValue)
                        {
                            WriteHelpToConsole(helpTopic.Value);
                        }
                        else
                        {
                            WriteHelpToConsole();
                        }

                        return ReturnValueOnSuccess;
                    }
                    else if (versionRequested)
                    {
                        WriteProductVersionToConsole();
                        return ReturnValueOnSuccess;
                    }
                    else if (Convert(brainVisionHeaderFilePath, destinationFolderPath, info))
                    {
                        return ReturnValueOnSuccess;
                    }
                    else
                    {
                        return ReturnValueOnFailure;
                    }
                }
            }
            else
            {
                WriteHelpToConsole();
            }

            return ReturnValueOnFailure;
        }

        private static bool TryParseCommandLine(string[] args, [NotNullWhen(true)] out string? brainVisionHeaderFilePath, [NotNullWhen(true)] out string? destinationFolderPath, [NotNullWhen(true)] out CustomizationInfo? info, out bool helpRequested, [NotNullWhen(true)] out CommandLineParser.Option? helpTopic, out bool versionRequested)
        {
            CommandLineParser commandLineParser = new CommandLineParser(args);
            try
            {
                commandLineParser.ParseCommandLine();

                helpRequested = commandLineParser.HelpRequested;
                helpTopic = commandLineParser.HelpTopic;
                versionRequested = commandLineParser.VersionRequested;

                if (helpRequested || versionRequested)
                {
                    // when help or version is requested, the command line is ignored, even if it is correct
                    brainVisionHeaderFilePath = null;
                    destinationFolderPath = null;
                    info = null;

                    return true;
                }
                else
                {
                    commandLineParser.ThrowExceptionIfRequiredParameterMissing();

                    brainVisionHeaderFilePath = commandLineParser.BrainVisionHeaderFilePath;
                    destinationFolderPath = commandLineParser.DestinationFolderPath;
                    info = commandLineParser.CustomizationInfo;

                    return true;
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"{Resources.ConsoleOutputInvalidSyntax} {e.Message}");
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e) // all possible exceptions must be caught, program may not crash
            {
                Console.WriteLine($"{Resources.ConsoleOutputUnexpectedError} {e.Message}");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            helpRequested = false;
            helpTopic = null;
            versionRequested = false;

            brainVisionHeaderFilePath = null;
            destinationFolderPath = null;
            info = null;

            return false;
        }

        private static bool Convert(string brainVisionHeaderFilePath, string destinationFolderPath, CustomizationInfo info)
        {
            try
            {
                BrainVisionToBidsConverter converter = new BrainVisionToBidsConverter(destinationFolderPath);
                converter.Convert(brainVisionHeaderFilePath, info);
                return true;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e) // all possible exceptions must be caught, program may not crash
            {
                Console.WriteLine($"{Resources.ConsoleOutputConversionFailed} {e.Message}");
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private static void WriteHelpToConsole(CommandLineParser.Option option)
        {
            string optionStr = BuildHelpOptionNameColumn(option);
            string helpText = GetHelpOptionText(option).Replace("\\n", Environment.NewLine, StringComparison.Ordinal);
            string optionStrAndHelpText = $"{optionStr}{Environment.NewLine}{helpText}";

            Console.Write(optionStrAndHelpText);
        }

        private static void WriteHelpToConsole()
        {
            BuildOptionInfo(out List<string> requiredOptions, out List<string> optionalOptions, out List<string> generalPurposeOptions);

            string fullPath = Assembly.GetExecutingAssembly().Location;
            string appName = Path.GetFileNameWithoutExtension(fullPath);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Resources.HelpMessageUsage);
            sb.AppendLine($" {appName} {Resources.HelpMessageRequiredParamsInCommandline} [{Resources.HelpMessageOptionalParamsInCommandline}]");
            sb.AppendLine($" {appName} -h");
            sb.AppendLine($" {appName} -h -hdr");
            sb.AppendLine($" {appName} --version");

            sb.AppendLine();
            sb.AppendLine(Resources.HelpMessageGeneralPurposeParams);

            foreach (string generalPurposeOption in generalPurposeOptions)
                sb.AppendLine(generalPurposeOption);

            sb.AppendLine();
            sb.AppendLine(Resources.HelpMessageRequiredParams);

            foreach (string requiredOption in requiredOptions)
                sb.AppendLine(requiredOption);

            sb.AppendLine();
            sb.AppendLine(Resources.HelpMessageOptionalParams);

            foreach (string optionalOption in optionalOptions)
                sb.AppendLine(optionalOption);

            Console.Write(sb);
        }

        private static void BuildOptionInfo(out List<string> requiredOptions, out List<string> optionalOptions, out List<string> generalPurposeOptions)
        {
            generalPurposeOptions = new List<string>();
            requiredOptions = new List<string>();
            optionalOptions = new List<string>();

            CommandLineParser.Option[] options = (CommandLineParser.Option[])Enum.GetValues(typeof(CommandLineParser.Option));
            for (int i = 0; i < options.Length; ++i)
            {
                CommandLineParser.Option option = (CommandLineParser.Option)i;

                string optionStr = BuildHelpOptionNameColumn(option);

                string helpText = GetHelpOptionText(option);
                int newLineIndex = helpText.IndexOf("\\n", StringComparison.Ordinal);
                string firstLineOfHelpText = newLineIndex < 0 ? helpText : helpText[0..newLineIndex];

                string optionStrAndHelpText = $" {optionStr,-32}{firstLineOfHelpText}";

                bool isGeneralPurpose = i < 2;
                
                if (isGeneralPurpose)
                {
                    generalPurposeOptions.Add(optionStrAndHelpText);
                }
                else
                {
                    bool isRequired = i < 4;

                    if (isRequired)
                        requiredOptions.Add(optionStrAndHelpText);
                    else
                        optionalOptions.Add(optionStrAndHelpText);
                }
            }
        }

        private static string BuildHelpOptionNameColumn(CommandLineParser.Option option)
        {
            string? optionShort = CommandLineParser.OptionShortNames[(int)option];
            string? optionLong = CommandLineParser.OptionLongNames[(int)option];
            bool bothPresent = (optionShort != null) && (optionLong != null);

            string? separator = bothPresent ? "|" : null;

            string optionStr = $"{optionShort}{separator}{optionLong}";
            return optionStr;
        }

        private static string GetHelpOptionText(CommandLineParser.Option option)
        {
            return option switch
            {
                CommandLineParser.Option.Help => Resources.HelpMessageHelp,
                CommandLineParser.Option.Version => Resources.HelpMessageVersion,
                CommandLineParser.Option.Src => Resources.HelpMessageSrc,
                CommandLineParser.Option.Dst => Resources.HelpMessageDst,
                CommandLineParser.Option.Subject => Resources.HelpMessageSubject,
                CommandLineParser.Option.Session => Resources.HelpMessageSession,
                CommandLineParser.Option.Task => Resources.HelpMessageTask,
                CommandLineParser.Option.DatasetName => Resources.HelpMessageDataset,
                CommandLineParser.Option.License => Resources.HelpMessageLicense,
                CommandLineParser.Option.Authors => Resources.HelpMessageAuthors,
                CommandLineParser.Option.Manufacturer => Resources.HelpMessageManufacturer,
                CommandLineParser.Option.PowerLineFrequency => Resources.HelpMessagePowerLineFreq,
                CommandLineParser.Option.EEGReference => Resources.HelpMessageEegReference,
                _ => throw new NotImplementedException() //should never happen
            };
        }

        private static void WriteProductVersionToConsole()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string? informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            Console.WriteLine(informationalVersion);
        }
    }
}
