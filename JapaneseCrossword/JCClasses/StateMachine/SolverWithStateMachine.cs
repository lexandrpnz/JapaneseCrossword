using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JCClasses
{
    public class SolverWithStateMachine: SolverBase
    {
        private IMath Math = new CachedMath();

        protected override bool Solvered(Byte[] row, Byte[] data)
        {
            return Attempt(row, data);
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
                bool isBlock = IsFill( row, data );
                row[i] = 100;
                bool isBan = IsFill( row, data );
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
            StateMachine<Int32, byte> stateMachine = CreateStateMachine(data);
            HashSet<Int32> stateCollection = new HashSet<Int32>();

            stateCollection.Add(stateMachine.CurrentState);

            foreach (var next in row)
            {
                stateCollection = Check(next, stateMachine, stateCollection);
                if(0 == stateCollection.Count)
                {
                    return false;
                }
            }
            return IsEnd(stateMachine, stateCollection);
        }

        private HashSet<Int32> Check(Byte next, StateMachine<Int32, byte> stateMachine, HashSet<Int32> stateCollection)
        {
            HashSet<Int32> resultStates = new HashSet<Int32>();
            foreach (Int32 currentstate in stateCollection)
            {
                stateMachine.CurrentState = currentstate;
                try
                {
                    if (0 == next)
                    {
                        ICollection<Int32> nextStates = stateMachine.GetNext();
                        resultStates.UnionWith(nextStates);
                    }
                    else
                    {
                        resultStates.Add(stateMachine.Next(next));
                    }
                }
                catch (System.Exception)
                {
                    continue;
                }
            }

            return resultStates;
        }

        private bool IsEnd(StateMachine<Int32, byte> stateMachine, HashSet<Int32> stateCollection)
        {
            foreach (Int32 currentstate in stateCollection)
            {
                stateMachine.CurrentState = currentstate;
                if (stateMachine.IsEnd())
                {
                    return true;
                }
            }
            return false;
        }

    }
}
