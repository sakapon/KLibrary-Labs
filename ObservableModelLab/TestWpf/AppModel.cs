using System;
using System.Reactive.Linq;
using KLibrary.Labs.ObservableModel;

namespace TestWpf
{
    public class AppModel
    {
        public ISettableProperty<string> FirstName { get; private set; }
        public ISettableProperty<string> LastName { get; private set; }
        public IGetOnlyProperty<string> FullName { get; private set; }
        public IGetOnlyProperty<string> Message { get; private set; }
        public IGetOnlyProperty<DateTime> CurrentTime { get; private set; }

        public IGetOnlyProperty<int> RandomNumber { get; private set; }
        ISettableProperty<int> _Count;
        public IGetOnlyProperty<int> Count { get; private set; }

        public AppModel()
        {
            var timer = Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => DateTime.Now);
            var random = new Random();

            FirstName = ObservableProperty.CreateSettable("Jiro");
            LastName = ObservableProperty.CreateSettable("Mita");
            FullName = ObservableProperty.CreateGetOnly(() => string.Format("{0} {1}", FirstName.Value, LastName.Value));
            FirstName.Merge(LastName).Subscribe(FullName);
            Message = FirstName.SelectToGetOnly(name => string.Format("Hello, {0}!", name));
            CurrentTime = timer.ToGetOnly(DateTime.Now);

            // 初期値をランダムに設定する場合。
            //RandomNumber = CurrentTime.SelectToGetOnly(_ => random.Next(0, 3), true);
            RandomNumber = timer.Select(_ => random.Next(0, 3)).ToGetOnly(0, true);
            _Count = ObservableProperty.CreateSettable(0);
            Count = _Count.ToGetOnlyMask();
            RandomNumber.Subscribe(_ => _Count.Value++);
        }
    }
}
