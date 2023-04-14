using Microsoft.VisualBasic;
using System.Drawing.Drawing2D;

namespace X_O
{
    public partial class Form1 : Form
    {
        private int[,] game = new int[3, 3]; // matrice care tine evidenta jocului ( 0 - 0, 1 - X ) 
        private int turn = 0;
        Boolean win = false;
        public Form1()
        {
            InitializeComponent(); // fiecare patratel din grid are 208 x 160 aproximativ
            Size size = new Size(203, 157);
            //.int x = 72, y = 40;
            int[] x = { 72, 72 + 209, 72 + 209 * 2 };
            int[] y = { 40, 40 + 163, 40 + 163 * 2 };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    PictureBox pb = new PictureBox();
                    pb.Location = new Point(x[i], y[j]);
                    pb.Size = size;
                    this.Controls.Add(pb);
                    pb.Click += Pb_Click;
                    pb.Tag = i + "" + j; // punem in fiecare patratel un picturebox
                    game[i, j] = -1; 
                }
            }
        }

        private void Pb_Click(object? sender, EventArgs e)
        {
            if (!win)
            {
                PictureBox pb = (PictureBox)sender;
                string tag = pb.Tag.ToString();
                int i = tag[1] - '0';
                int j = tag[0] - '0'; // luam linia si coloana din tag
                if (game[i, j] == -1) 
                {
                    if (turn % 2 != 0) // click impar se pune 0
                    {
                        pb.Image = Image.FromFile(Environment.CurrentDirectory + "..\\..\\..\\..\\Photos\\0.png");
                        game[i, j] = 0;
                        turn++;
                    }
                    else // click par se pune X
                    {
                        pb.Image = Image.FromFile(Environment.CurrentDirectory + "..\\..\\..\\..\\Photos\\X.png");
                        game[i, j] = 1;
                        turn++;
                    }
                    if (gameWon(game, i, j))
                    {
                        win = true;
                        if (game[i, j] == 1)
                            MessageBox.Show("Player with X WON!\nClick on any square to quit!");
                        else
                            MessageBox.Show("Player with 0 WON!\nClick on any square to quit!");

                    }
                    if (turn == 9 && !gameWon(game, i, j))
                    {
                        win = true;
                        MessageBox.Show("YOU DREW!\nClick on any square to quit!");
                    }

                }
                else
                {
                    MessageBox.Show("You cannot change a previous choice.");
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }
        // toate numerele astea sunt obtinute dupa niste numaraturi de pixeli

        public Boolean gameWon(int[,] game, int row, int column)
        {
            int value = game[row, column];
            bool winR = true, winC = true, winDP = true, winDS = true;
            for (int i = 0; i < 3; i++)
            {
                if (game[row, i] != value)
                    winR = false;
                if (game[i, column] != value)
                    winC = false;
                if (!winR && !winC)
                    break;
            }
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (i == j)
                        if (game[i, j] != value)
                            winDP = false;
                    if (i + j == 2)
                        if (game[i, j] != value)
                            winDS = false;
                }
            if (winR || winC || winDP || winDS)
            {
                if (winR)
                    drawRow(row);
                else if (winC)
                    drawColumn(column);
                else if (winDP)
                    drawDiagP();
                else 
                    drawDiagS();
                return true;
            }
            return false;
        }

        private void drawRow(int row)
        {
            Panel pan = new Panel();
            int aux = (row * 170);
            pan.Enabled = false;
            pan.Width = 612;
            pan.Height = 5;
            pan.Location = new Point(72, 110 + aux);
            pan.BackColor = Color.Red;
            Controls.Add(pan);
            pan.BringToFront();
        }

        private void drawColumn(int column)
        {
            Panel pan = new Panel();
            int aux = (column * 210);
            pan.Enabled = false;
            pan.Width = 5;
            pan.Height = 480;
            pan.Location = new Point(165 + aux, 40);
            pan.BackColor = Color.Red;
            Controls.Add(pan);
            pan.BringToFront();
        }
        
        private void drawDiagP()
        {
            Panel pan = new Panel();
            pan.Enabled = false;
            pan.Width = Width;
            pan.Height = Height;
            pan.BackColor = Color.Red;
            Point[] points = {
                  new Point(72, 40),
                  new Point(77, 40),
                  new Point(728, 550),
                  new Point(728, 555) ,
                  new Point(720, 550),
                  new Point(72, 45),
            };
            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(points);
            pan.Region = new Region(gp);
            Controls.Add(pan);
            pan.BringToFront();
        }

        private void drawDiagS()
        {
            Panel pan = new Panel();
            pan.Enabled = false;
            pan.Width = Width;
            pan.Height = Height;
            pan.BackColor = Color.Red;
            Point[] points = {
                  new Point(690, 40),
                  new Point(685, 40),
                  new Point(72, 530),
                  new Point(75, 530) ,
                  new Point(72, 540),
                  new Point(690, 45),
            };
            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(points);
            pan.Region = new Region(gp);
            Controls.Add(pan);
            pan.BringToFront();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen lineBrush = new Pen(Color.Black,6);
            Point p1, p2;
            int start_h = 72;
            int end_h = 690;
            int d_h = 200;
            int start_o = start_h + (end_h - start_h) / 3;
            for (int i = 0; i < 2; i = i + 1)
            {
                if (i == 1)
                    d_h += 3;
                p1 = new Point(start_h, d_h);
                p2 = new Point(end_h, d_h);
                e.Graphics.DrawLine(lineBrush, p1, p2);
                d_h += 160;
            }
            p1 = new Point(start_o, 200 - 160);
            p2 = new Point(start_o, d_h + 3);
            e.Graphics.DrawLine(lineBrush, p1, p2);
            p1 = new Point(start_o + 209, 200 - 160);
            p2 = new Point(start_o + 209, d_h + 3);
            e.Graphics.DrawLine(lineBrush, p1, p2);
        }
    }
}