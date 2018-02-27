using System;
using System.Diagnostics;

namespace JCClasses
{
    public class SolverWithStateMachine: SolverBase
    {
        private IMath Math = new CachedMath();

        protected override bool Solvered(Byte[] row, Byte[] data)
        {
            Byte FreeCellSize = CalcFreeCellSize((Byte)row.Length, data);
            return Attempt(row, data);
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
        private StateMachine<Int32, byte> CreateStateMachine(Byte[] data)
        {
            StateMachine<Int32, byte> ret = new StateMachine<Int32, byte>();
            int currentState = 0;
            ret.Add(currentState, 100, currentState);
            ret.Add(currentState, 1, currentState + 1);
            currentState++;

            for (byte dataIndex = 0; dataIndex < data.Length-1; dataIndex++)
            {
                byte current = data[dataIndex];
                for (byte cell = 0; cell < current; cell++)
                {
                    ret.Add(currentState, cell == current-1 ? (byte)100 : (byte)1, currentState + 1);
                    currentState++;
                }
                ret.Add(currentState, 100, currentState);
                ret.Add(currentState, 1, currentState + 1);
                currentState++;
            }

            byte last = data[data.Length - 1];
            for (byte cell = 1; cell < last; cell++)
            {
                ret.Add(currentState, 1, currentState + 1);
                currentState++;
            }
            ret.Add(currentState, 100, currentState);

            ret.CurrentState = 0;
            ret.EndState = currentState;
            return ret;
        }

        private bool Attempt(Byte[] row, Byte[] data)
        {
            bool ret = false;
            for (int i = 0; i < row.Length; i++)
            {
                if (0 != row[i])
                {
                    continue;
                }

                row[i] = 1;
                bool isBlock = IsFill(row, data);
                row[i] = 100;
                bool isBan = IsFill( row, data);
                row[i] = 0;

                if (!(isBan ^ isBlock))
                {
                    continue;
                }

                if (isBlock)
                {
                    ret = true;
                    row[i] = 1;
                }
                if (isBan)
                {
                    ret = true;
                    row[i] = 100;
                }
            }
            return ret;
        }

        private bool IsFill(Byte[] row, Byte[] data)
        {
            for (int i = 0; i < row.Length; i++)
            {
                if (0 != row[i])
                {
                    continue;
                }

                row[i] = 1;
                bool isBlock = IsFill(row, data);
                row[i] = 100;
                bool isBan = IsFill(row, data);
                row[i] = 0;
                return isBan || isBlock;
            }
            return Check(data, row);
        }
        private bool Check(Byte[] data, Byte[] row)
        {
            StateMachine<Int32, byte> res = CreateStateMachine(data);
            try
            {
                foreach (var current in row)
                {
                    res.Next(current);
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return res.EndState == res.CurrentState;
        }
    }
}
