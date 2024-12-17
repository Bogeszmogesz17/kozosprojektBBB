using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace kozosprojekt
{
    public partial class Form1 : Form
    {
        private int jelenlegiKerdezesIndex = 0;
        private int helyesValaszok = 0;

        private List<Kerdezes> kerdesek = new List<Kerdezes>
        {
            new Kerdezes("Mi Magyarország fõvárosa?", new[] { "Budapest", "Debrecen", "Szeged" }, 0),
            new Kerdezes("Mennyi 2+2?", new[] { "3", "4", "5" }, 1),
            new Kerdezes("Melyik az óceán?", new[] { "Balaton", "Csendes-óceán", "Duna" }, 1),
            new Kerdezes("Melyik nem gyümölcs?", new[] { "Alma", "Banán", "Kalapács" }, 2),
            new Kerdezes("Melyik szín a piros?", new[] { "Zöld", "Kék", "Piros" }, 2)
        };

        public Form1()
        {
            KezdoUIInicializalasa();
            KerdesMegjelenitese();
        }

        private Label kerdesLabel;
        private Button valaszGomb1;
        private Button valaszGomb2;
        private Button valaszGomb3;
        private Button kovetkezoGomb;
        private Panel nyeremenyPanel;
        private Button uresOpcioGomb;
        private Button autoOpcioGomb;
        private Button csokiOpcioGomb;

        private void KezdoUIInicializalasa()
        {
            
            this.Text = "Menjen Ön is csõdbe!";
            this.Size = new Size(400, 350);

            
            this.BackgroundImage = Image.FromFile("hatter.png"); 
            this.BackgroundImageLayout = ImageLayout.Stretch;

            
            kerdesLabel = new Label
            {
                Size = new Size(350, 50),
                Location = new Point(25, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(200, Color.White),
                ForeColor = Color.Black
            };
            this.Controls.Add(kerdesLabel);

            
            valaszGomb1 = CreateAnswerButton(0, 100);
            valaszGomb2 = CreateAnswerButton(1, 150);
            valaszGomb3 = CreateAnswerButton(2, 200);

            
            kovetkezoGomb = new Button
            {
                Text = "Tovább",
                Size = new Size(100, 40),
                Location = new Point(150, 250),
                Visible = false
            };
            kovetkezoGomb.Click += KovetkezoGomb_Click;
            this.Controls.Add(kovetkezoGomb);

            
            nyeremenyPanel = new Panel
            {
                Size = new Size(350, 200),
                Location = new Point(25, 50),
                Visible = false,
                BackColor = Color.FromArgb(150, Color.White)
            };

            
            uresOpcioGomb = CreateResultButton("", "Nem nyertél semmit! Gratulálunk te is csõdbe mentél!", 25);
            autoOpcioGomb = CreateResultButton("", "Nem nyertél semmit! Gratulálunk te is csõdbe mentél!", 130);
            csokiOpcioGomb = CreateResultButton("", "Nem nyertél semmit! Gratulálunk te is csõdbe mentél!", 235);

            nyeremenyPanel.Controls.Add(uresOpcioGomb);
            nyeremenyPanel.Controls.Add(autoOpcioGomb);
            nyeremenyPanel.Controls.Add(csokiOpcioGomb);
            this.Controls.Add(nyeremenyPanel);
        }

        private Button CreateAnswerButton(int index, int yPosition)
        {
            var gomb = new Button
            {
                Size = new Size(300, 40),
                Location = new Point(50, yPosition),
                BackColor = Color.LightGray
            };
            gomb.Click += ValaszGomb_Click;
            this.Controls.Add(gomb);
            return gomb;
        }

        private Button CreateResultButton(string text, string message, int xPosition)
        {
            var gomb = new Button
            {
                Text = text, 
                Size = new Size(100, 40),
                Location = new Point(xPosition, 20),
                BackColor = Color.LightGray
            };
            gomb.Click += (sender, e) =>
            {
                MessageBox.Show(message, "Nyeremény", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            return gomb;
        }

        private void KerdesMegjelenitese()
        {
            if (jelenlegiKerdezesIndex >= kerdesek.Count)
            {
                JatekVege();
                return;
            }

            var kerdes = kerdesek[jelenlegiKerdezesIndex];
            kerdesLabel.Text = kerdes.Szoveg;

            valaszGomb1.Text = kerdes.Valaszok[0];
            valaszGomb2.Text = kerdes.Valaszok[1];
            valaszGomb3.Text = kerdes.Valaszok[2];

            ResetButtonState(valaszGomb1);
            ResetButtonState(valaszGomb2);
            ResetButtonState(valaszGomb3);

            kovetkezoGomb.Visible = false;
        }

        private void ResetButtonState(Button button)
        {
            button.BackColor = Color.LightGray;
            button.Enabled = true;
        }

        private void ValaszGomb_Click(object sender, EventArgs e)
        {
            var gomb = sender as Button;
            int valasztottValasz = Array.IndexOf(new[] { valaszGomb1, valaszGomb2, valaszGomb3 }, gomb);

            if (valasztottValasz == kerdesek[jelenlegiKerdezesIndex].HelyesValaszIndex)
            {
                gomb.BackColor = Color.Green;
                helyesValaszok++;
            }
            else
            {
                gomb.BackColor = Color.Red;
            }

            valaszGomb1.Enabled = false;
            valaszGomb2.Enabled = false;
            valaszGomb3.Enabled = false;

            kovetkezoGomb.Visible = true;
        }

        private void KovetkezoGomb_Click(object sender, EventArgs e)
        {
            jelenlegiKerdezesIndex++;
            KerdesMegjelenitese();
        }

        private void JatekVege()
        {
            kerdesLabel.Text = "Vége a játéknak!";

            valaszGomb1.Visible = false;
            valaszGomb2.Visible = false;
            valaszGomb3.Visible = false;

            kovetkezoGomb.Visible = false;

            if (helyesValaszok >= 3)
            {
                nyeremenyPanel.Visible = true;
            }
            else
            {
                kerdesLabel.Text += "\nSajnos nem sikerült elérni a 3 helyes választ.";
            }
        }
    }

    public class Kerdezes
    {
        public string Szoveg { get; }
        public string[] Valaszok { get; }
        public int HelyesValaszIndex { get; }

        public Kerdezes(string szoveg, string[] valaszok, int helyesValaszIndex)
        {
            Szoveg = szoveg;
            Valaszok = valaszok;
            HelyesValaszIndex = helyesValaszIndex;
        }
    }
}
