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
    /// Originator: Corey Wunderlich
    /// This is the visual container box for the Schedule Pieces
    /// They store the information of the indvidual Schedule Events
    /// </summary>
    /// 
    public partial class ScheduleBox : UserControl
    {
        /**
         * The EvenHandler here is for Notifying the parent (SchedGrid)
         * of an object being clicked so that it can detach it from the grid
         */
        public event EventHandler<SchedBoxEventArgs> ScheduleBoxClicked;
        private bool MouseClicked = false;
        public ScheduleBox()
        {
            InitializeComponent();
        }

        /**
         * When ever the mouse moves, if the left button is down
         * move around creating a drag effect
         */ 
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            MouseButtonState clickState = e.LeftButton; 
            Point temp = e.GetPosition(Application.Current.MainWindow);
            if (clickState == MouseButtonState.Pressed && MouseClicked)
            {
                Canvas.SetLeft(this, temp.X - this.Width/2.0);
                Canvas.SetTop(this, temp.Y - this.Height / 2.0);
                titleBox.Text = (Canvas.GetLeft(this)).ToString();
                descriptionBox.Text = temp.X.ToString();
            }
        }

        /**
        * When the mouse clicked on a SBox, notify the listeners of who was clicked
        * In this case it will be the ScheduleGrid, the Schedule Grid will pop it out
        * of the grid and draw it on the canvas instead
        */
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventHandler<SchedBoxEventArgs> handler = ScheduleBoxClicked;
            SchedBoxEventArgs sbea = new SchedBoxEventArgs();
            MouseClicked = sbea.Clicked = true;
            Point temp = e.GetPosition(Application.Current.MainWindow);
            Canvas.SetLeft(this, temp.X - this.Width / 2.0);
            Canvas.SetTop(this, temp.Y - this.Height / 2.0);
            if (handler != null)
            {
                handler(this, sbea);
            }
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EventHandler<SchedBoxEventArgs> handler = ScheduleBoxClicked;
            SchedBoxEventArgs sbea = new SchedBoxEventArgs();
            MouseClicked = sbea.Clicked = false;
            Point temp = e.GetPosition(Application.Current.MainWindow);
            Canvas.SetLeft(this, temp.X - this.Width / 2.0);
            Canvas.SetTop(this, temp.Y - this.Height / 2.0);
            if (handler != null)
            {
                handler(this, sbea);
            }
        }

    }//end Class

    /// <summary>
    /// The EventArgs class for The Schedule Box
    /// </summary>
    public class SchedBoxEventArgs : EventArgs
    {
        public bool Clicked { get; set; }
    }

}
