using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _163311055_astar.Classess
{
    #region MinPriorityQueue <-- Minimum öncelik sırası belirlemek için ayrı bir sınıf tasarlanmıştır.  .. 
    internal sealed class MinPriorityQueue<T> where T : IComparable
    {
        #region Fields

        private T[] mArray;
        private int mCount;

        #endregion

        #region Constructor
        internal MinPriorityQueue(int capacity)
        {
            mArray = new T[capacity + 1];
            mCount = 0;
        }

        #endregion

        #region Methodss

        /// <summary>
        /// Genişletilebilirlik ayarlanmaktadır .. 
        /// </summary>
        /// <param name="capacity"></param>
        private void Expand(int capacity)
        {
            T[] temp = new T[capacity + 1];
            int i = 0;
            while (++i <= mCount)
            {
                temp[i] = mArray[i];
                mArray[i] = default(T);
            }
            mArray = temp;
        }

        /// <summary>
        /// Az değer döndürme işlemi 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private bool Less(int i, int k)
        {
            return mArray[i].CompareTo(mArray[k]) < 0;
        }

        /// <summary>
        /// Yer değiştirme işlemi
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void Swap(int i, int j)
        {
            T temp = mArray[j];
            mArray[j] = mArray[i];
            mArray[i] = temp;
        }

        #region Hareket işlemleri
        private void Sink(int index)
        {
            int getValue;
            while (index * 2 <= mCount)
            {
                getValue = index * 2;

                if (getValue + 1 <= mCount &&
                    Less(getValue + 1, getValue))
                    getValue += 1;

                if (!Less(getValue, index))
                    break;

                Swap(index, getValue);
                index = getValue;
            }
        }

        private void Swim(int index)
        {
            int getValue;

            while (index / 2 > 0)
            {
                getValue = index / 2;

                if (!Less(index, getValue))
                    break;

                Swap(index, getValue);
                index = getValue;
            }
        }
        #endregion

        /// <summary>
        /// Empty
        /// </summary>
        /// <returns></returns>
        internal bool IsEmpty()
        {
            return mCount == 0;
        }

        /// <summary>
        /// Enqueue <-- Sıraya alır. 
        /// </summary>
        /// <param name="item"></param>
        internal void Enqueue(T item)
        {
            if (mCount == mArray.Length - 1)
                Expand(mArray.Length * 3);
            mArray[++mCount] = item;
            Swim(mCount);
        }

        /// <summary>
        /// Kuyrukta önden ya da arkadan öğelerin eklenebileceği ya da önden çıkarılabileceği soyut bir veri türüdür.
        /// </summary>
        /// <returns></returns>
        internal T Dequeue()
        {
            if (!IsEmpty())
            {
                T item = mArray[1];
                mArray[1] = mArray[mCount];
                mArray[mCount--] = default(T);

                Sink(1);
                return item;
            }
            return default(T);
        }

        /// <summary>
        /// Bulma metodu
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal T Find(T item, out int index)
        {
            index = -1;
            if (!IsEmpty())
            {
                int i = 0;

                while (++i <= mCount)
                {
                    if (mArray[i].Equals(item))
                    {
                        index = i;
                        return mArray[i];
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="index"></param>
        internal void Remove(int index)
        {
            if (index > 0 && index <= mCount)
            {
                mArray[index] = mArray[mCount];
                mArray[mCount--] = default(T);
                Sink(index);
            }
        }

        #endregion
    }
    #endregion
}
