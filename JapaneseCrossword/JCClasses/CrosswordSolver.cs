using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JCClasses
{
    /// <summary>
    /// Класс предназначен для автоматизации решения японского кроссворда
    /// </summary>
    public class CrosswordSolver: SolverBase
    {
        private IMath Math = new CachedMath();
        //
        // TODO Разбить на более мелкие функции
        // 
        protected override bool Solvered(Byte[] Row, Byte[] Data)
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
