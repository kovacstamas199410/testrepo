using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSizeRanker
{
    internal class FolderNameAndSizeTuple
    {
        internal long Size { get; set; }
        internal DirectoryInfo Folder { get; set; }

        public FolderNameAndSizeTuple(DirectoryInfo folder)
        {
            Folder = folder;
        }

        public FolderNameAndSizeTuple(int size, DirectoryInfo folder)
        {
            Folder = folder;
            Size = size;
        }
    }

    internal class CustomTupleComparer : IComparer<FolderNameAndSizeTuple>
    {
        public int Compare(FolderNameAndSizeTuple x, FolderNameAndSizeTuple y)
        {
            if (x.Size < y.Size)
            {
                return - 1;
            }

            if (y.Size < x.Size)
            {
                return 1;
            }

            return string.Compare(x.Folder.FullName, y.Folder.FullName);
        }
    }
}
