using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _163311055_astar.Classess
{
    #region Bulmaca Stratejisi için açılmış olunan sınıftır.
    internal sealed class PuzzleStrategy
    {
        #region Fields

        private Stopwatch mStopWatch;
        internal event StateChanged OnStateChanged;
        internal event PuzzleSolution OnPuzzleSolved;

        #endregion Fields

        #region Constructor
        internal PuzzleStrategy()
        {
            mStopWatch = new Stopwatch();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Solve metodu.
        /// Yürütme için bir yöntem kuyruğa alır. Yöntemi, bir iş parçacığı havuzu iş parçacığı kullanılabilir hale geldiğinde yürütülür.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="heuristic"></param>
        internal void Solve(int[] nodes, Heuristic heuristic)
        {
            ThreadPool.QueueUserWorkItem(item => Start(nodes, heuristic));
        }

        #region Başlama işlemi 
        private void Start(int[] nodes, Heuristic heuristic)
        {
            int openStateIndex;
            int stateCount = -1;
            State currentState = null;
            List<State> nextStates = new List<State>();
            //Bir kümedeki elemeanları almak için kullanılır. HashSet her değerden sadece 1 tane içerir.
            HashSet<String> openStates = new HashSet<string>();
            MinPriorityQueue<State> openStateQueue = new MinPriorityQueue<State>(nodes.Length * 3);
            Dictionary<String, State> closedQueue = new Dictionary<string, State>(nodes.Length * 3);

            State state = new State(null, nodes, heuristic);
            openStateQueue.Enqueue(state);
            openStates.Add(state.GetStateCode());

            while (!openStateQueue.IsEmpty())
            {
                currentState = openStateQueue.Dequeue();
                openStates.Remove(currentState.GetStateCode());

                stateCount++;

                // Son durum olup olmadığı kontrol edilir 
                if (currentState.IsFinalState())
                {
                    EndMeasure(stateCount);
                    break;
                }

                // Bir sonraki duruma bakılır .. 
                currentState.GetNextStates(ref nextStates);

                if (nextStates.Count > 0)
                {
                    State closedState;
                    State openState;
                    State nextState;

                    for (int i = 0; i < nextStates.Count; i++)
                    {
                        closedState = null;
                        openState = null;
                        nextState = nextStates[i];

                        if (openStates.Contains(nextState.GetStateCode()))
                        {
                            // Açık kuyrukta zaten aynı bir kuyruk bulunmaktadır ... 
                            openState = openStateQueue.Find(nextState, out openStateIndex);

                            if (openState.IsCostlierThan(nextState))
                            {
                                // Duruma ulaşmak için daha iyi bir yol bulunmuş ise.
                                openStateQueue.Remove(openStateIndex);
                                openStateQueue.Enqueue(nextState);
                            }
                        }
                        else
                        {
                            // Durumun kapalı duurmda olup olmadığı kontrol edilir .. 
                            String stateCode = nextState.GetStateCode();

                            if (closedQueue.TryGetValue(stateCode, out closedState))
                            {
                                // Duruma ulaşmak için daha iyi bir yol bulunmuş ise.
                                if (closedState.IsCostlierThan(nextState))
                                {
                                    closedQueue.Remove(stateCode);
                                    closedQueue[stateCode] = nextState;
                                }
                            }
                        }

                        // Yeni bir durum ya da öncekinden daha iyi ise
                        if (openState == null && closedState == null)
                        {
                            openStateQueue.Enqueue(nextState);
                            openStates.Add(nextState.GetStateCode());
                        }
                    }

                    closedQueue[currentState.GetStateCode()] = currentState;
                }
            }

            if (currentState != null && !currentState.IsFinalState())
            {
                // Çözüm Bulunamadı. 
                currentState = null;
            }

            PuzzleSolved(currentState, stateCount);
            OnFinalState(currentState);
        }
        #endregion

        /// <summary>
        /// EndMeasure Ölçüm sonu
        /// </summary>
        /// <param name="stateCount"></param>
        private void EndMeasure(int stateCount)
        {
            mStopWatch.Stop();
        }

        /// <summary>
        /// Durum Sonucu
        /// </summary>
        /// <param name="state"></param>
        private void OnFinalState(State state)
        {
            if (state != null)
            {
                //Ağaç köküne gider.
                Stack<State> path = new Stack<State>();

                while (state != null)
                {
                    path.Push(state);
                    state = state.GetParent();
                }

                while (path.Count > 0)
                {
                    // Yolda birer birer hareket eder. 
                    OnStateChanged(path.Pop().GetState(), path.Count == 0);
                }
            }
            else
            {
                // Çözüm yoksa
                OnStateChanged(null, true);
            }
        }

        /// <summary>
        /// Puzzle Çözüldü ise
        /// </summary>
        /// <param name="state"></param>
        /// <param name="states"></param>
        private void PuzzleSolved(State state, int states)
        {
            int steps = -1;

            while (state != null)
            {
                state = state.GetParent();
                steps++;
            }

            if (OnPuzzleSolved != null)
            {
                OnPuzzleSolved(steps, (int)mStopWatch.ElapsedMilliseconds, states);
            }
        }

        #endregion Methods
    }
    #endregion
}
