using System;
using System.Diagnostics;

namespace JCClasses
{
    public class SolverWithStateMachine: SolverBase
    {
        private IMath Math = new CachedMath();
        private Int64 max;
        private Int64 current;

        protected override bool Solvered(Byte[] row, Byte[] data)
        {
            Byte FreeCellSize = CalcFreeCellSize((Byte)row.Length, data);
            max = Math.GetVar(data.Length, FreeCellSize);
            current = 0;
            return Attempt(0, row, data);
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
            ret.Add(currentState, 1, currentState+1);
            currentState++;

            foreach (var current in data)
            {
                for (byte cell = 1; cell < current; cell++)
                {
                    ret.Add(currentState, cell == current ? (byte)100 : (byte)1, currentState+1);
                    currentState++;
                }
                ret.Add(currentState, 100, currentState);
                ret.Add(currentState, 1, currentState + 1);
                currentState++;
            }
            ret.CurrentState = 0;
            ret.EndState = currentState-1;
            return ret;
        }

        private bool Attempt(int index, Byte[] row, Byte[] data)
        {
            if(index == row.Length)
            {
                current++;
                Debug.WriteLine("Attempt: {0}/{1}", current, max);
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

            for (int i = index; i < row.Length; i++)
            {
                if (0 != row[i])
                {
                    continue;
                }
                row[i] = 1;
                bool isBlock = Attempt(i + 1, row, data);
                row[i] = 100;
                bool isBan = Attempt(i + 1, row, data);
                row[i] = 0;

                if (isBan && isBlock)
                {
                    continue;
                }

                if (!isBan && !isBlock)
                {
                    return false;
                }

                if(0 != index)
                {
                    continue;
                }

                if (isBlock)
                {
                    row[i] = 1;
                }
                if (isBan)
                {
                    row[i] = 100;
                }
            }
            return true;
        }

    }
}
