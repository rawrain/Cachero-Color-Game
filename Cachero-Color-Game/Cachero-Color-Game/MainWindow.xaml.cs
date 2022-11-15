using System;
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

namespace Cachero_Color_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rectangle[] gameColorDice = new Rectangle[] { }; 

        public MainWindow()
        {
            InitializeComponent();
            initDices();
        }
        private void initDices() 
        {
            int h = 5;

            List<Color> listOfColors = new List<Color>();
            listOfColors.Add(Color.FromRgb(0, 0, 255));

            gameColorDice = new Rectangle[3];

            for (int i = 0; i < gameColorDice.Length; i++) 
            {
                Rectangle dice = new Rectangle();
                dice.VerticalAlignment = VerticalAlignment.Top;
                dice.HorizontalAlignment = HorizontalAlignment.Left;
                dice.Width = 60;
                dice.Height = 60;
                dice.Margin = new Thickness(h,10,0,0);
                dice.Fill = new SolidColorBrush(listOfColors.ElementAt(0));
                gameColorDice[i] = dice;
                mainGrid.Children.Add(gameColorDice[i]);

                h += 5 + (int)dice.Width;
            }

        }
    }
}
