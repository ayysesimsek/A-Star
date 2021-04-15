using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _163311055_astar.Classess
{
    #region Delegate tanımlamaları yapılmaktadır.

    /// <summary>
    /// StateChanged delegate -> Durum Değişimi
    /// </summary>
    /// <param name="currentState"></param>
    /// <param name="isFinal"></param>
    internal delegate void StateChanged(int[] currentState, bool isFinal);

    /// <summary>
    /// PuzzleSolution delegate -> Bulmaca Çözümü
    /// </summary>
    /// <param name="steps"></param>
    /// <param name="time"></param>
    /// <param name="stateExamined"></param>
    internal delegate void PuzzleSolution(int steps, int time, int stateExamined);

    #endregion
}
