﻿using System;
using System.Globalization;
using System.Linq;
using Windows.UI.Input.Inking;

namespace KLibrary.Labs.WinRT
{
    class InkManagerContext
    {
        public InkManager Value { get; private set; }

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
            Value = new InkManager();
            UpdateCulture();
        }

        void UpdateCulture()
        {
            var recognizers = Value.GetRecognizers();
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

            if (recognizer != null) Value.SetDefaultRecognizer(recognizer);
        }

        public void Fallback()
        {
            lock (this)
            {
                var strokes = Value.GetStrokes();

                Initialize();

                foreach (var stroke in strokes)
                {
                    Value.AddStroke(stroke.Clone());
                }
            }
        }

        public void Undo()
        {
            lock (this)
            {
                var strokes = Value.GetStrokes();
                if (strokes.Count == 0) return;

                Initialize();

                foreach (var stroke in strokes.Take(strokes.Count - 1))
                {
                    Value.AddStroke(stroke.Clone());
                }
            }
        }
    }
}
