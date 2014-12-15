using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class GeneralHistogram
    {
        /// <summary>
        /// 
        /// min+(delta/2)
        ///     ^
        ///     |
        ///     |   delta
        ///  |-----|-----| ... |-----|
        /// min                     max
        ///   cell0 cell1
        /// 
        /// [min,min+delta) --> cell0 --> represented as min+(delta/2)
        /// [min+delta, min+2delta) --> cell1 --> represented as min+delta+(delta/2)
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="res"></param>
        /// <param name="histXMin"> </param>
        /// <param name="histXMax"> </param>
        /// <returns></returns>
        public static Dictionary<double,int > MakeHistogram(IList<double> values, double min, double max, int res, out double histXMin, out double histXMax)
        {
            int[] cells = new int[res];
            for (int i = 0; i < res; i++)
            {
                cells[i] = 0;
            }
            double delta = (max - min) / res;
            foreach (double value in values)
            {
                int cell = value == max ? res - 1 : (int)Math.Floor((value - min) / delta);
                cells[cell]++;
            }

            // foreach delta/2 (starting from min+(delta/2)) represent the count
            Dictionary<double,int> hist = new Dictionary<double, int>(res);
            double offset = min + 0.5*delta;
            for (int i = 0; i < res; i++)
            {
                double x = offset + i*delta;
                hist[x] = cells[i];
            }
            histXMin = offset;
            histXMax = offset + (res - 1)*delta;
            return hist;
        }

        public static Dictionary<double,Dictionary<double,double >> MakeHeatMap(
            IList<double> xvalues, IList<double> yvalues, double xmin, double xmax,
            double ymin, double ymax, int xres, int yres, double alpha)
        {
            double deltax = (xmax - xmin)/xres;
            double deltay = (ymax - ymin)/yres;
            
            // Initializing cells
            int [][] cells = new int[yres][];
            for (int i = 0; i < yres; i++)
            {
                cells[i] = new int[xres];
                for (int j = 0; j < xres; j++)
                {
                    cells[i][j] = 0;
                }
            }

            // Filling cells
            int size = xvalues.Count;
            int cellmax = 0;
            for (int i = 0; i < size; i++)
            {
                double x = xvalues[i];
                double y = yvalues[i];

                int cellx = x == xmax ? xres - 1 : (int)Math.Floor((x - xmin) / deltax);
                int celly = y == ymax ? yres - 1 : (int)Math.Floor((y - ymin) / deltay);

                if (x > xmax || y > ymax || x < xmin || y < ymin)
                {
                    // now this should never be reached
                    throw new Exception("bad(1)-> x: " + x + " y: " + y + " xmax: " + xmax + " xmin: " + xmin + " ymax: " +
                                        ymax + " ymin: " + ymin);
                }

                if (cellx >= xres || celly >= yres)
                {
                    // now this should never be reached
                    throw new Exception("bad(2)-> x: " + x + " y:" + y + " xmax: " + xmax + " xmin: " + xmin + " ymax: " +
                                        ymax + " ymin: " + ymin + " cellx: " + cellx + " celly: " + celly);
                }

                ++cells[celly][cellx];
                if (cells[celly][cellx] > cellmax)
                {
                    cellmax = cells[celly][cellx];
                }
            }

            double cellmean = ((double) size)/(xres*yres);
            double power = cellmax < (alpha*cellmean) ? 1.0 : (Math.Log(alpha)/Math.Log(cellmax/cellmean));
            double c = 1.0/cellmax;

            int xpointcount = 2*xres;
            int ypointcount = 2*yres;

            Dictionary<double, Dictionary<double, double >> heatmap =
                new Dictionary<double, Dictionary<double, double>>(xpointcount);

            for (int i = 0; i < xpointcount; i++)
            {
                double x = xmin + ((IsOdd(i) ? (i + 1) / 2 : i / 2) * deltax);
                int cellx = IsOdd(i) ? (i - 1)/2 : i/2;

                for (int j = 0; j < ypointcount; j++)
                {
                    double y = ymin + ((IsOdd(j) ? (j + 1) / 2 : j / 2) * deltay);
                    int celly = IsOdd(j) ? (j - 1) / 2 : j / 2;

                    double cellvalue = Math.Pow((cells[celly][cellx] * c), power);

                    if (heatmap.ContainsKey(x))
                    {
                        heatmap[x][y] = cellvalue;
                    }
                }
            }
            return heatmap;
        }

        private static bool IsOdd(int value)
        {
            return (value & 1) == 1;
        }
    }
}