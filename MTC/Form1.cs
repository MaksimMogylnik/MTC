using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MTC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            textBox1.Text = Environment.GetLogicalDrives()[0].ToString();
            textBox2.Text = Environment.GetLogicalDrives()[0].ToString();

            listView1.SmallImageList = imageList1;
            listView2.SmallImageList = imageList1;
            ListViewUpdate(listView1, textBox1.Text);
            ListViewUpdate(listView2, textBox2.Text);
        }

        private Task ListViewUpdateAsync(ListView listView, string source)
        {
            return Task.Factory.StartNew(() => ListViewUpdate(listView, source));
        }

        private void ListViewUpdate(ListView listView, string source)
        {
            listView.Clear();

            DirectoryInfo directory = new DirectoryInfo(source);

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                listView.Items.Add(dir.Name, 1);
            }
            foreach (FileInfo file in directory.GetFiles())
            {
                listView.Items.Add(file.Name, 0);
            }
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems[0].ImageIndex == 1)
                await Terminal.DeleteDirectory(textBox1.Text + "\\" + listView1.SelectedItems[0].Text);
            else
                await Terminal.DeleteFile(textBox1.Text + "\\" + listView1.SelectedItems[0].Text);

            if (listView2.SelectedItems[0].ImageIndex == 1)
                await Terminal.DeleteDirectory(textBox2.Text + "\\" + listView2.SelectedItems[0].Text);
            else
                await Terminal.DeleteFile(textBox2.Text + "\\" + listView2.SelectedItems[0].Text);
        }

        private async void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string source = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
            string target = textBox2.Text;
            const int bufferSize = 1024;
            Form2 form2 = new Form2();
            form2.ShowDialog();

            await Terminal.Copy(source, target, bufferSize, form2.progressBar1);

            form2.Close();
        }

        private async void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string source = textBox2.Text + "\\" + listView2.SelectedItems[0].Text;
            string target = textBox1.Text;
            const int bufferSize = 1024;
            Form2 form2 = new Form2();
            form2.ShowDialog();

            await Terminal.Copy(source, target, bufferSize, form2.progressBar1);

            form2.Close();
        }

        private async void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            string source = textBox1.Text + "\\" + listView1.SelectedItems[0].Text;
            string target = textBox2.Text;
            const int bufferSize = 1024;
            Form2 form2 = new Form2();
            form2.ShowDialog();

            await Terminal.Move(source, target, bufferSize, form2.progressBar1);

            form2.Close();
        }

        private async void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            string source = textBox2.Text + "\\" + listView2.SelectedItems[0].Text;
            string target = textBox1.Text;
            const int bufferSize = 1024;
            Form2 form2 = new Form2();
            form2.ShowDialog();

            await Terminal.Move(source, target, bufferSize, form2.progressBar1);

            form2.Close();
        }

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                await ListViewUpdateAsync(listView1, textBox1.Text);
            }
            catch
            {
                return;
            }
        }

        private async void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                await ListViewUpdateAsync(listView2, textBox2.Text);
            }
            catch
            {
                return;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems[0].ImageIndex == 1)
            {
                textBox1.Text += "\\" + listView1.SelectedItems[0].Text;
                textBox1_TextChanged(this, e);
            }
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            if (listView2.SelectedItems[0].ImageIndex == 1)
            {
                textBox2.Text += "\\" + listView2.SelectedItems[0].Text;
                textBox2_TextChanged(this, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string path = textBox1.Text;
                path = path.Remove(path.LastIndexOf('\\'));
                textBox1.Text = path;
            }
            catch
            {
                MessageBox.Show(
                    "No previous directories",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string path = textBox2.Text;
                path = path.Remove(path.LastIndexOf('\\'));
                textBox2.Text = path;
            }
            catch
            {
                MessageBox.Show(
                    "No previous directories",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }
    }
}
