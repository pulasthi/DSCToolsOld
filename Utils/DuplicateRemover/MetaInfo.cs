using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace DuplicateRemover
{
    class MetaInfo
    {
        private int _count;
        private int _duplicateCount;
        private int _uniqueCount;
        private Hashtable _uniques;
        private List<string> _duplicateNames;
        private Hashtable _duplicates;

        public Hashtable Duplicates
        {
            get { return _duplicates; }
        }

        public int DuplicateCount
        {
            get
            {
                return _duplicateCount;
            }
            set
            {
                _duplicateCount = value;
            }
        }

        public int UniqueCount
        {
            get
            {
                return _uniqueCount;
            }
            set
            {
                _uniqueCount = value;
            }
        }

        public Hashtable Uniques
        {
            get
            {
                return _uniques;
            }
            set
            {
                _uniques = value;
            }
        }

//        public List<string> DuplicateNames
//        {
//            get
//            {
//                return _duplicateNames;
//            }
//            set
//            {
//                _duplicateNames = value;
//            }
//        }

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }

        public int DuplicateGroupCount { get; set; }

//        public MetaInfo(Hashtable uniques, List<string> duplicateNames)
        public MetaInfo(Hashtable uniques, Hashtable duplicates)
        {
            _uniques = uniques;
            _uniqueCount = uniques.Count;
//            _duplicateNames = duplicateNames;
            _duplicates = duplicates;
//            _duplicateCount = duplicateNames.Count;
            DuplicateGroupCount = duplicates.Count;
            _duplicateCount = 0;
            foreach (ICollection dupls in duplicates.Values)
            {
                _duplicateCount += dupls.Count;
            }
            _count = _duplicateCount + _uniqueCount;
        }


    }
}
