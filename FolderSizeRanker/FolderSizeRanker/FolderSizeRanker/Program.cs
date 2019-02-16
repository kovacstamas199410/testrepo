using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderSizeRanker
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DirectoryInfo rootFolder = null;

            if (args.Length == 1 && Directory.Exists(args[0]))
            {
                rootFolder = new DirectoryInfo(args[0]);
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    rootFolder = new DirectoryInfo(fbd.SelectedPath);
                }
                else
                {
                    Console.WriteLine("No folder specified!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }

            Calculate(rootFolder);
            Console.ReadLine();
        }

        static void Calculate(DirectoryInfo rootDirectory)
        {
            List<Task<long>> workers = new List<Task<long>>();
            List<FolderNameAndSizeTuple> tuples = new List<FolderNameAndSizeTuple>();

            foreach (var folder in rootDirectory.GetDirectories())
            {
                DirectoryInfo currentFolder = new DirectoryInfo(folder.FullName);
                tuples.Add(new FolderNameAndSizeTuple(currentFolder));
                workers.Add(Task.Factory.StartNew(() => new SizeCounter().CalculateFolderSize(currentFolder)));
            }

            Task.WaitAll(workers.ToArray());

            int counter = 0;
            workers.ForEach(w => tuples[counter++].Size = w.Result);

            PrintData(tuples);
        }

        static void PrintData(List<FolderNameAndSizeTuple> tuples)
        {
            tuples.Sort(new CustomTupleComparer());

            foreach (var values in tuples)
            {
                Console.WriteLine($"{values.Folder.Name}\t\t{values.Size/1024/1024/1024}");
            }
        }
    }
}
