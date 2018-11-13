using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDLL.Interfaces;
using GDLL.Figures;
using GDLL.Figures.AuxilaryItems;
using GDLL.Paint.Coloring;
using GDLL.Constants;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CourseDemonstration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Figure> demoFigures = new List<Figure>();
        List<GlFigure> drawFigures = new List<GlFigure>();
        Regex normalInputDot = new Regex("^-?\\d+(\\.\\d+)?$");
        Regex normalInputComma = new Regex("^-?\\d+(\\,\\d+)?$");
        Figure currentFigure;

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Type t in GlConstants.CERTAIN_FIGURES)
                this.comboBox1.Items.Add(t.Name);
            this.comboBox1.Text = this.comboBox1.Items[0].ToString();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            dataGridView1.Columns.Add("Name", "Figure name, ID");
            dataGridView1.Columns.Add("GeometryParams", "Geometrical parameters");
            dataGridView1.Columns.Add("DrawParams", "Draw parameters");
            dataGridView1.Columns.Add("Inter", "Catch intersections");
            dataGridView1.Columns.Add("Tangent", "Tangent from belongs number");
            dataGridView1.Columns.Add("CurvCircle", "Curvious Circle from belongs number");

            WinFormsGlContext WFGLC = new WinFormsGlContext(renderTimer, this, context, GraphicsInit, Redraw, Reshape, 60);
            WFGLC.RenderStart();
        }

        public void GraphicsInit()
        {
            
        }

        public void Redraw()
        {
            new GlRectangle(new GlPointR2(-10, 10), 25, 20, new GlVectorR2(1, 0)).DrawFill(GlConstants.BLACK);
            List<GlPointR2> inters = new List<GlPointR2>();
            for (int i = 0; i < demoFigures.Count; i++)
            {
                if (!demoFigures[i].IsFill)
                    demoFigures[i].View.Draw(demoFigures[i].DrawColor, demoFigures[i].DrawWidth);
                else
                    demoFigures[i].View.DrawFill(demoFigures[i].DrawColor);

                if (demoFigures[i].catchIntersections)
                    for (int j = i; j < demoFigures.Count; j++)
                        if (demoFigures[j].catchIntersections)
                            foreach (GlPointR2 p in demoFigures[i].View.getIntersection(demoFigures[j].View))
                                inters.Add(p);
                try
                {
                    GlCurve C = demoFigures[i].View as GlCurve;
                    if (demoFigures[i].tangentFrom != -1)
                        C.getTangentFromBelongs(C[demoFigures[i].tangentFrom]).Draw(5, GlConstants.RED);
                    if(demoFigures[i].curvFrom != -1)
                        C.getCurvatureCircle(C[demoFigures[i].curvFrom]).Draw(GlConstants.RED, 5);
                    if (demoFigures[i].boxDraw)
                        demoFigures[i].View.BOX.Draw(GlConstants.RED, 5);
                }
                catch { }

                demoFigures[i].Transformation();
            }
            foreach (GlPointR2 p in inters)
                new GlCircle(0.1f, p).DrawFill(GlConstants.RED);
        }

        public void Reshape(int width, int height)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateFigure();
        }

        private void updateFigure()
        {
            if (currentFigure != null)
                currentFigure.Remove(this);

            Figure F = new Figure(GlConstants.CERTAIN_FIGURES[comboBox1.SelectedIndex], comboBox1.SelectedIndex, label1.Font);
            currentFigure = F;

            F.PlaceDown(this, new Point(dataGridView1.Location.X, dataGridView1.Location.Y + dataGridView1.Height + 10), 
                                        this.Width - context.Width - 10, this.Height - 20 - dataGridView1.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || GlConstants.CERTAIN_FIGURES[comboBox1.SelectedIndex].Equals(GlConstants.VECTOR) ||
                GlConstants.CERTAIN_FIGURES[comboBox1.SelectedIndex].Equals(GlConstants.FIGURE_SYSTEM) ||
                GlConstants.CERTAIN_FIGURES[comboBox1.SelectedIndex].Equals(GlConstants.POLYGON))
                return;

            List<string> figureInfo = new List<string>();
            foreach (TextBox i in currentFigure.DataFields)
                if (i.Text == "" || (!normalInputDot.IsMatch(i.Text) && !normalInputComma.IsMatch(i.Text)))
                    return;
                else
                    figureInfo.Add(i.Text);
                    
            GlFigure f = currentFigure.constructFigure(GlConstants.CERTAIN_FIGURES[currentFigure.FigureIndex]);
            currentFigure.View = f;

            try
            {
                currentFigure.Angle = (float)Convert.ToDouble(textBox2.Text);
                currentFigure.DX = (float)Convert.ToDouble(textBox3.Text);
                currentFigure.DY = (float)Convert.ToDouble(textBox4.Text);
                currentFigure.deltaScale = (float)Convert.ToDouble(textBox5.Text);
                currentFigure.DR = Convert.ToInt32(numericUpDown1.Value);
                currentFigure.DG = Convert.ToInt32(numericUpDown2.Value);
                currentFigure.DB = Convert.ToInt32(numericUpDown3.Value);
                currentFigure.catchIntersections = checkBox1.Checked;
                currentFigure.tangentFrom = Convert.ToInt32(numericUpDown4.Value);
                currentFigure.curvFrom = Convert.ToInt32(numericUpDown5.Value);
                currentFigure.boxDraw = checkBox2.Checked;
            }
            catch 
            {
                MessageBox.Show("Fill in all the gaps!", "Error");
                return;
            }

            updateDrawing();
            demoFigures.Add(currentFigure);

            dataGridView1.Rows.Add();
            dataGridView1.Rows[demoFigures.Count - 1].Cells[0].Value = currentFigure.View.GetType().Name + ", " + (demoFigures.Count - 1);
            dataGridView1.Rows[demoFigures.Count - 1].Cells[1].Value = string.Join(";", figureInfo.ToArray());
            dataGridView1.Rows[demoFigures.Count - 1].Cells[2].Value = (comboBox2.SelectedIndex == -1 ? "Normal" : comboBox2.Items[comboBox2.SelectedIndex].ToString()) 
                                                                        + ";" + button2.BackColor.R + ";" + button2.BackColor.G + ";" 
                                                                        + button2.BackColor.B + ";" + textBox1.Text;
            dataGridView1.Rows[demoFigures.Count - 1].Cells[3].Value = checkBox1.Checked.ToString();
            dataGridView1.Rows[demoFigures.Count - 1].Cells[4].Value = numericUpDown4.Value.ToString();
            dataGridView1.Rows[demoFigures.Count - 1].Cells[5].Value = numericUpDown5.Value.ToString();
            updateFigure();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();
            CD.ShowDialog();
            (sender as Button).BackColor = CD.Color;
            currentFigure.DrawColor = new GlColor(CD.Color.R, CD.Color.G, CD.Color.B);
        }

        private void updateDrawing()
        {
            currentFigure.DrawColor = new GlColor(button2.BackColor.R, button2.BackColor.G, button2.BackColor.B);
            currentFigure.IsFill = comboBox2.SelectedIndex == 1;
            currentFigure.DrawWidth = textBox1.Text == "" ? 1 : (float)Convert.ToDouble(textBox1.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox TB = (sender as TextBox);

            if (TB.Text.Length != 0 && char.IsLetter(TB.Text[TB.Text.Length - 1]))
                TB.Text = TB.Text.Substring(0, TB.Text.Length - 1);
            else if (TB.Text.Length != 0 && TB.Text[TB.Text.Length - 1] == '.')
                TB.Text = TB.Text.Substring(0, TB.Text.Length - 1) + ',';

            SendKeys.Send("{END}");
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell D = (sender as DataGridView).CurrentCell;

            if (D.RowIndex >= demoFigures.Count)
                return;

            currentFigure.Remove(this);

            currentFigure = demoFigures[D.RowIndex];
            currentFigure.DrawWidth += 1;

            currentFigure.PlaceDown(this, new Point(dataGridView1.Location.X, dataGridView1.Location.Y + dataGridView1.Height + 10),
                                        this.Width - context.Width - 10, this.Height - 20 - dataGridView1.Height);

            string[] geomInfo = D.OwningRow.Cells[1].Value.ToString().Split(';');
            string[] drawInfo = D.OwningRow.Cells[2].Value.ToString().Split(';');

            for (int i = 0; i < currentFigure.DataFields.Count; i++)
                currentFigure.DataFields[i].Text = geomInfo[i];

            comboBox2.SelectedIndex = drawInfo[0] == "Normal" ? 0 : 1;
            button2.BackColor = Color.FromArgb(Convert.ToInt32(drawInfo[1]),Convert.ToInt32(drawInfo[2]),Convert.ToInt32(drawInfo[3]));
            textBox1.Text = drawInfo[4];

            button1.Enabled = false;
            comboBox1.Enabled = false;
            dataGridView1.Enabled = false;

            Button edit = new Button();
            edit.Text = "Edit Figure!";
            edit.Click += edit_Click;
            edit.Location = new Point(dataGridView1.Location.X, dataGridView1.Location.Y + dataGridView1.Height + currentFigure.DataFields.Count * 40);
            this.Controls.Add(edit);
        }

        void edit_Click(object sender, EventArgs e)
        {
            demoFigures.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);

            button1_Click(sender, e);

            button1.Enabled = true;
            comboBox1.Enabled = true;
            dataGridView1.Enabled = true;
            this.Controls.Remove(sender as Button);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
