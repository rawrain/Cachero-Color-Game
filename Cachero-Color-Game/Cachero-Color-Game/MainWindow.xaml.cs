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
        private Label[] gameColorDice = new Label[] { }; 
        private Grid gameDiceGrid = new Grid();

        public MainWindow()
        {
            InitializeComponent();
            initDices();
        }
        private void initDices() 
        {
            initDiceGrid();
            int h = 5;

            List<Color> listOfColors = new List<Color>();
            listOfColors.Add(Color.FromRgb(0, 0, 255)); //Color of Blue
            listOfColors.Add(Color.FromRgb(0, 255, 0)); //Color of Green
            listOfColors.Add(Color.FromRgb(255, 255, 0)); //Color of Yellow
            listOfColors.Add(Color.FromRgb(255, 0, 0)); //Color of Red
            listOfColors.Add(Color.FromRgb(255, 140, 0)); //Color of Orange
            listOfColors.Add(Color.FromRgb(255, 0, 255)); // Color of Purple

            gameColorDice = new Label[3];

            for (int i = 0; i < gameColorDice.Length; i++) 
            {
                Label dice = new Label();
                dice.VerticalAlignment = VerticalAlignment.Top;
                dice.HorizontalAlignment = HorizontalAlignment.Left;
                dice.VerticalContentAlignment= VerticalAlignment.Center;
                dice.HorizontalContentAlignment= HorizontalAlignment.Center;
                dice.Width = 80;
                dice.Height = 80;
                dice.Margin = new Thickness(h,10,0,0);
                dice.BorderBrush = new SolidColorBrush(Colors.Black);
                dice.BorderThickness = new Thickness(1,1,1,1);
                switch (i) 
                {
                    case 0:
                        dice.Content = "Dice 1";
                        break;
                    case 1:
                        dice.Content = "Dice 2";
                        break;
                    case 2:
                        dice.Content = "Dice 3";
                        break;
                }
                gameColorDice[i] = dice;
                gameDiceGrid.Children.Add(gameColorDice[i]);

                h += 10 + (int)dice.Width;
            }

        }

        private void initDiceGrid() 
        {
            gameDiceGrid = new Grid();
            gameDiceGrid.HorizontalAlignment = HorizontalAlignment.Left;
            gameDiceGrid.VerticalAlignment = VerticalAlignment.Top;
            gameDiceGrid.Width = 300;
            gameDiceGrid.Height = 90;
            gameDiceGrid.Margin = new Thickness(350,200,0,0);
            mainGrid.Children.Add(gameDiceGrid);
        }
    }
}
