using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GraphicEditorOOP
{
    public class MainForm : Form
    {
        private CanvasManager canvasManager;
        private ToolController toolController;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Графический редактор (OOP)";
            this.Size = new Size(900, 600);
            this.MinimumSize = new Size(800, 400);

            Panel toolPanel = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.WhiteSmoke };

            PictureBox canvasBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Cursor = Cursors.Cross
            };

            this.Controls.Add(canvasBox);
            this.Controls.Add(toolPanel);

            Button btnPencil = new Button { Text = "Карандаш", Location = new Point(10, 12) };
            Button btnLine = new Button { Text = "Линия", Location = new Point(90, 12) };
            Button btnRect = new Button { Text = "Прямоугольник", Location = new Point(170, 12), Width = 110 };
            Button btnEllipse = new Button { Text = "Эллипс", Location = new Point(290, 12) };
            Button btnColor = new Button { Text = "Цвет...", Location = new Point(380, 12), BackColor = Color.Black, ForeColor = Color.White };

            Label lblThick = new Label { Text = "Толщина:", Location = new Point(470, 17), Width = 60 };
            NumericUpDown nudThickness = new NumericUpDown { Location = new Point(530, 14), Width = 50, Minimum = 1, Maximum = 50, Value = 2 };

            Button btnClear = new Button { Text = "Очистить", Location = new Point(600, 12), Width = 90 };
            Button btnSave = new Button { Text = "Сохранить", Location = new Point(700, 12), Width = 90 };

            toolPanel.Controls.AddRange(new Control[] { btnPencil, btnLine, btnRect, btnEllipse, btnColor, lblThick, nudThickness, btnClear, btnSave });

            canvasManager = new CanvasManager(canvasBox);
            toolController = new ToolController(canvasManager);

            toolController.SetTool(new PencilTool());

            btnPencil.Click += (s, e) => toolController.SetTool(new PencilTool());
            btnLine.Click += (s, e) => toolController.SetTool(new LineTool());
            btnRect.Click += (s, e) => toolController.SetTool(new RectangleTool());
            btnEllipse.Click += (s, e) => toolController.SetTool(new EllipseTool());

            btnColor.Click += (s, e) => {
                using (ColorDialog cd = new ColorDialog { Color = toolController.CurrentColor })
                {
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        toolController.SetColor(cd.Color);
                        btnColor.BackColor = cd.Color;
                        btnColor.ForeColor = cd.Color.GetBrightness() < 0.5 ? Color.White : Color.Black;
                    }
                }
            };

            nudThickness.ValueChanged += (s, e) => toolController.SetThickness((float)nudThickness.Value);

            btnClear.Click += (s, e) => canvasManager.ClearCanvas();

            btnSave.Click += (s, e) => {
                using (SaveFileDialog sfd = new SaveFileDialog { Filter = "PNG Image|*.png|JPEG Image|*.jpg", DefaultExt = "png" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        var format = sfd.FileName.EndsWith(".jpg") ? ImageFormat.Jpeg : ImageFormat.Png;
                        canvasManager.SaveImage(sfd.FileName, format);
                    }
                }
            };
        }
    }
}