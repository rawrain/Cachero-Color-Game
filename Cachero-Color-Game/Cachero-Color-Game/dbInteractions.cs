using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cachero_Color_Game
{
    internal class dbInteractions
    {
        private AmazonDBDataContext dbCon = new AmazonDBDataContext(Properties.Settings.Default.Group_2___CasinoConnectionString);
        private int uID = 0;
        private int machineID = 5;
        private int gameID = 5;

        public string[] userLogin(string uName, string uPass) 
        {
            string[] errorMessage = new string[2];
            var allUsers = (from a in dbCon.table_Customers select a.Customer_Username).ToList();
           

            List<string> customers = new List<string>();

            foreach (var el in allUsers) 
            {
                customers.Add(el.ToString());
            }

            if (customers.Contains(uName))
            {
                var userLogin = (from b in dbCon.table_Customers where b.Customer_Username == uName select b).FirstOrDefault();

                if (userLogin.Customer_Password != uPass)
                {
                    for (int i = 0; i < errorMessage.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                errorMessage[i] = "6";
                                break;
                            case 1:
                                errorMessage[i] = "Incorrect Password/Username!";
                                break;
                        }
                    }
                }
                else
                {
                    uID = userLogin.Customer_ID;
                    int checkUserAct = checkUserActive(uID);

                    if (checkUserAct != 0)
                    {
                        for (int i = 0; i < errorMessage.Length; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    errorMessage[i] = "5";
                                    break;
                                case 1:
                                    errorMessage[i] = $"User {userLogin.Customer_ID} is already logged into another machine!";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < errorMessage.Length; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    errorMessage[i] = "1";
                                    break;
                                case 1:
                                    errorMessage[i] = $"User {userLogin.Customer_ID} has logged in";
                                    break;
                            }
                        }
                        dbCon.uspUpdateMachineCustomer(machineID, uID);
                    }
                }
            }
            else
            {
                for (int i = 0; i < errorMessage.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            errorMessage[i] = "4";
                            break;
                        case 1:
                            errorMessage[i] = "Username is not Found!";
                            break;
                    }
                }
            }

            return errorMessage;

        }

        private int checkUserActive(int uID) 
        {
            int checker = 0;
            var allUserID = dbCon.vwFunqAllMachines().ToList();
            List<int> userIDs = new List<int>();

            foreach (var el in allUserID) 
            {
                userIDs.Add(el.Customer_ID);
            }

            if (!userIDs.Contains(uID))
            {
                checker = 0;
            }
            else
            {
                checker = 1;
            }

            return checker;
        }

    }
}
