using _163311055_astar.Classess;
using _163311055_astar.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _163311055_astar
{
    public partial class MainPage : Form
    {
        #region Değişkenler

        /// <summary>
        /// PuzzleStrategy properties
        /// </summary>
        private PuzzleStrategy puzzleStrategy;
        /// <summary>
        /// Heuristic get values enum
        /// </summary>
        private Heuristic heuristic;
        /// <summary>
        /// LinearShuffle properties. Değiştirme karıştırma işlemleri yer alıyor. 
        /// </summary>
        private LinearShuffle<int> linearShuffle;
        /// <summary>
        /// WindowsFormsSynchronizationContext farklı eşitleme modellerinde çalışmayı saplar. 
        /// </summary>
        private WindowsFormsSynchronizationContext synchronizationContext;
        /// <summary>
        /// Button properties
        /// </summary>
        Dictionary<int, Button> buttons;
        /// <summary>
        /// initialState dizi properties
        /// </summary>
        private int[] initialState;
        /// <summary>
        /// Boolean properties
        /// </summary>
        private bool engaded;

        #endregion

        #region Constructor
        public MainPage()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current as WindowsFormsSynchronizationContext;
            InitializeLoadData();
        }
        #endregion

        #region Click

        #region Karıştır butonuna tıklanıdığında gerçekleşecek işlemler .. 
        private void buttonShuffle_Click(object sender, EventArgs e)
        {
            if (ActionAllowed())
            {
                linearShuffle.Shuffle(initialState); //Random tanımlamsı ile sayılar karıştıırlıyor .. 
                DisplayState(initialState, false); //Panel üzerindde karıştırılan sayıların görünümleri sağlanıyor.
            }
        }
        #endregion

        #region Başla butonuna basıldığında gerçekleşecek işlemler
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (ActionAllowed())
            {
                StartSolvingPuzzle();
            }
        }
        #endregion

        #region Menuden manhattan ya da misplaced olarak seçenek seçildiğinde enabled durumları ayarlanmaktadır. 
        private void manhattanDistanceMenu_Click_1(object sender, EventArgs e)
        {
            if(manhattanDistanceMenu.Text == "manhattanDistanceMenu")
            {
                if (ActionAllowed())
                {
                    heuristic = Heuristic.ManhattanDistance;
                    manhattanDistanceMenu.Checked = true;
                    misplacedTilesMenu.Checked = false;
                }
            }
            if(misplacedTilesMenu.Text== "misplacedTilesMenu")
            {
                if (ActionAllowed())
                {
                    heuristic = Heuristic.MisplacedTiles;
                    misplacedTilesMenu.Checked = true;
                    manhattanDistanceMenu.Checked = false;
                }
            }
        }
        #endregion

        #region Çıkış Butonu
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Common.WouldYouLikeToLogOut,
                Common.Report,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
            else
                return;
        }
        #endregion

        #region Refresh Butonu
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        #endregion

        #region Oyun hakkında bilgi alınmak istenildiğinde açılacak ekran
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Common.PlayAboutInformationIsOpenedMessage);
            Thread.Sleep(1500);
            UIPlayAbout about = new UIPlayAbout();
            about.Show();
        }
        #endregion

        #region Form Load
        private void MainPage_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            lblInfoPerson.Text = Common.FormLoadOpenedOfInformationMessage;
        }
        #endregion

        #region Timer --> Kayan yazının aktif olması için ..
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblInfoPerson.Text = lblInfoPerson.Text.Substring(1) + lblInfoPerson.Text.Substring(0, 1);
        }
        #endregion

        /// <summary>
        /// Tam ekranın modunun açılıp kapanması olayı tetiklenmektedir...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckFullScreen_CheckedChanged_1(object sender, EventArgs e)
        {
            #region Tam Ekran Modunu Açmak İçin
            if (ckFullScreen.Text == Common.FullScreenOpen)
            {
                WindowState = FormWindowState.Maximized;
                ckFullScreen.Text = Common.FullScreenClose;
            }
            #endregion
            #region Tam Ekranı Modunu Kapatmak İçin
            else if (ckFullScreen.Text == Common.FullScreenClose)
            {
                WindowState = FormWindowState.Normal;
                ckFullScreen.Text = Common.FullScreenOpen;
            }
            #endregion
        }

        #region Butonların yerlerinin manuel olarak karıştırılması için gerekli eventlar yer almaktadır. 
        /// <summary>
        /// MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            if (ActionAllowed())
            {
                Button button = sender as Button;

                if (button != null && button.Tag != null)
                {
                    int value;
                    Button tileButton;

                    if (int.TryParse(button.Tag.ToString(), out value) && buttons.TryGetValue(value, out tileButton) && button == tileButton)
                    {
                        button.DoDragDrop(button.Tag, DragDropEffects.Copy | DragDropEffects.Move);
                    }
                }
            }
        }

        /// <summary>
        /// DragDrop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_DragDrop_1(object sender, DragEventArgs e)
        {
            if (ActionAllowed())
            {
                Button button = sender as Button;
                if (button != null && button.Tag != null)
                {
                    int dropValue;
                    Button buttonToDrop;

                    if (int.TryParse(button.Tag.ToString(), out dropValue) && buttons.TryGetValue(dropValue, out buttonToDrop) && button == buttonToDrop)
                    {
                        int dragValue;

                        if (int.TryParse(e.Data.GetData(DataFormats.Text).ToString(), out dragValue) && dropValue != dragValue)
                        {
                            SwapValues(dragValue, dropValue);
                            DisplayState(initialState, false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// DragEnter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_DragEnter(object sender, DragEventArgs e)
        {
            if (ActionAllowed())
            {
                if (e.Data.GetDataPresent(DataFormats.Text))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        /// <summary>
        /// Form açılırken taşlar karıştırılıyor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPage_Load_1(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            lblInfoPerson.Text = Common.FormLoadOpenedOfInformationMessage;
            if (ActionAllowed())
            {
                linearShuffle.Shuffle(initialState); //Random tanımlamsı ile sayılar karıştıırlıyor .. 
                DisplayState(initialState, false); //Panel üzerindde karıştırılan sayıların görünümleri sağlanıyor.
            }
        }

        #endregion

        #region Methods

        #region Form ekranı açılırken oluşabilecek işlemler .. 
        private void InitializeLoadData()
        {
            initialState = new int[] { 8, 7, 2, 4, 6, 3, 1, -1, 5 };
            linearShuffle = new LinearShuffle<int>();
            puzzleStrategy = new PuzzleStrategy();
            heuristic = Heuristic.ManhattanDistance;
            puzzleStrategy.OnStateChanged += OnStrategyStateChangedEvent;
            puzzleStrategy.OnPuzzleSolved += OnPuzzleSolvedEvent;

            #region Ekran düğümleri ayarlanıyor ... 
            buttons = new Dictionary<int, Button>();
            buttons[0] = button1;
            buttons[1] = button2;
            buttons[2] = button3;
            buttons[3] = button4;
            buttons[4] = button5;
            buttons[5] = button6;
            buttons[6] = button7;
            buttons[7] = button8;
            buttons[8] = button9;
            #endregion

            //Görüntü Durumu <--
            DisplayState(initialState, false);

            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.Visible = false;
            manhattanDistanceMenu.Checked = true;
        }
        #endregion

        #region Sayıların görünüm durumları ayarlanmaktadır. 
        private void DisplayState(int[] nodes, bool isFinal)
        {
            if (nodes != null)
            {
                this.gamePanel.SuspendLayout();

                for (int i = 0; i < nodes.Length; i++)
                {
                    if (nodes[i] > 0)
                        buttons[i].Text = nodes[i].ToString();
                    else
                        buttons[i].Text = null;
                }
                this.gamePanel.ResumeLayout();
            }

            if (isFinal)
            {
                engaded = false;
                buttonStart.Enabled = true;
            }
        }
        #endregion

        #region Form açılırken tetiklenen eventlar 
        /// <summary>
        /// OnStrategyStateChangedEvent event
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isFinal"></param>
        private void OnStrategyStateChangedEvent(int[] state, bool isFinal)
        {
            synchronizationContext.Post(item => DisplayState(state, isFinal), null);
            Thread.Sleep(1500);
        }

        /// <summary>
        /// OnPuzzleSolvedEvent 
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="time"></param>
        /// <param name="statesExamined"></param>
        private void OnPuzzleSolvedEvent(int steps, int time, int statesExamined)
        {
            Action action = () =>
            {
                progressBar.Visible = false;
                this.Cursor = Cursors.Default;

                #region Progressbar kısmında bilgilendirme mesajarı yer almaktadır. 
                if (steps > -1)
                {
                    statusLabel.Text = Common.StepNumberMessage + steps.ToString("n0") +
                    Common.TimingMessage + (time / 1000.0).ToString("n2") +
                    Common.StepNumberMessage + statesExamined.ToString("n0");
                    MessageBox.Show(this, Common.SolutionFoundThereIsSeeStepCountingMessage);
                }
                else
                {
                    statusLabel.Text = Common.NoStepCountOfTimingMessage + (time / 1000.0).ToString("n3") +
                    Common.SecondOfStateMessage + statesExamined.ToString("n0");
                    MessageBox.Show(Common.SolutionNotFountMessage);
                }
                #endregion
            };

            synchronizationContext.Send(item => action.Invoke(), null);
        }
        #endregion

        #region Çözülen Bulmaca düzenlemesi yapılmaktadır. 
        private void StartSolvingPuzzle()
        {
            puzzleStrategy.Solve(initialState, heuristic);
            progressBar.Visible = true;
            this.Cursor = Cursors.WaitCursor;
            statusLabel.Text = Common.SolutionFoundMessage;
            engaded = true;
        }
        #endregion

        #region ActionAllowed Boolean 
        private bool ActionAllowed()
        {
            return !engaded;
        }
        #endregion

        #region Yer Değiştirme sistemi düzenleniyor .. 
        private void SwapValues(int x, int y)
        {
            int temp = initialState[x];
            initialState[x] = initialState[y];
            initialState[y] = temp;
        }

        #endregion

        #endregion        
    }
}
