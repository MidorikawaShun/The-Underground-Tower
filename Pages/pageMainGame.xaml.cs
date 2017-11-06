using System;
using System.Collections.Generic;

using System.Drawing.Drawing2D;
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
using TheUndergroundTower.OtherClasses;
using TheUndergroundTower.Pathfinding;
using WpfApp1;
using WpfApp1.Creatures;

namespace TheUndergroundTower.Pages
{
    /// <summary>
    /// Interaction logic for pageMainGame.xaml
    /// </summary>
    public partial class pageMainGame : Page
    {
        public pageMainGame()
        {
            InitializeComponent();
            GameData.InitializeTiles();
            Map map = new Map();

            for (int i = 0; i < map.XSize; i++)
            {
                for (int j = 0; j < map.YSize; j++)
                {
                    Image img = new Image();
                    img.Width = img.Height = 50;
                    img.Source = map.Tiles[i, j].Image;
                    XAMLMap.Children.Add(img);
                }

            }
            map.Tiles[1, 1].Objects = new List<GameObject>();
            Player p = new Player();
            map.Tiles[1, 1].Objects.Add(p);

            ((Image)XAMLMap.Children[map.XSize+1]).Source= CreateTile.Overlay(map.Tiles[1, 1].Image, p.Image);




            //zeroZero.Source = GameData.POSSIBLE_TILES.FirstOrDefault().Image;
        }
    }
}
