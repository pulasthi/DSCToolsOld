using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MDSTryout
{
    public class ListBoxUtil
    {
        public static void ClearAll(ListBox box)
        {
            box.ClearSelected();
        }

        public static void SelectInverse(ListBox box)
        {
            for (int i = 0; i < box.Items.Count; i++)
            {
                box.SetSelected(i, !box.GetSelected(i));
            }
        }

        public static void SelectAll(ListBox box)
        {
            for (int i = 0; i < box.Items.Count; i++)
            {
                if (!box.GetSelected(i))
                {
                    box.SetSelected(i, true);
                }
            }
        }

        public static void RemoveSelected(ListBox box)
        {
            for (int i = 0; i < box.Items.Count; i++)
            {
                if (box.GetSelected(i))
                {
                    box.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void UpdateList(ListBox box, string[] files)
        {
            box.BeginUpdate();
            box.Items.Clear();
            foreach (string file in files)
            {
                box.Items.Add(file);
            }
            box.EndUpdate();
        }
    }
}
