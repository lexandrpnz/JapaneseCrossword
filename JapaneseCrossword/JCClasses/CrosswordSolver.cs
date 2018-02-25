using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JCClasses
{
    /**
     * Класс предназначен для автоматизации решения японского кроссворда
     */
    public class CrosswordSolver: ISolver
    {
        private Crossword _SolvedSudocu = null;
        private bool[] _RowMap;
        private bool[] _ColumnMap;
        private int threadCount = 5;
        private IMath Math = new CachedMath();

        public event UpdateProgressEvent ProgressEvent;


        public void DoSolve(Crossword Sudocu)
        {
            Thread mainSolvethread = new Thread(Solve);
            _SolvedSudocu = Sudocu;
            _RowMap = new bool[_SolvedSudocu.Size.Height];
            _ColumnMap = new bool[_SolvedSudocu.Size.Width];
            mainSolvethread.Start();
        }


        /**
         * Основной метод нахождения правильного решения 
         */
        private void Solve(object sender)
        {
            try
            {
                long tick = System.DateTime.Now.ToBinary();
                int i = 0;
                while(!SolveIter())
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

        /**
         * Выполняем один проход по вертикали и горизонтали
         */
        private bool SolveIter()
        {
            bool isChanged = false;
            Task[] rowThreads = new Task[threadCount];
            object lockObj = new object();
            for (Byte i = 0; i < rowThreads.Length; i++)
            {
                rowThreads[i] = Task.Factory.StartNew((threadIndex) =>
                {
                    for (int j = (byte)threadIndex; j < _SolvedSudocu.Size.Height; j += rowThreads.Length)
                    {
                        if (!_RowMap[j])
                        {
                            bool res = SolveRow((byte)j);
                            lock(lockObj)
                            {
                                isChanged |= res;
                            }
                            _RowMap[j] = true;
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
                    int count = System.Math.Min(((byte)threadIndex + 1) * 10, _SolvedSudocu.Size.Width);
                    for (int j = (byte)threadIndex; j < _SolvedSudocu.Size.Width; j += collThreads.Length)
                    {
                        if (!_ColumnMap[j])
                        {
                            bool res = SolveColumn((byte)j);
                            lock (lockObj)
                            {
                                isChanged |= res;
                            }
                            _ColumnMap[j] = true;
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
            Byte[] row = new Byte[_SolvedSudocu.Size.Width];

            for (Byte i = 0; i < _SolvedSudocu.Size.Width; i++)
            {
                row[i] = _SolvedSudocu.GetCell(i, Index);
            }

            bool isChange = Solvered(row, _SolvedSudocu.Vertical[Index].list);

            for (Byte i = 0; i < _SolvedSudocu.Size.Width; i++)
            {
                if (row[i] != _SolvedSudocu.GetCell(i, Index))
                {
                    _ColumnMap[i] = false;
                    _SolvedSudocu.SetCell(i, Index, row[i]);
                }
            }
            return isChange;
        }

        private bool SolveColumn(Byte Index)
        {
            Byte[] row = new Byte[_SolvedSudocu.Size.Height];

            for (Byte i = 0; i < _SolvedSudocu.Size.Height; i++)
            {
                row[i] = _SolvedSudocu.GetCell(Index, i);
            }

            bool isChange = Solvered(row, _SolvedSudocu.Horizontal[Index].list);

            for (Byte i = 0; i < _SolvedSudocu.Size.Height; i++)
            {
                if (row[i] != _SolvedSudocu.GetCell(Index, i))
                {
                    _RowMap[i] = false;
                    _SolvedSudocu.SetCell(Index, i, row[i]);
                }
            }
            return isChange;
        }

        //
        // TODO Разбить на более мелкие функции
        // 
        private bool Solvered(Byte[] Row, Byte[] Data)
        {
            Byte FreeCellSize = CalcFreeCellSize((Byte)Row.Length, Data);
            Int64 Var = Math.GetVar(Data.Length, FreeCellSize);
            Byte[] BlockRow = CreateBlockedArray(Row.Length);
            Byte[] BanRow = CreateBanArray(Row.Length);

            for (Int64 i = 1; i <= Var; i++)
            {
                Byte[] Positions = Math.CalcPositions((Byte)Data.Length, FreeCellSize, i);
                Byte[] IterRow = CreateVariant(Positions,Data,(Byte)Row.Length);

                if(!IsSuccess(Row, IterRow))
                {
                    continue;
                }

                for(Int32 j = 0;j< Row.Length; j ++)
                {
                    if (0 == IterRow[j])
                    {
                        BlockRow[j] = IterRow[j];
                    }else if(1 == IterRow[j])
                    {
                        BanRow[j] = 0;
                    }
                }
            }

            bool isChange = false;
            for (Int32 j = 0; j < Row.Length; j++)
            {
                if (100 == BanRow[j])
                {
                    if (0 == BlockRow[j])
                    {
                        BlockRow[j] = BanRow[j];
                    }
                }

                if (BlockRow[j] != Row[j])
                {
                    isChange = true;
                }
            }
            
            BlockRow.CopyTo(Row,0);
            return isChange;
        }

        private Byte[] CreateBlockedArray(Int64 Count)
        {
            Byte[] NewRow = new Byte[Count];

            for (Int64 i = 0; i < Count; i++)
            {
                NewRow[i] = 1;
            }
            return NewRow;
        }

        private Byte[] CreateBanArray(Int64 Count)
        {
            Byte[] NewRow = new Byte[Count];

            for (Int64 i = 0; i < Count; i++)
            {
                NewRow[i] = 100;
            }
            return NewRow;
        }

        private Byte[] CreateVariant(Byte[] Positions, Byte[] Data, Byte Length)
        {
            Byte[] Variant = new Byte[Length];
            Byte Position = 0;

            for (Int64 j = 0; j < Positions.Length; j++)
            {
                Byte k;
                Byte StartPosition = (Byte)(Position + Positions[j]);
                for (k = StartPosition; k < (Byte)(StartPosition + Data[j]); k++)
                {
                    Variant[k] = 1;
                }
                Position = (Byte)(k + 1);
            }
            return Variant;
        }
        
        private Byte CalcFreeCellSize(Byte RowCount, Byte[] Data)
        {
            Byte FreeCellSize = (Byte)RowCount;
            FreeCellSize -= (Byte)(Data.Length - 1);
            for (Int32 i = 0; i < Data.Length; i++)
            {
                FreeCellSize -= Data[i];
            }
            return FreeCellSize;
        }

        private bool IsSuccess(Byte[] Mask, Byte[] Value)
        {
            for (Int32 j = 0; j < Mask.Length; j++)
            {
                if (0 == Mask[j])
                {
                    continue;
                }

                if (100 == Mask[j])
                {
                    if (1 == Value[j])
                        return false;
                    else
                        continue;
                }

                if (Mask[j] != Value[j])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
