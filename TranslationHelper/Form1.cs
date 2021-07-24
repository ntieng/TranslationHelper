using System;
using System.IO;
using System.Windows.Forms;

namespace TranslationHelper
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new OpenFileDialog();
            using (var opnDlg = new OpenFileDialog())
            {
                if (opnDlg.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = Path.GetDirectoryName(opnDlg.FileName);
                }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
                dataGridView1.ContextMenuStrip = contextMenuStrip1;
        }

        private void PasteClipboard()
        {
            try
            {
                dataGridView1.Rows.Add();
                string s = Clipboard.GetText();
                string[] lines = s.Replace("\n", "").Split('\r');
                string[] fields;
                int row = 0;
                int column = 0;
                foreach (string l in lines)
                {
                    fields = l.Split('\t');
                    foreach (string f in fields)
                    {
                        dataGridView1[column++, row].Value = f;
                    }
                    dataGridView1.Rows.Add();
                    row++;
                    column = 0;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            PasteClipboard();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void btnAddToFile_Click(object sender, EventArgs e)
        {
            //This line of code creates a text file for the data export.
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("No path directory selected");
            }
            else
            {
                try
                {
                    string sLine = "";

                    //This for loop loops through each row in the table
                    for (int r = 0; r <= dataGridView1.Rows.Count - 1; r++)
                    {
                        //This for loop loops through each column, and the row number
                        //is passed from the for loop above.
                        for (int c = 0; c <= dataGridView1.Columns.Count - 1; c++)
                        {
                            sLine = sLine + dataGridView1.Rows[r].Cells[c].Value;
                            if (c != dataGridView1.Columns.Count - 1)
                            {
                                //A comma is added as a text delimiter in order
                                //to separate each field in the text file.
                                //You can choose another character as a delimiter.
                                if (string.IsNullOrEmpty(sLine))
                                {
                                    continue;
                                }
                                sLine = sLine + ",";
                            }
                        }
                        //The exported text is written to the text file, one line at a time.
                        if (!string.IsNullOrEmpty(sLine))
                        {
                            File.AppendAllText(textBox1.Text + @"\\sample.txt", sLine + Environment.NewLine);
                        }
                        sLine = "";
                    }

                    MessageBox.Show("Export Complete.", "Program Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
