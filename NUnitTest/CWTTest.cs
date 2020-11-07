using MSPeakArea.Algorithm;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NUnitTest
{
    public class CWTTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void test1()
        {
            int n = 20;
            double[] t = new double[n];
            for (int i = 0; i < n; i++)
            {
                t[i] = i - (n - 1) / 2.0;
            }

            double[] y = MaxicanHat.Wavelet(t, 2.0);

            foreach(Complex v in y)
            {
                Console.WriteLine(v);
            }

            //Assert.AreEqual(y[2], 0.2173769, 0.0001);
        }

        [Test]
        public void test2()
        {
            double[] u = new double[] { -2, 1, 3 };
            double[] v = new double[] { 3, 4, 1, 5, 6};
            double[] w = CWT.Convolve(u, v);
            foreach(double i in w) 
            {
                Console.WriteLine(i);
            }
        }

        [Test]
        public void test3()
        {
            int n = 20;
            double[] t = new double[n];
            for(int i = 0; i < n;  i++)
            {
                t[i] = i - (n - 1) / 2.0;
            }

            double[] y = CWT.Transform(t, 1.0);

            foreach (double v in y)
            {
                Console.WriteLine(v); 
            }
        }

        [Test]
        public void test4()
        {
            double[] t = new double[]
            { 1385,1458,1546,1359,1365,1434,1344,1485,1415,1327,1396,1363,1418,1405,1414,1481,1324,1321,1260,1441,1409,1501,1440,1428,1456,1546,1598,1444,1389,1395,1425,1318,1384,1489,1456,1424,1491,1476,1493,1470,1452,1409,1463,1406,1470,1506,1410 };
            //{
            //      1385,1458,1546,1359,1365,1434,1344,1485,1415,1327,1396,1363,1418,1405,1414,1481,1324,1321,1260,1441,1409,1501,1440,1428,1456,1546,1598,1444,1389,1395,1425,1318,1384,1489,1456,1424,1491,1476,1493,1470,1452,1409,1463,1406,1470,1506,1410,1452,1424,1418,1306,1335,1299,1391,1423,1609,1449,1385,1417,1400,1457,1504,1395,1628,1481,1430,1302,1468,1411,1423,1421,1424,1411,1454,1492,1514,1531,1340,1388,1360,1412,1531,1379,1397,1392,1475,1450,1484,1448,1513,1321,1408,1558,1623,1537,1515,1562,1434,1470,1470,1473
            //};

            double[] y = CWT.Transform(t, 2.0);

            foreach (double v in y)
            {
                Console.WriteLine(v);
            }


        }

    }
}
