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
        int NextEventCol = 0;
        public ScheduleGrid(int cols, int rows)
        {
            InitializeComponent();

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
            newBox.ScheduleBoxClicked += this.ScheduleBox_Clicked;
            timeGrid.Children.Add(newBox);
        }

        public void NewEvent(String title = "New Event", String desc = "A New Event")
        {
            ScheduleBox newBox = new ScheduleBox(title,desc);
            newBox.ScheduleBoxClicked += this.ScheduleBox_Clicked;
            eventGrid.Children.Add(newBox);
            Grid.SetColumn(newBox, NextEventCol++);
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
                    case GridNames.Event: eventGrid.Children.Add(sb); break;
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
                eventGrid.Children.Remove(sb);
                theCanvas.Children.Add(sb);
                return true;
            }
            return false;
        }

        private void ScheduleBox_Clicked(object sender, SchedBoxEventArgs e)
        {
            if (sender is ScheduleBox)
            {
                ScheduleBox sbSender = ((ScheduleBox)sender);
                if (e.Clicked)
                {
                    PopFromGrid(sbSender);
                }
                else
                {
                    SnapToGrid(sbSender);
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
