using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParseQstat
{
    class Program
    {
        static void Main(string[] args)
        {
            var nodesString =
                @"i51/7+i51/6+i51/5+i51/4+i51/3+i51/2+i51/1+i51/0+i52/7+i52/6+i52/5+i52/4
   +i52/3+i52/2+i52/1+i52/0+i53/7+i53/6+i53/5+i53/4+i53/3+i53/2+i53/1+i53/0
   +i54/7+i54/6+i54/5+i54/4+i54/3+i54/2+i54/1+i54/0+i75/7+i75/6+i75/5+i75/4
   +i75/3+i75/2+i75/1+i75/0+i81/7+i81/6+i81/5+i81/4+i81/3+i81/2+i81/1+i81/0
   +i82/7+i82/6+i82/5+i82/4+i82/3+i82/2+i82/1+i82/0+i83/7+i83/6+i83/5+i83/4
   +i83/3+i83/2+i83/1+i83/0+i84/7+i84/6+i84/5+i84/4+i84/3+i84/2+i84/1+i84/0
   +i85/7+i85/6+i85/5+i85/4+i85/3+i85/2+i85/1+i85/0+i86/7+i86/6+i86/5+i86/4
   +i86/3+i86/2+i86/1+i86/0+i87/7+i87/6+i87/5+i87/4+i87/3+i87/2+i87/1+i87/0
   +i88/7+i88/6+i88/5+i88/4+i88/3+i88/2+i88/1+i88/0+i89/7+i89/6+i89/5+i89/4
   +i89/3+i89/2+i89/1+i89/0+i91/7+i91/6+i91/5+i91/4+i91/3+i91/2+i91/1+i91/0
   +i92/7+i92/6+i92/5+i92/4+i92/3+i92/2+i92/1+i92/0+i93/7+i93/6+i93/5+i93/4
   +i93/3+i93/2+i93/1+i93/0+i94/7+i94/6+i94/5+i94/4+i94/3+i94/2+i94/1+i94/0
   +i95/7+i95/6+i95/5+i95/4+i95/3+i95/2+i95/1+i95/0+i97/7+i97/6+i97/5+i97/4
   +i97/3+i97/2+i97/1+i97/0+i98/7+i98/6+i98/5+i98/4+i98/3+i98/2+i98/1+i98/0
   +i99/7+i99/6+i99/5+i99/4+i99/3+i99/2+i99/1+i99/0+i6/7+i6/6+i6/5+i6/4+i6/3
   +i6/2+i6/1+i6/0+i63/7+i63/6+i63/5+i63/4+i63/3+i63/2+i63/1+i63/0+i66/7+i66/6
   +i66/5+i66/4+i66/3+i66/2+i66/1+i66/0+i65/7+i65/6+i65/5+i65/4+i65/3+i65/2
   +i65/1+i65/0+i64/7+i64/6+i64/5+i64/4+i64/3+i64/2+i64/1+i64/0+i62/7+i62/6
   +i62/5+i62/4+i62/3+i62/2+i62/1+i62/0+i60/7+i60/6+i60/5+i60/4+i60/3+i60/2
   +i60/1+i60/0+i67/7+i67/6+i67/5+i67/4+i67/3+i67/2+i67/1+i67/0+i68/7+i68/6
   +i68/5+i68/4+i68/3+i68/2+i68/1+i68/0+i25/7+i25/6+i25/5+i25/4+i25/3+i25/2
   +i25/1+i25/0";
            var splits = nodesString.Split('+');
            var nodesSet = new HashSet<string>();
            int count = 0;
            foreach (var node in splits.Select(split => split.Split('/')[0]).Where(node => !nodesSet.Contains(node)))
            {
                nodesSet.Add(node);
                Console.WriteLine(node);
                ++count;
            }
            Console.WriteLine(count);
            Console.Read();

        }
    }
}
