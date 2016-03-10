using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    public class Hierarchy
    {
        public int N;
        public List<List<int>> Dist;
        public List<int> Header;
        public List<Point> Points; 

        private int n;
        private List<List<int>> dist;
        private List<int> header;
        private List<Point> points;

        private readonly List<string> names;
        private readonly Chart chart;
        public List<int> Numbers;

        public Hierarchy(int n) : this(n, null, null) { }

        public Hierarchy(int n, DataGridView dataGridView, Chart curchart)
        {
            N = n;

            if(dataGridView == null)
                Dist = AutoFilling(n, 10);
            else
                Dist = Filling(dataGridView, curchart, n);

            chart = curchart;
            names = new List<string>();
        }

        private List<List<int>> Filling(DataGridView dataGridViev, Control chart, int n)
        {
            var result = new List<List<int>>();

            for (var i = 0; i < n; i++)
                result.Add(new List<int>());

            Header = new List<int>();
            for (var i = 0; i < n; i++)
                Header.Add(i);

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < i; j++)
                    result[i].Add(result[j][i]);
                result[i].Add(0);

                for (var j = i + 1; j < n; j++)
                {
                    var val = Convert.ToInt32(dataGridViev[j,i].Value.ToString());
                    result[i].Add(val);
                }
            }

            Points = new List<Point>();
            var koef = new[] {chart.Width/dataGridViev.ColumnCount, chart.Height/dataGridViev.RowCount};
            for (var i = 0; i < N; i++)
                Points.Add(new Point((i + 2) * koef[0], 1));
            return result;
        }

        private List<List<int>> AutoFilling(int n, int maxValue)
        {
            var result = new List<List<int>>();
            for (var i = 0; i < n; i++)
                result.Add(new List<int>());

            var r = new Random();

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < i; j++)
                    result[i].Add(result[j][i]);

                result[i].Add(0);
                for (var j = i + 1; j < n; j++)
                {
                    var val = r.Next(maxValue) + 2;
                    result[i].Add(val);
                }
            }

            return result;
        }

        public void BuildHierarchies(bool isMin)
        {
            header = new List<int>(Header);
            points = new List<Point>(Points);
            dist = new List<List<int>>();
            Numbers = new List<int>();

            foreach (var elem in Dist)
                dist.Add(new List<int>(elem));

            n = N;

            var count = 1;
            var numb = n;
            while (n > 1)
            {
                int[] indMin;
                int y;
                int[] indObj;

                if (isMin)
                    indMin = Min(out indObj, out y);
                else
                    indMin = Max(out indObj, out y);

                Draw(indObj, ref count);

                header.Remove(indObj[0]);
                header.Remove(indObj[1]);
                header.Add(numb++);

                dist.RemoveAt(indMin[1]);
                dist.RemoveAt(indMin[0]);

                int[] distToHier;
                if (isMin)
                    SearchMinDist(indMin, out distToHier);
                else
                    SearchMaxDist(indMin, out distToHier);

                foreach (var elem in dist)
                {
                    elem.RemoveAt(indMin[1]);
                    elem.RemoveAt(indMin[0]);
                }

                var ind = 0;
                foreach (var elem in dist)
                    elem.Add(distToHier[ind++]);

                dist.Add(new List<int>());
                for (var i = 0; i < n - 2; i++)
                    dist[n - 2].Add(dist[i][n - 2]);

                dist[n - 2].Add(0);

                n--;
            }
        }

        public int[] Min(out int[] indObj, out int valMin)
        {
            indObj = new int[2];

            valMin = dist[0][1];
            var indMin = new[] { 0, 1 };
            for (var i = 0; i < n; i++)
            {
                for (var j = i + 1; j < n; j++)
                {
                    if (dist[i][j] < valMin)
                    {
                        valMin = dist[i][j];
                        indMin[0] = i;
                        indMin[1] = j;
                    }
                }
            }

            if (indMin[0] > indMin[1])
            {
                var temp = indMin[0];
                indMin[0] = indMin[1];
                indMin[1] = temp;
            }

            indObj[0] = header[indMin[0]];
            indObj[1] = header[indMin[1]];

            return indMin;
        }

        private void SearchMinDist(IList<int> indMin, out int[] distToHier)
        {
            distToHier = new int[n - 2];
            var ind = 0;
            foreach (var elem in dist)
            {
                distToHier[ind] = 200;
                for (var j = 0; j < n; j++)
                {
                    if (((j == indMin[0]) || (j == indMin[1])) && elem[j] < distToHier[ind])
                        distToHier[ind] = elem[j];
                }
                ind++;
            }
        }

        public int[] Max(out int[] indObj, out int valMax)
        {
            indObj = new int[2];

            valMax = dist[0][1];
            var indMax = new[] { 0, 1 };
            for (var i = 0; i < n; i++)
            {
                for (var j = i + 1; j < n; j++)
                {
                    if (dist[i][j] > valMax)
                    {
                        valMax = dist[i][j];
                        indMax[0] = i;
                        indMax[1] = j;
                    }
                }
            }

            if (indMax[0] > indMax[1])
            {
                var temp = indMax[0];
                indMax[0] = indMax[1];
                indMax[1] = temp;
            }

            indObj[0] = header[indMax[0]];
            indObj[1] = header[indMax[1]];

            return indMax;
        }

        private void SearchMaxDist(IList<int> indMax, out int[] distToHier)
        {
            distToHier = new int[n - 2];
            var ind = 0;
            foreach (var elem in dist)
            {
                distToHier[ind] = 0;
                for (var j = 0; j < n; j++)
                {
                    if (((j == indMax[0]) || (j == indMax[1])) && elem[j] > distToHier[ind])
                        distToHier[ind] = elem[j];
                }
                ind++;
            }
        }

        private void Draw(IList<int> indObj, ref int count)
        {
            var y = count + 2;
            string name1;
            string name2;
            Point points1;
            Point points2;

            if (indObj[0] > Points.Count - 1)
            {
                name1 = names[indObj[0] % names.Count];
                points1 = points[indObj[0]];
                count--;
            }
            else
            {
                name1 = (indObj[0] + 1).ToString(CultureInfo.InvariantCulture);
                points1 = points[count-1];
                Numbers.Add(indObj[0]);
            }

            if (indObj[1] > Points.Count - 1)
            {
                name2 = names[indObj[1] % names.Count];
                points2 = points[indObj[1]];
                count--;
            }
            else
            {
                name2 = (indObj[1] + 1).ToString(CultureInfo.InvariantCulture);
                points2 = points[count];
                Numbers.Add(indObj[1]);
            }

            var ie = string.Format("(x{0}) - (x{1})", name1, name2);
            names.Add(ie);
            points.Add(new Point(Convert.ToInt32((points1.X + points2.X) / 2), y));
            chart.Series.Add(ie);
            chart.Series[ie].ChartType = SeriesChartType.Line;
            chart.Series[ie].BorderWidth = 3;
            chart.Series[ie].Points.AddXY(points1.X, points1.Y);
            chart.Series[ie].Points.AddXY(points1.X, y);
            chart.Series[ie].Points.AddXY(points2.X, y);
            chart.Series[ie].Points.AddXY(points2.X, points2.Y);
            count+=2;
        }
    }
}
