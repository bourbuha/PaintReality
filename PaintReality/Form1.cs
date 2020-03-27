using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace PaintReality
{
    public partial class Form1 : Form
    {
        Point CurrentPoint;
        Point PrevPoint;
        private bool paint = false;
        private Image img = null;
        private BufferedGraphicsContext bgc = BufferedGraphicsManager.Current;
        private BufferedGraphics bg;
        Color color1 = Color.Black;
        Color color2 = Color.White;
        Graphics g;
        Graphics gimp;
        ColorDialog ColorDialog = new ColorDialog();
        float width1, width2;
        int Tool1;
        private int t;
        Pen pen1, pen2;
        private GraphicsPath tr;
        PointF[] points;
        int N;

        public Form1()
        {
            InitializeComponent();
            paint = false;
            Tool1 = 1;
            width1 = 5; 
            width2 = 5;
            N = 5;
        }

        private void Pen1()
        {
            
        }

        private void Pen2()
        {
            g = panel1.CreateGraphics();
            Pen pen = new Pen(color2, width2);
            g.DrawLine(pen, PrevPoint, CurrentPoint);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e){}

        private void panel5_MouseClick(object sender, MouseEventArgs e){}

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            PrevPoint = e.Location;
            paint = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                if (img == null) return;
                if (bg == null) return;
                pen1 = new Pen(color1, width1);
                pen2 = new Pen(color2, width2);
                pen1.EndCap = pen1.StartCap = pen2.EndCap = pen2.StartCap = LineCap.Round;
                Graphics bgg = bg.Graphics;
                SolidBrush b = new SolidBrush(color1);
                g = panel1.CreateGraphics();
                gimp = Graphics.FromImage(img);
                float x1, y1, x2, y2;
                x2 = e.X;
                y2 = e.Y;
                x1 = PrevPoint.X;
                y1 = PrevPoint.Y;
                switch (Tool1)
                {
                    case 1:
                        g.DrawImage(img, 0, 0);
                        CurrentPoint = e.Location;
                        if (e.Button == MouseButtons.Left) gimp.DrawLine(pen1, PrevPoint, CurrentPoint);
                        else gimp.DrawLine(pen2, PrevPoint, CurrentPoint);
                        PrevPoint = CurrentPoint;
                        break;
                    case 2:
                        bgg.DrawImage(img, 0, 0);
                        if (e.Button == MouseButtons.Left)
                        {
                            t = 1;
                            if (x2 < x1)
                            {
                                if (y2 < y1) bgg.DrawRectangle(pen1, x2, y2, x1 - x2, y1 - y2);
                                else bgg.DrawRectangle(pen1, x2, y1, x1 - x2, y2 - y1);
                            }
                            else
                            {
                                if (y2 > y1) bgg.DrawRectangle(pen1, x1, y1, x2 - x1, y2 - y1);
                                else bgg.DrawRectangle(pen1, x1, y2, x2 - x1, y1 - y2);

                            }
                            bg.Render();
                        }
                        else
                        {
                            if (e.Button == MouseButtons.Right)
                            {
                                t = 1;
                                if (x2 < x1)
                                {
                                    if (y2 < y1) bgg.FillRectangle(b, x2, y2, x1 - x2, y1 - y2);
                                    else bgg.FillRectangle(b, x2, y1, x1 - x2, y2 - y1);
                                }
                                else
                                {
                                    if (y2 > y1) bgg.FillRectangle(b, x1, y1, x2 - x1, y2 - y1);
                                    else bgg.FillRectangle(b, x1, y2, x2 - x1, y1 - y2);
                                }
                                bg.Render();
                            }
                        }
                        break;
                    case 3:
                        bgg.DrawImage(img, 0, 0);
                        if (e.Button == MouseButtons.Left)
                        {
                            bgg.DrawEllipse(pen1, x1, y1, x2 - x1, y2 - y1);
                        }
                        else
                        {
                            if (e.Button == MouseButtons.Right)
                            {
                                {
                                    bgg.DrawImage(img, 0, 0);
                                    bgg.DrawEllipse(pen1, x1, y1, x2 - x1, y2 - y1);
                                    bgg.FillEllipse(b, x1, y1, x2 - x1, y2 - y1);

                                }
                            }
                        }
                        bg.Render();
                        break;
                    case 4:
                        bgg.DrawImage(img, 0, 0);
                        tr = new GraphicsPath();
                        tr.StartFigure();
                        tr.AddLine(x1, y1, x2, y2);
                        tr.AddLine(x2, y2, 2 * x1 - x2, y2);
                        tr.AddLine(2 * x1 - x2, y2, x1, y1);
                        tr.CloseFigure();
                        if (e.Button == MouseButtons.Left)
                        {
                            bgg.DrawPath(pen1, tr);
                        }
                        else
                        {
                            if (e.Button == MouseButtons.Right)
                            {
                                bgg.FillPath(b, tr);
                            }
                        }
                        bg.Render();
                        break;
                    case 5:
                        double a0 = Math.PI / 2, a = a0, q;
                        points = new PointF[2 * N + 1];
                        for (int i = 0; i < 2 * N + 1; i++)
                        {
                            if (i % 2 == 0)
                            {
                                q = y2 - y1;
                            }
                            else
                            {
                                q = x2 - x1;
                            }
                            points[i] = new PointF((float)(x2 + Math.Cos(a) * q), (float)(y2 + Math.Sin(a) * q));
                            a += Math.PI / N;
                        }
                        bgg.DrawImage(img, 0, 0);
                        if (e.Button == MouseButtons.Left)
                        {
                            bgg.DrawPolygon(pen1, points);
                        }
                        else
                        {
                            if (e.Button == MouseButtons.Right)
                            {
                                bgg.FillPolygon(b, points);
                            }
                        }
                        bg.Render();
                        break;
                    case 6:
                        bgg.DrawImage(img, 0, 0);
                        tr = new GraphicsPath();
                        tr.StartFigure();
                        tr.AddLine(x1, y1, x2, y1);
                        tr.AddLine(x2, y1, (x1 + 2 * x2) / 3, y1 - (x2 - x1) / 3);
                        tr.AddLine((x1 + 2 * x2) / 3, y1 - (x2 - x1) / 3, (x1 + 2 * x2) / 3, y1 + (x2 - x1) / 3);
                        tr.AddLine((x1 + 2 * x2) / 3, y1 + (x2 - x1) / 3, x2, y1 + 1);
                        tr.AddLine(x2, y1 + 1, x1, y1 + 1);
                        tr.AddLine(x1, y1 + 1, x1, y1);
                        tr.CloseFigure();
                        if (e.Button == MouseButtons.Left)
                        {
                            bgg.DrawPath(pen1, tr);
                        }
                        else
                        {
                            if (e.Button == MouseButtons.Right)
                            {
                                bgg.FillPath(b, tr);
                            }
                        }
                        bg.Render();
                        break;
                    case 7:
                        bgg.DrawImage(img, 0, 0);
                        tr = new GraphicsPath();
                        tr.StartFigure();
                        tr.AddLine(x1, y1, x2, y2);
                        tr.CloseFigure();
                        if (e.Button == MouseButtons.Left)
                        {
                            bgg.DrawPath(pen1, tr);
                        }
                        else
                        {
                            if (e.Button == MouseButtons.Right)
                            {
                                bgg.DrawPath(pen2, tr);
                            }
                        }
                        bg.Render();
                        break;
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            pen1 = new Pen(color1, width1);
            pen1.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pen1.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen2 = new Pen(color2, width2);
            pen2.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pen2.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            SolidBrush b = new SolidBrush(color1);
            int x1, y1, x2, y2;
            x2 = e.X;
            y2 = e.Y;
            x1 = PrevPoint.X;
            y1 = PrevPoint.Y;
            switch (Tool1)
            {
                case 1:
                    paint = false;
                    break;
                case 2:
                    if (t == 1)
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            if (x2 < x1)
                            {
                                if (y2 < y1) gimp.FillRectangle(b, new Rectangle(x2, y2, x1 - x2, y1 - y2));
                                else gimp.FillRectangle(b, new Rectangle(x2, y1, x1 - x2, y2 - y1));
                            }
                            else
                            {
                                if (y2 > y1) gimp.FillRectangle(b, new Rectangle(x1, y1, x2 - x1, y2 - y1));
                                else gimp.FillRectangle(b, new Rectangle(x1, y2, x2 - x1, y1 - y2));
                            }
                            panel1.CreateGraphics().DrawImage(img, 0, 0);
                        }
                        else
                        {
                            if (e.Button == MouseButtons.Left)
                            {

                                if (x2 < x1)
                                {
                                    if (y2 < y1) gimp.DrawRectangle(pen1, new Rectangle(x2, y2, x1 - x2, y1 - y2));
                                    else gimp.DrawRectangle(pen1, new Rectangle(x2, y1, x1 - x2, y2 - y1));
                                }
                                else
                                {
                                    if (y2 > y1) gimp.DrawRectangle(pen1, new Rectangle(x1, y1, x2 - x1, y2 - y1));
                                    else gimp.DrawRectangle(pen1, new Rectangle(x1, y2, x2 - x1, y1 - y2));
                                }
                            }
                            t = 0;
                        }
                    }
                    panel1.CreateGraphics().DrawImage(img, 0, 0);
                    paint = false;
                    break;
                case 3:
                    gimp = Graphics.FromImage(img);
                    if (e.Button == MouseButtons.Left)
                    {
                        gimp.DrawEllipse(pen1, x1, y1, x2 - x1, y2 - y1);
                    }
                    else
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            {
                                gimp.FillEllipse(b, x1, y1, x2 - x1, y2 - y1);
                            }
                        }
                    }
                    paint = false;
                    break;
                case 4:
                    gimp = Graphics.FromImage(img);
                    if (e.Button == MouseButtons.Left)
                    {
                        gimp.DrawPath(pen1, tr);
                    }
                    else
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            gimp.FillPath(b, tr);
                        }
                    }
                    paint = false;
                    break;
                case 5:
                    gimp = Graphics.FromImage(img);
                    if (e.Button == MouseButtons.Left)
                    {
                        gimp.DrawPolygon(pen1, points);
                    }
                    else
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            gimp.FillPolygon(b, points);
                        }
                    }
                    paint = false;
                    break;
                case 6:
                    gimp = Graphics.FromImage(img);
                    if (e.Button == MouseButtons.Left)
                    {
                        gimp.DrawPath(pen1, tr);
                    }
                    else
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            gimp.FillPath(b, tr);
                        }
                    }
                    paint = false;
                    break;
                case 7:
                    gimp = Graphics.FromImage(img);
                    if (e.Button == MouseButtons.Left)
                    {
                        gimp.DrawPath(pen1, tr);
                    }
                    else
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            gimp.DrawPath(pen2, tr);
                        }
                    }
                    paint = false;
                    break;
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gimp.Clear(System.Drawing.Color.White);
            g.DrawImage(img, 0, 0);
            panel1.Refresh();
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            Tool1 = 1;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (img == null)
            {
                var b1 = new SolidBrush(System.Drawing.Color.White);
                int MW = panel1.Width;
                int MH = panel1.Height;
                img = new Bitmap(2000,1000,panel1.CreateGraphics());
                gimp = Graphics.FromImage(img);

                gimp.Clear(Color.White);

            }
            bg = bgc.Allocate(panel1.CreateGraphics(), new Rectangle(0, 0, panel1.Width, panel1.Height));

            //e.Graphics.DrawImage(img, 0, 0);
            //bg =bgc.Allocate(panel1.CreateGraphics(),)
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            Tool1 = 2;
        }

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                color1 = ColorDialog.Color;
                label1.BackColor = ColorDialog.Color;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            width2 = Decimal.ToInt32(numericUpDown1.Value);
            pen2.Width = width2;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "Файлы картинок в формате JPG|*.jpg|Файлы картинок в формате PNG|*.png";
            var res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                img.Save(saveFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tool1 = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Tool1 = 4;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Tool1 = 5;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            N = Decimal.ToInt32(numericUpDown1.Value);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Tool1 = 6;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Tool1 = 7;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(g==null) g = panel1.CreateGraphics();
            openFileDialog1.AddExtension = true;
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                gimp.Clear(Color.White);
                Image img2 = Image.FromFile(this.openFileDialog1.FileName.ToString());
                img = new Bitmap(img2, img.Size);
                gimp.Clear(Color.White);
                gimp.Clear(color1);
                g.DrawImage(img, 0, 0);
                gimp.DrawImage(img2, 0, 0);
                g.DrawImage(img, 0, 0);
            }
        }

        private void openAndStretchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g == null) g = panel1.CreateGraphics();
            openFileDialog1.AddExtension = true;
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                gimp.Clear(Color.White);
                Image img2 = Image.FromFile(openFileDialog1.FileName.ToString());
                gimp.DrawImage(img2, 0, 0);
                g.DrawImage(img, 0, 0);
            }
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            width1 = Decimal.ToInt32(numericUpDown1.Value);
            pen1.Width = width1;
        }

        private void numericUpDown3_ValueChanged_1(object sender, EventArgs e)
        {
            width2 = Decimal.ToInt32(numericUpDown1.Value);
            pen2.Width = width2;
        }

        private void label2_MouseClick(object sender, MouseEventArgs e)
        {
            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                color2 = ColorDialog.Color;
                label2.BackColor = ColorDialog.Color;
            }
        }
    }
}