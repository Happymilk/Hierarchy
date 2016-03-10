using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private Hierarchy hierarchy;

        public Form1()
        {
            InitializeComponent();
        }

        private void Random(Hierarchy h)
        {
            for (var i = 0; i < h.N; i++)
            {
                dataGridView1.Columns.Add("Column" + i, "");
                dataGridView1.Rows.Add();
            }

            for (var i = 0; i < h.N; i++)
            {
                for (var j = 0; j < h.N; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = Convert.ToString(h.Dist[i][j]);
                }
            }
        }
        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            dataGridView1.Columns[dataGridView1.ColumnCount - 1].Width = 25;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = (int)numericUpDown1.Value;
            dataGridView1.RowCount = (int)numericUpDown1.Value;
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                dataGridView1[i, i].Value = 0;
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                for (int j = 0; j < dataGridView1.RowCount; j++)
                    dataGridView1[i, j].Value = dataGridView1[j, i].Value;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = (int)numericUpDown1.Value;
            dataGridView1.RowCount = (int)numericUpDown1.Value;
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                dataGridView1[i, i].Value = 0;
            dataGridView1[0,1].Value = 10; 
            dataGridView1[0,2].Value = 1; 
            dataGridView1[0,3].Value = 4;
            dataGridView1[1,0].Value = 10;
            dataGridView1[1,2].Value = 3;
            dataGridView1[1,3].Value = 2;
            dataGridView1[2,0].Value = 1;
            dataGridView1[2,1].Value = 3;
            dataGridView1[2,3].Value = 5;
            dataGridView1[3,0].Value = 4;
            dataGridView1[3,1].Value = 2;
            dataGridView1[3,2].Value = 5;
        } 

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            hierarchy = new Hierarchy((int)numericUpDown1.Value);
            Random(hierarchy);
            hierarchy = new Hierarchy((int)numericUpDown1.Value, dataGridView1, chart1);
            hierarchy.BuildHierarchies(radioButton1.Checked);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            hierarchy = new Hierarchy((int)numericUpDown1.Value, dataGridView1, chart1);
            hierarchy.BuildHierarchies(radioButton1.Checked);
        }       
    }
}
