using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KLibrary.Labs.Mathematics
{
    /// <summary>
    /// Represents a polynomial with a single variable.
    /// </summary>
    /// <remarks>
    /// The dictionary does not contain entries whose coefficient is 0.
    /// TODO: Check if each coefficient in Dictionary is not 0 (pass through when accessed internally).
    /// </remarks>
    public struct Polynomial
    {
        /// <summary>
        /// Represents the variable x.
        /// </summary>
        public static readonly Polynomial X = new Polynomial(new Dictionary<int, double> { { 1, 1 } });

        static readonly IDictionary<int, double> _coefficients_empty = new Dictionary<int, double>();

        IDictionary<int, double> _coefficients_org;
        IReadOnlyDictionary<int, double> _coefficients_readonly;

        IDictionary<int, double> CoefficientsOrg
        {
            get { return _coefficients_org == null ? _coefficients_empty : _coefficients_org; }
        }

        /// <summary>
        /// Gets the dictionary which represents index/coefficient pairs.
        /// </summary>
        /// <value>The dictionary which represents index/coefficient pairs.</value>
        public IReadOnlyDictionary<int, double> Coefficients
        {
            get
            {
                if (_coefficients_readonly == null) _coefficients_readonly = new ReadOnlyDictionary<int, double>(CoefficientsOrg);

                return _coefficients_readonly;
            }
        }

        /// <summary>
        /// Gets the degree of the polynomial.
        /// </summary>
        /// <value>The degree of the polynomial.</value>
        public int Degree
        {
            get { return CoefficientsOrg.Count == 0 ? 0 : CoefficientsOrg.Max(c => c.Key); }
        }

        /// <summary>
        /// Gets the coefficient for the specified index.
        /// </summary>
        /// <param name="index">The index of the variable.</param>
        /// <returns>The coefficient for the specified index.</returns>
        public double this[int index]
        {
            get { return CoefficientsOrg.ContainsKey(index) ? CoefficientsOrg[index] : 0; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polynomial"/> structure.
        /// </summary>
        /// <param name="coefficients">The dictionary which represents index/coefficient pairs.</param>
        public Polynomial(IDictionary<int, double> coefficients)
        {
            _coefficients_org = coefficients;
            _coefficients_readonly = null;
        }

        #region Operators

        public static implicit operator Polynomial(double value)
        {
            return value == 0 ? default(Polynomial) : new Polynomial(new Dictionary<int, double> { { 0, value } });
        }

        /// <summary>
        /// Adds two polynomials.
        /// </summary>
        /// <param name="p1">The first polynomial.</param>
        /// <param name="p2">The second polynomial.</param>
        /// <returns>The sum of two polynomials.</returns>
        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            var coefficients = new Dictionary<int, double>(p1.CoefficientsOrg);

            foreach (var item2 in p2.CoefficientsOrg)
            {
                AddMonomial(coefficients, item2.Key, item2.Value);
            }
            return new Polynomial(coefficients);
        }

        public static Polynomial operator +(Polynomial p, double value)
        {
            var coefficients = new Dictionary<int, double>(p.CoefficientsOrg);

            AddMonomial(coefficients, 0, value);

            return new Polynomial(coefficients);
        }

        public static Polynomial operator +(double value, Polynomial p)
        {
            return p + value;
        }

        /// <summary>
        /// Subtracts one specified polynomial from another.
        /// </summary>
        /// <param name="p1">The polynomial.</param>
        /// <param name="p2">The polynomial to subtract.</param>
        /// <returns>The difference of two polynomials.</returns>
        public static Polynomial operator -(Polynomial p1, Polynomial p2)
        {
            var coefficients = new Dictionary<int, double>(p1.CoefficientsOrg);

            foreach (var item2 in p2.CoefficientsOrg)
            {
                AddMonomial(coefficients, item2.Key, -item2.Value);
            }
            return new Polynomial(coefficients);
        }

        public static Polynomial operator -(Polynomial p, double value)
        {
            var coefficients = new Dictionary<int, double>(p.CoefficientsOrg);

            AddMonomial(coefficients, 0, -value);

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

        public static Polynomial operator *(Polynomial p, double value)
        {
            return value * p;
        }

        public static Polynomial operator *(double value, Polynomial p)
        {
            var coefficients = new Dictionary<int, double>();

            foreach (var item in p.CoefficientsOrg)
            {
                AddMonomial(coefficients, item.Key, item.Value * value);
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

        /// <summary>
        /// Returns the specified power of the specified polynomial.
        /// </summary>
        /// <param name="p">The polynomial.</param>
        /// <param name="power">The power.</param>
        /// <returns>The calculated value.</returns>
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

        #endregion

        #region Equivalence Operators

        /// <summary>
        /// Compares two polynomials for equality.
        /// </summary>
        /// <param name="p1">The first polynomial.</param>
        /// <param name="p2">The second polynomial.</param>
        /// <returns><see langword="true"/> if two polynomials are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Polynomial p1, Polynomial p2)
        {
            return Enumerable.SequenceEqual(p1.CoefficientsOrg, p2.CoefficientsOrg);
        }

        /// <summary>
        /// Compares two polynomials for inequality.
        /// </summary>
        /// <param name="p1">The first polynomial.</param>
        /// <param name="p2">The second polynomial.</param>
        /// <returns><see langword="true"/> if two polynomials are different; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Polynomial p1, Polynomial p2)
        {
            return !(p1 == p2);
        }

        /// <summary>
        /// Compares the specified object to this polynomial.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><see langword="true"/> if the object is a <see cref="Polynomial"/> and is equal to this polynomial; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Polynomial && this == (Polynomial)obj;
        }

        /// <summary>
        /// Returns the hash code for this polynomial.
        /// </summary>
        /// <returns>The hash code for this polynomial.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Returns the string which represents this polynomial.
        /// </summary>
        /// <returns>The string which represents this polynomial.</returns>
        public override string ToString()
        {
            if (CoefficientsOrg.Count == 0) return "0";

            var monomialsQuery = CoefficientsOrg
                .OrderByDescending(c => c.Key)
                .Select((c, i) => ToMonomialString(c.Key, c.Value, i == 0));
            return string.Join("", monomialsQuery);
        }

        static string ToMonomialString(int index, double coefficient, bool isDegree)
        {
            // Premise: coefficient is not 0.
            var c_abs = Math.Abs(coefficient);
            return string.Format("{0}{1}{2}",
                coefficient < 0 ? "-" : isDegree ? "" : "+",
                c_abs == 1 && index != 0 ? "" : c_abs.ToString(),
                index == 0 ? "" : index == 1 ? "x" : "x^" + index.ToString());
        }

        #endregion

        /// <summary>
        /// Substitutes the specified value into this polynomial.
        /// </summary>
        /// <param name="value">The value to substitute.</param>
        /// <returns>The calculated value.</returns>
        public double Substitute(double value)
        {
            return CoefficientsOrg.Sum(c => c.Value * Math.Pow(value, c.Key));
        }

        /// <summary>
        /// Solve the linear equation whose left operand is this polynomial and right operand is 0.
        /// </summary>
        /// <returns>The solution.</returns>
        public double SolveLinearEquation()
        {
            if (Degree != 1) throw new InvalidOperationException("The degree must be 1.");

            // ax + b = 0
            var a = this[1];
            var b = this[0];

            return -b / a;
        }

        /// <summary>
        /// Solve the quadratic equation whose left operand is this polynomial and right operand is 0.
        /// </summary>
        /// <returns>The solution.</returns>
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
