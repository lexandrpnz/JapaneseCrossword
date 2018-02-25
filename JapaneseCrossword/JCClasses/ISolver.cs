using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCClasses
{
    public delegate void UpdateProgressEvent();

    public interface ISolver
    {
        /// <summary>
        /// Делегат вызывается при каждой новой итерации нахождения решения
        /// </summary>
        event UpdateProgressEvent ProgressEvent;

        /// <summary>
        /// Решить кроссворд. Создает нить в которой  выполняется решение
        /// </summary>
        /// <param name="sudocu"></param>
        void DoSolve(Crossword sudocu);
    }
}
