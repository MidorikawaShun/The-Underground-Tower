﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheUndergroundTower.Windows;
using WpfApp1.Windows;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class pageMainMenu : Page
    {
        public pageMainMenu()
        {
            InitializeComponent();
        }

        private void Exit_Game_Click(object sender, RoutedEventArgs e)
        {
            new windowExit();
        }

        private void New_Game_Click(object sender, RoutedEventArgs e)
        {
            Definitions.MAIN_WINDOW.Main.Content = new pageCharacterCreation();
        }

        private void Leaderboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            new windowOptions();
        }
    }
}