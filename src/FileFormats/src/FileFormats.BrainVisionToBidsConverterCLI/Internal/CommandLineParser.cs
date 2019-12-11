using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using BrainVision.Lab.FileFormats.Properties;

namespace BrainVision.Lab.FileFormats
{
    internal class CommandLineParser
    {
        // using Apache naming convention for command line arguments. MS dotnet CLI uses this standard as well.
        // see: https://commons.apache.org/proper/commons-cli/

        private readonly Option[] _parameterlessOptions = new Option[] { Option.Help, Option.Version }; // options that may be parameterless

        private readonly string[] _args;
        public CommandLineParser(string[] args)
            => _args = args;

        public bool HelpRequested { get; private set; } = false;
        public Option? HelpTopic { get; private set; }
        public bool VersionRequested { get; private set; } = false;

        public string? BrainVisionHeaderFilePath { get; private set; }
        public string? DestinationFolderPath { get; private set; }
        public CustomizationInfo CustomizationInfo { get; } = new CustomizationInfo();

        public enum Option
        {
            // general purpose options
            Help,
            Version,
            // required options (must be provided in command-line)
            Src,
            Dst,
            // optional options (may be provided in command-line, otherwise default values will be applied)
            Subject,
            Session,
            Task,
            DatasetName,
            License,
            Authors,
            Manufacturer,
            PowerLineFrequency,
            EEGReference,
        }

        public static string?[] OptionLongNames { get; } = new string?[]
        {
            "--help",
            "--version",
            "--bv-header-file",
            "--bids-destination-folder",
            "--subject",
            "--session",
            "--task",
            "--paradigm",
            "--license",
            "--authors",
            "--manufacturer",
            "--power-line-frequency",
            "--EEG-reference"
        };

        public static string?[] OptionShortNames { get; } = new string?[]
        {
            "-h",
            "-v",
            "-hdr",
            "-dst",
            "-sub",
            "-ses",
            "-tsk",
            "-par",
            "-lic",
            "-aut",
            "-mnf",
            "-plf",
            "-ref"
        };

        public void ParseCommandLine()
        {
            Debug.Assert(OptionLongNames.Length == Enum.GetValues(typeof(Option)).Length);
            Debug.Assert(OptionShortNames.Length == Enum.GetValues(typeof(Option)).Length);

            // checking whether duplicated strings do not occur
            Debug.Assert(OptionLongNames.Length == OptionLongNames.Distinct().Count(), "Some long options are duplicated");
            Debug.Assert(OptionShortNames.Length == OptionShortNames.Distinct().Count(), "Some short options are duplicated");

            List<Option> alreadyParsedOptions = new List<Option>();

            for (int i = 0; i < _args.Length; i++)
            {
                string arg = _args[i];

                Option option = ParseOptionArg(arg);

                if (alreadyParsedOptions.Contains(option))
                {
                    string text = $"{Resources.ExceptionTextParamDuplicated} {arg}";
                    throw new InvalidOperationException(text);
                }

                i += ParseOptionValues(i, option);

                alreadyParsedOptions.Add(option);
            }
        }

        private static Option ParseOptionArg(string arg)
        {
            int optionIndex = Array.FindIndex(OptionShortNames, p => p == arg);
            if (optionIndex < 0)
            {
                optionIndex = Array.FindIndex(OptionLongNames, p => p == arg);

                if (optionIndex < 0)
                {
                    string text = $"{Resources.ExceptionTextParamUnrecognized} {arg}.";
                    throw new InvalidOperationException(text);
                }
            }

            return (Option)optionIndex;
        }

        private static bool IsValidOption(string s)
            => OptionShortNames.Contains(s) || OptionLongNames.Contains(s);

        private static double ParseArgumentAsDouble(string s, string name)
        {
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                return val;

            string text = $"{Resources.ExceptionTextParamValueUnrecognized} {name} {s}. {Resources.ExceptionTextExpectedFloat}.";
            throw new InvalidOperationException(text);
        }

        private List<string> ParseArgumentsAsListTillNextValidOptionFound(int indexOfFirstArgumentToParse)
        {
            List<string> list = new List<string>();

            for (int a = indexOfFirstArgumentToParse; a < _args.Length; ++a)
            {
                string s = _args[a];
                if (IsValidOption(s))
                    break;

                list.Add(s.Trim('"'));
            }

            return list;
        }

        // I leave this commented methods in case I need them to parse an enum:

        //private static T ParseEnum<T>(string textToParse, string paramName) where T : struct, Enum, IConvertible // Enum is a struct with IConvertible interface
        //{
        //    if (Enum.TryParse(textToParse, out T value)) // case sensitive
        //        return value;

        //    string[] names = Enum.GetNames(typeof(T));
        //    string commaSeparated = ToCommaSeparatedString(names);
        //    string text = $"{Resources.ExceptionTextParamValueUnrecognized} {paramName} {textToParse}. {Resources.ExceptionTextExpectedEnum} {commaSeparated}.";
        //    throw new InvalidOperationException(text);
        //}

        //private static string ToCommaSeparatedString(string[] names)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    foreach (string name in names)
        //    {
        //        if (sb.Length != 0)
        //            sb.Append(", ");
        //        sb.Append(name);
        //    }

