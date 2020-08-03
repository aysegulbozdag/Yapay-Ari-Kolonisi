using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_Algoritmasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int cs, d, iteration, lowLim, topLim;
        ABC abc;
        ABC abc1 = new ABC();
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            getValue();
            abc = new ABC(cs, d, lowLim, topLim, iteration);
            drawChart();
            dataTable();
        }


        public void getValue()
        {
            cs = Convert.ToInt32(txtCs.Text);
            d = Convert.ToInt32(txtD.Text);
            iteration = Convert.ToInt32(txtL.Text);
            lowLim = Convert.ToInt32(txtLow.Text);
            topLim = Convert.ToInt32(txtTop.Text);

        }
        List<double> list = new List<double>();
        double[] bestofValueArray;
        public void drawChart()
        {
            bestofValueArray = new double[iteration];
            for (int i = 0; i < abc.list.Count; i++)
            {
                bestofValueArray[i] = abc.list[i];

            }

            //chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";
            //Array.Sort(bestofValueArray);
            //  chart1.Series.Clear();
            for (int i = 0; i < iteration; i++)
            {
                chart1.Series["En iyi iterasyon değeri"].Points.AddXY(i + 1, bestofValueArray[i]);
            }
        }

        public void dataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("İterasyon");
            dt.Columns.Add("En iyi değerler");
            //for (int i = 0; i < d; i++)
            //{
            //    dt.Columns.Add("x" + (i + 1));
            //}

            //dt.Columns.Add("Uygunluk değeri");
            //dt.Columns.Add("Fit değeri");

            for (int i = 0; i < iteration; i++)
            {
                //   dt.Rows.Add(i + 1, abc.swarm[i, 0], abc.swarm[i, 1], abc.functionValue[i], abc.ffit[i]);
                dt.Rows.Add(i + 1, bestofValueArray[i]);
            }
            dataGridView1.DataSource = dt;
        }
    }
}
