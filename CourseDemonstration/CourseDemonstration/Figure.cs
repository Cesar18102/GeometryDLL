using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDLL.Figures;
using GDLL.Figures.AuxilaryItems;
using System.Windows.Forms;
using GDLL.Constants;
using System.Reflection;
using System.Drawing;
using GDLL.Constants;
using GDLL.Paint.Coloring;

namespace CourseDemonstration
{
    class Figure
    {
        private Label[] info;
        private Control[] data;
        private GlFigure view;
        private GlColor drawColor = GlConstants.BLACK;
        private float drawWidth = 1f;
        private bool isFill = false;

        private float angle = 0;
        private float dx = 0;
        private float dy = 0;
        private float dR = 0;
        private float dG = 0;
        private float dB = 0;
        private float dScale = 1;
        public bool catchIntersections = false;
        public int tangentFrom = -1;
        public int curvFrom = -1;
        public bool boxDraw = false;

        public float Angle
        {
            get { return angle; }
            set { angle = (float)(value * Math.PI / 90.0f); updateTransformation(); }
        }
        public float DX
        {
            get { return dx; }
            set { dx = value; updateTransformation(); }
        }
        public float DY
        {
            get { return dy; }
            set { dy = value; updateTransformation(); }
        }
        public float DR
        {
            get { return dR; }
            set { dR = value / 255.0f; updateTransformation(); }
        }
        public float DG
        {
            get { return dG; }
            set { dG = value / 255.0f; updateTransformation(); }
        }
        public float DB
        {
            get { return dB; }
            set { dB = value / 255.0f; updateTransformation(); }
        }
        public float deltaScale
        {
            get { return dScale; }
            set { dScale = value; updateTransformation(); }
        }

        private int dataIndex = 0;

        public GlFigure View { get { return view; } set { if (value != null) view = value; } }
        public GlColor DrawColor { get { return new GlColor(drawColor); } set { if (value != null) drawColor = value; } }
        public float DrawWidth { get { return drawWidth; } set { if (value != 0) drawWidth = Math.Abs(value); } }

        public List<Label> labelNames { get { return info.ToList<Label>(); } }
        public List<Control> DataFields { get { return data.ToList<Control>(); } }
        public bool IsFill { get { return isFill; } set { isFill = value; } }
        public int FigureIndex { get; private set; }
        public Action Transformation { get; private set; }

        public Figure(Type T, int index, Font F)
        {
            FigureIndex = index;
            List<ParameterInfo> inputFieldsInfo = paramsData(T);
            int startIndex = 0;

            if (T.Equals(GlConstants.POLYGON) || T.Equals(GlConstants.FIGURE_SYSTEM))
                startIndex = 1;

            info = new Label[inputFieldsInfo.Count + startIndex];
            data = new Control[inputFieldsInfo.Count + startIndex];

            if (startIndex == 1)
            {
                info[0] = new Label();
                info[0].Text = "Count of " + (T.Equals(GlConstants.POLYGON) ? "Vertexes" : "Figures");
                info[0].BackColor = Color.Transparent;
                info[0].ForeColor = Color.White;
                info[0].Font = F;

                data[0] = new TextBox();
                data[0].TextChanged += Figure_TextChanged;
            }

            for (int j = startIndex; j < inputFieldsInfo.Count + startIndex; j++)
            {
                info[j] = new Label();
                info[j].Text = inputFieldsInfo[j - startIndex].Name + ", " + 
                               (inputFieldsInfo[j - startIndex].ParameterType.Name == "Single" ? "float" : 
                               inputFieldsInfo[j - startIndex].ParameterType.Name) + ": ";
                info[j].BackColor = Color.Transparent;
                info[j].ForeColor = Color.White;
                info[j].Font = F;

                data[j] = new TextBox();
                data[j].TextChanged += Figure_TextChanged;
            }
        }

