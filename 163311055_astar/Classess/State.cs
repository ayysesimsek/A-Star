using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _163311055_astar.Classess
{
    #region Durumun belirlenmesi için oluşturulan sınıftır.
    /// <summary>
    ///  IComparable Interface'i, miras alan sınıfa CompareTo metodunu uygulatmakta ve bu metot aracılığıyla karşılaştırmayı sağlamaktadır
    /// </summary>
    internal sealed class State : IComparable
    {
        #region Fields
        private int[] mNodes;
        private int mSpaceIndex;
        private string mStateCode;
        private int F;
        private int G;
        private int H;
        private Heuristic mHeuristic;
        private State mParent;
        #endregion

        #region Constructor
        /// <summary>
        /// State yapıcı metod tanımlaması .
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodes"></param>
        /// <param name="heuristic"></param>
        internal State(State parent, int[] nodes, Heuristic heuristic)
        {
            mNodes = nodes;
            mParent = parent;
            mHeuristic = heuristic;
            CalculatedCost();
            mStateCode = GenerateStateCode();
        }
        /// <summary>
        /// State yapıcı metod tanımlaması.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodes"></param>
        private State(State parent, int[] nodes)
        {
            mNodes = nodes;
            mParent = parent;
            mHeuristic = parent.mHeuristic;
            CalculatedCost();
            mStateCode = GenerateStateCode();
        }
        #endregion

        #region Method

        #region IComparable override methodları
        public override bool Equals(object obj)
        {
            State that = obj as State;

            return that != null && this.mStateCode.Equals(that.mStateCode);
        }

        public override int GetHashCode()
        {
            return mStateCode.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            State that = obj as State;

            if (that != null)
            {
                return (this.F).CompareTo(that.F);
            }

            return 0;
        }
        #endregion

        #region G kontrolü
        public bool IsCostlierThan(State thatState)
        {
            return this.G > thatState.G;
        }
        #endregion

        #region Durum kodu döndürülüyor .. 
        public String GetStateCode()
        {
            return mStateCode;
        }
        #endregion

        #region F = G + H hesaplaması işlemi yapılmaktadır. 
        private void CalculatedCost()
        {
            if (mParent == null)
                G = 0;
            else
                G = mParent.G + 1;
            H = GetHeuristicCost();
            F = G + H;
        }
        #endregion

        #region MisplacedTiles & ManhattanDistance Yöntemleri 
        #region Algoritmaa yöntemine göre işlem döndürülüyor ...
        private int GetHeuristicCost()
        {
            if (mHeuristic == Heuristic.ManhattanDistance)
                return GetManhattanDistanceCost();
            else
                return GetMisplacedTilesCost();
        }
        #endregion

        #region MisplacedTiles yöntemine göre işlem yapılmaktadır. 
        private int GetMisplacedTilesCost()
        {
            int heuristicCost = 0;

            for (int i = 0; i < mNodes.Length; i++)
            {
                int value = mNodes[i] - 1;

                if (value == -2)
                {
                    value = mNodes.Length - 1;
                    mSpaceIndex = i;
                }

                if (value != i)
                    heuristicCost++;
            }
            return heuristicCost;
        }
        #endregion

        #region ManhattanDistance yöntemine göre işlem yapılmaktadır. 
        private int GetManhattanDistanceCost()
        {
            int heuristicCost = 0;
            int gridX = (int)Math.Sqrt(mNodes.Length);
            int idealX;
            int idealY;
            int currentX;
            int currentY;
            int value;

            for (int i = 0; i < mNodes.Length; i++)
            {
                value = mNodes[i] - 1;
                if (value == -2)
                {
                    value = mNodes.Length - 1;
                    mSpaceIndex = i;
                }

                if (value != i)
                {
                    idealX = value % gridX;
                    idealY = value / gridX;

                    currentX = i % gridX;
                    currentY = i / gridX;

                    heuristicCost += (Math.Abs(idealY - currentY) + Math.Abs(idealX - currentX));
                }
            }

            return heuristicCost;
        }
        #endregion
        #endregion        

        #region Durum kodu oluşturuluyor ..
        private String GenerateStateCode()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < mNodes.Length; i++)
            {
                stringBuilder.Append(mNodes[i] + "*");
            }

            return stringBuilder.ToString().Trim(new char[] { '*' });
        }
        #endregion

        #region Durum oluşturuluyor .. 
        public int[] GetState()
        {
            int[] state = new int[mNodes.Length];
            Array.Copy(mNodes, state, mNodes.Length);

            return state;
        }
        #endregion

        #region Tüm hepsi doğru pozisyonda ise son hal döndürülür
        public bool IsFinalState()
        {
            return H == 0;
        }
        #endregion

        /// <summary>
        /// GetParent method
        /// </summary>
        /// <returns></returns>
        public State GetParent()
        {
            return mParent;
        }

        #region Sonraki Durumlar belirleniyor .. 
        /// <summary>
        /// GetNextStates method. Gelecek durumların bulunmasını sağlıyor..
        /// </summary>
        /// <param name="nextStates"></param>
        /// <returns></returns>
        public List<State> GetNextStates(ref List<State> nextStates)
        {
            nextStates.Clear();
            State state;

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                state = GetNextState(direction);

                if (state != null)
                {
                    nextStates.Add(state);
                }
            }

            return nextStates;
        }
        /// <summary>
        /// GetNextState yer değiştirme taşınma işlemleri yapılmakatdır. 
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private State GetNextState(Direction direction)
        {
            int position;

            if (CanMove(direction, out position))
            {
                int[] nodes = new int[mNodes.Length];
                Array.Copy(mNodes, nodes, mNodes.Length);

                // Get new state nodes
                Swap(nodes, mSpaceIndex, position);

                return new State(this, nodes);
            }

            return null;
        }
        #endregion

        #region Yer değiştirme işlemi 
        private void Swap(int[] nodes, int first, int second)
        {
            int temp = nodes[first];
            nodes[first] = nodes[second];
            nodes[second] = temp;
        }
        #endregion

        #region Hareket Etme Aksiyonu .. 
        private bool CanMove(Direction direction, out int newPosition)
        {
            int newX = -1;
            int newY = -1;
            int gridX = (int)Math.Sqrt(mNodes.Length);
            int currentX = mSpaceIndex % gridX;
            int currentY = mSpaceIndex / gridX;
            newPosition = -1;

            #region Yön belirleniyor .. 
            switch (direction)
            {
                #region  Yukarı ise
                case Direction.UP:
                    {
                        if (currentY != 0)
                        {
                            newX = currentX;
                            newY = currentY - 1;
                        }
                    }
                    break;
                #endregion
                #region Aşağı ise
                case Direction.DOWN:
                    {
                        if (currentY < (gridX - 1))
                        {
                            newX = currentX;
                            newY = currentY + 1;
                        }
                    }
                    break;
                #endregion
                #region Sol ise
                case Direction.LEFT:
                    {
                        if (currentX != 0)
                        {
                            newX = currentX - 1;
                            newY = currentY;
                        }
                    }
                    break;
                #endregion
                #region Sağ ise
                case Direction.RIGHT:
                    {
                        if (currentX < (gridX - 1))
                        {
                            newX = currentX + 1;
                            newY = currentY;
                        }
                    }
                    break;
                    #endregion
            }
            #endregion


            if (newX != -1 &&
                newY != -1)
            {
                newPosition = newY * gridX + newX;
            }

            return newPosition != -1;
        }
        #endregion

        /// <summary>
        /// override ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Durum : " + mStateCode + ", G: " + G + ", H : " + H + ", F : " + F;
        }

        #endregion
    }
    #endregion
}
