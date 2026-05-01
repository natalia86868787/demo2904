using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
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
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for adminwindow.xaml
    /// </summary>
    public partial class adminwindow : Window
    {
        public adminwindow()
        {
            InitializeComponent();
            updateUserGrid();

        }
        private DemkaEntities2 db = new DemkaEntities2();
        private User selectUser = new User();

        private void updateUserGrid()
        {
            UserGrid.ItemsSource = db.User.ToList();
        }

        private void UserGrid_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (UserGrid.SelectedItem is User selectedItem)
                {
                    selectUser = selectedItem;
                    txtLogin.Text = selectUser.login;
                    txtPassword.Password = selectUser.password;
                }
            } catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Ошибка"); }

        
        }
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newUser = new User
                    {
                    login = txtLogin.Text,
                        password = txtPassword.Password,
                        roleID = 2,
                        enterAttepmts = 0,
                        isBlocked = false
                    };
                db.User.AddOrUpdate(newUser);
                db.SaveChanges();
                updateUserGrid();


            }
            catch (DbUpdateException sqlEx) { MessageBox.Show(sqlEx.Message.ToString(), "Пользователь с таким логином уже существует."); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Ошибка"); }
        }
        private void btnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UserGrid.SelectedItem is User selectedItem)
                {
                    selectUser = selectedItem;
                    selectUser.login = txtLogin.Text;
                    txtPassword.Password = txtPassword.Password;
                    db.User.AddOrUpdate(selectUser);
                    db.SaveChanges();
                    updateUserGrid();
                }
                else
                {
                    MessageBox.Show("Пользователь не выбран.", "Ошибка");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Ошибка"); }
        }
        private void btnUnblockUser_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                if (UserGrid.SelectedItem is User selectedItem)
                {
                    selectUser = selectedItem;
                    selectUser.isBlocked = !selectedItem.isBlocked;
                    selectUser.enterAttepmts = 0;
                    db.User.AddOrUpdate(selectUser);
                    db.SaveChanges();
                    updateUserGrid();
                }
                else
                {
                    MessageBox.Show("Пользователь не выбран.", "Ошибка");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Ошибка"); }
        }



    }
}
