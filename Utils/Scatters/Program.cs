using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scatters
{
    class PointComparer : IComparer
    {
        public int Compare(object one, object two)
        {
            Point pone = (Point) one;
            Point ptwo = (Point) two;
            if (pone.X < ptwo.X) return -1;
            if (pone.X == ptwo.X) return 0;
            return 1;
        }
    }

    struct Point
    {
        private double x;
        private double y;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Program
    {
        

        static void Main(string[] args)
        {
            /*
             * dataDir	G:\SugarSyncSharedByMe\SalsaBio\cog\100k\manxcat\nw\sqrt4d\meandistance\perclusdistances
             * dataFilePrefix	distances_for_cluster
             * 	0.0
             * xmax	0.3
             * ymin	0.0
             * ymax	0.3
             * xres	50
             * yres	50
             * mult	100
             * alpha	2
             * method	count
             * xdiv 4
             */
            using (StreamReader reader = new StreamReader(args[0]))
            {
                char [] sep = new[]{' ','\t'};
                /* Reading parameters file */

                // Data directory
                string dataDir = reader.ReadLine().Trim().Split(sep)[1];

                // Data file prefix. Content of the data file should be idx<sep>xvalue<sep>yvalue
                string dataFilePrefix = reader.ReadLine().Trim().Split(sep)[1];
                
                /* Probably these should not be read in, instead automatically done*/
                /*
                double xmin = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                double xmax = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                double ymin = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                double ymax = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                 */

                double xmaxbound = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                double ymaxbound = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                int xres = int.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                int yres = int.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                double mult = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                double alpha = double.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                string method = reader.ReadLine().Trim().Split(sep)[1];
                int xdiv = int.Parse(reader.ReadLine().Trim().Split(sep)[1]);
                string value = reader.ReadLine().Trim().Split(sep)[1];
                double pcutf = "+inf".Equals(value) ? double.MaxValue : double.Parse(value);
                value = reader.ReadLine().Trim().Split(sep)[1];
                double ncutf = "-inf".Equals(value) ? double.MinValue : double.Parse(value);
                value = reader.ReadLine().Trim().Split(sep)[1];
                bool zto1 = bool.Parse(value);

                string[] dataFiles = Directory.GetFiles(dataDir, dataFilePrefix + "*.txt");
                string outFile;
                foreach (string dataFile in dataFiles)
                {
                    using (StreamReader dataReader = new StreamReader(dataFile))
                    {
                        double ymin = double.MaxValue;
                        double ymax = double.MinValue;

                        ArrayList points = new ArrayList();
                        string[] splits;
                        int count = 0;
                        while (!dataReader.EndOfStream)
                        {
                            string line = dataReader.ReadLine();
                            if (!string.IsNullOrEmpty(line) && line.StartsWith("#"))
                            {
                                continue; // ignore comment lines
                            }
                            splits = line.Trim().Split(sep);

                            Point point = splits.Length == 3 ? new Point(double.Parse(splits[1]), double.Parse(splits[2])) : new Point(double.Parse(splits[0]), double.Parse(splits[1]));
                            if (point.Y < ymin)
                            {
                                ymin = point.Y;
                            }

                            if (point.Y > ymax)
                            {
                                ymax = point.Y;
                            }
                            points.Add(point);
                            ++count;
                        }

                        // Sort points by their x values
                        points.Sort(new PointComparer());

                        int xdivsize = count/xdiv;
                        int remainder = count%xdiv;

                        int pos = 0, startIdx, endIdx;
                        for (int r = 0; r < xdiv; ++r)
                        {
                            startIdx = pos;
                            int size = remainder-- > 0 ? xdivsize + 1 : xdivsize;
                            double xmin = ((Point) points[startIdx]).X;

                            pos += (size - 1);
                            endIdx = pos;
                            double xmax = ((Point) points[endIdx]).X;

                            outFile = Path.Combine(dataDir,
                                                   "scatter_hist_" + Path.GetFileNameWithoutExtension(dataFile) +
                                                   "_" + r + ".txt");
                            HistogramScatter(points, startIdx, endIdx, outFile, xmin, xmax, ymin, ymax, xres, yres, mult,
                                             alpha, method, pcutf, ncutf, zto1, xmaxbound, ymaxbound);
                            ++pos;
                        }
                    }
                    
                }
            }
        }

        private static void HistogramScatter(ArrayList points, int startIdx, int endIdx, string outFile, double xmin, double xmax, double ymin, double ymax, 
            int xres, int yres, double mult, double alpha,string method, double pcutf, double ncutf, bool zto1, double xmaxbound, double ymaxbound)
        {
            double deltax = (xmax - xmin) / xres;
            double deltay = (ymax - ymin) / yres;

            // Surface area of each small 2D square
            double deltas = deltax * deltay;


            long count = 0, cellmax = 0;
            double cellmean = 0.0;
            Hashtable cells = new Hashtable();
            string cell;

            int cellx, celly;
            double x, y;

            for (int i = startIdx; i <= endIdx; ++i)
            {
                // original x and y values
                x = ((Point)points[i]).X;
                y = ((Point)points[i]).Y;

                ++count;

                // cell number based on zero index from bottom left corner
                // if x is equal to xmax then it's placed in the last cell, which is xres-1 in zero based index
                // same is done for y when y == ymax
                cellx = x == xmax ? xres - 1 : (int) Math.Floor((x - xmin)/deltax);
                celly = y == ymax ? yres - 1 : (int) Math.Floor((y - ymin)/deltay);

                // Note. test code
                if (cellx >= xres || celly >= yres)
                {
                    // now this should never be reached
                    Console.WriteLine("bad");
                }

                cell = cellx + "," + celly;
                cells[cell] = cells.ContainsKey(cell) ? ((long)cells[cell]) + 1 : 1L;
                if ((long)cells[cell] > cellmax)
                {
                    cellmax = (long)cells[cell];
                }
            }
            cellmean = ((double) count)/(xres*yres);

            double power = cellmax < (alpha*cellmean) ? 1.0 : (Math.Log(alpha)/Math.Log(cellmax/cellmean));

            // Constant value by which the number of points in a 2D square is multiplied.
            // The resulting value is independent of the total number of points as well as 
            // the x,y resolution. The mult value is a factor changing the z value scale.
            double c = zto1 ? (1.0/cellmax) :  (1.0 / (count * deltas));

            int xpointcount = 2*xres;
            int ypointcount = 2*yres;

            using (StreamWriter writer = new StreamWriter(outFile))
            {
                writer.WriteLine("#xcoord\tycoord\thistogramValue");
                double cellvalue;
                for (int i = 0; i < xpointcount; i++)
                {
                    x = xmin + ((Odd(i) ? (i + 1) / 2 : i / 2) * deltax);
                    cellx = Odd(i) ? (i - 1)/2 : i/2;
                    
                    for (int j = 0; j < ypointcount; j++)
                    {
                        y = ymin + ((Odd(j) ? (j + 1)/2 : j/2)*deltay);
                        celly = Odd(j) ? (j - 1)/2 : j/2;
                        cell = cellx + "," + celly;

                        if ("count".Equals(method))
                        {
                            cellvalue = (cells.ContainsKey(cell) ? (long)cells[cell] : 0L) * c;
                        }
                        else if ("log".Equals(method))
                        {
                            cellvalue = Math.Log((cells.ContainsKey(cell) ? (long) cells[cell] : 0L)*c);
                        }
                        else if ("power".Equals(method))
                        {
                            cellvalue = Math.Pow(((cells.ContainsKey(cell) ? (long) cells[cell] : 0L)*c), power);
                            cellvalue = cellvalue > pcutf ? pcutf : cellvalue < ncutf ? ncutf : cellvalue;
                        }
                        else
                        {
                            // Default is count method
                            cellvalue = (cells.ContainsKey(cell) ? (long)cells[cell] : 0L) * c;
                        }

                        writer.WriteLine(x + "\t" + y + "\t" + cellvalue);
                    }
                    writer.WriteLine();
                }

                // Fill up the remaining region from beyond x=xmax and y=ymax as zero 
                writer.WriteLine();
                writer.WriteLine(xmin + "\t" + ymax + "\t" + 0.0);
                writer.WriteLine(xmin + "\t" + ymaxbound + "\t" + 0.0);
                writer.WriteLine();
                writer.WriteLine(xmax + "\t" + ymax + "\t" + 0.0);
                writer.WriteLine(xmax + "\t" + ymaxbound + "\t" + 0.0);
                writer.WriteLine();
                writer.WriteLine(xmax + "\t" + ymin + "\t" + 0.0);
                writer.WriteLine(xmax + "\t" + ymaxbound + "\t" + 0.0);
                writer.WriteLine();
                writer.WriteLine(xmaxbound + "\t" + ymin + "\t" + 0.0);
                writer.WriteLine(xmaxbound + "\t" + ymaxbound + "\t" + 0.0);
            }
        }

        private static  bool Odd(int x)
        {
            return x%2 != 0;
        }

        /*
         * This simply produce a checkerboard (a grid). The average of the z values of four corners of each square represents the density for that square.
         * 
        private static void HistogramScatter(string dataFile, string outFile, double xmin, double xmax, double ymin, double ymax, int xres, int yres, double mult)
        {
            char [] sep = new[]{' ', '\t'};
            double deltax = (xmax - xmin)/xres;
            double deltay = (ymax - ymin)/yres;

            // Surface area of each small 2D square
            double deltas = deltax*deltay;

            
            long count = 0, cellmax = 0;
            Hashtable cells = new Hashtable();
            string cell;
            using (StreamReader reader = new StreamReader(dataFile))
            {
                string[] splits;
                double x, y;
                int cellx, celly;
                while (!reader.EndOfStream)
                {
                    splits = reader.ReadLine().Trim().Split(sep);
                    if (splits[0].StartsWith("#")) continue; // ignore comment lines
                    
                    // original x and y values
                    x = double.Parse(splits[1]);
                    y = double.Parse(splits[2]);

                    ++count;

                    // cell number based on zero index from bottom left corner
                    cellx = (int) Math.Floor((x - xmin)/deltax);
                    celly = (int) Math.Floor((y - ymin)/deltay);

                    // Note. test code
                    if (cellx >= xres || celly >= yres)
                    {
                        Console.WriteLine("bad");
                    }

                    cell = cellx + "," + celly;
                    cells[cell] = cells.ContainsKey(cell) ? ((long) cells[cell]) + 1 : 1L;
                    if ((long)cells[cell] > cellmax)
                    {
                        cellmax = (long) cells[cell];
                    }
                }
            }

            // Constant value by which the number of points in a 2D square is multiplied.
            // The resulting value is independent of the total number of points as well as 
            // the x,y resolution. The mult value is a factor changing the z value scale.
            double c = mult/(count*deltas*(cellmax > 0 ? cellmax : 1));
//             Note. test code
//            double c = 1;


            int xpointcount = xres + 1;
            int ypointcount = yres + 1;
            
            double [][] coords = new double[xpointcount][];
            for (int i = 0; i < xpointcount; ++i)
            {
                coords[i] = new double[ypointcount];
            }

            using (StreamWriter writer = new StreamWriter(outFile))
            {
                writer.WriteLine("#xcoord\tycoord\thistogramValue");
                double cellvalue, temp;
                for (int i = 0; i < xres; i++)
                {
                    for (int j = 0; j < yres; j++)
                    {
                        cell = i + "," + j;
                        cellvalue = (cells.ContainsKey(cell) ? (long) cells[cell] : 0L) * c;
                        if (i == 0 && j == 0)
                        {
                            // bottom-left cell
                            coords[i][j] = coords[i][j + 1] = coords[i + 1][j]  = coords[i + 1][j + 1] = cellvalue;
                        }
                        else if (i == 0 && j > 0)
                        {
                            // cells in left most edge except bottom-left cell
                            temp = (cellvalue*4 - (coords[i][j] + coords[i + 1][j]))/2;
                            coords[i][j + 1] = coords[i + 1][j + 1] = temp;
                        }
                        else if (i > 0 && j == 0)
                        {
                            // cells in the bottom most edge except bottom-left cell
                            temp = (cellvalue*4 - (coords[i][j] + coords[i][j + 1]))/2;
                            coords[i + 1][j] = coords[i + 1][j + 1] = temp;
                        }
                        else
                        {
                            // any other cell
                            temp = (cellvalue*4 - (coords[i][j] + coords[i][j + 1] + coords[i + 1][j]));
                            coords[i + 1][j + 1] = temp;
                        }
                        writer.WriteLine((xmin + i*deltax) +"\t" + (ymin+j*deltay) + "\t" + coords[i][j]);
                    }
                    writer.WriteLine((xmin + i * deltax) + "\t" + ymax + "\t" + coords[i][yres]);
                    writer.WriteLine();
                }

                for (int j = 0; j < ypointcount; ++j)
                {
                    writer.WriteLine(xmax + "\t" + (ymin + j * deltay) + "\t" + coords[xres][j]);
                }
            }
        }*/
    }
}
