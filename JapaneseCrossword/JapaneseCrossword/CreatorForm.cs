using JCClasses;
using System;
using System.Data;
using System.Windows.Forms;

namespace JapaneseСrossword
{
    public partial class CreatorForm : Form
    {
        public CreatorForm()
        {
            InitializeComponent();
        }


        private void btnForm_Click(object sender, EventArgs e)
        {
            if (_isForm)
            {
                if (DialogResult.No == MessageBox.Show(
                    "Вы действительно хотите переформировать данные?",
                    "Предупреждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation))
                {
                    return;
                }
            }
            FormGrid();
            _isForm = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Сохраняем
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "(*.xml)|*.xml";
            if (DialogResult.OK == saveFileDialog.ShowDialog(this))
            {
                // Формируем объект для сохранения
                Crossword newSudocu = new Crossword();
                newSudocu.SetSize((byte)numWidth.Value, (byte)numHeigth.Value);

                Byte RowIndex = 0;
                foreach (DataGridViewRow row in dataHorizontal.Rows)
                {
                    for (Byte i = 1; i < row.Cells.Count; i++)
                    {
                        try
                        {
                            byte value = (byte)(Int32)row.Cells[i].Value;
                            newSudocu.Horizontal[RowIndex].Add(value);
                        }
                        catch
                        { }
                    }
                    RowIndex++;
                }

                RowIndex = 0;
                foreach (DataGridViewRow row in dataVertical.Rows)
                {
                    for (Byte i = 1; i < row.Cells.Count; i++)
                    {
                        try
                        {
                            byte value = (byte)(Int32)row.Cells[i].Value;
                            newSudocu.Vertical[RowIndex].Add(value);
                        }
                        catch
                        { }
                    }
                    RowIndex++;
                }
                newSudocu.Save(saveFileDialog.FileName);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // Сохраняем
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog .Filter = "(*.xml)|*.xml";
            if (DialogResult.OK == openFileDialog.ShowDialog(this))
            {
                Crossword newSudocu = Crossword.Load(openFileDialog.FileName);

                numHeigth.Value = newSudocu.Size.Height;
                numWidth.Value = newSudocu.Size.Width;
                numVertical.Value = newSudocu.Vertical.GetMaxCount();
                numHorizontal.Value = newSudocu.Horizontal.GetMaxCount();

                FormGrid();

                for (byte i = 0; i < newSudocu.Horizontal.Count; i++)
                {
                    byte[] bytelst = newSudocu.Horizontal[i].list;
                    for (byte j = 0; j < bytelst.Length; i++)
                    {
                        dataHorizontal.Rows[i].Cells[j+1].Value =
                            bytelst[j].ToString();
                    }
                }


                    _isForm = true;
            }
        }

        private void FormGrid()
        {
            DataTable dtH = new DataTable();
            dataHorizontal.DataSource = dtH;

            dtH.Columns.Add("№ Колонки");
            dtH.Columns[0].DataType = typeof(Int32);
            for (Int32 i = 1; i < numHorizontal.Value + 1; i++)
            {
                dtH.Columns.Add(i.ToString());
                dtH.Columns[i].DataType = typeof(Int32);

                dataHorizontal.Columns[i].Width = 50;
            }
            for (Int32 i = 1; i < numWidth.Value + 1; i++)
            {
                dtH.Rows.Add(i.ToString());
            }

            DataTable dtV = new DataTable();
            dataVertical.DataSource = dtV;

            dtV.Columns.Add("№ Строки");
            dtV.Columns[0].DataType = typeof(Int32);
            for (Int32 i = 1; i < numVertical.Value + 1; i++)
            {
                dtV.Columns.Add(i.ToString());
                dtV.Columns[i].DataType = typeof(Int32);

                dataVertical.Columns[i].Width = 50;
            }
            for (Int32 i = 1; i < numHeigth.Value + 1; i++)
            {
                dtV.Rows.Add(i.ToString());
            }
        }
        bool _isForm = false;
    }
}
