using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PetqLight
{
    class BatGenerator
    {
        #region RemoteBats

        #region setup.bat

        /// <summary>
        /// Generates the string for setup.bat for remote deployment
        /// </summary>
        /// <param name="headNode">name of the headnode</param>
        /// <param name="targetDir">the path to the parent folder where the project folder will reside in cluster</param>
        /// <param name="projectName">name of the project, which is also the name of the folder of the project</param>
        /// <param name="nodeNames">the names of compute nodes</param>
        /// <returns>the generated string for setup.bat</returns>
        public static string GenerateSetupBat(string headNode, string targetDir, string projectName, IEnumerable<string> nodeNames)
        {
            StringBuilder sb = new StringBuilder("@echo off");
            sb.AppendLine();
            sb.Append("SET headnode=");
            sb.AppendLine(headNode);
            sb.Append("SET folder=");
            sb.AppendLine(projectName);
            sb.Append("SET nodes=");
            sb.AppendLine(string.Join(",", nodeNames));
            sb.Append("clusrun /nodes:%nodes% mkdir ");
            sb.Append(targetDir);
            sb.Append(Path.DirectorySeparatorChar);
            sb.AppendLine("%folder%");
            sb.Append(@"clusrun /nodes:%nodes% xcopy /E /Y \\%headnode%");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(targetDir.ElementAt(0));
            sb.Append("$");
            sb.Append(targetDir.Substring(2));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("%folder%");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("*.* ");
            sb.Append(targetDir);
            sb.Append(Path.DirectorySeparatorChar);
            sb.AppendLine("%folder%");
            sb.AppendLine("PAUSE");
            return sb.ToString();
        }

        #endregion

        #region cleanup.bat

        /// <summary>
        /// Generates the string for cleanup.bat for remote deployment
        /// </summary>
        /// <param name="targetDir">the path to the parent folder where the project folder will reside in cluster</param>
        /// <param name="projectName">name of the project, which is also the name of the folder of the project</param>
        /// <param name="nodeNames">the names of compute nodes</param>
        /// <returns>generated string for cleanup.bat</returns>
        public static string GenerateCleanupBat(string targetDir, string projectName, IEnumerable<string> nodeNames)
        {
            StringBuilder sb = new StringBuilder("@echo off");
            sb.AppendLine();
            sb.Append("SET folder=");
            sb.AppendLine(projectName);
            sb.Append("SET nodes=");
            sb.AppendLine(string.Join(",", nodeNames));
            sb.Append("clusrun /nodes:%nodes% rmdir /S /Q ");
            sb.Append(targetDir);
            sb.Append(Path.DirectorySeparatorChar);
            sb.AppendLine("%folder%");
            sb.AppendLine("PAUSE");
            return sb.ToString();
        }

        #endregion

        #region go.bat

        /// <summary>
        /// Generates the string for go.bat for remote deployment
        /// </summary>
        /// <param name="headNode">name of the headnode</param>
        /// <param name="targetDir">the path to the parent folder where the project folder will reside in cluster</param>
        /// <param name="projectName">name of the project, which is also the name of the folder of the project</param>
        /// <param name="nodes">total number of compute nodes</param>
        /// <param name="cores">total number of cores</param>
        /// <param name="avg">floor of cores/nodes</param>
        /// <param name="nodeNames">the names of compute nodes</param>
        /// <returns>the generated string for go.bats</returns>
        public static string GenerateGoBat(string headNode, string targetDir, string projectName, 
            int nodes, int cores, int avg, IEnumerable<string> nodeNames, ApplicationType saApp, ApplicationType pwcApp, ApplicationType mdsApp)
        {
            StringBuilder sb = new StringBuilder("@echo off");
            sb.AppendLine();
            sb.Append("SET jobname=");
            sb.AppendLine(projectName);
            sb.Append("SET wrk=");
            sb.Append(targetDir);
            sb.Append(Path.DirectorySeparatorChar);
            sb.AppendLine("%jobname%");
            sb.Append("SET apps=%wrk%");
            sb.Append(Path.DirectorySeparatorChar);
            sb.AppendLine(Constants.AppsDirName);
            sb.Append("SET nodes=");
            sb.AppendLine(nodes.ToString());
            sb.Append("SET cores=");
            sb.AppendLine(cores.ToString());
            sb.Append("SET requested=");
            sb.AppendLine(string.Join(",", nodeNames));
            sb.Append("SET hosts01=%nodes% ");
            sb.Append(string.Join(" 1 ", nodeNames));
            sb.AppendLine(" 1"); // the last 1, which should come after nodenames
            sb.Append("SET hosts");
            sb.Append(avg);
            sb.Append("=%nodes% ");
            sb.Append(string.Join(" " + avg + " ", nodeNames));
            sb.AppendLine(" " + avg); // the last avg, which should come after nodenames
            sb.Append(@"SET config=\\");
            sb.Append(headNode);
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(targetDir.ElementAt(0));
            sb.Append("$");
            sb.Append(targetDir.Substring(2));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("%jobname%");
            sb.Append(Path.DirectorySeparatorChar);
            sb.AppendLine(Constants.ConfigDirName);

            if (saApp != ApplicationType.UNDEFINED)
            {
                switch (saApp)
                {
                    case ApplicationType.SWMS:
                        // Call SWMS
                        sb.Append("CALL job submit /jobname:%jobname%_swms /workdir:%wrk% /stdout:%wrk%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append(Constants.OutputDirName);
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("swms_out.txt /stderr:%wrk%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append(Constants.OutputDirName);
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("swms_err.txt /numcores:%cores% /requestednodes:%requested% /exclusive:true mpiexec /hosts %hosts");
                        sb.Append(avg);
                        sb.Append("% %apps%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("Salsa.SmithWatermanMS.exe /configFile=%config%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.AppendLine("config.xml /nodeCount=%nodes%");
                        break;
                    case ApplicationType.SWG:
                        // Call SWG
                        sb.Append("CALL job submit /jobname:%jobname%_swg /workdir:%wrk% /stdout:%wrk%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append(Constants.OutputDirName);
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("swg_out.txt /stderr:%wrk%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append(Constants.OutputDirName);
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("swg_err.txt /numcores:%cores% /requestednodes:%requested% /exclusive:true mpiexec /hosts %hosts");
                        sb.Append(avg);
                        sb.Append("% %apps%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("Salsa.SmithWatermanTPL.exe /configFile=%config%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.AppendLine("config.xml /nodeCount=%nodes%");
                        break;
                    case ApplicationType.NW:
                        // Call NW
                        sb.Append("CALL job submit /jobname:%jobname%_nw /workdir:%wrk% /stdout:%wrk%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append(Constants.OutputDirName);
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("nw_out.txt /stderr:%wrk%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append(Constants.OutputDirName);
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("nw_err.txt /numcores:%cores% /requestednodes:%requested% /exclusive:true mpiexec /hosts %hosts");
                        sb.Append(avg);
                        sb.Append("% %apps%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("Salsa.NeedlemanWunschTPL.exe /configFile=%config%");
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.AppendLine("config.xml /nodeCount=%nodes%");
                        break;
                }
            }

            //SET copyto=<List without first compute node>
            sb.Append("SET copyto=");
            sb.AppendLine(string.Join(",",nodeNames.ToArray(),1,(nodeNames.Count()-1)));
           
            /*
            //clusrun /nodes:%copyto% copy \\TEMPEST-CN01\E$\salsa\saliya\mina-Nov11-2010\%jobname%\Output\distance.bin E:\salsa\saliya\mina-Nov11-2010\%jobname%\Output\
            sb.Append("clusrun /nodes:%copyto% copy \\\\");
            sb.Append(nodeNames.First());
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(targetDir.Replace(':','$'));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("%jobname%");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("Output");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("distance.bin");
            sb.Append(" %wrk%");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("Output");
            sb.Append(Path.DirectorySeparatorChar);
            sb.AppendLine();*/

            //CALL job submit /jobname:%jobname%_copy /requestednodes:%copyto% /numnodes:4 /exclusive:true /parametric:0-64 copy \\MADRID-101\C$\salsa\petq\%jobname%\Output\distance.bin %wrk%\Output\
            sb.Append("CALL job submit /jobname:%jobname%_copy /requestednodes:%copyto% /numnodes:");
            sb.Append((nodes-1));
            sb.Append(" /exclusive:true /parametric:0-");
            sb.Append(cores.ToString());
            sb.Append(" copy \\\\");
            sb.Append(nodeNames.First());
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(targetDir.Replace(':','$'));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("%jobname%");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("Output");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("distance.bin");
            sb.Append(" %wrk%");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append("Output");
            sb.Append(Path.DirectorySeparatorChar);
            sb.AppendLine();           
 
             
            if (pwcApp != ApplicationType.UNDEFINED)
            {
                // Call PWC
                sb.Append("CALL job submit /jobname:%jobname%_pwc /workdir:%wrk% /stdout:%wrk%");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append(Constants.OutputDirName);
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("pwc_out.txt /stderr:%wrk%");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append(Constants.OutputDirName);
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("pwc_err.txt /numcores:%cores% /requestednodes:%requested% /exclusive:true mpiexec /hosts %hosts01% %apps%");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("Salsa.PairwiseClusteringTPL.exe /configFile=%config%");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("config.xml /nodeCount=%nodes% /threadCount=");
                sb.AppendLine(avg.ToString());
            }

            if (mdsApp != ApplicationType.UNDEFINED)
            {
                // Call MDS
                sb.Append("CALL job submit /jobname:%jobname%_mds /workdir:%wrk% /stdout:%wrk%");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append(Constants.OutputDirName);
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("mds_out.txt /stderr:%wrk%");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append(Constants.OutputDirName);
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("mds_err.txt /numcores:%cores% /requestednodes:%requested% /exclusive:true mpiexec /hosts %hosts01% %apps%");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("ManxcatMDS.exe /configFile=%config%");
                sb.Append(Path.DirectorySeparatorChar);
                sb.Append("config.xml /nodeCount=%nodes% /threadCount=");
                sb.AppendLine(avg.ToString());
            }

            return sb.ToString();
        }

        #endregion

        #endregion

        #region LocalBats

        public static string GenerateGoLocalBat(string projectDir, string projectName, int numOfProcesses, ApplicationType saApp, ApplicationType pwcApp, ApplicationType mdsApp)
        {
            char ds = Path.DirectorySeparatorChar;
            StringBuilder sb = new StringBuilder("@echo off");
            sb.AppendLine();

            string configPath = projectDir + ds + projectName + ds + Constants.ConfigDirName + ds + Constants.ConfigFileName;
            
            if (saApp != ApplicationType.UNDEFINED)
            {
                string executable;
                switch (saApp)
                {
                    case ApplicationType.SWMS:
                        executable = Constants.SmithWatermanMSExe;
                        break;
                    case ApplicationType.SWG:
                        executable = Constants.SmithWatermanExe;
                        break;
                    case ApplicationType.NW:
                        executable = Constants.NeedlemanWunschExe;
                        break;
                    default:
                        executable = Constants.SmithWatermanExe;
                        break;
                }
                executable = Constants.AppsDirName + ds + executable;
                sb.AppendLine(string.Format(Constants.LocalSequenceAlignMpiexec, numOfProcesses.ToString(), executable, configPath));
            }

            if (pwcApp != ApplicationType.UNDEFINED)
            {
                sb.AppendLine(string.Format(Constants.LocalPairwiseClusteringMpiexec, numOfProcesses.ToString(), Constants.AppsDirName + ds + Constants.PairwiseClusteringExe, configPath));
            }

            if (mdsApp != ApplicationType.UNDEFINED)
            {
                sb.AppendLine(string.Format(Constants.LocalMultiDimensionalScalingMpiexec, numOfProcesses.ToString(), Constants.AppsDirName + ds + Constants.MultiDimensionalScalingExe, configPath));
            }

            return sb.ToString();
        }

        #endregion

    }
}
