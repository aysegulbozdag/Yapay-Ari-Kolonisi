using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Algoritmasi
{
    class ABC
    {
        int d, cs, l, lowLim, topLim, iteration;
        public static Random rnd = new Random();
        public double[,] swarm;
        public double[] functionValue;
        public double[] ffit;

        int lCount = 0;
        public ABC()
        {

        }

        public ABC(int CS, int D, int LowLim, int TopLim, int Iteration)
        {
            //this.cs = cs;
            //this.d = d;
            //this.l = l;
            //this.lowLim = lowLim;
            //this.topLim = topLim;
            cs = CS;
            d = D;
            l = (cs * d) / 2;
            lowLim = LowLim;
            topLim = TopLim;
            iteration = Iteration;

            int iterationCount = 0;

            while (iteration > iterationCount)
            {
                createSwarm(cs, d, lowLim, topLim);
                workerBeePhase();
                observerBeePhase();

                if (lCount >= l)
                {
                    l = (cs * d) / 2;
                    createSwarm(cs, d, lowLim, topLim);
                    //workerBeePhase();
                    //observerBeePhase();
                }
                iterationCount++;
            }
        }

        //public  int CS
        //{
        //    get { return cs; }
        //    set { cs = value; }
        //}

        //Sürü oluşturuldu.
        public void createSwarm(int cs, int d, int lowLim, int topLim)
        {
            swarm = new double[cs, d];
            for (int i = 0; i < cs; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    //Alt limit ve üst limit arasında, random olarak sürü oluşturuldu.
                    swarm[i, j] = ((topLim - lowLim) * rnd.NextDouble()) + lowLim;
                }
            }


            //for (int i = 0; i < cs; i++)
            //{
            //    function(swarm[i, 0], swarm[i, 1]);
            //}
            functionValue = new double[cs];
            ffit = new double[cs];
            function(functionValue, swarm);//Amaç fonksiyonu ile uygunluk değerleri hesaplanıldı.
            calculateFit(ffit, functionValue);//Uygunluk değerleri  ile Fit değerleri hesaplanır. 
        }

        //Okul numaramın sonu 32 olduğundan 14. fonksiyon hesaplanıldı.
        public List<double> list = new List<double>();
        public void function(double[] objArray, double[,] hesaplanılacakArray)
        {
            //Array.Clear(objArray, 0, objArray.Length);
            for (int i = 0; i < cs; i++)
            {
                objArray[i] = -(Math.Abs(Math.Sin(hesaplanılacakArray[i, 0]) * Math.Cos(hesaplanılacakArray[i, 1])
                * Math.Exp(Math.Abs(1 - (Math.Sqrt(Math.Pow(hesaplanılacakArray[i, 0], 2) + Math.Pow(hesaplanılacakArray[i, 1], 2) / Math.PI))))));
            }
            //list.Add( -(Math.Abs(Math.Sin(x1) * Math.Cos(x2)
            //   * Math.Exp(Math.Abs(1 - (Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(x2, 2) / Math.PI)))))));

        }

        public void calculateFit(double[] fit, double[] hesaplanılacakArray)
        {

            for (int i = 0; i < cs; i++)
            {
                if (hesaplanılacakArray[i] >= 0)
                {
                    fit[i] = 1 / (1 + hesaplanılacakArray[i]);
                }

                else if (hesaplanılacakArray[i] < 0)
                {
                    fit[i] = 1 + Math.Abs(hesaplanılacakArray[i]);
                }
            }
            //Burada hesaplanan fit.Max değeri en kaliteli besin kaynağıdır.   
        }

        int k, j;
        double fi;
        double[] vFit;
        // k,  rastgele seçilen komşu
        // j, rastgele seçilen parametre
        //fi, [-1,1] arasından rastgele seçilen sayı
        // Vi,j = Xi,j + Fİi,j  * ( Xi,j - Xk,j)

        double[,] v;
        public void workerBeePhase()
        {
            v = new double[cs, d];
            vFit = new double[cs];

            for (int i = 0; i < cs; i++)
            {
                k = rnd.Next(0, cs);
                j = rnd.Next(0, d);
                fi = (1 - (-1) * rnd.NextDouble()) + (-1);

                v[i, j] = swarm[i, j] + fi * (swarm[i, j] - swarm[k, j]);
            }

            for (int i = 0; i < cs; i++)
            {
                for (int m = 0; m < d; m++)
                {
                    if (v[i, m] == 0)
                    {
                        v[i, m] = swarm[i, m];
                    }
                }

            }
            double[] newObjOfV = new double[cs];
            function(newObjOfV, v);
            calculateFit(vFit, newObjOfV);

            for (int i = 0; i < cs; i++)
            {

                if (vFit[i] > ffit[i])
                {
                    swarm[i, j] = v[i, j];
                    lCount = 0;
                }
                else
                {
                    lCount++;
                }
            }
            function(functionValue, swarm);
            calculateFit(ffit, functionValue);
        }

        int selectedIndex = 0;
        public void selectionPlan()
        {
            double[] p = new double[cs];
            double sum = 0.0;
            sum = ffit.Sum();

            for (int i = 0; i < cs; i++)
            {
                p[i] = ffit[i] / sum;
            }
            double pickRnd = rnd.NextDouble();// p.Sum değeri her zaman 1 olacaktır. Bu yüzden 0 ile 1 arasında bir değer alınıyor.

            double currentValue = 0.0;

            for (int i = 0; i < p.Length; i++)
            {
                currentValue += p[i];
                if (currentValue > pickRnd)
                {
                    selectedIndex = i;
                    break;//Şart sağlandıktan sonra döngüden çıkılmalı çünkü bundan sonrakiler zaten şartı sağlayacaktır.
                }
            }
        }
        public double bestofValue;
        public void observerBeePhase()
        {

            for (int i = 0; i < cs; i++)
            {
                k = rnd.Next(0, cs);
                j = rnd.Next(0, d);
                fi = (1 - (-1) * rnd.NextDouble()) + (-1);
                selectionPlan();
                v[selectedIndex, j] = swarm[selectedIndex, j] + fi * (swarm[selectedIndex, j] - swarm[k, j]);
            }

            double[] newObjOfV = new double[cs];
            function(newObjOfV, v);
            calculateFit(vFit, newObjOfV);

            for (int i = 0; i < cs; i++)
            {

                if (vFit[i] > ffit[i])
                {
                    swarm[i, j] = v[i, j];
                    lCount = 0;
                    // break;
                }
                else
                {
                    lCount++;
                }
            }
            function(functionValue, swarm);
            calculateFit(ffit, functionValue);

            bestofValue = ffit.Max();
            int bestofIndex = Array.IndexOf(ffit, bestofValue);

            //for (int i = 0; i < d; i++)
            //{
            //    list.Add(swarm[bestofIndex, i]);
            //}
            list.Add(bestofValue);
        }

    }
}
