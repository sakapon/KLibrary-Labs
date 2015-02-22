using System;
using System.Windows.Controls;

namespace KLibrary.Labs.UI.Input
{
    public class InkCanvasTextEx
    {
        public InkCanvas Canvas { get; private set; }
        public InkTextRecognizer Recognizer { get; private set; }

        public InkCanvasTextEx(InkCanvas canvas)
        {
            if (canvas == null) throw new ArgumentNullException("canvas");

            Canvas = canvas;
            Recognizer = new InkTextRecognizer();

            Canvas.StrokeCollected += Canvas_StrokeCollected;
        }

        void Canvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            Recognizer.AddStroke(e.Stroke);
            Recognizer.RecognizeAsync();
        }

        public void Clear()
        {
            Canvas.Strokes.Clear();
            Recognizer.Clear();
        }

        public void Undo()
        {
            if (Canvas.Strokes.Count == 0) return;
            Canvas.Strokes.RemoveAt(Canvas.Strokes.Count - 1);

            Recognizer.UndoStroke();
            Recognizer.RecognizeAsync();
        }
    }
}
