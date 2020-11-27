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
using System.Text.RegularExpressions;

namespace WhatsAppGraph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static List<User> users = new List<User>();
        static List<string> _users = new List<string>();

        void Work(string file)
        {
            string[] messages = File.ReadAllLines(file);
            for (int i = 0; i < messages.Length; i++)
            {
                try
                {
                    string message = Regex.Replace(messages[i], @"\p{Cs}", "&").Remove(0, 20);
                    if (message.Contains(':'))
                    {
                        string name = message.Split(':')[0];
                        if (_users.Contains(name))
                        {
                            foreach (User u in users)
                            {
                                if (u.name == name)
                                    u.msgCount++;
                            }
                        }
                        else { _users.Add(name); users.Add(new User() { name = name }); }
                    }
                }
                catch { }
            }
            Visualise();
        }
        
        #region UI
        void Visualise()
        {
            // Add data to the chart
            foreach (User c in users)
            {
                chart1.ChartAreas[0].AxisX.Interval = 1;
                if (c.msgCount > 1) chart1.Series["S"].Points.AddXY(c.name, c.msgCount);
            }
        }
        private void DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                if (File.Exists(files[0]))
                    Work(files[0]);
            }
        }
        private void DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        #endregion
    }
    class User
    {
        public string name;
        public int msgCount;
    }
}
