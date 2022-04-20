using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LunariStock
{
    public partial class lunariStockForm : Form
    {
        List<TextBox> textBoxes = new List<TextBox>();
        List<TextBox> playerNamesTextBoxes = new List<TextBox>();
        List<TextBox> playerInvestTextBoxes = new List<TextBox>();
        List<TextBox> p1InvestTextBoxes = new List<TextBox>();
        List<TextBox> p2InvestTextBoxes = new List<TextBox>();
        List<TextBox> p3InvestTextBoxes = new List<TextBox>();
        List<TextBox> p4InvestTextBoxes = new List<TextBox>();
        List<Double> percentageList = new List<Double>(new double[8]);

        Random random = new Random();
        string totalFile = "TotalData.txt";
        string player1File = "player1Data.txt";
        string player2File = "player2Data.txt";
        string player3File = "player3Data.txt";
        string player4File = "player4Data.txt";
        string percentageFile = "percentages.txt";
        string playerNamesFile = "playerNames.txt";



        public static double lunarisMin = 0.75, lunarisMax = 1.3;
        public static double duremarMin = 0.85, duremarMax = 1.2;
        public static double ramesMin = 0.65, ramesMax = 1.4;
        public static double inelloMin = 0.45, inelloMax = 1.6;

        //TODO: Data kansio

        public lunariStockForm()
        {
            InitializeComponent();

            textBoxes.Add(lunarisValueText);
            textBoxes.Add(duremarValueText);
            textBoxes.Add(ramesValueText);
            textBoxes.Add(inelloValueText);

            playerNamesTextBoxes.Add(playerName1);
            playerNamesTextBoxes.Add(playerName2);
            playerNamesTextBoxes.Add(playerName3);
            playerNamesTextBoxes.Add(playerName4);

            playerInvestTextBoxes.Add(investTextOne);
            playerInvestTextBoxes.Add(investTextTwo);
            playerInvestTextBoxes.Add(investTextThree);
            playerInvestTextBoxes.Add(investTextFour);

            p1InvestTextBoxes.Add(p1InvestLunaris);
            p1InvestTextBoxes.Add(p1InvestDuremar);
            p1InvestTextBoxes.Add(p1InvestRames);
            p1InvestTextBoxes.Add(p1InvestInello);

            p2InvestTextBoxes.Add(p2InvestLunaris);
            p2InvestTextBoxes.Add(p2InvestDuremar);
            p2InvestTextBoxes.Add(p2InvestRames);
            p2InvestTextBoxes.Add(p2InvestInello);

            p3InvestTextBoxes.Add(p3InvestLunaris);
            p3InvestTextBoxes.Add(p3InvestDuremar);
            p3InvestTextBoxes.Add(p3InvestRames);
            p3InvestTextBoxes.Add(p3InvestInello);

            p4InvestTextBoxes.Add(p4InvestLunaris);
            p4InvestTextBoxes.Add(p4InvestDuremar);
            p4InvestTextBoxes.Add(p4InvestRames);
            p4InvestTextBoxes.Add(p4InvestInello);


            //TODO: Tee uusi funktio, jossa luodaan uusi jokaiselle pelaajalle tekstitiedosto, jos ei ole vielä olemassa. 
            //Tekstitiedostot sisältävät sijoitetut summat

            CreatePlayerNamesFile();
            CreatePlayerDataFiles();
            CreatePlayerTotalDataFile();
            CreatePercentageFile();

            ReadPlayerNames();
            ReadPercentageData();
            ReadPlayerSpecificData();
            ReadPlayerTotalData();

        }


        public double GetRandomNumber(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }        


        private void UpdateButton_Click(object sender, EventArgs e)
        {
            //Change stock percentages
            lunarisValueText.Text = (GetRandomNumber(lunarisMin, lunarisMax)).ToString("F2");
            duremarValueText.Text = GetRandomNumber(duremarMin, duremarMax).ToString("F2");
            ramesValueText.Text = GetRandomNumber(ramesMin, ramesMax).ToString("F2");
            inelloValueText.Text = GetRandomNumber(inelloMin, inelloMax).ToString("F2");

            //Change color
            for (int i = 0; i < textBoxes.Count; i++)
            {
                //If loss
                if(float.Parse(textBoxes[i].Text) < 1)
                {
                    textBoxes[i].BackColor = Color.Red;
                }
                //If profit
                else
                {
                    textBoxes[i].BackColor = Color.Green;
                }
            }

            UpdateInvestedSums();

            //How much invested
            float player1InvestValue = float.Parse(p1InvestLunaris.Text) + float.Parse(p1InvestDuremar.Text) +
                float.Parse(p1InvestRames.Text) + float.Parse(p1InvestInello.Text);
            float player2InvestValue = float.Parse(p2InvestLunaris.Text) + float.Parse(p2InvestDuremar.Text) +
                float.Parse(p2InvestRames.Text) + float.Parse(p2InvestInello.Text);
            float player3InvestValue = float.Parse(p3InvestLunaris.Text) + float.Parse(p3InvestDuremar.Text) +
                float.Parse(p3InvestRames.Text) + float.Parse(p3InvestInello.Text);
            float player4InvestValue = float.Parse(p4InvestLunaris.Text) + float.Parse(p4InvestDuremar.Text) +
                float.Parse(p4InvestRames.Text) + float.Parse(p4InvestInello.Text);

            //Multiply invested amount by the new stock percentage
            investTextOne.Text = player1InvestValue.ToString("F1");
            investTextTwo.Text = player2InvestValue.ToString("F1");
            investTextThree.Text = player3InvestValue.ToString("F1");
            investTextFour.Text = player4InvestValue.ToString("F1");

            //Save the new data to txt file.
            SavePlayerSpecificData();
            SaveNewTotalData(investTextOne.Text, investTextTwo.Text, investTextThree.Text, investTextFour.Text);



        }

        private void UpdateInvestedSums()
        {
            p1InvestLunaris.Text = (float.Parse(p1InvestLunaris.Text) * float.Parse(lunarisValueText.Text)).ToString("F1");
            p2InvestLunaris.Text = (float.Parse(p2InvestLunaris.Text) * float.Parse(lunarisValueText.Text)).ToString("F1");
            p3InvestLunaris.Text = (float.Parse(p3InvestLunaris.Text) * float.Parse(lunarisValueText.Text)).ToString("F1");
            p4InvestLunaris.Text = (float.Parse(p4InvestLunaris.Text) * float.Parse(lunarisValueText.Text)).ToString("F1");

            p1InvestDuremar.Text = (float.Parse(p1InvestDuremar.Text) * float.Parse(duremarValueText.Text)).ToString("F1");
            p2InvestDuremar.Text = (float.Parse(p2InvestDuremar.Text) * float.Parse(duremarValueText.Text)).ToString("F1");
            p3InvestDuremar.Text = (float.Parse(p3InvestDuremar.Text) * float.Parse(duremarValueText.Text)).ToString("F1");
            p4InvestDuremar.Text = (float.Parse(p4InvestDuremar.Text) * float.Parse(duremarValueText.Text)).ToString("F1");

            p1InvestRames.Text = (float.Parse(p1InvestRames.Text) * float.Parse(ramesValueText.Text)).ToString("F1");
            p2InvestRames.Text = (float.Parse(p2InvestRames.Text) * float.Parse(ramesValueText.Text)).ToString("F1");
            p3InvestRames.Text = (float.Parse(p3InvestRames.Text) * float.Parse(ramesValueText.Text)).ToString("F1");
            p4InvestRames.Text = (float.Parse(p4InvestRames.Text) * float.Parse(ramesValueText.Text)).ToString("F1");

            p1InvestInello.Text = (float.Parse(p1InvestInello.Text) * float.Parse(inelloValueText.Text)).ToString("F1");
            p2InvestInello.Text = (float.Parse(p2InvestInello.Text) * float.Parse(inelloValueText.Text)).ToString("F1");
            p3InvestInello.Text = (float.Parse(p3InvestInello.Text) * float.Parse(inelloValueText.Text)).ToString("F1");
            p4InvestInello.Text = (float.Parse(p4InvestInello.Text) * float.Parse(inelloValueText.Text)).ToString("F1");

        }



        private void SaveNewTotalData(string player1Data, string player2Data, string player3Data, string player4Data)
        {

            try
            {
                if (File.Exists(totalFile))
                {
                    using (StreamWriter sw = File.CreateText(totalFile))
                    {
                        sw.WriteLine(player1Data);
                        sw.WriteLine(player2Data);
                        sw.WriteLine(player3Data);
                        sw.WriteLine(player4Data);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < p1InvestTextBoxes.Count; i++)
            {
                p1InvestTextBoxes[i].ReadOnly = false;
            }

            for (int i = 0; i < p2InvestTextBoxes.Count; i++)
            {
                p2InvestTextBoxes[i].ReadOnly = false;
            }

            for (int i = 0; i < p3InvestTextBoxes.Count; i++)
            {
                p3InvestTextBoxes[i].ReadOnly = false;
            }

            for (int i = 0; i < p4InvestTextBoxes.Count; i++)
            {
                p4InvestTextBoxes[i].ReadOnly = false;
            }

            for (int i = 0; i < playerNamesTextBoxes.Count; i++)
            {
                playerNamesTextBoxes[i].ReadOnly = false;
            }

            UpdateButton.Enabled = false;
            SaveButton.Enabled = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SavePlayerSpecificData();
            SavePlayerNames();

            for (int i = 0; i < p1InvestTextBoxes.Count; i++)
            {
                p1InvestTextBoxes[i].ReadOnly = true;
            }

            for (int i = 0; i < p2InvestTextBoxes.Count; i++)
            {
                p2InvestTextBoxes[i].ReadOnly = true;
            }

            for (int i = 0; i < p3InvestTextBoxes.Count; i++)
            {
                p3InvestTextBoxes[i].ReadOnly = true;
            }

            for (int i = 0; i < p4InvestTextBoxes.Count; i++)
            {
                p4InvestTextBoxes[i].ReadOnly = true;
            }


            for (int i = 0; i < playerNamesTextBoxes.Count; i++)
            {
                playerNamesTextBoxes[i].ReadOnly = true;
            }

            SaveButton.Enabled = false;
            UpdateButton.Enabled = true;
        }



        private void SavePlayerSpecificData()
        {
            try
            {
                if (File.Exists(player1File))
                {
                    using (StreamWriter sw = File.CreateText(player1File))
                    {
                        sw.WriteLine(p1InvestLunaris.Text);
                        sw.WriteLine(p1InvestDuremar.Text);
                        sw.WriteLine(p1InvestRames.Text);
                        sw.WriteLine(p1InvestInello.Text);
                    }
                }

                if (File.Exists(player2File))
                {
                    using (StreamWriter sw = File.CreateText(player2File))
                    {
                        sw.WriteLine(p2InvestLunaris.Text);
                        sw.WriteLine(p2InvestDuremar.Text);
                        sw.WriteLine(p2InvestRames.Text);
                        sw.WriteLine(p2InvestInello.Text);
                    }
                }

                if (File.Exists(player3File))
                {
                    using (StreamWriter sw = File.CreateText(player3File))
                    {
                        sw.WriteLine(p3InvestLunaris.Text);
                        sw.WriteLine(p3InvestDuremar.Text);
                        sw.WriteLine(p3InvestRames.Text);
                        sw.WriteLine(p3InvestInello.Text);
                    }
                }

                if (File.Exists(player4File))
                {
                    using (StreamWriter sw = File.CreateText(player4File))
                    {
                        sw.WriteLine(p4InvestLunaris.Text);
                        sw.WriteLine(p4InvestDuremar.Text);
                        sw.WriteLine(p4InvestRames.Text);
                        sw.WriteLine(p4InvestInello.Text);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        public void CreatePlayerDataFiles()
        {
            try
            {
                if (!File.Exists(player1File))
                {
                    using (StreamWriter sw = File.CreateText(player1File))
                    {
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                    }
                }

                if (!File.Exists(player2File))
                {
                    using (StreamWriter sw = File.CreateText(player2File))
                    {
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                    }
                }

                if (!File.Exists(player3File))
                {
                    using (StreamWriter sw = File.CreateText(player3File))
                    {
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                    }
                }

                if (!File.Exists(player4File))
                {
                    using (StreamWriter sw = File.CreateText(player4File))
                    {
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }


        public void CreatePlayerTotalDataFile()
        {
            try
            {
                if (!File.Exists(totalFile))
                {
                    using (StreamWriter sw = File.CreateText(totalFile))
                    {
                       
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                        sw.WriteLine("0");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        public void CreatePercentageFile()
        {
            try
            {
                if (!File.Exists(percentageFile))
                {
                    using (StreamWriter sw = File.CreateText(percentageFile))
                    {
                        //TODO: Lisää tähän ominaisuus että katsoo kuinka paljon on sijoitettu. Eli siis yhteissummat.
                        sw.WriteLine(lunarisMin.ToString());
                        sw.WriteLine(lunarisMax.ToString());
                        sw.WriteLine(duremarMin.ToString());
                        sw.WriteLine(duremarMax.ToString());                        
                        sw.WriteLine(ramesMin.ToString());
                        sw.WriteLine(ramesMax.ToString());
                        sw.WriteLine(inelloMin.ToString());
                        sw.WriteLine(inelloMax.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }


        public void ReadPlayerTotalData()
        {
            int counter = 0;
            foreach (string line in System.IO.File.ReadLines(totalFile))
            {

                playerInvestTextBoxes[counter].Text = line;
                counter++;

            }

        }

        public void ReadPercentageData()
        {
            int counter = 0;
            foreach (string line in System.IO.File.ReadLines(percentageFile))
            {

                percentageList[counter] = double.Parse(line);
                counter++;

            }
            lunarisMin = percentageList[0];
            lunarisMax = percentageList[1];
            duremarMax = percentageList[2];
            duremarMax = percentageList[3];
            ramesMax = percentageList[4];
            ramesMax = percentageList[5];
            inelloMin = percentageList[6];
            inelloMax = percentageList[7];
        }

        public void ReadPlayerSpecificData()
        {

            int counter1 = 0;
            int counter2 = 0;
            int counter3 = 0;
            int counter4 = 0;

            foreach (string line in System.IO.File.ReadLines(player1File))
            {
                p1InvestTextBoxes[counter1].Text = line;
                counter1++;
            }

            foreach (string line in System.IO.File.ReadLines(player2File))
            {
                p2InvestTextBoxes[counter2].Text = line;
                counter2++;
            }

            foreach (string line in System.IO.File.ReadLines(player3File))
            {
                p3InvestTextBoxes[counter3].Text = line;
                counter3++;
            }

            foreach (string line in System.IO.File.ReadLines(player4File))
            {
                p4InvestTextBoxes[counter4].Text = line;
                counter4++;
            }


        }


        private void PercentageButton_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<PercentagesForm>().Count() == 1)
            {
                Application.OpenForms.OfType<PercentagesForm>().First().Close();
            }

            PercentagesForm form = new PercentagesForm();
            form.Show();
        }

        private void CreatePlayerNamesFile()
        {
            try
            {
                if (!File.Exists(playerNamesFile))
                {
                    using (StreamWriter sw = File.CreateText(playerNamesFile))
                    {
                        sw.WriteLine("Testi1");
                        sw.WriteLine("Testi2");
                        sw.WriteLine("Testi3");
                        sw.WriteLine("Testi4");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }
 
        private void ReadPlayerNames()
        {
            int counter = 0;
            foreach (string line in System.IO.File.ReadLines(playerNamesFile))
            {

                playerNamesTextBoxes[counter].Text = line;
                counter++;

            }
            player1Name.Text = playerNamesTextBoxes[0].Text;
            player2Name.Text = playerNamesTextBoxes[1].Text;
            player3Name.Text = playerNamesTextBoxes[2].Text;
            player4Name.Text = playerNamesTextBoxes[3].Text;
        }

        private void SavePlayerNames()
        {
            try
            {
                if (File.Exists(playerNamesFile))
                {
                    using (StreamWriter sw = File.CreateText(playerNamesFile))
                    {
                        sw.WriteLine(playerName1.Text);
                        sw.WriteLine(playerName2.Text);
                        sw.WriteLine(playerName3.Text);
                        sw.WriteLine(playerName4.Text);
                    }

                    player1Name.Text = playerNamesTextBoxes[0].Text;
                    player2Name.Text = playerNamesTextBoxes[1].Text;
                    player3Name.Text = playerNamesTextBoxes[2].Text;
                    player4Name.Text = playerNamesTextBoxes[3].Text;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }


    }
}
