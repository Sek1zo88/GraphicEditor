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
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.Text = "Графический редактор (OOP)";
            this.Size = new Size(900, 600);
            this.MinimumSize = new Size(800, 400);

            FlowLayoutPanel toolPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(5),
                BackColor = Color.WhiteSmoke
            };

            PictureBox canvasBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Cursor = Cursors.Cross
            };

            this.Controls.Add(canvasBox);
            this.Controls.Add(toolPanel);

            Button btnPencil = new Button { Text = "Карандаш", AutoSize = true };
            Button btnLine = new Button { Text = "Линия", AutoSize = true };
            Button btnRect = new Button { Text = "Прямоугольник", AutoSize = true };
            Button btnEllipse = new Button { Text = "Эллипс", AutoSize = true };
            Button btnColor = new Button { Text = "Цвет...", AutoSize = true, BackColor = Color.Black, ForeColor = Color.White };

            Label lblThick = new Label { Text = "Толщина:", AutoSize = true, Margin = new Padding(10, 8, 0, 0) };
            NumericUpDown nudThickness = new NumericUpDown { Width = 50, Minimum = 1, Maximum = 50, Value = 2, Margin = new Padding(3, 5, 3, 3) };

            Button btnClear = new Button { Text = "Очистить", AutoSize = true, Margin = new Padding(20, 3, 3, 3) };
            Button btnSave = new Button { Text = "Сохранить", AutoSize = true };

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