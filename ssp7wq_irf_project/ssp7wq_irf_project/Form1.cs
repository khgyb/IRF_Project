using ssp7wq_irf_project.Entities;
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
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Microsoft.SqlServer.Server;

namespace ssp7wq_irf_project
{
    public partial class Form1 : Form
    {
        Mozi_jegy_musor_kezelesEntities context = new Mozi_jegy_musor_kezelesEntities();
        MainPanel mp;
        bool[] foglalas;
        int foglalt_helyek_szama = 0;
        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;
        List<Musor> Musor;

        public Form1()
        {
            InitializeComponent();

            context.Film.Load();
            context.Musor.Load();


            LoadMenu();
            LoadData();

            int width = this.Width;
            int height = this.Height - 250;
            mp = new MainPanel(width, height);
            Controls.Add(mp);
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
                    filmBindingSource.DataSource = (from x in context.Film
                                                    where x.Id_Film == film
                                                    select x).ToList();

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
                    filmBindingSource.DataSource = (from x in context.Film
                                                    where x.Id_Film == filmcim
                                                    select x).ToList();
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
            mp.Controls.Clear();

            if (listBox3.SelectedItem!=null)
            {
                var msr = ((Musor)listBox3.SelectedItem).Id_Musor;

                var letezik = (from x in context.Foglalas
                               where x.Musor_Id == msr
                               select x.Id_Foglalas).FirstOrDefault();
                if (letezik != 0)
                {

                    foglalas = (from x in context.Foglalas
                                where x.Musor_Id == msr
                                select x.Foglalt).ToArray();

                    int szamlalo = 0;
                    int status = 1;

                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 14; j++)
                        {
                            status = stat(foglalas, szamlalo, status);

                            Seat s = new Seat(status);
                            s.sszam = szamlalo + 1;
                            s.Top = 100 + i * 60;
                            s.Left = 20 + j * 60;
                            mp.Controls.Add(s);
                            szamlalo++;
                        }
                    }
                    LoadElements();
                }
                else
                {
                    MessageBox.Show("A kiválasztott előadás nem létezik!");
                }
            }
            else
            {
                MessageBox.Show("Nincs kiválasztva műsor!");
            }

            


        }

        private void LoadElements()
        {
            int a = mp.Controls[13].Left + 20;
            Vaszon v = new Vaszon(a);
            mp.Controls.Add(v);

            Label l = new Label();
            l.Text = "Vászon";
            l.Top = 30;
            l.Left = (a + 20) / 2;
            mp.Controls.Add(l);
        }

        private static int stat(bool[] foglalas, int szamlalo, int status)
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

        private void btn_save_Click(object sender, EventArgs e)
        {
            //var controls = from Control in mp.Controls select Control;
            /*var foglaltak = from x in mp.Controls.OfType<Control>()
                             where x is Seat && ((Seat)x).Status == 2
                             select x;*/


            foreach (Seat s in mp.Controls.OfType<Seat>())
            {
                var foglalt = (from x in context.Foglalas
                               where x.Id_Foglalas == s.sszam
                               select x).FirstOrDefault();

                if (foglalt.Foglalt == false && s.Status == 2)
                {
                    foglalt_helyek_szama++;
                }

            }
            if (foglalt_helyek_szama!=0)
            {
                Mentes m = new Mentes();
                m.textBox1.Text = foglalt_helyek_szama.ToString();
                m.textBox2.Text = ((foglalt_helyek_szama * 1000).ToString() + " Ft");

                if (m.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        foreach (Seat s in mp.Controls.OfType<Seat>())
                        {
                            var foglalt = (from x in context.Foglalas
                                           where x.Id_Foglalas == s.sszam
                                           select x).FirstOrDefault();

                            if (foglalt.Foglalt == false && s.Status == 2)
                            {
                                s.Status = 3;
                                s.Enabled = false;
                                foglalt.Foglalt = true;
                                foglalt_helyek_szama++;
                            }

                        }
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nincs kiválasztva ülőhely!");
            }

        }

        private void btn_ccel_Click(object sender, EventArgs e)
        {
            foreach (Seat s in mp.Controls.OfType<Seat>())
            {
                if (s.Status == 2)
                {
                    s.Status = 1;
                }

            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            Musor = context.Musor.ToList();
            CreateExcel();
        }

        private void CreateExcel()
        {
            try
            {

                xlApp = new Excel.Application();

                
                xlWB = xlApp.Workbooks.Add(Missing.Value);

                
                xlSheet = xlWB.ActiveSheet;

                
                CreateTable();

                
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");

                
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }

        private void CreateTable()
        {
            
            string[] headers = new string[]
            {
                "Műsor azonosítója",
                "Dátum",
                "Időpont",
                "Film címe",
                "Eladott jegyek száma",
                "Bevétel"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = headers[i];                
            }
            
            var eladott_jegyek = (from x in context.Foglalas
                              group x by x.Musor_Id into g
                              select new {id=g.Key, jegyek= g.Count()});
            
            object[,] values = new object[Musor.Count, headers.Length];

            int counter = 0;
            foreach (Musor m in Musor)
            {
                values[counter, 0] = m.Id_Musor;
                values[counter, 1] = m.Datum.ToString("yyyy.MM.dd");
                values[counter, 2] = m.Idopont.ToString();
                values[counter, 3] = m.Film.Cím;
                values[counter, 4] = (from x in eladott_jegyek
                                      where x.id == m.Id_Musor
                                      select x.jegyek).FirstOrDefault();
                values[counter, 5] = (int)values[counter, 4] * 1000;
                counter++;
            }


            
            xlSheet.get_Range(
                        GetCell(2, 1),
                        GetCell(1 + values.GetLength(0), values.GetLength(1))).Value = values;
            
            /*int counter = 0;
            foreach (var item in eladott_jegyek)
            {
                xlSheet.Cells[counter + 5, 1].Value = item.id.ToString();
                counter++;
            }*/


            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

        }

        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }

        private void LoadData()
        {
            musorbindingSource.DataSource = (from x in context.Musor
                                             select new
                                             {
                                                 Műror=x.Id_Musor,
                                                 Dátum=x.Datum,
                                                 Időpont=x.Idopont,
                                                 Film=x.Film.Cím

                                             }).ToList();
            dataGridView1.DataSource = musorbindingSource;
        }
    }
}
