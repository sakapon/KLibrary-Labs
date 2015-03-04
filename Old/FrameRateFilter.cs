using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KLibrary.Labs.Mathematics
{
    /// <summary>
    /// フレーム レートを制御するためのフィルターを表します。
    /// </summary>
    [DebuggerDisplay(@"\{{ArrangedFps} / {OriginalFps}\}")]
    public class FrameRateFilter : INotifyPropertyChanged
    {
        /// <summary>
        /// FPS の最大値を取得または設定します。
        /// </summary>
        /// <value>FPS の最大値。</value>
        public double MaxFps
        {
            get { return maxFps; }
            set
            {
                if (maxFps == value) return;
                maxFps = value;
                NotifyPropertyChanged();
            }
        }
        double maxFps;

        /// <summary>
        /// 元の FPS の値を取得します。
        /// この値は、<see cref="CheckFrame"/> メソッドが呼び出された頻度を表します。
        /// </summary>
        /// <value>元の FPS の値。</value>
        public double OriginalFps { get; private set; }

        /// <summary>
        /// 調整された FPS の値を取得します。
        /// この値は、<see cref="MaxFps"/> 以下となるように調整されます。
        /// </summary>
        /// <value>調整された FPS の値。</value>
        public double ArrangedFps { get; private set; }

        /// <summary>
        /// FPS の値が再計算されると発生します。
        /// </summary>
        public event Action<FrameRateFilter> FpsUpdated = f => { };

        Queue<DateTime> originalTimestamps = new Queue<DateTime>();
        Queue<DateTime> arrangedTimestamps = new Queue<DateTime>();
        const int MaxTimestamps = 30;
        const int MinTimestamps = 3;
        static readonly TimeSpan MaxSpan = TimeSpan.FromSeconds(1);

        /// <summary>
        /// <see cref="FrameRateFilter"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="maxFps">FPS の最大値。</param>
        public FrameRateFilter(double maxFps)
        {
            MaxFps = maxFps;
        }

        /// <summary>
        /// 現在のフレームを報告し、それが有効かどうかを判定します。
        /// </summary>
        /// <returns>現在のフレームが有効かどうかを示す値。</returns>
        public bool CheckFrame()
        {
            var now = DateTime.Now;

            if (originalTimestamps.Count == 0)
            {
                originalTimestamps.Enqueue(now);
                arrangedTimestamps.Enqueue(now);
                return true;
            }

            RemoveOldTimestamps(originalTimestamps, now);
            OriginalFps = GetFps(originalTimestamps, now);
            originalTimestamps.Enqueue(now);

            RemoveOldTimestamps(arrangedTimestamps, now);
            var arrangedFps = GetFps(arrangedTimestamps, now);
            var isActive = arrangedFps <= MaxFps;
            if (isActive)
            {
                ArrangedFps = arrangedFps;
                arrangedTimestamps.Enqueue(now);
            }

            NotifyPropertyChanged("OriginalFps");
            NotifyPropertyChanged("ArrangedFps");
            FpsUpdated(this);
            return isActive;
        }

        static void RemoveOldTimestamps(Queue<DateTime> timestamps, DateTime now)
        {
            while (timestamps.Count > MaxTimestamps)
            {
                timestamps.Dequeue();
            }
            while (now - timestamps.Peek() > MaxSpan && timestamps.Count > MinTimestamps)
            {
                timestamps.Dequeue();
            }
        }

        static double GetFps(Queue<DateTime> timestamps, DateTime now)
        {
            return timestamps.Count / (now - timestamps.Peek()).TotalSeconds;
        }

        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };

        public void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddPropertyChangedHandler(string propertyName, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");

            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    action();
                }
            };
        }
    }
}
