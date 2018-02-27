using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JCClasses
{
    public abstract class SolverBase: ISolver
    {
        private Crossword solvedSudocu { get; set; }
        private bool[] RowChanged { get; set; }
        private bool[] ColumnChanged { get; set; }
        private int threadCount = 1;

        protected abstract bool Solvered(Byte[] Row, Byte[] Data);

        public void DoSolve(Crossword Sudocu)
        {
            Thread mainSolvethread = new Thread(Solve);
            solvedSudocu = Sudocu;
            mainSolvethread.Start();
        }

        /// <summary>
        /// Основной метод нахождения правильного решения
        /// </summary>
        /// <param name="sender"></param>
        private void Solve(object sender)
        {
            try
            {
                RowChanged = new bool[solvedSudocu.Size.Height];
                ColumnChanged = new bool[solvedSudocu.Size.Width];
                long tick = System.DateTime.Now.ToBinary();
                int i = 0;
                while (!SolveIter())
                {
                    i++;
                }

                tick = System.DateTime.Now.ToBinary() - tick;
                DateTime time = new System.DateTime(tick);

                string smessage =
                    string.Format("Кроссворд решен\nЗатрачено времени: {0}\nКоличество полных проходов по исходным данным: {1}",
                    time.ToString("HH:mm:ss"),
                    i.ToString());

                MessageBox.Show(
                                 smessage,
                                 "Информация",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Выполняем один проход по вертикали и горизонтали
        /// </summary>
        /// <returns></returns>
        private bool SolveIter()
        {
            bool isChanged = false;
            Task[] rowThreads = new Task[threadCount];
            object lockObj = new object();
            for (Byte i = 0; i < rowThreads.Length; i++)
            {
                rowThreads[i] = Task.Factory.StartNew((threadIndex) =>
                {
                    for (int j = (byte)threadIndex; j < solvedSudocu.Size.Height; j += rowThreads.Length)
                    {
                        if (!RowChanged[j])
                        {
                            bool res = SolveRow((byte)j);
                            lock (lockObj)
                            {
                                isChanged |= res;
                            }
                            RowChanged[j] = true;
                            ProgressEvent.BeginInvoke(null, null);
                        }
                    }
                }, i);
            }

            Task.WaitAll(rowThreads);
            Task[] collThreads = new Task[threadCount];
            for (Byte i = 0; i < collThreads.Length; i++)
            {
                collThreads[i] = Task.Factory.StartNew((threadIndex) =>
                {
                    int count = System.Math.Min(((byte)threadIndex + 1) * 10, solvedSudocu.Size.Width);
                    for (int j = (byte)threadIndex; j < solvedSudocu.Size.Width; j += collThreads.Length)
                    {
                        if (!ColumnChanged[j])
                        {
                            bool res = SolveColumn((byte)j);
                            lock (lockObj)
                            {
                                isChanged |= res;
                            }
                            ColumnChanged[j] = true;
                            ProgressEvent.BeginInvoke(null, null);
                        }
                    }
                }, i);
            }
            Task.WaitAll(collThreads);
            return !isChanged;
        }

        private bool SolveRow(Byte Index)
        {
            Byte[] row = new Byte[solvedSudocu.Size.Width];

            for (Byte i = 0; i < solvedSudocu.Size.Width; i++)
            {
                row[i] = solvedSudocu.GetCell(i, Index);
            }

            bool isChange = Solvered(row, solvedSudocu.Vertical[Index].list);

            for (Byte i = 0; i < solvedSudocu.Size.Width; i++)
            {
                if (row[i] != solvedSudocu.GetCell(i, Index))
                {
                    ColumnChanged[i] = false;
                    solvedSudocu.SetCell(i, Index, row[i]);
                }
            }
            return isChange;
        }

        private bool SolveColumn(Byte Index)
        {
            Byte[] row = new Byte[solvedSudocu.Size.Height];

            for (Byte i = 0; i < solvedSudocu.Size.Height; i++)
            {
                row[i] = solvedSudocu.GetCell(Index, i);
            }

            bool isChange = Solvered(row, solvedSudocu.Horizontal[Index].list);

            for (Byte i = 0; i < solvedSudocu.Size.Height; i++)
            {
                if (row[i] != solvedSudocu.GetCell(Index, i))
                {
                    RowChanged[i] = false;
                    solvedSudocu.SetCell(Index, i, row[i]);
                }
            }
            return isChange;
        }

        public event UpdateProgressEvent ProgressEvent;
    }
}
