﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using cardGame;
using MaterialSkin;
using MaterialSkin.Controls;

namespace CardGame2
{
    public  partial class GameBoard : MaterialForm
    {
        private string _userName = "";
        private string _levelName = "";
        private int _gameScore = 0;   
        private string _gameTime = "";
        private int _quick = 0;     // 카드를 보여주는 시간을 사용하는 변수.
        int dicCheckCnt = 0;        // 카드의 클릭횟수를 기록하기 위해 사용하는 변수.

        // 게임시간을 계산하기 위해 사용되는 변수.
        System.Timers.Timer t;
        int h, m, s;

        // DB에 연결에 사용할 변수.
        DBQuery db = DBQuery.getInstance();

        Random rCard = new Random();     
               
        Dictionary<object, Image> dicImages = new Dictionary<object, Image>();

        PictureBox pendingImage1;           // 이미지를 나타낼 수 있는 PictureBox 변수.
        PictureBox pendingImage2;

        SoundPlayer successP = new SoundPlayer(@"E:/workspace/visualstudio/C#/CSharpBook/CardGame2/Sound/success.wav");
        SoundPlayer failP = new SoundPlayer(@"E:/workspace/visualstudio/C#/CSharpBook/CardGame2/Sound/fail.wav");

        List<object> successTagList = new List<object>(); 

        public GameBoard()
        {
            GameUser gU = new GameUser();
            gU.ShowDialog();
            gU.Close();

            GameLevel gL = new GameLevel();
            gL.ShowDialog();
            gL.Close(); 

            InitializeComponent();

            _userName = gU.UserName;
            _levelName = gL.LevelName;

            gU = null;
            gL = null;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue500, Primary.Blue700, Primary.Blue50, Accent.LightBlue200, TextShade.WHITE);
            
            this.Text += _userName;

            switch (_levelName)
            {   // 선택 레벨에 따른 카드 보여주기//
                case "초급":
                    _quick = 5000;
                    break;
                case "중급":
                    _quick = 4000;
                    break;
                case "고급":
                    _quick = 3000;
                    break;
            }

            for (int i = 1; i < 9; i++)
            {
                var image = (Image)Properties.Resources.ResourceManager.GetObject("movie" + i);
                dicImages.Add(i, image);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            t = new System.Timers.Timer();
            t.Interval = 1000;          // 1s
            t.Elapsed += OnTimeEvent;

            dicCheckCnt = dicImages.Count;

            successTagList.Clear();

            foreach (PictureBox picture in CardsHolder.Controls)
            {
                picture.Enabled = false;
            }

           
            int[] iCard = new int[16];
            int randNum2, temp;

            for (int i = 0; i < 16; i++)
            {
                iCard[i] = i + 1;
            }

            for (int i = 0; i < 16; i++)
            {
                randNum2 = rCard.Next(0, 15);
                
                temp = iCard[randNum2];
                iCard[randNum2] = iCard[i];
                iCard[i] = temp;
            }

            // 카드게임을 시작할 때 보여주는 시간을 결정하는 것.
            timer1.Interval = _quick;
            timer1.Start();

            int iCnt = 0;
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                iCard[iCnt] = iCard[iCnt] % 8 + 1;
                picture.Tag = iCard[iCnt];
                picture.Image = dicImages[picture.Tag];
                iCnt++;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 맨 처음, 카드를 보여주는 시간이 끝나면 카드의 상태가 front로 변경됨.

            timer1.Stop();
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                picture.Enabled = true;
                picture.Cursor = Cursors.Hand;
                picture.Image = Properties.Resources.front;
            }

            t.Start();  // 게임시작시 시간 누적하기위한 타이머
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            // 힌트 버튼 클릭 후 다시 카드의 상태를 front.png 파일로 변경함.
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                if (successTagList.Contains(picture.Tag))
                {
                    continue;
                }
                picture.Image = Properties.Resources.front;
            }
            timer2.Stop();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            // 틀렸을 경우에 원래 이미지(front.png)를 보여주도록 처리.
            timer3.Stop();
            pendingImage1.Image = Properties.Resources.front;
            pendingImage2.Image = Properties.Resources.front;
            pendingImage1 = null;
            pendingImage2 = null;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            // 카드가 일치했을 때 처리하는 타이머.
            timer4.Stop();

