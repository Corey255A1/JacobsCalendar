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
        public event EventHandler<SchedBoxEventArgs> ScheduleBoxEvent;
        private bool MouseClicked = false;
        private bool Cloned = false;
        private static int AutoScheduleID = 0;
        public int ScheduleID {get; private set;}
        public ScheduleBox()
        {
            InitializeComponent();
            ScheduleID = AutoScheduleID++;
        }
        public ScheduleBox(String title, String desc)
        {
            InitializeComponent();
            titleBox.Text = title;
            descriptionBox.Text = desc;
            ScheduleID = AutoScheduleID++;
        }
        public ScheduleBox(String title, String desc, int nID)
        {
            InitializeComponent();
            titleBox.Text = title;
            descriptionBox.Text = desc;
            ScheduleID = nID;
            Cloned = true;
        }

        /**
         * When ever the mouse moves, if the left button is down
         * move around creating a drag effect
         */ 
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            MouseButtonState clickState = e.LeftButton; 
            Point temp = e.GetPosition(Application.Current.MainWindow);
            if (clickState == MouseButtonState.Pressed && (MouseClicked || Cloned))
            {
                Canvas.SetLeft(this, temp.X - this.Width/2.0);
                Canvas.SetTop(this, temp.Y - this.Height / 2.0);
                //Debugging
                //titleBox.Text = (Canvas.GetLeft(this)).ToString();
                //descriptionBox.Text = temp.X.ToString();
            }
        }

        /**
        * When the mouse clicked on a SBox, notify the listeners of who was clicked
        * In this case it will be the ScheduleGrid, the Schedule Grid will pop it out
        * of the grid and draw it on the canvas instead
        */
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventHandler<SchedBoxEventArgs> handler = ScheduleBoxEvent;
            SchedBoxEventArgs sbea = new SchedBoxEventArgs();
            MouseClicked = true;
            sbea.EventType = SchedBoxEventType.MouseDown;
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
            EventHandler<SchedBoxEventArgs> handler = ScheduleBoxEvent;
            SchedBoxEventArgs sbea = new SchedBoxEventArgs();
            MouseClicked = false;
            sbea.EventType = SchedBoxEventType.MouseUp;
            Point temp = e.GetPosition(Application.Current.MainWindow);
            Canvas.SetLeft(this, temp.X - this.Width / 2.0);
            Canvas.SetTop(this, temp.Y - this.Height / 2.0);
            Cloned = false;
            if (handler != null)
            {
                handler(this, sbea);
            }
        }

        public void Title(String title)
        {
            titleBox.Text = title;
        }
        public String Title()
        {
           return titleBox.Text;
        }
        public void Description(String des)
        {
            descriptionBox.Text = des;
        }
        public String Description()
        {
            return descriptionBox.Text;
        }

        private void Color_Menu_Chosen(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Menu_Chosen(object sender, RoutedEventArgs e)
        {
            EventHandler<SchedBoxEventArgs> handler = ScheduleBoxEvent;
            SchedBoxEventArgs sbea = new SchedBoxEventArgs();
            sbea.EventType = SchedBoxEventType.Deleted;
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
        public SchedBoxEventType EventType { get; set; }
    }
    public enum SchedBoxEventType { MouseUp, MouseDown, Deleted }

}
