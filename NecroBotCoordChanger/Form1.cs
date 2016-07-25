using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.Threading;

namespace NecroBotCoordChanger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
        }
        public string pokemonPath;
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult r = folderBrowserDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                pokemonPath = textBox1.Text + @"\PokemonGo.RocketAPI.Console.exe";
                if (checkPokemonExec(textBox1.Text) == true)
                {
                    label2.ForeColor = Color.LimeGreen;
                    label2.Text = "FOUND!";
                }
            }
        }
        private bool checkPokemonExec(string path)
        {
            return File.Exists(pokemonPath);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.ForeColor = Color.Black;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = String.Empty;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int r = rnd.Next(255);
            int g = rnd.Next(255);
            int b = rnd.Next(255);
            linkLabel1.LinkColor = Color.FromArgb(r, g, b);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.forocoches.com/foro/member.php?u=677250");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string pokeVisionLink = textBox2.Text;
            if (pokeVisionLink.StartsWith("https://pokevision.com/"))
            {
                string latitude = pokeVisionLink.Split('@')[1].Split(',')[0];
                string longitude = pokeVisionLink.Split('@')[1].Split(',')[1];
                if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Por favor, busque la ruta del Bot!");
                }
                else
                {
                    if (checkPokemonExec(pokemonPath) == false)
                    {
                        MessageBox.Show("Por favor, seleccione una ruta correcta!");
                    }
                    else
                    {
                        try
                         {
                             Process[] processlist = Process.GetProcessesByName("PokemonGo.RocketAPI.Console");
                             foreach (Process theprocess in processlist)
                             {
                                 if (theprocess.MainModule.FileName == pokemonPath)
                                     theprocess.Kill();
                             }
                         }
                         catch(Exception ex)
                         {
                             MessageBox.Show(ex.Message);
                         }
                         if (File.Exists(textBox1.Text + @"\Configs\Coords.ini"))
                         {
                             File.Delete(textBox1.Text + @"\Configs\Coords.ini");
                         }
                        if (File.Exists(textBox1.Text + @"\PokemonGo.RocketAPI.Console.vshost.exe.config"))
                        {
                            File.Delete(textBox1.Text + @"\PokemonGo.RocketAPI.Console.vshost.exe.config");
                        }
                        XmlDocument doc = new XmlDocument();
                        doc.Load(pokemonPath + ".config");
                        XmlNodeList elemList = doc.GetElementsByTagName("setting");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            if (elemList[i].Attributes["name"].Value == "DefaultLatitude")
                                elemList[i].FirstChild.InnerText = latitude;
                            if (elemList[i].Attributes["name"].Value == "DefaultLongitude")
                                elemList[i].FirstChild.InnerText = longitude;
                        }
                        doc.Save(pokemonPath + ".config");
                        Process process = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = "/C cd " + textBox1.Text + " && start " + pokemonPath;
                        process.StartInfo = startInfo;
                        process.Start();
                    }
                }
            }
            else
            {
                textBox2.Text = String.Empty;
                MessageBox.Show("Bad Link!");
            }
        }
    }
}
