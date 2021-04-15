using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _163311055_astar.Classess
{
    #region Doğrusal olarak karşılaştırma yapılmak için ayrı bir sınıf tasarlanmıştır. 
    internal sealed class LinearShuffle<T>
    {
        #region Fields
        private Random random;
        #endregion

        #region Constructor
        internal LinearShuffle()
        {
            int seed = 37 + 37 + ((int)DateTime.Now.TimeOfDay.TotalSeconds % 37);
            random = new Random(seed);
        }
        #endregion

        #region Methods

        #region Randomm değer oluşturuluyor. 
        private int NextRandom(int min, int max)
        {
            return random.Next(min, max);
        }
        #endregion

        #region Yer Değiştirme sağlanıyor .. 
        /// <summary>
        /// Swap method
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        private void Swap(T[] temp, int minValue, int maxValue)
        {
            T tempValue = temp[minValue];
            temp[minValue] = temp[maxValue];
            temp[maxValue] = tempValue;
        }
        #endregion

        #region Karıştırılıyor .. 
        internal void Shuffle(T[] array)
        {
            int position;
            for (int i = 0; i < array.Length; i++)
            {
                position = NextRandom(0, i);
                Swap(array, i, position);
            }
        }
        #endregion

        #endregion
    }
    #endregion
}
