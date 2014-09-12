﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JacobsCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ScheduleGrid schedGrid = new ScheduleGrid(8,2);
            theDisplayCvs.Children.Add(schedGrid);
            schedGrid.NewEvent();
            schedGrid.NewEvent();
            schedGrid.NewEvent();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

        }

    }
}
