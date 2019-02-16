using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSizeRanker
{
    internal class SizeCounter
    {
        internal long CalculateFolderSize(DirectoryInfo currentFolder)
        {
            long size = 0;
            try
            {
                foreach (var file in currentFolder.GetFiles())
                {
                    size += file.Length;
                }

                foreach (var folder in currentFolder.GetDirectories())
                {
                    size += CalculateFolderSize(folder);
                }
            }
            catch (UnauthorizedAccessException)
            {
                return 0;
            }            
            
            return size;
        }
    }
}
