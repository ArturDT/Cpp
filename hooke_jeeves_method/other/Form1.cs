using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MetodaHookeJeevesa
{
    public partial class Form1 : Form
    {
        double  ileX1, ileX2, ileX1X2,
                x01, x02,
                p,
                r,
                e1;

        double  fX0,     //f aktualne
                fX0pe1,  //f krok próbny
                fX0re1;  //f krok roboczy
        int maxIter;

        int cykle;

        bool warunek1, warunek2;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonZnajdz_Click(object sender, EventArgs e)
        {
            textBoxConsole.Text = ""; // czyszczenie konsoli
            warunek1 = false;
            warunek2 = false;
            cykle = 0;
            ileX1 = Convert.ToDouble(textBoxIloscX1.Text);
            ileX2 = Convert.ToDouble(textBoxIloscX2.Text);
            ileX1X2 = Convert.ToDouble(textBoxIloscX2X1.Text);
            x01 = Convert.ToDouble(textBoxX01.Text);
            x02 = Convert.ToDouble(textBoxX02.Text);
            p = Convert.ToDouble(textBoxP.Text);
            r = Convert.ToDouble(textBoxR.Text);
            e1 = Convert.ToDouble(textBoxE.Text);
            maxIter = Convert.ToInt32(textBoxMaxIter.Text);
            punktStartowy();
            while (warunek1 == false && warunek2 == false)//warunki spełnienia pętli krok roboczy mniejszy od poprzedniego 
            {
                cykle++;
                warunek1 = false;
                warunek2 = false;
                krokProbny();
                krokProbnyX2();
                if (cykle >= maxIter)
                    break;

            }
            wynik();
        }

        private void punktStartowy()
        {
            fX0 = ileX1 * x01 * x01 + x02 * x02 + ileX1X2 * x01 * x02;//oblizenie pierwszego kroku(wartość)
            textBoxConsole.Text += "punkt starowy f(x0) = " + fX0 + "\r\n";
        }

        private void krokProbny()
        {
            // w kierunku x1 (e1):
            // krok polega na dodaniu do x01 wartośći p:
            fX0pe1 = ileX1 * (x01 + p) * (x01 + p) + (x02) * 
                (x02) + ileX1X2 * (x01 + p) * (x02);
            textBoxConsole.Text += "krok próbny w kierunku x1: f(x0 +pe) = " + fX0pe1 + "\r\n";
            if (fX0pe1 >= fX0)
            {
                textBoxConsole.Text += "zły krok bo  f(x0 + pe) >= f(x0) \r\n";
                textBoxConsole.Text += "krok w drugą stronę: \r\n";
                // w kierunku (-e1):
                // krok polega na odjęciu od x01 wartośći p:
                fX0pe1 = ileX1 * (x01 - p) * (x01 - p) + (x02) *
                    (x02) + ileX1X2 * (x01 - p) * (x02);
                textBoxConsole.Text += "krok próbny w kierunku x1: f(x0 - pe) = " + fX0pe1 + "\r\n";
                if (fX0pe1 < fX0) //warunek do wykonania kroku roboczego
                {
                    textBoxConsole.Text += "dobry krok bo  f(x0 - pe) < f(x0) \r\n";
                    krokRoboczy(0);
                }
                else
                {
                    warunek1 = true;
                }

            }
            else
            {
                textBoxConsole.Text += "dobry krok bo  f(x0 +pe) < f(x0) \r\n";
                krokRoboczy(1);
            }
        }

        private void krokRoboczy(int kierunek) //jesli kierunek = 1 to +p, jesli 0 to -p
        {
            //krok roboczy polega na dodaniu w odpowiedim kierunku (-p lub +p) wartości r
            if (kierunek == 0)
            {
                textBoxConsole.Text += "krok roboczy dla -p: \r\n";
                fX0re1 = ileX1 * (x01 - r) * (x01 - r) + (x02) *
                    (x02) + ileX1X2 * (x01 - r) * (x02);
                textBoxConsole.Text += "krok roboczy w kierunku x1: f(x0 - re) = " + fX0re1 + "\r\n";
                x01 -= r;
            }
            else
            {
                textBoxConsole.Text += "krok roboczy dla +p: \r\n";
                fX0re1 = ileX1 * (x01 + r) * (x01 + r) + (x02) *
                    (x02) + ileX1X2 * (x01 + r) * (x02);
                textBoxConsole.Text += "krok roboczy w kierunku x1: f(x0 + re) = " + fX0re1 + "\r\n";
                x01 += r;
            }
            fX0 = fX0re1;
        }

        private void krokProbnyX2()
        {
            // w kierunku x1 (e1):
            // krok polega na dodaniu do x02 wartośći p:
            fX0pe1 = ileX1 * (x01) * (x01) + (x02 + p) *
                (x02 + p) + ileX1X2 * (x01) * (x02 + p);
            textBoxConsole.Text += "krok próbny w kierunku x2: f(x02 +pe) = " + fX0pe1 + "\r\n";
            if (fX0pe1 >= fX0)
            {
                textBoxConsole.Text += "zły krok bo  f(x02 + pe) >= f(x0) \r\n";
                textBoxConsole.Text += "krok w drugą stronę: \r\n";
                // w kierunku  (-e1):
                // krok polega na odjęciu od x02 wartośći p:
                fX0pe1 = ileX1 * (x01) * (x01) + (x02 - p) *
                    (x02 - p) + ileX1X2 * (x01) * (x02 - p);
                textBoxConsole.Text += "krok próbny w kierunku x2: f(x02 - pe) = " + fX0pe1 + "\r\n";
                if (fX0pe1 < fX0)
                {
                    textBoxConsole.Text += "dobry krok bo  f(x02 - pe) < f(x0) \r\n";
                    krokRoboczyX2(0);
                }
                else
                {
                    warunek2 = true;
                }

            }
            else
            {
                textBoxConsole.Text += "dobry krok bo  f(x02 +pe) < f(x0) \r\n";
                krokRoboczyX2(1);
            }
        }

        private void krokRoboczyX2(int kierunek) //jesli kierunek = 1 to +p, jesli 0 to -p
        {
            //krok roboczy polega na dodaniu w odpowiedim kierunku (-p lub +p) wartości r
            if (kierunek == 0)
            {
                textBoxConsole.Text += "krok roboczy dla -p: \r\n";
                fX0re1 = ileX1 * (x01) * (x01) + (x02 - r) *
                    (x02 - r) + ileX1X2 * (x01) * (x02 - r);
                textBoxConsole.Text += "krok roboczy w kierunku x2: f(x02 - re) = " + fX0re1 + "\r\n";
                x02 -= r;
            }
            else
            {
                textBoxConsole.Text += "krok roboczy dla +p: \r\n";
                fX0re1 = ileX1 * (x01) * (x01) + (x02 + r) *
                    (x02 + r) + ileX1X2 * (x01) * (x02 + r);
                textBoxConsole.Text += "krok roboczy w kierunku x2: f(x02 + re) = " + fX0re1 + "\r\n";
                x02 += r;
            }
            fX0 = fX0re1;
        }

        private void wynik()
        {
            textBoxConsole.Text += "Znalezione minimum = " + fX0 + "\r\n";
            textBoxConsole.Text += "x01 = " + x01 + "\r\n";
            textBoxConsole.Text += "x02 = " + x02 + "\r\n";
            textBoxConsole.Text += "w " + cykle + " cyklach\r\n";
        }
    
    }
}
