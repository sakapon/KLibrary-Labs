using System;
using System.Globalization;
using System.Linq;
using Windows.UI.Input.Inking;

namespace KLibrary.Labs.UI.Input
{
    class InkManagerContext
    {
        public InkManager Manager { get; private set; }

        CultureInfo _culture = CultureInfo.CurrentCulture;

        public CultureInfo Culture
        {
            get { return _culture; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");

                _culture = value;
                UpdateCulture();
            }
        }

        public InkManagerContext()
        {
            Initialize();
        }

        // Can be repeated to call.
        public void Initialize()
        {
            Manager = new InkManager();
            UpdateCulture();
        }

        void UpdateCulture()
        {
            var recognizers = Manager.GetRecognizers();
            var recognizer = default(InkRecognizer);

            switch (_culture.Name)
            {
                case "ja-JP":
                    recognizer = recognizers.FirstOrDefault(r => r.Name.Contains("日本語"));
                    break;
                case "en-US":
                    recognizer = recognizers.FirstOrDefault(r => r.Name.Contains("English") && r.Name.Contains("US"));
                    break;
                case "en-GB":
                    recognizer = recognizers.FirstOrDefault(r => r.Name.Contains("English") && r.Name.Contains("UK"));
                    break;
                case "en-CA":
                    recognizer = recognizers.FirstOrDefault(r => r.Name.Contains("English") && r.Name.Contains("Canada"));
                    break;
                case "en-AU":
                    recognizer = recognizers.FirstOrDefault(r => r.Name.Contains("English") && r.Name.Contains("Australia"));
                    break;
                default:
                    break;
            }

            if (recognizer != null) Manager.SetDefaultRecognizer(recognizer);
        }

        public void Fallback()
        {
            lock (this)
            {
                var strokes = Manager.GetStrokes();

                Initialize();

                foreach (var stroke in strokes)
                {
                    Manager.AddStroke(stroke.Clone());
                }
            }
        }

        public void UndoStroke()
        {
            lock (this)
            {
                var strokes = Manager.GetStrokes();
                if (strokes.Count == 0) return;

                Initialize();

                foreach (var stroke in strokes.Take(strokes.Count - 1))
                {
                    Manager.AddStroke(stroke.Clone());
                }
            }
        }
    }
}
