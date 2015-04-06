using System;
using System.Reactive.Linq;
using KLibrary.Labs.ObservableModel;

namespace BmiWpf2
{
    public class Body
    {
        public ISettableProperty<double> Height { get; private set; }
        public ISettableProperty<double> Weight { get; private set; }
        public IGetOnlyProperty<double> Bmi { get; private set; }

        public Body()
        {
            Height = ObservableProperty.CreateSettable(170.0);
            Weight = ObservableProperty.CreateSettable(70.0);
            Bmi = ObservableProperty.CreateGetOnly(() => Weight.Value / Math.Pow(Height.Value / 100, 2));

            Height.Merge(Weight)
                //.Throttle(TimeSpan.FromSeconds(0.1))
                .Subscribe(Bmi);
        }
    }
}
