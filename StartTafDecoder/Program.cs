﻿using CommandLine;
using CommandLine.Text;
using csharp_taf_decoder;
using csharp_taf_decoder.entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StartTafDecoder
{
    class Program
    {
        class Options
        {
            [Option("Taf", Required = true, HelpText = "Path to the XML Configuration File.")]
            public string Taf { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        static void Main(string[] args)
        {
            TafDecoder.SetStrictParsing(true);

            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                var decodedTaf = TafDecoder.ParseWithMode(options.Taf);
                Display(decodedTaf);
            }
        }

        private static void Display(object o, string prefix = "")
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(o))
            {
                var name = descriptor.Name;
                var value = descriptor.GetValue(o);

                if (value is ReadOnlyCollection<Exception>)
                {
                    Console.WriteLine($"{name}.Count={(value as ReadOnlyCollection<Exception>).Count}");
                }
                else if (value is SurfaceWind)
                {
                    var surfaceWind = value as SurfaceWind;
                    Display(surfaceWind, prefix + "SurfaceWind.");
                }
                else if (value is Visibility)
                {
                    var visibility = value as Visibility;
                    Display(visibility, prefix + "Visibility.");
                }
                //else if (value is List<RunwayVisualRange>)
                //{
                //    var listRunwayVisualRange = value as List<RunwayVisualRange>;
                //    foreach (var runwayVisualRange in listRunwayVisualRange)
                //    {
                //        Display(runwayVisualRange, prefix + "RunwayVisualRange.");
                //    }
                //}
                else if (value is List<WeatherPhenomenon>)
                {
                    var listPresentWeather = value as List<WeatherPhenomenon>;
                    foreach (var presentWeather in listPresentWeather)
                    {
                        Display(presentWeather, prefix + "PresentWeather.");
                    }
                }
                else if (value is List<CloudLayer>)
                {
                    var listCloud = value as List<CloudLayer>;
                    foreach (var cloud in listCloud)
                    {
                        Display(cloud, prefix + "Cloud.");
                    }
                }
                else
                {
                    Console.WriteLine($"{prefix}{name}={value?.ToString()}");
                }
            }
        }
    }
}
