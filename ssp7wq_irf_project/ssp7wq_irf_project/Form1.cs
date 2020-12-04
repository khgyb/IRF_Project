using ssp7wq_irf_project.reserve;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ssp7wq_irf_project
{
    public partial class Form1 : Form
    {
        Mozi_jegy_musor_kezelesEntities context = new Mozi_jegy_musor_kezelesEntities();

        public Form1()
        {
            InitializeComponent();

            context.Film.Load();
            context.Musor.Load();


            LoadMenu();


        }



        private void LoadMenu()
        {
            ListBox_refresh();
            var szuro = comboBox1.SelectedItem;
            switch (szuro)
            {
                case "Film címe":
                    textBox1.Enabled = true;
                    textBox1.Clear();


                    var cim = (from x in context.Film
                               where x.Cím.StartsWith(textBox1.Text)
                               select x);
                    listBox1.DisplayMember = "Cím";
                    listBox1.DataSource = cim.ToList();


                    break;
                case "Dátum":
                    textBox1.Enabled = false;
                    var datum = (from x in context.Musor
                                 select x);
                    listBox1.DisplayMember = "Datum";
                    listBox1.DataSource = datum.Distinct().ToList();
                    break;
                default:
                    break;
            }
        }

        private void ListBox_refresh()
        {
            listBox1.Refresh();
            listBox2.Refresh();
            listBox3.Refresh();
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMenu();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox_refresh();
            var szuro = comboBox1.SelectedItem;
            switch (szuro)
            {
                case "Film címe":
                    textBox1.Enabled = true;
                    textBox1.Clear();

                    var film = ((Film)listBox1.SelectedItem).Id_Film;
                    var musor = (from x in context.Musor
                                 where x.Film_Id==film
                                 select x);
                    listBox2.DisplayMember = "Datum";
                    listBox2.DataSource = musor.ToList();

                    break;
                case "Dátum":
                    textBox1.Enabled = false;
                    var musor_aznapi = ((Musor)listBox1.SelectedItem);
                    var cim =   (from x in context.Musor
                                 where x.Datum==musor_aznapi.Datum
                                 select x.Film);
                    listBox2.DisplayMember = "Cím";
                    listBox2.DataSource = cim.Distinct().ToList();
                    break;
                default:
                    break;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox_refresh();
            var szuro = comboBox1.SelectedItem;
            switch (szuro)
            {
                case "Film címe":
                    textBox1.Enabled = true;
                    textBox1.Clear();
                    var film = ((Film)listBox1.SelectedItem).Id_Film;
                    var musor = ((Musor)listBox2.SelectedItem).Datum;
                    var idop = (from x in context.Musor
                                 where x.Film_Id == film && x.Datum==musor
                                 select x);
                    listBox3.DisplayMember = "Idopont";
                    listBox3.DataSource = idop.ToList();

                    break;
                case "Dátum":
                    textBox1.Enabled = false;
                    var filmcim = ((Film)listBox2.SelectedItem).Id_Film;
                    var datum = ((Musor)listBox1.SelectedItem).Datum;
                    var idopont = (from x in context.Musor
                                   where x.Datum==datum && x.Film_Id==filmcim
                                 select x);
                    listBox3.DisplayMember = "Idopont";
                    listBox3.DataSource = idopont.ToList();
                    break;
                default:
                    break;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadMenu();
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            int width = this.Width;
            int height = this.Height - 250;
            MainPanel mp = new MainPanel(width, height);
            Controls.Add(mp);

            var msr = ((Musor)listBox3.SelectedItem).Id_Musor;
            /*var musor_kiv = (from x in context.Musor
                             where x.Id_Musor
                             select x);*/

            bool[] foglalas = (from x in context.Foglalas
                               where x.Musor_Id == msr
                               select x.Foglalt).ToArray();
            int szamlalo = 0;
            int status = 1;

            
            MessageBox.Show(foglalas[szamlalo].ToString());
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    status = asdf(foglalas, szamlalo, status);

                    Seat s = new Seat(status);
                    s.Top = i * 30;
                    s.Left = j * 30;
                    mp.Controls.Add(s);
                    szamlalo++;
                }
            }

        }

        private static int asdf(bool[] foglalas, int szamlalo, int status)
        {
            if (foglalas[szamlalo] == false)
            {
                status = 1;
            }
            else if (foglalas[szamlalo] == true)
            {
                status = 3;
            }

            return status;
        }
    }
}
