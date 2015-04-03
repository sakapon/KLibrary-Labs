using KLibrary.ComponentModel;
using System;
using System.ComponentModel;

namespace BmiWpf
{
    public class Body : NotifyBase
    {
        [DefaultValue(170.0)]
        public double Height
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        [DefaultValue(70.0)]
        public double Weight
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        [DependentOn("Height")]
        [DependentOn("Weight")]
        public double Bmi
        {
            get { return Weight / Math.Pow(Height / 100, 2); }
        }
    }
}