        //    return sb.ToString();
        //}

        /// <summary>
        /// <returns>returns how many option line arguments have been parsed for this option</returns>
        /// <exception cref="InvalidOperationException">Thrown when requested argument's value has a wrong format.</exception>
        private int ParseOptionValues(int i, Option option)
        {
            if (!_parameterlessOptions.Contains(option))
            {
                if (i + 1 >= _args.Length)
                {
                    string text = $"{Resources.ExceptionTextParamValueMissing} {_args[i]}.";
                    throw new InvalidOperationException(text);
                }
            }

            switch (option)
            {
                case Option.Help:
                    {
                        HelpRequested = true;

                        if (i + 1 < _args.Length)
                        {
                            string nextItem = _args[i + 1];
                            if (IsValidOption(nextItem))
                            {
                                HelpTopic = ParseOptionArg(nextItem);
                                return 1;
                            }
                            else
                            {
                                throw new InvalidOperationException($"{Resources.ExceptionTextParamValueUnrecognized} {_args[i]} {_args[i + 1]}. {Resources.ExceptionTextExpectedValidOptionOrEmpty}.");
                            }
                        }

                        return 0;
                    }
                case Option.Version:
                    {
                        VersionRequested = true;
                        return 0;
                    }
                case Option.Src:
                    {
                        BrainVisionHeaderFilePath = _args[i + 1];

                        if (string.IsNullOrWhiteSpace(BrainVisionHeaderFilePath))
                            throw new InvalidOperationException($"{Resources.ExceptionTextParamEmpty} {_args[i]}");

                        return 1;
                    }
                case Option.Dst:
                    {
                        DestinationFolderPath = _args[i + 1];

                        if (string.IsNullOrWhiteSpace(DestinationFolderPath))
                            throw new InvalidOperationException($"{Resources.ExceptionTextParamEmpty} {_args[i]}");

                        return 1;
                    }
                case Option.DatasetName:
                    {
                        CustomizationInfo.DatasetName = _args[i + 1];
                        // throws no exception. Empty value is accepted
                        return 1;
                    }
                case Option.Subject:
                    {
                        CustomizationInfo.SubjectName = _args[i + 1];

                        if (string.IsNullOrWhiteSpace(CustomizationInfo.SubjectName))
                            throw new InvalidOperationException($"{Resources.ExceptionTextParamEmpty} {_args[i]}");

                        return 1;
                    }
                case Option.Session:
                    {
                        CustomizationInfo.SessionName = _args[i + 1];

                        if (string.IsNullOrWhiteSpace(CustomizationInfo.SessionName))
                            throw new InvalidOperationException($"{Resources.ExceptionTextParamEmpty} {_args[i]}");

                        return 1;
                    }
                case Option.Task:
                    {
                        CustomizationInfo.TaskName = _args[i + 1];

                        if (string.IsNullOrWhiteSpace(CustomizationInfo.TaskName))
                            throw new InvalidOperationException($"{Resources.ExceptionTextParamEmpty} {_args[i]}");

                        return 1;
                    }
                case Option.License:
                    {
                        // throws no exception. Empty value is accepted
                        CustomizationInfo.License = _args[i + 1];
                        return 1;
                    }
                case Option.Authors: // space separated items
                    {
                        // throws no exception. Empty value is accepted
                        CustomizationInfo.Authors = ParseArgumentsAsListTillNextValidOptionFound(i + 1);
                        return CustomizationInfo.Authors.Count;
                    }
                case Option.Manufacturer:
                    {
                        // throws no exception. Empty value is accepted
                        CustomizationInfo.Manufacturer = _args[i + 1];
                        return 1;
                    }
                case Option.EEGReference:
                    {
                        CustomizationInfo.EEGReference = _args[i + 1];

                        if (string.IsNullOrWhiteSpace(CustomizationInfo.EEGReference))
                            throw new InvalidOperationException($"{Resources.ExceptionTextParamEmpty} {_args[i]}");

                        return 1;
                    }
                case Option.PowerLineFrequency:
                    {
                        CustomizationInfo.PowerLineFrequency = ParseArgumentAsDouble(_args[i + 1], _args[i]);

                        if (CustomizationInfo.PowerLineFrequency <= 0.0)
                            throw new InvalidOperationException($"{Resources.ExceptionTextParamZeroOrNegative} {_args[i]}");

                        return 1;
                    }
                default:
                    throw new NotImplementedException(); // should never happen
            }
        }

        public void ThrowExceptionIfRequiredParameterMissing()
        {
            if (BrainVisionHeaderFilePath == null)
                throw new InvalidOperationException($"{Resources.ExceptionTextRequiredParamMissing} {CombinedOptionSyntax(Option.Src)}");

            if (DestinationFolderPath == null)
                throw new InvalidOperationException($"{Resources.ExceptionTextRequiredParamMissing} {CombinedOptionSyntax(Option.Dst)}");
        }

        private static string CombinedOptionSyntax(Option option)
            => $"{OptionShortNames[(int)option]}|{OptionLongNames[(int)option]}";
    }
}
