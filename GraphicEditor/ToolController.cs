using System.Drawing;

namespace GraphicEditorOOP
{
    public class ToolController
    {
        public Color CurrentColor { get; private set; } = Color.Black;
        public float CurrentThickness { get; private set; } = 2f;

        private DrawingTool activeTool;
        private CanvasManager canvasManager;

        public ToolController(CanvasManager canvasManager)
        {
            this.canvasManager = canvasManager;
        }

        public void SetTool(DrawingTool tool)
        {
            activeTool = tool;
            UpdateToolSettings();
            canvasManager.SetTool(activeTool);
        }

        public void SetColor(Color color)
        {
            CurrentColor = color;
            UpdateToolSettings();
        }

        public void SetThickness(float thickness)
        {
            CurrentThickness = thickness;
            UpdateToolSettings();
        }

        private void UpdateToolSettings()
        {
            if (activeTool != null)
            {
                activeTool.Color = CurrentColor;
                activeTool.Thickness = CurrentThickness;
            }
        }
    }
}