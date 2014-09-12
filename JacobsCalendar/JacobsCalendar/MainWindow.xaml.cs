using System;
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
        ScheduleGrid schedGrid;
        public MainWindow()
        {
            InitializeComponent();
            schedGrid = new ScheduleGrid(8, 2);
            theDisplayCvs.Children.Add(schedGrid);
            Canvas.SetTop(schedGrid, 20);
            //schedGrid.NewEvent();
            //schedGrid.NewEvent("This is a test");
            //schedGrid.NewEvent("Test2","A better test");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            schedGrid.NewEvent();
        }

    }
}
