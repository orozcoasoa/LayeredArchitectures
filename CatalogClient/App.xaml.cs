﻿using System.Windows;
using Microsoft.Extensions.Configuration;

namespace CatalogClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static IConfiguration Config { get; private set; }

        public App()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }


    }
}