            dicCheckCnt -= 1;
            pendingImage1.Enabled = true;
            pendingImage2.Enabled = true;
            pendingImage1 = null;
            pendingImage2 = null;

            if (dicCheckCnt == 0)
            {
                // 카드가 모두 일치됫을 때 처리.
                // insert 작업.
                t.Stop();
                _gameTime = gameTime.Text;
                db.Insert(_userName, _levelName, _gameScore, _gameTime);
                
                GameRank frm2 = new GameRank();
                frm2.ShowDialog();

                if (MessageBox.Show("다시 실행하시겠습니까?", "다시시도", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    this.Visible = false;
                    ScoreLabel.Text = "0";
                    gameTime.Text = string.Format("00:00:00");
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Click += card_Click;
                    }
                    new GameUser().Visible = true;
                }
                else
                {
                    ThreadExit();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 폼 종료시 
            if (MessageBox.Show("종료하시겠습니까?", "종료메시지", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnHint_Click(object sender, EventArgs e)
        {
            // card Hint 
            timer2.Start();
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                if (successTagList.Contains(picture.Tag))
                {
                    continue;
                }
                picture.Image = dicImages[picture.Tag];
            }
            // hint를 사용했기 때문에 -10씩 감소.
            ScoreLabel.Text = Convert.ToString(_gameScore >= 0 || _gameScore < 0 ? _gameScore -= 10 : _gameScore);
        }

        private void btnPlayAgain_Click(object sender, EventArgs e)
        {   // Play Again 버튼이 눌려졌을 때 처리하는 부분.
            if (MessageBox.Show("다시 실행하시겠습니까?", "다시시도", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Form1_Load(sender, e);
                ScoreLabel.Text = "0";
                foreach (PictureBox picture in CardsHolder.Controls)
                {
                    picture.Click += card_Click;
                }
            }
        }

        // Change Card Value
        private void card_Click(object sender, EventArgs e)
        {
            var card =  sender as PictureBox;

            // Choose Card 
            if (pendingImage1 == null)
            {   
                pendingImage1 = card;
                card.Image = dicImages[pendingImage1.Tag];
                return;
            }
            else if (pendingImage1 == card)
            {       
                // 자기자신 두번클릭할 경우 처리.
                return;
            }
            else if (pendingImage2 == null)
            {
                pendingImage2 = card;
                card.Image = dicImages[pendingImage2.Tag];
            }

            // Card Control
            if(pendingImage1 != null && pendingImage2 != null)
            {
                if (pendingImage1.Tag.Equals(pendingImage2.Tag))         //  두개가 일치했을 때,
                {
                    pendingImage1.Image = Properties.Resources.back;
                    pendingImage2.Image = Properties.Resources.back;

                    pendingImage1.Enabled = false;
                    pendingImage2.Enabled = false;

                    successTagList.Add(pendingImage1.Tag);      // Tag 값이 같기때문.하나만 넣는다.

                    // 점수가 0 이하로 떨어지지 않기 위한 처리.
                    ScoreLabel.Text = Convert.ToString(_gameScore >= 0 || _gameScore < 0 ? _gameScore += 10 : _gameScore);
                    
                    pendingImage1.Click -= card_Click;
                    pendingImage2.Click -= card_Click;

                    //successP.Play();

                    timer4.Start();
                }
                else
                {
                    //failP.Play();
                    timer3.Start();
                    ScoreLabel.Text = Convert.ToString(_gameScore > 0 ? _gameScore -= 10 : _gameScore);
                }
            }
        }

        private void GameBoard_FormClosed(object sender, FormClosedEventArgs e)
        {

            ThreadExit();
        }


        // 게임시간을 표시하기위한 계산 및 format setting
        private void OnTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    s += 1;
                    if (s == 60)
                    {
                        s = 0;
                        m += 1;
                    }
                    if (m == 60)
                    {
                        m = 0;
                        h += 1;
                    }
                    gameTime.Text = string.Format("{0}:{1}:{2}", h.ToString().PadLeft(2, '0'), m.ToString().PadLeft(2, '0'), s.ToString().PadLeft(2, '0'));
                }));
            }catch(Exception e1)
            {
                t.Dispose();
                MessageBox.Show(e1.Message, "Error Message");
            }
        }
        public void ThreadExit()
        {
            timer1.Dispose();
            timer2.Dispose();
            timer3.Dispose();
            timer4.Dispose();
        }
    }
}