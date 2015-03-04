using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using KLibrary.Labs.Reactive;
using KLibrary.Labs.Reactive.Models;
using Windows.UI.Input.Inking;

namespace KLibrary.Labs.UI.Input
{
    /// <summary>
    /// Provides handwriting recognition for strokes.
    /// </summary>
    public class InkTextRecognizer
    {
        public static readonly CultureInfo Culture_ja_JP = CultureInfo.GetCultureInfo("ja-JP");
        public static readonly CultureInfo Culture_en_US = CultureInfo.GetCultureInfo("en-US");
        public static readonly CultureInfo Culture_en_GB = CultureInfo.GetCultureInfo("en-GB");
        public static readonly CultureInfo Culture_en_CA = CultureInfo.GetCultureInfo("en-CA");
        public static readonly CultureInfo Culture_en_AU = CultureInfo.GetCultureInfo("en-AU");

        InkManagerContext _context = new InkManagerContext();

        /// <summary>
        /// Gets or sets the culture to be used for handwriting recognition.
        /// </summary>
        public CultureInfo Culture
        {
            get { return _context.Culture; }
            set { _context.Culture = value; }
        }

        IObservableEvent<string[][]> _AllResultsArrived = ObservableProperty.CreateEvent<string[][]>();
        IObservableEvent<string[]> _CandidatesArrived = ObservableProperty.CreateEvent<string[]>();
        IObservableEvent<string> _BestResultArrived = ObservableProperty.CreateEvent<string>();

        public IObservable<string[][]> AllResultsArrived { get; private set; }
        public IObservable<string[]> CandidatesArrived { get; private set; }
        public IObservable<string> BestResultArrived { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkTextRecognizer"/> class.
        /// </summary>
        public InkTextRecognizer()
        {
            AllResultsArrived = _AllResultsArrived.ToObservableMask();
            CandidatesArrived = _CandidatesArrived.ToObservableMask();
            BestResultArrived = _BestResultArrived.ToObservableMask();
        }

        /// <summary>
        /// Clear the strokes.
        /// The empty states will be notified via <see cref="AllResultsArrived"/>, <see cref="CandidatesArrived"/> and <see cref="BestResultArrived"/>.
        /// </summary>
        public void Clear()
        {
            _context.Initialize();

            _AllResultsArrived.OnNext(new string[0][]);
            _CandidatesArrived.OnNext(new string[0]);
            _BestResultArrived.OnNext("");
        }

        /// <summary>
        /// Adds a stroke.
        /// </summary>
        /// <param name="stroke">The <see cref="Stroke"/> object.</param>
        public void AddStroke(Stroke stroke)
        {
            if (stroke == null) throw new ArgumentNullException("stroke");

            _context.Manager.AddStroke(stroke.StylusPoints.ToInkStroke());
        }

        /// <summary>
        /// Adds a stroke by points.
        /// </summary>
        /// <param name="points">The collection of points.</param>
        public void AddStroke(IEnumerable<Point> points)
        {
            if (points == null) throw new ArgumentNullException("points");

            _context.Manager.AddStroke(points.ToInkStroke());
        }

        /// <summary>
        /// Undo the last stroke.
        /// </summary>
        public void UndoStroke()
        {
            _context.UndoStroke();
        }

        /// <summary>
        /// Performs handwriting recognition for the strokes.
        /// The results of the recognition will be notified via <see cref="AllResultsArrived"/>, <see cref="CandidatesArrived"/> and <see cref="BestResultArrived"/>.
        /// </summary>
        public async void RecognizeAsync()
        {
            try
            {
                var results = await _context.Manager.RecognizeAsync(InkRecognitionTarget.All);

                // 候補 (text candidates) は 5 つ取得されます。
                // ja-JP の場合、単語で区切られません (InkRecognitionResult は 1 つ)。
                var allResults = results.Select(r => r.GetTextCandidates().ToArray()).ToArray();
                var candidatesLength = allResults.Min(r => r.Length);
                var candidates = Enumerable.Range(0, candidatesLength)
                    .Select(i => string.Join(" ", allResults.Select(r => r[i])))
                    .ToArray();

                _AllResultsArrived.OnNext(allResults);
                _CandidatesArrived.OnNext(candidates);
                _BestResultArrived.OnNext(candidates.FirstOrDefault());
            }
            catch (ArgumentException ex)
            {
                // Message: 値が有効な範囲にありません。
                // ja-JP で、しばらく放置したあとに認識させると発生します。
                _context.Fallback();
                RecognizeAsync();

                System.Diagnostics.Debug.WriteLine(ex);
            }
            catch (InvalidOperationException ex)
            {
                // Message: 予期しないときにメソッドが呼び出されました。
                // 短い期間に認識が多く実行されると発生します。
                // とくに対処しなくても、再度認識が実行されるようです。
                System.Diagnostics.Debug.WriteLine(ex);
            }
            catch (Exception ex)
            {
                // Message: ターゲット コマンドは無効化されています。
                // ストロークが空のときに認識を実行すると発生します。
                _AllResultsArrived.OnNext(new string[0][]);
                _CandidatesArrived.OnNext(new string[0]);
                _BestResultArrived.OnNext("");

                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }

    static class InkStrokeHelper
    {
        static InkStrokeBuilder _builder = new InkStrokeBuilder();

        public static InkStroke ToInkStroke(this IEnumerable<StylusPoint> points)
        {
            return _builder.CreateStroke(points.Select(p => new Windows.Foundation.Point(p.X, p.Y)));
        }

        public static InkStroke ToInkStroke(this IEnumerable<Point> points)
        {
            return _builder.CreateStroke(points.Select(p => new Windows.Foundation.Point(p.X, p.Y)));
        }
    }
}
