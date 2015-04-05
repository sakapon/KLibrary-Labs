using System;
using KLibrary.Labs.ObservableModel;

namespace BmiWpf2
{
    public class MainViewModel
    {
        public Body Body { get; private set; }

        public IGetOnlyProperty<double> BmiRectangleWidth { get; private set; }
        public IGetOnlyProperty<string> BmiRectangleFill { get; private set; }

        public MainViewModel()
        {
            Body = new Body();

            BmiRectangleWidth = Body.Bmi.SelectToGetOnly(i => 5 * i);
            BmiRectangleFill = Body.Bmi.SelectToGetOnly(i => i < 25 ? "#FF009900" : i < 40 ? "#FFDDDD00" : "#FFEE0000");
        }
    }
}
