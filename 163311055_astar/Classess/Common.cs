using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _163311055_astar.Classess
{
    #region Direction <-- Yönlendirme enum değeri tanımlanmıştır. 
    internal enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
    #endregion

    #region Heuristic<-- Menüde seçim olarak enum değeri tanımlanmıştır.
    internal enum Heuristic
    {
        MisplacedTiles,
        ManhattanDistance
    }
    #endregion

    public static class Common
    {
        #region Messaging tanımlamaları yapılmıştır ..
        /// <summary>
        /// FormLoadOpenedOfInformationMessage --> " Hoşgeldiniz Dr. Öğr. Üy. Tahir SAĞ --> "
        /// </summary>
        public static readonly string FormLoadOpenedOfInformationMessage = " Hoşgeldiniz Dr. Öğr. Üy. Tahir SAĞ --> ";

        /// <summary>
        /// StepNumberMessage --> "Adım Sayısı : "
        /// </summary>
        public static readonly string StepNumberMessage = "Adım Sayısı : ";

        /// <summary>
        /// TimingMessage --> ", Zamanlama : "
        /// </summary>
        public static readonly string TimingMessage = ", Zamanlama : ";

        /// <summary>
        /// StateMessage --> ", Durum : "
        /// </summary>
        public static readonly string StateMessage = ", Durum : ";

        /// <summary>
        /// SolutionFoundThereIsSeeStepCountingMessage --> "Çözüm Bulundu! Adımları görmek için Tamam'ı tıklayınız .. "
        /// </summary>
        public static readonly string SolutionFoundThereIsSeeStepCountingMessage = "Çözüm Bulundu! Adımları görmek için Tamam'ı tıklayınız .. ";

        /// <summary>
        /// NoStepCountOfTimingMessage --> "Adım Sayısı Yok. Zamanlama : "
        /// </summary>
        public static readonly string NoStepCountOfTimingMessage = "Adım Sayısı Yok. Zamanlama : ";

        /// <summary>
        /// SecondOfStateMessage --> " second, Durum : "
        /// </summary>
        public static readonly string SecondOfStateMessage = " second, Durum : ";

        /// <summary>
        /// SolutionNotFountMessage --> "Çözüm Bulunamadı ! "
        /// </summary>
        public static readonly string SolutionNotFountMessage = "Çözüm Bulunamadı ! ";

        /// <summary>
        /// SolutionFoundMessage --> "Çözüm Aranıyor !"
        /// </summary>
        public static readonly string SolutionFoundMessage = "Çözüm Aranıyor !";

        /// <summary>
        /// PlayAboutInformationIsOpenedMessage --> "Oyun Hakkında Bilgi Ekranı Açılıyorr ... "
        /// </summary>
        public static readonly string PlayAboutInformationIsOpenedMessage = "Oyun Hakkında Bilgi Ekranı Açılıyorr ... ";

        /// <summary>
        /// FullScreenOpen --> "Tam Ekran Modunu Aç"
        /// </summary>
        public static readonly string FullScreenOpen = "Tam Ekran";

        /// <summary>
        /// FullScreenClose --> "Tam Ekran Modunu Kapat"
        /// </summary>
        public static readonly string FullScreenClose = "Tam Ekran Modunu Kapat";

        /// <summary>
        /// WouldYouLikeToLogOut -> "Çıkış yapmak istiyor musunuz ? "
        /// </summary>
        public static readonly string WouldYouLikeToLogOut = "Çıkış yapmak istiyor musunuz ? ";

        /// <summary>
        /// Report -> "Bildiri"
        /// </summary>
        public static readonly string Report = "Bildiri";

        #endregion
    }
}
