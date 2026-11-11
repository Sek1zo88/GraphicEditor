using GraphicEditorOOP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GraphicEditorOOP
{
    public abstract class DrawingTool
    {
        public virtual Color Color { get; set; } = Color.Black;
        public float Thickness { get; set; } = 2f;
        public bool IsDrawing { get; protected set; }
        public virtual bool RequiresPreview => true;

        public abstract void MouseDown(Point p);
        public abstract void MouseMove(Point p, Graphics g);
        public abstract void MouseUp(Point p, Graphics g);

        protected Pen GetPen() => new Pen(Color, Thickness)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            LineJoin = LineJoin.Round
        };
    }

    public class PencilTool : DrawingTool
    {
        private Point lastPoint;

        public override bool RequiresPreview => false;

        public override void MouseDown(Point p)
        {
            IsDrawing = true;
            lastPoint = p;
        }

        public override void MouseMove(Point p, Graphics g)
        {
            if (!IsDrawing) return;

            using (var pen = GetPen())
                g.DrawLine(pen, lastPoint, p);

            lastPoint = p;
        }

        public override void MouseUp(Point p, Graphics g)
        {
            if (!IsDrawing) return;

            if (lastPoint == p)
            {
                using (var brush = new SolidBrush(Color))
                    g.FillEllipse(brush, p.X - Thickness / 2f, p.Y - Thickness / 2f, Thickness, Thickness);
            }

            IsDrawing = false;
        }
    }

    public abstract class ShapeTool : DrawingTool
    {
        protected Point startPoint;

        public override void MouseDown(Point p)
        {
            IsDrawing = true;
            startPoint = p;
        }

        public override void MouseMove(Point p, Graphics g)
        {
            if (!IsDrawing) return;
            DrawShape(g, startPoint, p);
        }

        public override void MouseUp(Point p, Graphics g)
        {
            if (!IsDrawing) return;
            DrawShape(g, startPoint, p);
            IsDrawing = false;
        }

        protected abstract void DrawShape(Graphics g, Point start, Point end);

        protected Rectangle GetRectangle(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y));
        }
    }

    public class LineTool : ShapeTool
    {
        protected override void DrawShape(Graphics g, Point start, Point end)
        {
            using (var pen = GetPen())
                g.DrawLine(pen, start, end);
        }
    }

    public class RectangleTool : ShapeTool
    {
        protected override void DrawShape(Graphics g, Point start, Point end)
        {
            using (var pen = GetPen())
                g.DrawRectangle(pen, GetRectangle(start, end));
        }
    }

    public class EllipseTool : ShapeTool
    {
        protected override void DrawShape(Graphics g, Point start, Point end)
        {
            using (var pen = GetPen())
                g.DrawEllipse(pen, GetRectangle(start, end));
        }
    }
}
public class EraserTool : PencilTool
{
    public override Color Color
    {
        get => System.Drawing.Color.White;
        set {}
    }
}