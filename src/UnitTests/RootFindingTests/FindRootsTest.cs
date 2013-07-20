﻿// <copyright file="FindRootsTest.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
// 
// Copyright (c) 2009-2013 Math.NET
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using MathNet.Numerics.RootFinding;
using NUnit.Framework;

namespace MathNet.Numerics.UnitTests.RootFindingTests
{
    [TestFixture]
    internal class FindRootsTest
    {
        [Test]
        public void MultipleRoots()
        {
            // Roots at -2, 2
            Func<double, double> f1 = x => x * x - 4;
            Assert.AreEqual(0, f1(FindRoots.OfFunction(f1, 0, 5, 1e-14)));
            Assert.AreEqual(-2, FindRoots.OfFunction(f1, -5, -1, 1e-14));
            Assert.AreEqual(2, FindRoots.OfFunction(f1, 1, 4, 1e-14));
            Assert.AreEqual(0, f1(FindRoots.OfFunction(x => -f1(x), 0, 5, 1e-14)));
            Assert.AreEqual(-2, FindRoots.OfFunction(x => -f1(x), -5, -1, 1e-14));
            Assert.AreEqual(2, FindRoots.OfFunction(x => -f1(x), 1, 4, 1e-14));

            // Roots at 3, 4
            Func<double, double> f2 = x => (x - 3) * (x - 4);
            Assert.AreEqual(0, f2(FindRoots.OfFunction(f2, 3.5, 5, 1e-14)), 1e-14);
            Assert.AreEqual(3, FindRoots.OfFunction(f2, -5, 3.5, 1e-14));
            Assert.AreEqual(4, FindRoots.OfFunction(f2, 3.2, 5, 1e-14));
            Assert.AreEqual(3, FindRoots.OfFunction(f2, 2.1, 3.9, 0.001), 0.001);
            Assert.AreEqual(3, FindRoots.OfFunction(f2, 2.1, 3.4, 0.001), 0.001);
        }

        [Test]
        public void LocalMinima()
        {
            Func<double, double> f1 = x => x * x * x - 2 * x + 2;
            Assert.AreEqual(0, f1(FindRoots.OfFunction(f1, -5, 5, 1e-14)), 1e-14);
            Assert.AreEqual(0, f1(FindRoots.OfFunction(f1, -2, 4, 1e-14)), 1e-14);
        }

        [Test]
        public void NoRoot()
        {
            Func<double, double> f1 = x => x * x + 4;
            Assert.Throws<NonConvergenceException>(() => FindRoots.OfFunction(f1, -5, 5, 1e-14));
        }

        [Test]
        public void Oneeq3()
        {
            // Test case from http://www.polymath-software.com/library/nle/Oneeq3.htm
            Func<double, double> f1 = T => Math.Exp(21000 / T) / (T * T) - 1.11e11;
            double x = FindRoots.OfFunction(f1, 550, 560, 1e-2);
            Assert.AreEqual(551.773822885233, x, 1e-5);
            Assert.AreEqual(0, f1(x), 1e-2);
        }

        [Test]
        public void Oneeq5()
        {
            // Test case from http://www.polymath-software.com/library/nle/Oneeq5.htm
            Func<double, double> f1 = TR =>
            {
                const double ALPHA = 7.256;
                const double BETA = 2.298E-3;
                const double GAMMA = 0.283E-6;
                const double DH = -57798.0;
                return DH + TR * (ALPHA + TR * (BETA / 2 + TR * GAMMA / 3)) - 298.0 * (ALPHA + 298.0 * (BETA / 2 + 298.0 * GAMMA / 3));
            };

            double x = FindRoots.OfFunction(f1, 3000, 5000, 1e-10);
            Assert.AreEqual(4305.30991366774, x, 1e-5);
            Assert.AreEqual(0, f1(x), 1e-10);
        }

        [Test]
        public void Oneeq6a()
        {
            // Test case from http://www.polymath-software.com/library/nle/Oneeq6a.htm
            Func<double, double> f1 = V =>
            {
                const double R = 0.08205;
                const double T = 273.0;
                const double B0 = 0.05587;
                const double A0 = 2.2769;
                const double C = 128300.0;
                const double A = 0.01855;
                const double B = -0.01587;
                const double P = 100.0;
                const double Beta = R * T * B0 - A0 - R * C / (T * T);
                const double Gama = -R * T * B0 * B + A0 * A - R * C * B0 / (T * T);
                const double Delta = R * B0 * B * C / (T * T);

                return R * T / V + Beta / (V * V) + Gama / (V * V * V) + Delta / (V * V * V * V) - P;
            };

            double x = FindRoots.OfFunction(f1, 0.1, 1);
            Assert.AreEqual(0.174749531708621, x, 1e-5);
            Assert.AreEqual(0, f1(x), 1e-13);
        }    
 
        [Test]
        public void Oneeq7()
        {
            // Test case from http://www.polymath-software.com/library/nle/Oneeq7.htm
            Func<double, double> f1 = x => x / (1 - x) - 5 * Math.Log(0.4 * (1 - x) / (0.4 - 0.5 * x)) + 4.45977;
            double r = FindRoots.OfFunction(f1, 0, 0.79, 1e-2);
            Assert.AreEqual(0.757396293891, r, 1e-5);
            Assert.AreEqual(0, f1(r), 1e-13);
        }

        [Test]
        public void Oneeq8()
        {
            // Test case from http://www.polymath-software.com/library/nle/Oneeq8.htm
            Func<double, double> f1 = v =>
            {
                const double a = 240;
                const double b = 40;
                const double c = 200;

                return a * v * v + b * Math.Pow(v, 7 / 4) - c;
            };

            double x = FindRoots.OfFunction(f1, 0.01, 1);
            Assert.AreEqual(0.842524411168525, x, 1e-2);
            Assert.AreEqual(0, f1(x), 1e-13);
        }
    }
}