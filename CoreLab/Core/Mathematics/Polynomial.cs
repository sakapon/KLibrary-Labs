using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KLibrary.Labs.Mathematics
{
    /// <summary>
    /// Represents the polynomials with a single variable.
    /// </summary>
    public struct Polynomial
    {
        /// <summary>
        /// Represents the variable x.
        /// </summary>
        public static readonly Polynomial X = new Polynomial(new Dictionary<int, double> { { 1, 1 } });

        static readonly IDictionary<int, double> _coefficients_empty = new Dictionary<int, double>();

        IDictionary<int, double> _coefficients_org;
        ReadOnlyDictionary<int, double> _coefficients_readonly;

        IDictionary<int, double> CoefficientsOrg
        {
            get { return _coefficients_org == null ? _coefficients_empty : _coefficients_org; }
        }

        public ReadOnlyDictionary<int, double> Coefficients
        {
            get
            {
                if (_coefficients_readonly == null) _coefficients_readonly = new ReadOnlyDictionary<int, double>(CoefficientsOrg);

                return _coefficients_readonly;
            }
        }

        public int Degree
        {
            get { return CoefficientsOrg.Count == 0 ? 0 : CoefficientsOrg.Max(c => c.Key); }
        }

        public double this[int index]
        {
            get { return CoefficientsOrg.ContainsKey(index) ? CoefficientsOrg[index] : 0; }
        }

        // The dictionary represents index/coefficient pairs.
        // The dictionary does not contain entries whose coefficient is 0.
        public Polynomial(IDictionary<int, double> coefficients)
        {
            _coefficients_org = coefficients;
            _coefficients_readonly = null;
        }

        public static implicit operator Polynomial(double value)
        {
            return value == 0 ? default(Polynomial) : new Polynomial(new Dictionary<int, double> { { 0, value } });
        }

        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            var coefficients = new Dictionary<int, double>(p1.CoefficientsOrg);

            foreach (var item2 in p2.CoefficientsOrg)
            {
                AddMonomial(coefficients, item2.Key, item2.Value);
            }
            return new Polynomial(coefficients);
        }

        public static Polynomial operator -(Polynomial p1, Polynomial p2)
        {
            var coefficients = new Dictionary<int, double>(p1.CoefficientsOrg);

            foreach (var item2 in p2.CoefficientsOrg)
            {
                AddMonomial(coefficients, item2.Key, -item2.Value);
            }
            return new Polynomial(coefficients);
        }

        public static Polynomial operator *(Polynomial p1, Polynomial p2)
        {
            var coefficients = new Dictionary<int, double>();

            foreach (var item1 in p1.CoefficientsOrg)
            {
                foreach (var item2 in p2.CoefficientsOrg)
                {
                    AddMonomial(coefficients, item1.Key + item2.Key, item1.Value * item2.Value);
                }
            }
            return new Polynomial(coefficients);
        }

        public static Polynomial operator /(Polynomial p, double value)
        {
            var coefficients = new Dictionary<int, double>();

            foreach (var item in p.CoefficientsOrg)
            {
                AddMonomial(coefficients, item.Key, item.Value / value);
            }
            return new Polynomial(coefficients);
        }

        // Power
        public static Polynomial operator ^(Polynomial p, int power)
        {
            if (power < 0) throw new ArgumentOutOfRangeException("power", "The value must be non-negative.");

            Polynomial result = 1;

            for (var i = 0; i < power; i++)
            {
                result *= p;
            }
            return result;
        }

        public static Polynomial operator +(Polynomial p)
        {
            return p;
        }

        public static Polynomial operator -(Polynomial p)
        {
            return -1 * p;
        }

        static void AddMonomial(Dictionary<int, double> coefficients, int index, double coefficient)
        {
            if (coefficients.ContainsKey(index))
            {
                coefficient += coefficients[index];
            }

            if (coefficient != 0)
            {
                coefficients[index] = coefficient;
            }
            else
            {
                coefficients.Remove(index);
            }
        }

        public double Substitute(double value)
        {
            return CoefficientsOrg.Sum(c => c.Value * Math.Pow(value, c.Key));
        }

        // Solve the equation whose right operand is 0. 
        public double SolveLinearEquation()
        {
            if (Degree != 1) throw new InvalidOperationException("The degree must be 1.");

            // ax + b = 0
            var a = this[1];
            var b = this[0];

            return -b / a;
        }

        // Solve the equation whose right operand is 0. 
        public double[] SolveQuadraticEquation()
        {
            if (Degree != 2) throw new InvalidOperationException("The degree must be 2.");

            // ax^2 + bx + c = 0
            var a = this[2];
            var b = this[1];
            var c = this[0];
            var d = b * b - 4 * a * c;

            return d > 0 ? new[] { (-b - Math.Sqrt(d)) / (2 * a), (-b + Math.Sqrt(d)) / (2 * a) }
                : d == 0 ? new[] { -b / (2 * a) }
                : new double[0];
        }
    }
}
