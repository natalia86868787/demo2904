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

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DemkaEntities2 db = new DemkaEntities2();
        private Image firstButton;
        //private object PuzzleGrid;

        public MainWindow()
        {
            InitializeComponent();
            LoadPuzzle();
        }


        private void LoadPuzzle()
        {
            var pices = Enumerable.Range(1, 4).ToList();
            var rnd = new Random();
            pices = pices.OrderBy(x => rnd.Next()).ToList();
            pices.ForEach(x =>
            {
                var Image = new Image {
                    Source = new BitmapImage(new Uri($"capcha/{x}.png", UriKind.Relative)),
                    Tag = x,
                    Stretch = Stretch.Fill
                };
                Image.MouseLeftButtonUp += Pices_Click;
               PuzzleGrid.Children.Add(Image);

            });

}
        private void Pices_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image clicked) {
                if (firstButton == null) {
                    firstButton = clicked;
                    firstButton.Opacity = 0.5;
                    return;
                }
                if (clicked != firstButton) {
                    (firstButton.Source, clicked.Source) = (clicked.Source, firstButton.Source);
                    (firstButton.Tag, clicked.Tag) = (clicked.Tag, firstButton.Tag);
                }
                firstButton.Opacity = 1;
                firstButton = null;
                CheckPuzzle();
            }
        }

        private void CheckPuzzle()
        {
            if (PuzzleGrid.Children
                .OfType<Image>()
                .Select((img, i) => i + 1 == (int)img.Tag)
                .All(x => x))
            {
                capchInfo.Text = "Каптча решена!";
                capchInfo.Foreground = new SolidColorBrush(Colors.Green);
            }
            else {
                capchInfo.Text = "Каптча не решена!";
                capchInfo.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
 

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var user = db.User.FirstOrDefault(x => x.login == txtUsername.Text);
            if (user  != null)
            {
                if (user.password == txtPassword.Text && capchInfo.Text == "Каптча решена!")
                {
                    if (user.isBlocked == false)
                    {
                        if (user.roleID == 0)
                        {
                            MessageBox.Show("Вы успешно авторизовались");
                        }
                        else if (user.roleID == 1)
                        {
                            var adminwindow = new adminwindow();
                            adminwindow.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы заблокированы. Обратитесь к администратору");
                    }
                }
                else
                {
                    user.enterAttepmts++;
                    if (user.enterAttepmts >= 3)
                    {
                        user.isBlocked = true;
                    }
                    db.SaveChanges();
                    MessageBox.Show("Неверный пароль!");
                }
            }
            else
            {
                MessageBox.Show("Такого пользователя нет!");
            }
        }
    }
}
  

