using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSPeakArea.Process.PeakPicking.CWT
{
    public class CoeffMatrix
    {
        private double window; // sliding window
        private List<double> t; // time series
        private SortedDictionary<double, List<double>> matrix; // 2-d matix (a, coeff)
        private List<RidgeLine> lines;
        private List<RidgeLine> finalLines;
        private double thresh; // gap number threshold
        private int minLength; // min ridgeline length

        public CoeffMatrix(List<double> t, SortedDictionary<double, List<double>> matrix,
            double window=-1, double thresh=-1, int minLength=7)
        {
            this.t = t;
            this.matrix = matrix;
            lines = new List<RidgeLine>();
            finalLines = new List<RidgeLine>();
            this.window = window;
            
            this.thresh = thresh;
            if (thresh < 0)
                this.thresh = matrix.Keys.Min();
            this.minLength = minLength;
        }

        public List<double> Coeffcient (double scale)
        {
            if (!matrix.ContainsKey(scale))
                return new List<double>();
            return matrix[scale];
        }       
        
        // find the max value within a window size
        public List<int> WindowMaxamIndexes(double scale)
        {
            HashSet<int> indexes = new HashSet<int>();
            double winSize = window;
            if (winSize <= 0)
                winSize = scale / 2.0;
        
            if (!matrix.ContainsKey(scale) || t.Count == 0)
                return indexes.ToList();

            List<double> coeff = matrix[scale];
            if (coeff.Count != t.Count)
                throw new Exception("the coefficient size is not equal to time size!");

            LinkedList<int> indexer = new LinkedList<int>();

            // init
            int idx = 0, maxIdx = 0;
            for(; idx < t.Count; idx++)
            {
                if (t[idx] - t[0] >= winSize)
                    break;

                while (indexer.Count > 0 && coeff[indexer.Last.Value] < coeff[idx])
                {
                    indexer.RemoveLast();
                }
                indexer.AddLast(idx);
                if (coeff[idx] > coeff[maxIdx])
                    maxIdx = idx;
            }
            indexes.Add(maxIdx);

            //sliding window
            for(; idx < t.Count; idx++)
            {
                while(indexer.Count > 0 && t[idx] - t[indexer.First.Value] > winSize)
                {
                    indexer.RemoveFirst();
                }

                while (indexer.Count > 0 && coeff[indexer.Last.Value] <= coeff[idx])
                {
                    indexer.RemoveLast();
                }
                indexer.AddLast(idx);
                indexes.Add(indexer.First.Value); 
            }

            return indexes.OrderBy(i => i).ToList();
        }
         
        public List<int> LocalMaxamIndexes(double scale)
        {
            List<int> indexes = new List<int>();
            List<int> maxIndexes = WindowMaxamIndexes(scale);
            List<double> coeff = matrix[scale];

            foreach (int index in maxIndexes)
            {
                if (index > 0 && coeff[index - 1] > coeff[index])
                    continue;
                else if (index < coeff.Count - 1 && coeff[index + 1] > coeff[index])
                    continue;
                indexes.Add(index);
            }
            return indexes;
        }

        private void IdentifyRidgeLines(double scale, int indx)
        {
            List<int> local = LocalMaxamIndexes(scale);

            // init ridge line
            if (lines.Count == 0)
            {
                foreach(int pos in local)
                {
                    lines.Add(new RidgeLine(pos, indx));
                }
                return;
            }

            // try extend ridge lines
            double winSize = window;
            if (winSize <= 0)
                winSize = scale / 2.0;

            List<RidgeLine> new_lines = new List<RidgeLine>();
            // Add 1 to gap, later will reset to zero if extend
            foreach(RidgeLine l in lines)
            {
                l.Gap += 1;
            }
            // try to extend the lines
            foreach (int p in local)
            {
                int i = BinarySearch.Search(
                    lines.Select(l => l.Pos).ToList(),
                    p, winSize);
                if (i == -1)
                    new_lines.Add(new RidgeLine(p, indx));
                else
                {
                    lines[i].Add(p, indx);
                }
            }
            // remove lines if they are over gapping
            foreach(RidgeLine l in lines)
            {
                if (l.Gap <= thresh)
                    new_lines.Add(l);
                else
                {
                    finalLines.Add(l);
                }
            }
            lines = new_lines;
        }

        //private bool TryExtend( List<double> localPos)
        //{   
        //    int idx = BinarySearch.Search(localPos, line.Pos, window);
        //    if (idx == -1) return false;

        //    line.Add(localPos[idx]);
        //    return true;
        //}

        //private List<RidgeLine> UniqueLines(List<RidgeLine> lines)
        //{

        //    if (lines.Count < 2) return lines;
        //    List<RidgeLine> unique = new List<RidgeLine>();
        //    RidgeLine curr = lines[0];
        //    for(int i = 1; i < lines.Count; i++)
        //    {
        //        if (lines[i].Pos == curr.Pos)
        //        {
        //            curr.Length = Math.Max(curr.Length, lines[i].Length);
        //            curr.Gap = Math.Min(curr.Gap, lines[i].Gap);
        //        }
        //        else
        //        {
        //            unique.Add(curr);
        //            curr = lines[i];
        //        }
        //    }
        //    unique.Add(curr);

        //    return unique;
        //}



        public List<RidgeLine> FindRidgeLine()
        {
            int indx = matrix.Count-1;
            lines.Clear();
            finalLines.Clear();
            foreach (var item in matrix.Reverse())
            {
                double scale = item.Key;
                IdentifyRidgeLines(scale, indx--);
            }
            finalLines.AddRange(lines);
            finalLines.Sort();
            return finalLines;
        }



    }
}
