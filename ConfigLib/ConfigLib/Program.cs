using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigLib
{
    class Program
    {
        static void Main(string[] args)
        {
            /* create a Smithe-Waterman config */
            SWConfig swConfig = new SWConfig();
            /* set SW config options */
            swConfig.OutputFile = @"someplace\to\output\swresults";

            /* create a pairwise cluster config */
            PairwiseClusteringConfig clusConfig = new PairwiseClusteringConfig();
            /* set pairwise config options */
            clusConfig.DataLabelsFileName = @"somefilename";

            /* create a mds config */
            MDSConfig mdsConfig = new MDSConfig();
            /* set mds config options */
            mdsConfig.Comment = @"this is a comment";

            /* create global config */
            GlobalConfig globalConfig = new GlobalConfig();
            /* set global config options */
            globalConfig.BaseResultDirectoryName = @"some\directory";

            /* create a configuration object contatining component configs */
            Configuration conf= new Configuration();
            conf.Name = "Alu35339";

            conf.Global = globalConfig;
            conf.MDS = mdsConfig;
            conf.SmithWaterman = swConfig;
            conf.PairwiseClustering = clusConfig;

            /* serialize the configuration */
            //conf.SerializeToFile(@"C:\Documents and Settings\Saliya Ekanayake\My Documents\Visual Studio 2008\Projects\ConfigLib\ConfigLib\conf.xml");
            
            /* build the configuration from file */
            conf = Configuration.BuildFromFile(@"C:\Documents and Settings\Saliya Ekanayake\My Documents\Visual Studio 2008\Projects\ConfigLib\ConfigLib\conf.xml");
            Console.WriteLine(conf.SmithWaterman.EndIndex);
            

        }
    }
}
