using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LunariStock;


namespace LunariStock
{
    
    public partial class PercentagesForm : Form
    {
        string percentageFile = "percentages.txt";

        public PercentagesForm()
        {
            InitializeComponent();
            LunarisMinValue.Value = (decimal)lunariStockForm.lunarisMin;
            LunarisMaxValue.Value = (decimal)lunariStockForm.lunarisMax;
            DuremarMinValue.Value = (decimal)lunariStockForm.duremarMin;
            DuremarMaxValue.Value = (decimal)lunariStockForm.duremarMax;
            RamesMinValue.Value = (decimal)lunariStockForm.ramesMin;
            RamesMaxValue.Value = (decimal)lunariStockForm.ramesMax;
            InelloMinValue.Value = (decimal)lunariStockForm.inelloMin;
            InelloMaxValue.Value = (decimal)lunariStockForm.inelloMax;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            //Lunaris percentages
            lunariStockForm.lunarisMin = decimal.ToDouble(LunarisMinValue.Value);
            lunariStockForm.lunarisMax = decimal.ToDouble(LunarisMaxValue.Value);

            //Duremar percentages
            lunariStockForm.duremarMin = decimal.ToDouble(DuremarMinValue.Value);
            lunariStockForm.duremarMax = decimal.ToDouble(DuremarMaxValue.Value);

            //Rames percentages
            lunariStockForm.ramesMin = decimal.ToDouble(RamesMinValue.Value);
            lunariStockForm.ramesMax = decimal.ToDouble(RamesMaxValue.Value);

            //Inello percentages
            lunariStockForm.inelloMin = decimal.ToDouble(InelloMinValue.Value);
            lunariStockForm.inelloMax = decimal.ToDouble(InelloMaxValue.Value);


            SavePercentageData();

            this.Close();
        }


        private void SavePercentageData()
        {

            try
            {
                if (File.Exists(percentageFile))
                {
                    using (StreamWriter sw = File.CreateText(percentageFile))
                    {
                        sw.WriteLine(LunarisMinValue.Value.ToString());
                        sw.WriteLine(LunarisMaxValue.Value.ToString());
                        sw.WriteLine(DuremarMinValue.Value.ToString());
                        sw.WriteLine(DuremarMaxValue.Value.ToString());
                        sw.WriteLine(RamesMinValue.Value.ToString());
                        sw.WriteLine(RamesMaxValue.Value.ToString());
                        sw.WriteLine(InelloMinValue.Value.ToString());
                        sw.WriteLine(InelloMaxValue.Value.ToString());
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
    }
}
