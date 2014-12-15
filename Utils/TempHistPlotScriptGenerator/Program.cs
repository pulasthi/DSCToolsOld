using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TempHistPlotScriptGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string paramFile = args[0];
            using (StreamReader reader = new StreamReader(paramFile))
            {
                char [] sep = new []{' ','\t'};
                string dir = reader.ReadLine().Trim().Split(sep)[1];
                reader.ReadLine(); // ignore comment line
                Hashtable names = new Hashtable();
                while (!reader.EndOfStream)
                {
                    string[] splits = reader.ReadLine().Trim().Split(sep);
                    names.Add(splits[0], new []{splits[1],splits[2]});
                }

                StringBuilder sb = GeneratePlotScriptBegin();
                
                string[] files = Directory.GetFiles(dir, "selected*.txt");
                Hashtable triples = FormTriples(files);
                WriteScripts(dir, triples, names, "Seven", sb.ToString());

                files = Directory.GetFiles(dir, "whole*.txt");
                triples = FormTriples(files);
                WriteScripts(dir, triples, names, "All", sb.ToString());


//                ProcessFiles(files, sb, "Seven", names);
//                files = Directory.GetFiles(dir, "whole*.txt");
//                ProcessFiles(files, sb, "All", names);
//
//                string scriptFile = Path.Combine(dir, "hist_script.txt");
//                File.WriteAllText(scriptFile, sb.ToString());

                Console.WriteLine("Done.");
                Console.Read();
            }
        }

        private static void WriteScripts(string dir, Hashtable triples, Hashtable names, string clusters, string header)
        {
            foreach (DictionaryEntry kv in triples)
            {
                string prefix = ((string) kv.Key);
                using (StreamWriter writer = new StreamWriter(Path.Combine(dir, prefix + "-Blast-Clus[" + clusters + "]-gnuplot.txt")))
                {
                    writer.WriteLine(header);
                    string[] tripplefiles = ((string[]) kv.Value);
                    string[] nameitems = (string[]) names[prefix];

                    string pngfile = prefix + "-Blast-Clus[" + clusters + "]-Sat[0.85]-" + nameitems[1] +
                                    "-Euclidean-Vs-" +
                                    nameitems[0] + ".png";

                    writer.WriteLine("set output '" + pngfile + "'");
                    writer.WriteLine("set size 1.0, 1.0");
                    writer.WriteLine("set multiplot");
                    
                    writer.WriteLine();
                    
                    // Y histogram (rotated)
                    writer.WriteLine("set origin 0.0, 0.5");
                    writer.WriteLine("set size 0.5, 0.5");
                    writer.WriteLine("set xtics rotate by -90");
                    string xlabel = "Count";
                    writer.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"white\"");
                    string ylabel = nameitems[1] + "-Euclidean-of-" + nameitems[0]; 
                    writer.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"white\"");
                    string title = prefix + "-Blast-Clus[" + clusters + "]-(rotated)-Y-Histogram \\n of \\n " + nameitems[1] + "-Euclidean-of-" +
                                   nameitems[0];
                    writer.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"white\"");
                    writer.WriteLine("plot '" + tripplefiles[0] + "' using 2:1 with filledcurves y1");

                    writer.WriteLine("set xtics rotate by 0");
                    writer.WriteLine();

                    // Density plot
                    writer.WriteLine("set origin 0.5, 0.5");
                    writer.WriteLine("set size 0.5, 0.5");
                    xlabel = nameitems[0];
                    writer.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"white\"");
                    ylabel = nameitems[1] + "-Euclidean-of-" + nameitems[0];
                    writer.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"white\"");
                    title = prefix + "-Blast-Clus[" + clusters + "]-Sat[0.85]-" + nameitems[1] + "-Euclidean \\n Vs " +
                            nameitems[0];
                    writer.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"white\"");
                    writer.WriteLine("splot [0:1.8] [0:1.8] '" + tripplefiles[1] + "'");


                    writer.WriteLine();

                    // Y histogram (unrotated)
                    writer.WriteLine("set origin 0.0, 0.0");
                    writer.WriteLine("set size 0.5, 0.5");
                    xlabel = nameitems[1] + "-Euclidean-of-" + nameitems[0]; 
                    writer.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"white\"");
                    ylabel = "Count";
                    writer.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"white\"");
                    title = prefix + "-Blast-Clus[" + clusters + "]-Y-Histogram \\n of \\n " + nameitems[1] + "-Euclidean-of-" +
                                   nameitems[0];
                    writer.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"white\"");
                    writer.WriteLine("plot '" + tripplefiles[0] + "' with filledcurves x1");


                    writer.WriteLine();

                    // X histogram
                    writer.WriteLine("set origin 0.5, 0.0");
                    writer.WriteLine("set size 0.5, 0.5");
                    xlabel = nameitems[0];
                    writer.WriteLine("set xlabel \"" + xlabel + "\" textcolor rgbcolor \"white\"");
                    ylabel = "Count";
                    writer.WriteLine("set ylabel \"" + ylabel + "\" textcolor rgbcolor \"white\"");
                    title = prefix + "-Blast-Clus[" + clusters + "]-X-Histogram \\n of \\n " + nameitems[0];
                    writer.WriteLine("set title \"" + title + "\" textcolor rgbcolor \"white\"");
                    writer.WriteLine("plot '" + tripplefiles[2] + "' with filledcurves x1");

                    
                    
                    writer.WriteLine();

                    writer.WriteLine("unset multiplot");
                }
            }
        }

        private static Hashtable FormTriples(string[] files)
        {
            Hashtable triples = new Hashtable();
            foreach (string file in files)
            {
                /*
                 * Example file names
                 * selected-xHist-pts_A_chisq_1_cog_uniq_first50k_blast_untrans_clustered_selected_7_deleted_1186_points_w_orig_pnum-Vs-cog_95672_bitscore_refined_first50k_c#
                 * whole-xHist-pts_A_chisq_1_cog_uniq_first50k_blast_untrans_clustered_selected_7_deleted_1186_points_w_orig_pnum-Vs-cog_95672_bitscore_refined_first50k_c#
                 */

                string prefix = file[file.IndexOf('_') + 1].ToString();
                // plotpos is 0 for yHist, 1 for density, 2 for xHist
                int plotpos = file.Contains("Hist") ? (file.Contains("-x") ? 2 : 0) : 1;

                if (triples.ContainsKey(prefix))
                {
                    ((string[]) triples[prefix])[plotpos] = file;
                }
                else
                {
                    string[] arr = new string[3];
                    arr[plotpos] = file;
                    triples[prefix] = arr;
                }
            }
            return triples;
        }

        private static StringBuilder GeneratePlotScriptBegin()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("set terminal png truecolor nocrop enhanced font arial 14 size 1200,1200 x000000");
            sb.AppendLine("set border linecolor rgbcolor \"white\"");
            sb.AppendLine("set key textcolor rgbcolor \"white\"");
            sb.AppendLine("set dummy u,v");
            sb.AppendLine("unset key");
            sb.AppendLine("set size ratio 1.0");
            sb.AppendLine("set style fill  solid 0.85 noborder");
            sb.AppendLine("set style line 1 lt 1 lw 4");
            sb.AppendLine("set pm3d map");
            sb.AppendLine("set palette model RGB positive");
            return sb;
        }

        private static void ProcessFiles(string[] files, StringBuilder sb, string clusters, Hashtable names)
        {
            foreach (string file in files)
            {
                /*
                     * Example file names
                     * selected-xHist-pts_A_chisq_1_cog_uniq_first50k_blast_untrans_clustered_selected_7_deleted_1186_points_w_orig_pnum-Vs-cog_95672_bitscore_refined_first50k_c#
                     * whole-xHist-pts_A_chisq_1_cog_uniq_first50k_blast_untrans_clustered_selected_7_deleted_1186_points_w_orig_pnum-Vs-cog_95672_bitscore_refined_first50k_c#
                     */
                int idx = file.IndexOf('-') + 1;
                string axis = file[idx].ToString();
                idx = file.IndexOf('_', idx) + 1;
                string prefix = file[idx].ToString();

                string xlabel = "x".Equals(axis)
                                    ? ((string[]) names[prefix])[0]
                                    : ((string[]) names[prefix])[1] + "-Euclidean-of-" + ((string[]) names[prefix])[0];
                sb.AppendLine(string.Format("set xlabel '{0}' textcolor rgbcolor \"white\"", xlabel));
                sb.AppendLine("set ylabel 'Count' textcolor rgbcolor \"white\"");
                string title = string.Format("Clus[{0}]-{1}-Histogram-{2}", clusters, axis.ToUpper(), xlabel);
                sb.AppendLine("set title '" + title +  "' textcolor rgbcolor \"white\"");
                sb.AppendLine("set output '" + prefix + "-Blast-" + title + ".png'");
                sb.AppendLine("plot '" + Path.GetFileName(file) + "' with imp ls 1");
            }
        }
    }
}
