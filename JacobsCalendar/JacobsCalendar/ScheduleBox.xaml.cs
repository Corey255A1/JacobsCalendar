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
    /// This is the visual container box for the Schedule Pieces
    /// </summary>
    public partial class ScheduleBox : UserControl
    {
        public ScheduleBox()
        {
            InitializeComponent();
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            MouseButtonState clickState = e.LeftButton; 
            Point temp = e.GetPosition(Application.Current.MainWindow);
            if (clickState == MouseButtonState.Pressed)
            {
                Canvas.SetLeft(this, temp.X - this.Width/2.0);
                Canvas.SetTop(this, temp.Y - this.Height / 2.0);
                titleBox.Text = (Canvas.GetLeft(this)).ToString();
                descriptionBox.Text = temp.X.ToString();
            }
        }

    }
}
