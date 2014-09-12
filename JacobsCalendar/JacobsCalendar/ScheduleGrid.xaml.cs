using System;
using System.Collections.Generic;
using System.Collections;
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
    /// Main Schedule Box Controller. Handles the storage of their locations and the snapping into
    /// of grids
    /// </summary>
    public partial class ScheduleGrid : UserControl
    {
        public const int GRID_SIZE = 150;
        int Rows = 0;
        int Cols = 0;
        double TGrid_OffsetY;
        double TGrid_OffsetX;
        double EGrid_OffsetY;
        double EGrid_OffsetX;
        ArrayList EventList;
        public ScheduleGrid(int cols, int rows)
        {
            InitializeComponent();
            EventList = new ArrayList();
            Rows = rows;
            Cols = cols;
            TGrid_OffsetX = Canvas.GetLeft(timeGrid)+GRID_SIZE/2;
            TGrid_OffsetY = Canvas.GetTop(timeGrid) + GRID_SIZE / 2;
            EGrid_OffsetX = Canvas.GetLeft(eventGrid);
            EGrid_OffsetY = Canvas.GetTop(eventGrid);
            // Initialize the Grid Cols and Rows

            //Label Fields : half sized
            ColumnDefinition cd = new ColumnDefinition();
            cd.SetValue(ColumnDefinition.WidthProperty, new GridLength(GRID_SIZE/2));
            timeGrid.ColumnDefinitions.Add(cd);

            RowDefinition rd = new RowDefinition();
            rd.SetValue(RowDefinition.HeightProperty, new GridLength(GRID_SIZE/2));
            timeGrid.RowDefinitions.Add(rd);

            //Data Fields
            int i;
            TextBox lbl;
            for (i = 0; i < Cols; i++)
            {
                cd = new ColumnDefinition();
                cd.SetValue(ColumnDefinition.WidthProperty, new GridLength(GRID_SIZE));
                timeGrid.ColumnDefinitions.Add(cd);
                lbl = new TextBox();
                lbl.Text = "COLUMN: " + i;
                Grid.SetColumn(lbl, i + 1);
                timeGrid.Children.Add(lbl);

                cd = new ColumnDefinition();
                cd.SetValue(ColumnDefinition.WidthProperty, new GridLength(GRID_SIZE));
                eventGrid.ColumnDefinitions.Add(cd);

            }
            for (i = 0; i < Rows; i++)
            {
                rd = new RowDefinition();
                rd.SetValue(RowDefinition.HeightProperty, new GridLength(GRID_SIZE));
                timeGrid.RowDefinitions.Add(rd);
                lbl = new TextBox();
                lbl.Text = "ROW: " + i;
                Grid.SetRow(lbl, i + 1);
                timeGrid.Children.Add(lbl);
            }
        }

        /**
         * Add a new Box to the Grid
         */
        public void Add(ScheduleBox newBox)
        {
            newBox.ScheduleBoxEvent += this.ScheduleBox_Event;
            timeGrid.Children.Add(newBox);
        }

        public void NewEvent(String title = "New Event", String desc = "A New Event")
        {
            ScheduleBox newBox = new ScheduleBox(title,desc);
            newBox.ScheduleBoxEvent += this.ScheduleBox_Event;
            eventGrid.Children.Add(newBox);
            EventList.Add(newBox);
            Grid.SetColumn(newBox, (EventList.Count-1));
        }

        private GridPos CanvasToGrid(double x, double y)
        {
            GridPos gp;
            if (y >= TGrid_OffsetY)
            {
                gp.Col = (int)((x - TGrid_OffsetX) / GRID_SIZE) + 1;
                gp.Row = (int)((y - TGrid_OffsetY) / GRID_SIZE) + 1;
                gp.WhichGrid = GridNames.Time;
            }
            else
            {
                gp.Col = (int)((x - EGrid_OffsetX) / GRID_SIZE);
                gp.Row = 0;
                gp.WhichGrid = GridNames.Event;
            }
            return gp;
        }


        /**
         * Called when moving from the Canvas to the Grid. This way it snaps into a place
         * and can be processed easier in the future
         */
        private bool SnapToGrid(ScheduleBox sb)
        {
            if (theCanvas.Children.Contains(sb))
            {
                double x = Canvas.GetLeft(sb);
                double y = Canvas.GetTop(sb);
                //Get Center Point
                x += (sb.Width / 2);
                y += (sb.Height / 2);
                GridPos gp = CanvasToGrid(x, y);
                theCanvas.Children.Remove(sb);
                Grid.SetColumn(sb, gp.Col);
                Grid.SetRow(sb, gp.Row);
                switch (gp.WhichGrid)
                {
                    case GridNames.Time: timeGrid.Children.Add(sb); break;
                    //case GridNames.Event: eventGrid.Children.Add(sb); break;
                }
                
                return true;
            }
            return false;

        }

        private bool PopFromGrid(ScheduleBox sb)
        {
            if (timeGrid.Children.Contains(sb))
            {
                timeGrid.Children.Remove(sb);
                theCanvas.Children.Add(sb);
                return true;
            }
            
            else if (eventGrid.Children.Contains(sb))
            {
                //Possibly going a new direction with the event grid
                //eventGrid.Children.Remove(sb);
                //theCanvas.Children.Add(sb);

                /* New Direction: Adding events to the event list, will leave them there
                 * As resources to be chosen and dragged on to the schedule grid
                 * So here we clone the schedule box and add it to the canvas
                 * at the location of the original Event Box
                 * 
                 */
                ScheduleBox nsb = new ScheduleBox(sb.Title(), sb.Description(), sb.ScheduleID);
                nsb.ScheduleBoxEvent += this.ScheduleBox_Event;
                Canvas.SetLeft(nsb, Canvas.GetLeft(sb));
                Canvas.SetTop(nsb, Canvas.GetTop(sb));
                theCanvas.Children.Add(nsb);
               return true;
            }
            return false;
        }
        public bool DeleteScheduleBox(ScheduleBox sb)
        {
            if (timeGrid.Children.Contains(sb))
            {
                timeGrid.Children.Remove(sb);
                return true;
            }
            else if (eventGrid.Children.Contains(sb))
            {
                //When we delete someone from the event grid
                //Slide all of the remaining grid items to the left
                int c = Grid.GetColumn(sb);
                eventGrid.Children.Remove(sb);
                for (int col = c+1; col < EventList.Count; col++)
                {
                    Grid.SetColumn((UIElement)EventList[col], col - 1);
                }
                EventList.Remove(sb);
                return true;
            }
            else if (theCanvas.Children.Contains(sb))
            {
                theCanvas.Children.Remove(sb);
                return true;
            }
            return false;
        }

        private void ScheduleBox_Event(object sender, SchedBoxEventArgs e)
        {
            if (sender is ScheduleBox)
            {
                ScheduleBox sbSender = ((ScheduleBox)sender);
                switch(e.EventType)
                {
                    case SchedBoxEventType.MouseDown: PopFromGrid(sbSender); break;
                    case SchedBoxEventType.MouseUp: SnapToGrid(sbSender); break;
                    case SchedBoxEventType.Deleted: DeleteScheduleBox(sbSender); break;
                } 
            }
        }

    }//end class
    public enum GridNames
    {
        Time,
        Event
    };
    public struct GridPos
    {
        public GridNames WhichGrid;
        public int Row;
        public int Col;
    }

}
