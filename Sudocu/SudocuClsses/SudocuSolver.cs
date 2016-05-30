using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace SudocuClsses
{
    /**
     * Класс предназначен для автоматизации решения судоку
     */
    
    public class SudocuSolver
    {
        private CSudocu _SolvedSudocu = null;
        private bool[] _RowMap;
        private bool[] _ColumnMap;
        private CachedMath Math = new CachedMath();

        /**
         * Делегат вызывается при каждой новой итерации нахождения решения
         */
        public delegate void UpdateProgressEvent();
        public event UpdateProgressEvent ProgressEvent;

        /**
         * Решить судоку создает нить в которой собственно выполняется решение
         */
        public void DoSolve(CSudocu Sudocu)
        {
            Thread mainSolvethread = new Thread(_Solve);
            _SolvedSudocu = Sudocu;
            _RowMap = new bool[_SolvedSudocu.Size.Height];
            _ColumnMap = new bool[_SolvedSudocu.Size.Width];
            mainSolvethread.Start();
        }


        /**
         * Основной метод нахождения правильного решения 
         */
        private void _Solve(object sender)
        {
            try
            {
                long tick = System.DateTime.Now.ToBinary();
                int i = 0;
                while(!_SolveIter())
                {
                    i++;
                }

                tick = System.DateTime.Now.ToBinary() - tick;
                DateTime Time = new System.DateTime(tick);
 

                string smessage = "Кроссворд решен\n";
                smessage += "Затрачено времени: ";
                smessage += Time.ToString("HH:mm:ss:");
                smessage += Time.Millisecond.ToString();
                smessage += "\n";
                smessage += "Количество полных проходов по исходным данным";
                smessage += i.ToString();

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
        private bool _SolveIter()
        {
            bool isChanged = false;
            for (Byte i = 0; i < _SolvedSudocu.Size.Height; i++)
            {
                if (!_RowMap[i])
                {
                    isChanged |= _SolveRow(i);
                    _RowMap[i] = true;
                    ProgressEvent.BeginInvoke(null, null);
                }
            }

            for (Byte i = 0; i < _SolvedSudocu.Size.Width; i++)
            {
                if (!_ColumnMap[i])
                {
                    isChanged |= _SolveColumn(i);
                    ProgressEvent.BeginInvoke(null, null);
                    _ColumnMap[i] = true;
                }
            }

            return !isChanged; 
        }

        private bool _SolveRow(Byte Index)
        {
            Byte[] row = new Byte[_SolvedSudocu.Size.Width];

            for (Byte i = 0; i < _SolvedSudocu.Size.Width; i++)
            {
                row[i] = _SolvedSudocu.GetCell(i, Index);
            }

            bool isChange = _Solvered(row, _SolvedSudocu.Vertical[Index].list);

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

        private bool _SolveColumn(Byte Index)
        {
            Byte[] row = new Byte[_SolvedSudocu.Size.Height];

            for (Byte i = 0; i < _SolvedSudocu.Size.Height; i++)
            {
                row[i] = _SolvedSudocu.GetCell(Index, i);
            }

            bool isChange = _Solvered(row, _SolvedSudocu.Horizontal[Index].list);

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
        private bool _Solvered(Byte[] Row, Byte[] Data)
        {
            Byte FreeCellSize = _CalcFreeCellSize((Byte)Row.Length, Data);
            Int64 Var = Math.GetVar(Data.Length, FreeCellSize);
            Byte[] BlockRow = _CreateBlockedArray(Row.Length);
            Byte[] BanRow = _CreateBanArray(Row.Length);

            for (Int64 i = 1; i <= Var; i++)
            {
                Byte[] Positions = Math.CalcPositions((Byte)Data.Length, FreeCellSize, i);
                Byte[] IterRow = _CreateVariant(Positions,Data,(Byte)Row.Length);

                if(!_IsSuccess(Row, IterRow))
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

            bool isChannge = false;
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
                    isChannge = true;
                }
            }
            
            BlockRow.CopyTo(Row,0);
            return isChannge;
        }

        private Byte[] _CreateBlockedArray(Int64 Count)
        {
            Byte[] NewRow = new Byte[Count];

            for (Int64 i = 0; i < Count; i++)
            {
                NewRow[i] = 1;
            }
            return NewRow;
        }

        private Byte[] _CreateBanArray(Int64 Count)
        {
            Byte[] NewRow = new Byte[Count];

            for (Int64 i = 0; i < Count; i++)
            {
                NewRow[i] = 100;
            }
            return NewRow;
        }

        private Byte[] _CreateVariant(Byte[] Positions, Byte[] Data, Byte Length)
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
        
        private Byte _CalcFreeCellSize(Byte RowCount, Byte[] Data)
        {
            Byte FreeCellSize = (Byte)RowCount;
            FreeCellSize -= (Byte)(Data.Length - 1);
            for (Int32 i = 0; i < Data.Length; i++)
            {
                FreeCellSize -= Data[i];
            }
            return FreeCellSize;
        }

        private bool _IsSuccess(Byte[] Mask, Byte[] Value)
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