        private void updateTransformation()
        {
            this.Transformation = () =>
            {
                this.view.Rotate(angle);
                this.view.moveTo(this.view.Center.X + dx, this.view.Center.Y + dy);
                this.view = this.view.getScaled(dScale);
                this.drawColor.R += dR;
                this.drawColor.G += dG;
                this.drawColor.B += dB;
            };
        }

        private void Figure_TextChanged(object sender, EventArgs e)
        {
            TextBox TB = (sender as TextBox);

            if (TB.Text.Length != 0 && char.IsLetter(TB.Text[TB.Text.Length - 1]))
                TB.Text = TB.Text.Substring(0, TB.Text.Length - 1);
            else if (TB.Text.Length != 0 && TB.Text[TB.Text.Length - 1] == '.')
                TB.Text = TB.Text.Substring(0, TB.Text.Length - 1) + ',';

            SendKeys.Send("{END}");
        }//fix this

        public GlFigure constructFigure(Type T)
        {
            dataIndex = 0;
            return constructParams(T)[0] as GlFigure;
        }

        public List<Object> constructParams(Type T)
        {
            List<Object> arguments = new List<Object>();

            for (int i = 0; i < GlConstants.CERTAIN_FIGURES.Length; i++)
                if (GlConstants.CERTAIN_FIGURES[i].Equals(T))
                {
                    ConstructorInfo constructorInfo = GlConstants.CONSTRUCT_FIGURE[i];
                    List<ParameterInfo> paramsInfo = constructorInfo.GetParameters().ToList<ParameterInfo>();

                    for (int j = 0; j < paramsInfo.Count; j++)
                        if (paramsInfo[j].ParameterType.Name == "Single")
                            arguments.Add((float)Convert.ToDouble(data[dataIndex++].Text));
                        else
                            foreach (Object k in constructParams(paramsInfo[j].ParameterType))
                                arguments.Add(k);
                    return new Object[] { GlConstants.CONSTRUCT_FIGURE[i].Invoke(arguments.ToArray()) }.ToList<Object>();
                }
            return arguments;
        }

        private List<ParameterInfo> paramsData(Type T)
        {
            List<ParameterInfo> resultInfo = new List<ParameterInfo>();
            for (int i = 0; i < GlConstants.CERTAIN_FIGURES.Length; i++)
                if (GlConstants.CERTAIN_FIGURES[i].Equals(T))
                {
                    ConstructorInfo constructorInfo = GlConstants.CONSTRUCT_FIGURE[i];
                    List<ParameterInfo> paramsInfo = constructorInfo.GetParameters().ToList<ParameterInfo>();

                    for (int j = 0; j < paramsInfo.Count; j++)
                        if (paramsInfo[j].ParameterType.Name != "Single")
                            foreach (ParameterInfo pInfo in paramsData(paramsInfo[j].ParameterType))
                                resultInfo.Add(pInfo);
                        else
                            resultInfo.Add(paramsInfo[j]);

                    return resultInfo;
                }

            return resultInfo;
        }

        public void PlaceDown(Form parent, Point LeftTop, int width, int height)
        {
            int deltaY = (int)(height / info.Length) - info[0].Height;
            int deltaX = (int)(width / 2) - info[0].Width;
            deltaY = deltaY > 10 ? 10 : deltaY;

            for (int i = 0; i < info.Length; i++)
            {
                info[i].Location = new Point(LeftTop.X, LeftTop.Y + i * (info[i].Height + deltaY));
                data[i].Location = new Point(LeftTop.X + info[i].Width + deltaX, LeftTop.Y + i * (info[i].Height + deltaY));

                parent.Controls.Add(info[i]);
                parent.Controls.Add(data[i]);
            }
        }

        public void Remove(Form parent)
        {
            for (int i = 0; i < info.Length; i++)
            {
                parent.Controls.Remove(info[i]);
                parent.Controls.Remove(data[i]);
            }
        }
    }
}
