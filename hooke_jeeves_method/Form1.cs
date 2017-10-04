/* Artur Dobrzanski */ 


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hooke_jeeves
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //int NVARS = 250;	/* liczba zmiennych	     */
        double RHO = 0.5;	/* Krok geometryczny */
        double EPSMIN = 1E-6;	/* Końcowa wartość kroku  */
        int IMAX = Liter;	/* liczba iteracji	     */
        int funevals = 0;

        public double f(double[] x, int n)
        {

            double a, b, c;
            funevals++;
            a = x[0];
            b = x[1];
            c = 100.0 * (b - (a * a)) * (b - (a * a));
            return (c + ((3.0 - a) * (3.0 - a)));
        }

        double best_nearby(double[] delta, double[] point, double prevbest, int nvars)
        {
            double[] z;
            double minf, ftmp;
            int i;
            z = new double[1000];
            minf = prevbest;
            for (i = 0; i < nvars; i++)
                z[i] = point[i];
            for (i = 0; i < nvars; i++)
            {
                z[i] = point[i] + delta[i];
                ftmp = f(z, nvars);
                if (ftmp < minf)
                    minf = ftmp;
                else
                {
                    delta[i] = 0.0 - delta[i];
                    z[i] = point[i] + delta[i];
                    ftmp = f(z, nvars);
                    if (ftmp < minf)
                        minf = ftmp;
                    else
                        z[i] = point[i];
                }
            }
            for (i = 0; i < nvars; i++)
                point[i] = z[i];
            return (minf);
        }

        int hooke(int nvars, double[] startpt, double[] endpt, double rho, double epsilon, int itermax)
        {
            double[] delta;
            double newf, fbefore, steplength, tmp;
            double[] xbefore, newx;
            int i, j, keep;
            int iters, iadj;
            delta = new double[1000];
            xbefore = new double[1000];
            newx = new double[1000];
            for (i = 0; i < nvars; i++)
            {
                newx[i] = xbefore[i] = startpt[i];
                delta[i] = Math.Abs(startpt[i] * rho);
                if (delta[i] == 0.0)
                    delta[i] = rho;
            }
            iadj = 0;
            steplength = rho;
            iters = 0;
            fbefore = f(newx, nvars);
            newf = fbefore;
            while ((iters < itermax) && (steplength > epsilon))
            {
                iters++;
                iadj++;
                //MessageBox.Show("After %5d funevals, f(x) = " + funevals.ToString()+ fbefore.ToString());
                for (j = 0; j < nvars; j++)
                    //printf("   x[%2d] = %.4le\n", j, xbefore[j]);
                    /* Znajdowanie najlepszego punktu  */
                    for (i = 0; i < nvars; i++)
                    {
                        newx[i] = xbefore[i];
                    }
                newf = best_nearby(delta, newx, fbefore, nvars);
                /* Jeśli pónkt jest dobry szukaj dalej w tym kierunku */
                keep = 1;
                while ((newf < fbefore) && (keep == 1))
                {
                    iadj = 0;
                    for (i = 0; i < nvars; i++)
                    {
                        /* pierwsze określenie kierunku */
                        if (newx[i] <= xbefore[i])
                            delta[i] = 0.0 - Math.Abs(delta[i]);
                        else
                            delta[i] = Math.Abs(delta[i]);
                        /* Dążenie dalej w tym kierunku */
                        tmp = xbefore[i];
                        xbefore[i] = newx[i];
                        newx[i] = newx[i] + newx[i] - tmp;
                    }
                    fbefore = newf;
                    newf = best_nearby(delta, newx, fbefore, nvars);
                    /* 
                     jeśli dalej wartość rośnie zły ruch*/
                    if (newf >= fbefore)
                        break;

                    keep = 0;
                    for (i = 0; i < nvars; i++)
                    {
                        keep = 1;
                        if (Math.Abs(newx[i] - xbefore[i]) >
                            (0.5 * Math.Abs(delta[i])))
                            break;
                        else
                            keep = 0;
                    }
                }
                if ((steplength >= epsilon) && (newf >= fbefore))
                {
                    steplength = steplength * rho;
                    for (i = 0; i < nvars; i++)
                    {
                        delta[i] *= rho;
                    }
                }
            }
            for (i = 0; i < nvars; i++)
                endpt[i] = xbefore[i];
            //MessageBox.Show(nvars.ToString()+ "   " + newx[0].ToString());
            return (iters);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] startpt, endpt;
            int nvars, itermax;
            double rho, epsilon;
            int i, jj;
            startpt = new double[2];
            endpt = new double[2];

            /* Domyślne punkty dla funkcji (testowanie) */
            nvars = 2;
            startpt[0] = -1.2;
            startpt[1] = 1.0;



            itermax = Liter;
            rho = RHO;
            epsilon = EPSMIN;
            jj = hooke(nvars, startpt, endpt, rho, epsilon, itermax);
            MessageBox.Show("\n\n\nHOOKE USED %d ITERATIONS, AND RETURNED\n" + jj.ToString());
            iter.Text = jj.ToString();
            for (i = 0; i < nvars; i++)
                MessageBox.Show("x[%3d] = %15.7le \n" + i.ToString() + "   " + endpt[i].ToString());
            max.Text = endpt[i].ToString(); 

        }

        


      
    }
}
