using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio.IO.Newick;
using Bio.Phylogenetics;

namespace TreeDist
{
    class Program
    {
        static void Main(string[] args)
        {
            string tf = args[0];
            using (NewickParser parser = new NewickParser())
            {
                Tree tree = parser.Parse(tf);
                //tree.Root.Edges
            }
        }
    }
}
