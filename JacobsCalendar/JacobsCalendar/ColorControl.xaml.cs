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
    /// Interaction logic for ColorControl.xaml
    /// </summary>
    public partial class ColorControl : UserControl
    {
        public event EventHandler<ColorCtlEventArgs> ColorCtlEvent;
        public ColorControl()
        {
            InitializeComponent();
        }
        public void SetColor(Brush br)
        {
            redSlide.Value = ((SolidColorBrush)br).Color.R;
            greenSlide.Value = ((SolidColorBrush)br).Color.G;
            blueSlide.Value = ((SolidColorBrush)br).Color.B;
        }

        public Brush GetColor()
        {
            return new SolidColorBrush(Color.FromRgb((byte)redSlide.Value, (byte)greenSlide.Value, (byte)blueSlide.Value));
        }

        private void redSlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EventHandler<ColorCtlEventArgs> handler = ColorCtlEvent;
            ColorCtlEventArgs sbea = new ColorCtlEventArgs();
            sbea.Changed = true;
            if (handler != null)
            {
                handler(this, sbea);
            }
        }

        private void greenSlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EventHandler<ColorCtlEventArgs> handler = ColorCtlEvent;
            ColorCtlEventArgs sbea = new ColorCtlEventArgs();
            sbea.Changed = true;
            if (handler != null)
            {
                handler(this, sbea);
            }
        }

        private void blueSlide_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EventHandler<ColorCtlEventArgs> handler = ColorCtlEvent;
            ColorCtlEventArgs sbea = new ColorCtlEventArgs();
            sbea.Changed = true;
            if (handler != null)
            {
                handler(this, sbea);
            }
        }

        private void redSlide_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            int t=0;
            t++;
        }
        

    }

    /// <summary>
    /// The EventArgs class for The Schedule Box
    /// </summary>
    public class ColorCtlEventArgs : EventArgs
    {
        public bool Changed;
    }
}
