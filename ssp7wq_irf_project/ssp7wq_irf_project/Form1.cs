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

        BindingSource FilmBindingSource = new BindingSource();
        BindingSource MusorBindingSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();

            context.Film.Load();
            context.Musor.Load();

            LoadMenu();

            LoadPanel();

        }

        private void LoadMenu()
        {
            var szuro = comboBox1.SelectedItem;
            switch (szuro)
            {
                case "Film címe":
                    var cim = (from x in context.Film
                               select x.Cím);
                    FilmBindingSource.DataSource = cim.ToList();
                    listBox1.DataSource = FilmBindingSource;
                    break;
                case "Dátum":
                    var datum = (from x in context.Musor
                                 select x.Datum);
                    MusorBindingSource.DataSource = datum.Distinct().ToList();
                    listBox1.DataSource = MusorBindingSource;
                    break;
                default:
                    break;
            }
        }

        private void LoadPanel()
        {
            Panel MainPanel = new Panel();
            MainPanel.Top = 50;
            MainPanel.Left = 0;
            MainPanel.Width = this.Width;
            MainPanel.Height = this.Height - 50;
            MainPanel.BackColor = Color.Green;
            Controls.Add(MainPanel);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMenu();
        }
    }
}
