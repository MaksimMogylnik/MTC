using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.VisualBasic;
using System.IO;
using System.Windows.Forms;

namespace MTC
{
    public static class Terminal
    {
        public static Task Copy(string source, string target, int bufferSize, ProgressBar progressBar)
        {
            return Task.Factory.StartNew(() =>
            {
                FileStream fs1 = new FileStream(source, FileMode.Open);

                FileStream fs2 = new FileStream(target, FileMode.Create);

                progressBar.Maximum = Convert.ToInt32(fs1.Length);

                byte[] bytes = new byte[bufferSize];

                int counter = 0;
                while((counter = fs1.Read(bytes, 0, bytes.Length)) != 0)
                {
                    progressBar.Value += counter;
                    fs2.Write(bytes, 0, counter);
                }

                fs1.Close();
                fs2.Close();
            });
        }

        public static Task DeleteDirectory(string direction)
        {
            return Task.Factory.StartNew(() =>
            {
                DirectoryInfo directory = new DirectoryInfo(direction);
                directory.Delete(true);
            });
        }

        public static Task DeleteFile(string direction)
        {
            return Task.Factory.StartNew(() =>
            {
                FileInfo file = new FileInfo(direction);
                file.Delete();
            });
        }

        public static Task Move(string source, string target, int bufferSize, ProgressBar progressBar)
        {
            return Task.Factory.StartNew(async () =>
            {
                await Copy(source, target, bufferSize, progressBar);
                await DeleteDirectory(source);
            });
        }

    }
}
