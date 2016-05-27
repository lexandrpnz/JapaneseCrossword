using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace SudocuClsses
{
    /**
     * Класс предназначен для автоматизации решения судоку
     */
    
    public class SudocuSolver
    {
        public SudocuSolver()
        {}

        ~SudocuSolver()
        { }

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
            _SolvedSudocu = Sudocu;
            BackgroundWorker bw = new BackgroundWorker();
            _RowMap = new bool[_SolvedSudocu.Size.Height];
            _ColumnMap = new bool[_SolvedSudocu.Size.Width];
            bw.DoWork += _Solve;
            bw.RunWorkerAsync(this);
        }


        /**
         * Основной метод нахождения правильного решения 
         */
        private static void _Solve(object sender,DoWorkEventArgs e)
        {
            try
            {
                long tick = System.DateTime.Now.ToBinary();
                int i = 0;
                SudocuSolver Solver = (SudocuSolver)e.Argument;
                while(!Solver._SolveIter())
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
            _IsSucces = true;
            for (Byte i = 0; i < _SolvedSudocu.Size.Height; i++)
            {
                if (!_RowMap[i])
                {
                    _SolveRow(i);
                    _RowMap[i] = true;
                    ProgressEvent.BeginInvoke(null, null);
                }
            }

            for (Byte i = 0; i < _SolvedSudocu.Size.Width; i++)
            {
                if(!_ColumnMap[i])
                {
                    _SolveColumn(i);
                    ProgressEvent.BeginInvoke(null, null);
                    _ColumnMap[i] = true;
                }
            }

            return _IsSucces; 
        }

        private void _SolveRow(Byte Index)
        {
            Byte[] row = new Byte[_SolvedSudocu.Size.Width];

            for (Byte i = 0; i < _SolvedSudocu.Size.Width; i++)
            {
                row[i] = _SolvedSudocu.GetCell(i, Index);
            }

            _Solvered(row, _SolvedSudocu.Vertical[Index].list);

            for (Byte i = 0; i < _SolvedSudocu.Size.Width; i++)
            {
                if (row[i] != _SolvedSudocu.GetCell(i, Index))
                {
                    _ColumnMap[i] = false;
                    _SolvedSudocu.SetCell(i, Index, row[i]);
                }
            }
        }

        private void _SolveColumn(Byte Index)
        {
            Byte[] row = new Byte[_SolvedSudocu.Size.Height];

            for (Byte i = 0; i < _SolvedSudocu.Size.Height; i++)
            {
                row[i] = _SolvedSudocu.GetCell(Index, i);
            }

            _Solvered(row, _SolvedSudocu.Horizontal[Index].list);

            for (Byte i = 0; i < _SolvedSudocu.Size.Height; i++)
            {
                if (row[i] != _SolvedSudocu.GetCell(Index, i))
                {
                    _RowMap[i] = false;
                    _SolvedSudocu.SetCell(Index, i, row[i]);
                }
            }
        }

        //
        // TODO Разбить на более мелкие функции
        // 
        private void _Solvered(Byte[] Row, Byte[] Data)
        {
            Byte FreeCellSize = _CalcFreeCellSize((Byte)Row.Length, Data);
            Int64 Var = _GetVar(Data.Length, FreeCellSize);
            Byte[] BlockRow = _CreateBlockedArray(Row.Length);
            Byte[] BanRow = _CreateBanArray(Row.Length);

            for (Int64 i = 1; i <= Var; i++)
            {
                Byte[] Positions = _CalcPositions((Byte)Data.Length, FreeCellSize, i);
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
                    _IsSucces = false;
                }
            }
            
            BlockRow.CopyTo(Row,0);
            return;
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
        
        private Byte[] _CalcPositions(Byte ObjCount, Byte FreeCellCount, Int64 Var)
        {
	        Byte[] Positions = new Byte[ObjCount];

            if (0 == Var)
            {
                return Positions;
            }
            if (Var > _GetVar(ObjCount,FreeCellCount))
	        {
		        return Positions;
	        }

            Int64 tmpVar = Var;
            Byte Summposition = 0;
	        for (Byte i = 1; i < ObjCount;i++)
	        {
		        do 
		        {
			        if (tmpVar > _GetVar(ObjCount - i,FreeCellCount - Summposition))
			        {
				        tmpVar -= _GetVar(ObjCount - i,FreeCellCount - Summposition);
				        Positions[i-1]++;
				        Summposition++;
			        }
			        else
			        {
				        break;
			        }
		        } while (true);
	        }

	        Positions[ObjCount-1] = (Byte)(tmpVar -1);
            return Positions;
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

        private Int64 _GetVar(Int32 ObjectCount, Int32 CellCount)
        {
            try
            {
                return _GetVarCahe[ObjectCount][CellCount];
            }
            catch
            {
                if (!_GetVarCahe.ContainsKey(ObjectCount))
                {
                    _GetVarCahe.Add(ObjectCount, new Dictionary<Int32, Int64>());
                }
                _GetVarCahe[ObjectCount][CellCount] = _FactorialEx(
                ObjectCount + CellCount, 
                ObjectCount > CellCount ? ObjectCount : CellCount)
                / _FactorialEx(
                ObjectCount < CellCount ? ObjectCount : CellCount, 
                1);
            }

            return _GetVarCahe[ObjectCount][CellCount];
        }

        private Int64 _FactorialEx(Int32 Begin,Int32 End)
        {
            if (Begin > End)
            {
                return Begin * _FactorialEx(Begin - 1, End);
            }
            return 1;
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

        private CSudocu _SolvedSudocu = null;
        private Boolean _IsSucces;
        private bool[] _RowMap;
        private bool[] _ColumnMap;
        private Dictionary<Int32, Dictionary<Int32, Int64>> _GetVarCahe = new Dictionary<Int32, Dictionary<Int32, Int64>>();
    }


}
