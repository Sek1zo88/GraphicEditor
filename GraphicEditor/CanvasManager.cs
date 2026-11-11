using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GraphicEditorOOP
{
    public class CanvasManager
    {
        private PictureBox canvas;
        private Bitmap mainBitmap;
        private Bitmap previewBitmap;
        private DrawingTool currentTool;

        public CanvasManager(PictureBox canvas)
        {
            this.canvas = canvas;
            this.canvas.MouseDown += Canvas_MouseDown;
            this.canvas.MouseMove += Canvas_MouseMove;
            this.canvas.MouseUp += Canvas_MouseUp;
            this.canvas.SizeChanged += Canvas_SizeChanged;

            InitBitmap(canvas.Width, canvas.Height);
        }

        private void InitBitmap(int width, int height)
        {
            if (width <= 0 || height <= 0) return;
            Bitmap newBmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(newBmp))
            {
                g.Clear(Color.White);
                if (mainBitmap != null)
                {
                    g.DrawImage(mainBitmap, 0, 0);
                    mainBitmap.Dispose();
                }
            }
            mainBitmap = newBmp;
            canvas.Image = mainBitmap;
        }

        private void Canvas_SizeChanged(object sender, EventArgs e) => InitBitmap(canvas.Width, canvas.Height);

        public void SetTool(DrawingTool tool) => currentTool = tool;

        public void ClearCanvas()
        {
            using (Graphics g = Graphics.FromImage(mainBitmap))
                g.Clear(Color.White);
            canvas.Invalidate();
        }

        public void SaveImage(string fileName, ImageFormat format)
        {
            mainBitmap.Save(fileName, format);
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentTool == null || e.Button != MouseButtons.Left) return;
            currentTool.MouseDown(e.Location);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentTool == null || !currentTool.IsDrawing) return;

            if (!currentTool.RequiresPreview)
            {
                using (Graphics gMain = Graphics.FromImage(mainBitmap))
                {
                    gMain.SmoothingMode = SmoothingMode.AntiAlias;
                    currentTool.MouseMove(e.Location, gMain);
                }
                canvas.Invalidate();
                return;
            }

            previewBitmap?.Dispose();
            previewBitmap = (Bitmap)mainBitmap.Clone();

            using (Graphics gPreview = Graphics.FromImage(previewBitmap))
            {
                gPreview.SmoothingMode = SmoothingMode.AntiAlias;
                currentTool.MouseMove(e.Location, gPreview);
            }
            canvas.Image = previewBitmap;
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentTool == null || !currentTool.IsDrawing || e.Button != MouseButtons.Left) return;

            using (Graphics gMain = Graphics.FromImage(mainBitmap))
            {
                gMain.SmoothingMode = SmoothingMode.AntiAlias;
                currentTool.MouseUp(e.Location, gMain);
            }

            canvas.Image = mainBitmap;
            previewBitmap?.Dispose();
            previewBitmap = null;
        }
    }
}