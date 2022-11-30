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
        private Dictionary<string,List<Object>> logTemp = new Dictionary<string,List<Object>>();
        private int logTempID = 0;
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
                    int uID = userLogin.Customer_ID;
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
                        this.uID = uID;
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

        public string getUserName(string uID) 
        {
            var getUserName = (from a in dbCon.table_Customers where a.Customer_ID == int.Parse(uID) select a.Customer_FirstName).FirstOrDefault();
            string userName = getUserName.ToString();
            return userName;
        }

        public decimal getBalance(string uID)
        {
            var getUserBalance = (from a in dbCon.table_Customers where a.Customer_ID == int.Parse(uID) select a.Customer_CurrentBalance).FirstOrDefault();
            decimal userBalance = getUserBalance;
            return userBalance;
        }

        public int getUID() 
        {
            return this.uID;
        }

        public void updateCurrentMachineWinnings(decimal gameRoundWinnings) 
        {
            dbCon.uspUpdateMachineCurrentWinnings(machineID, gameRoundWinnings);
        }

        public void updateMachineCurrentBalance(decimal newMachineBalance)
        {
            dbCon.uspUpdateMachineBalance(machineID, newMachineBalance);
        }

        public void updatePlayerCurrentBalance(int uID, decimal gameRoundTotalWager) 
        {
            dbCon.uspUpdateCustomerCurrentBalance(uID, gameRoundTotalWager);
        }

        public decimal getMachineBal() 
        {
            var getMachineBal = (from a in dbCon.table_Machines where a.Machine_ID == machineID select a.Machine_CurrentBalance).FirstOrDefault();
            decimal machineBal = getMachineBal;
            return machineBal;
        }

        public int checkMachineBal()
        {
            decimal currentMachineBal = getMachineBal();
            int checker = 0;

            if (currentMachineBal <= 10000)
            {
                checker = 1;
            }
            else 
            {
                checker = 0;
            }
            
            return checker;
        }

        public void MachineNotEnoughBalLog() 
        {
            DateTime timeNow = DateTime.Now;
            dbCon.uspCreateGameLog(timeNow, customerID: 1, machineID, gameID, errorcodeID: 3, "Machine is running low on balance", 0, getMachineBal());
        }

        public void changeMachineCustomerLogOut() 
        {
            dbCon.uspUpdateMachineCustomer(machineID, 1);
        }

        public void createLog(DateTime date, int CID, int EID, string GLC, decimal CW, decimal MCB) 
        {
            /*
             * Game Logs Components
             * 
             * DateTime
             * CustomerID
             * MachineID
             * gameID
             * errorID
             * gamelogComments
             * customerWinnings
             * machineCurrentBalance
             */

            List<Object> log = new List<Object>
            {
                date,
                CID,
                machineID,
                gameID,
                EID,
                GLC,
                CW,
                MCB
            };

            logTemp.Add($"log{logTempID+1}",log);
            logTempID++;

        }

        public void pushLogToDB() 
        {
            for (int i = 0; i < logTemp.Count; i++) 
            {
                int x = 0;

                dbCon.uspCreateGameLog
                    (
                        DateTime.Parse(logTemp.Values.ElementAt(i).ElementAt(x).ToString()),
                        int.Parse(logTemp.Values.ElementAt(i).ElementAt(x+1).ToString()),
                        int.Parse(logTemp.Values.ElementAt(i).ElementAt(x + 2).ToString()),
                        int.Parse(logTemp.Values.ElementAt(i).ElementAt(x + 3).ToString()),
                        int.Parse(logTemp.Values.ElementAt(i).ElementAt(x + 4).ToString()),
                        logTemp.Values.ElementAt(i).ElementAt(x + 5).ToString(),
                        decimal.Parse(logTemp.Values.ElementAt(i).ElementAt(x + 6).ToString()),
                        decimal.Parse(logTemp.Values.ElementAt(i).ElementAt(x + 7).ToString())
                    );
            }
        }
    }
}
