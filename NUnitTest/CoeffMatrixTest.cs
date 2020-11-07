using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MSPeakArea.Process.PeakPicking.CWT;
using MSPeakArea.Algorithm;
using System.Linq;

namespace NUnitTest
{
    public class CoeffMatrixTest
    {
        [Test]
        public void test1()
        {
            int n = 7;
            List<double> t = new List<double>();
            for(int i = 0; i < n; i++)
            {
                t.Add(i);
            }

            List<double> coeff = new List<double>() { 1.0, 2.0, 5.0, 4.0, 5.0, 6.0, 7.0};
            SortedDictionary<double, List<double>> matrix
                = new SortedDictionary<double, List<double>>();
            matrix[1] = coeff;

            CoeffMatrix processor = new CoeffMatrix(t, matrix, 4);
            List<int> pos = processor.LocalMaxamIndexes(1);

            Console.WriteLine(pos.Count);
            foreach(int i in pos)
            {
                Console.WriteLine(i.ToString() + " " + coeff[i].ToString());
            }

            //Assert.AreEqual(5,coeff[pos[pos.Count-1]]);

        }

        string GetString<T>(List<T> values)
        {
            string temp = "";
            foreach(T i in values)
            {
                temp += i.ToString() + ",";
            }
            return temp;
        }

        [Test]
        public void test2()
        {
            double[] signal = new double[]
              { 1385,1458,1546,1359,1365,1434,1344,1485,1415,1327,1396,1363,1418,1405,1414,1481,
                  1324,1321,1260,1441,1409,1501,1440,1428,1456,1546,1598,1444,1389,
                  1395,1425,1318,1384,1489,1456,1424,1491,1476,1493,1470,1452,1409,1463,1406,1470,1506,1410 };
            List<double> t = new List<double>();
            for(int i = 0; i < signal.Length; i++)
            {
                t.Add(i);
            }

            SortedDictionary<double, List<double>> matrix =
                new SortedDictionary<double, List<double>>();
            for (int a = 2; a <= 5; a += 1)
            {
                double[] processed = CWT.Transform(signal, a);
                matrix[a] = processed.ToList();
            }

            CoeffMatrix coeffMatrix = new CoeffMatrix(t, matrix);
            List<RidgeLine> lines = coeffMatrix.FindRidgeLine();
            foreach(var item in lines)
            {
                Console.WriteLine(item.Pos);
                Console.WriteLine(item.Length);
                Console.WriteLine(GetString<int>(item.Index));
                Console.WriteLine(GetString<double>(item.Trace) + "\n");
            }

        }

    }
}
