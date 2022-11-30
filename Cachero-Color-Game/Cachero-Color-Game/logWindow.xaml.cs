﻿using System;
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
using System.Windows.Shapes;

namespace Cachero_Color_Game
{
    /// <summary>
    /// Interaction logic for logWindow.xaml
    /// </summary>
    public partial class logWindow : Window 
    {
        private dbInteractions dbOps = new dbInteractions();

        public logWindow()
        {
            InitializeComponent();
        }

        private void logInBtn_Click(object sender, RoutedEventArgs e)
        {
            string uName = string.Empty;
            string uPass = string.Empty;
            
            uName = uNameTbx.Text;
            uPass = uPassTbx.Password;
            userIDHol.Content = string.Empty;
           

            if (dbOps.userLogin(uName, uPass)[0] != "1")
            {
                userIDHol.Content = dbOps.getUID().ToString();
                if (dbOps.userLogin(uName, uPass)[0] == "5")
                {
                    MessageBox.Show(dbOps.userLogin(uName, uPass)[1]);
                    dbOps.playerActiveLog(int.Parse(userIDHol.Content.ToString()));
                    this.Close();
                }
                else if (dbOps.userLogin(uName, uPass)[0] == "7")
                {
                    MessageBox.Show(dbOps.userLogin(uName, uPass)[1]);
                    dbOps.zeroBalanceLog(int.Parse(userIDHol.Content.ToString()));
                    this.Close();
                }
                else if (dbOps.userLogin(uName, uPass)[0] == "4")
                {
                    MessageBox.Show(dbOps.userLogin(uName, uPass)[1]);
                    dbOps.userNameNotFound(uName);     
                }
                else
                {
                    MessageBox.Show(dbOps.userLogin(uName, uPass)[1]);
                }
            }
            else
            {
                userIDHol.Content = dbOps.getUID().ToString();
                MessageBox.Show("Welcome to the game!");
                MainWindow mw = new MainWindow(int.Parse(userIDHol.Content.ToString()));
                mw.Show();
                this.Close();
            } 
        }

        private void loginWindow_Activated(object sender, EventArgs e)
        {
            int machineBalCheck = 0;
            machineBalCheck = dbOps.checkMachineBal();

            if(machineBalCheck != 0)
            {
                MessageBox.Show("MACHINE RUNNING LOW ON FUNDS! MAINTENANCE NEEDED!\nPLEASE CONTACT THE NEAREST STAFF");
                dbOps.MachineNotEnoughBalLog();
                this.Close();
            }
        }
    }
}
