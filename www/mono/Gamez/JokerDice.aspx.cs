using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using System.Windows.Media.Animation;

namespace Area23.At.Mono.Gamez
{
    public enum JokerDiceEnum
    {
        Joker = 0,
        Ace = 5,
        King = 4,
        Queen = 3,
        Jack = 2,
        Ten = 1
    }


    public enum JokerDiceScore
    {                                        
        StrikeThrough = 0,
        Bust = 1,
        Pair = 2,
        TwoPairs = 4,
        Triple = 6,
        Straight = 8,
        FullHouse = 10,
        Poker = 12,
        Grande = 15
    }


    public static class JokerDiceExtensions
    {
        public static string ToImgUrl(this JokerDiceEnum value)
        {
            switch (value)
            {
                case JokerDiceEnum.Joker:       return "../res/gamez/dicepoker/Joker.gif";
                case JokerDiceEnum.Ace:         return "../res/gamez/dicepoker/Ace.gif";
                case JokerDiceEnum.King:        return "../res/gamez/dicepoker/King.gif";
                case JokerDiceEnum.Queen:       return "../res/gamez/dicepoker/Queen.gif";
                case JokerDiceEnum.Jack:        return "../res/gamez/dicepoker/Jack.gif";
                case JokerDiceEnum.Ten:         return "../res/gamez/dicepoker/Ten.gif";
                default:                        return value.ToString();
            }
        }

        public static JokerDiceEnum ToJokerDiceEnum(this string value)
        {
            JokerDiceEnum jokerDiceEnum;
            if (Enum.TryParse(value, out jokerDiceEnum))
                return jokerDiceEnum;
            else
                throw new ArgumentException($"Invalid string {value} for conversion to JokerDiceEnum");            
        }
    }

    public partial class JokerDice : System.Web.UI.Page
    {
        public bool DiceCupClickable         
        {
            get => (bool)Session["DiceCupClickable"];            
            set => Session["DiceCupClickable"] = value;
        }

        public int PlayerScore
        {
            get => (int)Session["PlayerScore"];
            set => Session["PlayerScore"] = value;
        }


        public int GameRound 
        { 
            get
            {
                int round = (Session["Round"] == null) ? 0 : (int)(Session["Round"]);
                Session["Round"] = (round > 65535) ? 2 : round;
                return (int)Session["Round"];
            }
            set => Session["Round"] = (value < 0) ? 0 : value;
        }
        JokerDiceEnum[] dices = new JokerDiceEnum[5];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["DiceCupClickable"] == null)
                Session["DiceCupClickable"] = true;
            if (Session["PlayerScore"] == null)
                Session["PlayerScore"] = 0;

            if (!IsPostBack)
            {
                if (Request.UrlReferrer == null || !Request.UrlReferrer.Equals(Request.Url))
                    Session["Round"] = 0;
                else 
                    Session["Round"] = (Session["Round"] == null) ? 0 : (((int)Session["Round"] + ((int)Session["Round"] % 2)));
                ResetCheckBoxes(sender, e);                
            }

            dices = (Session["JokerDices"] != null) ? (JokerDiceEnum[])Session["JokerDices"] : new JokerDiceEnum[5];
        }


        protected void ImageP_Click(object sender, EventArgs e)
        {
            if (GameRound % 2 == 0)
                return;

            if (sender is ImageButton im)
            {
                if (!im.ImageUrl.EndsWith("CupOverDice.gif"))
                {
                    im.BorderStyle = BorderStyle.Dashed;
                    im.ImageUrl = "../res/gamez/dicepoker/CupOverDice.gif";
                }
                else if (im.BorderStyle == BorderStyle.Dashed || im.ImageUrl.EndsWith("CupOverDice.gif"))
                {
                    if (!Enum.TryParse<JokerDiceEnum>(im.AlternateText, out JokerDiceEnum jDice))
                    {
                        int imi = Int32.Parse(im.ID.Replace("ImageP", "")) - 1;
                        jDice = dices[imi];
                    }                    
                    im.ImageUrl = jDice.ToImgUrl();
                    im.BorderStyle = BorderStyle.Solid;
                }
            }
        }

        protected void ImageButton_DiceCup_Click(object sender, EventArgs e)
        {
            if (!DiceCupClickable)
            {
                return;
            }
            if (GameRound % 2 == 0)
            {
                ImageP1.BorderStyle = BorderStyle.None;
                ImageP2.BorderStyle = BorderStyle.None;
                ImageP3.BorderStyle = BorderStyle.None;
                ImageP4.BorderStyle = BorderStyle.None;
                ImageP5.BorderStyle = BorderStyle.None;
            }

            Random rnd = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);
            int[] dice = new int[5];
            dice[0] = (ImageP1.BorderStyle == BorderStyle.None || ImageP1.ImageUrl.EndsWith("CupOverDice.gif")) ?
                        rnd.Next(0, 6) : (int)ImageP1.AlternateText.ToJokerDiceEnum();
            dice[1] = (ImageP2.BorderStyle == BorderStyle.None || ImageP2.ImageUrl.EndsWith("CupOverDice.gif")) ?
                        rnd.Next(0, 6) : (int)ImageP2.AlternateText.ToJokerDiceEnum();           
            dice[2] = (ImageP3.BorderStyle == BorderStyle.None || ImageP3.ImageUrl.EndsWith("CupOverDice.gif")) ?
                        rnd.Next(0, 6) : (int)ImageP3.AlternateText.ToJokerDiceEnum();
            dice[3] = (ImageP4.BorderStyle == BorderStyle.None || ImageP4.ImageUrl.EndsWith("CupOverDice.gif")) ?
                        rnd.Next(0, 6) : (int)ImageP4.AlternateText.ToJokerDiceEnum();           
            dice[4] = (ImageP5.BorderStyle == BorderStyle.None || ImageP5.ImageUrl.EndsWith("CupOverDice.gif")) ?
                        rnd.Next(0, 6) : (int)ImageP5.AlternateText.ToJokerDiceEnum();

            for (int j = 0; j < 5; j++)
            {
                for (int k = j; k < 5; k++)
                {
                    if (k != j && dice[k] < dice[j])
                    {
                        int temp = dice[j];
                        dice[j] = dice[k];
                        dice[k] = temp;
                    }
                }
            }

            dices = new JokerDiceEnum[5];
            dices[0] = (JokerDiceEnum)dice[0];
            dices[1] = (JokerDiceEnum)dice[1];
            dices[2] = (JokerDiceEnum)dice[2];
            dices[3] = (JokerDiceEnum)dice[3];
            dices[4] = (JokerDiceEnum)dice[4];
            Session["JokerDices"] = dices;

            ImageP1.AlternateText = dices[0].ToString();
            ImageP1.ImageUrl = dices[0].ToImgUrl();
            ImageP2.AlternateText = dices[1].ToString();
            ImageP2.ImageUrl = dices[1].ToImgUrl();
            ImageP3.AlternateText = dices[2].ToString();
            ImageP3.ImageUrl = dices[2].ToImgUrl();
            ImageP4.AlternateText = dices[3].ToString();
            ImageP4.ImageUrl = dices[3].ToImgUrl();
            ImageP5.AlternateText = dices[4].ToString();
            ImageP5.ImageUrl = dices[4].ToImgUrl();

            if (GameRound % 2 == 0)
            {
                DisableCheckBoxesPenImageButtons(sender, e);
                this.ImageButton_DiceCup.ImageUrl = "../res/gamez/dicepoker/CupNextRound.gif";
                ImageP1.BorderStyle = BorderStyle.Solid;
                ImageP2.BorderStyle = BorderStyle.Solid;
                ImageP3.BorderStyle = BorderStyle.Solid;
                ImageP4.BorderStyle = BorderStyle.Solid;
                ImageP5.BorderStyle = BorderStyle.Solid;
                this.Literal_Action.Text = "Select dices you want to keep<br /> and roll the others again.";
            }
            if (GameRound % 2 == 1)
            {
                DiceCupClickable = false;
                this.ImageButton_DiceCup.ImageUrl = "../res/gamez/dicepoker/CupNextGame.gif";
                ImageP1.BorderStyle = BorderStyle.Solid;
                ImageP2.BorderStyle = BorderStyle.Solid;
                ImageP3.BorderStyle = BorderStyle.Solid;
                ImageP4.BorderStyle = BorderStyle.Solid;
                ImageP5.BorderStyle = BorderStyle.Solid;
                bool checkBoxesShown = ShowCheckBoxes(dices);
                if (!checkBoxesShown)
                    ShowStrikeThroughPens();
                this.Literal_Action.Text = "Select your best combination.";                
            }
            Computer_DiceCup(sender, e);
            CheckWin(sender, e);
            GameRound = GameRound + 1;
        }


        protected void Computer_DiceCup(object sender, EventArgs e)
        {
            if (GameRound % 2 == 0)
            {
                ImageC1.BorderStyle = BorderStyle.None;
                ImageC2.BorderStyle = BorderStyle.None;
                ImageC3.BorderStyle = BorderStyle.None;
                ImageC4.BorderStyle = BorderStyle.None;
                ImageC5.BorderStyle = BorderStyle.None;
            }

            Random rnd = new Random(DateTime.Now.Second);
            int[] dicec = new int[5];
            dicec[0] = rnd.Next(0, 6);
            dicec[1] = rnd.Next(0, 6);
            dicec[2] = rnd.Next(0, 6);
            dicec[3] = rnd.Next(0, 6);
            dicec[4] = rnd.Next(0, 6);

            for (int j = 0; j < 5; j++)
            {
                for (int k = j; k < 5; k++)
                {
                    if (k != j && dicec[k] < dicec[j])
                    {
                        int temp = dicec[j];
                        dicec[j] = dicec[k];
                        dicec[k] = temp;
                    }
                }
            }

            JokerDiceEnum[] dicesc = new JokerDiceEnum[5];
            dicesc[0] = (JokerDiceEnum)dicec[0];
            dicesc[1] = (JokerDiceEnum)dicec[1];
            dicesc[2] = (JokerDiceEnum)dicec[2];
            dicesc[3] = (JokerDiceEnum)dicec[3];
            dicesc[4] = (JokerDiceEnum)dicec[4];
            ImageC1.AlternateText = dicesc[0].ToString();
            ImageC1.ImageUrl = dicesc[0].ToImgUrl();
            ImageC2.AlternateText = dicesc[1].ToString();
            ImageC2.ImageUrl = dicesc[1].ToImgUrl();
            ImageC3.AlternateText = dicesc[2].ToString();
            ImageC3.ImageUrl = dicesc[2].ToImgUrl();
            ImageC4.AlternateText = dicesc[3].ToString();
            ImageC4.ImageUrl = dicesc[3].ToImgUrl();
            ImageC5.AlternateText = dicesc[4].ToString();
            ImageC5.ImageUrl = dicesc[4].ToImgUrl();

            if (GameRound % 2 == 0)
            {
                DisableCheckBoxesPenImageButtons(sender, e);
                ImageP1.BorderStyle = BorderStyle.Dashed;
                ImageP2.BorderStyle = BorderStyle.Dashed;
                ImageP3.BorderStyle = BorderStyle.Dashed;
                ImageP4.BorderStyle = BorderStyle.Dashed;
                ImageP5.BorderStyle = BorderStyle.Dashed;
                this.Literal_Action.Text = "Select dices you want to keep<br /> and roll the others again.";
            }
            if (GameRound % 2 == 1)
            {
                ImageC1.BorderStyle = BorderStyle.Solid;
                ImageC2.BorderStyle = BorderStyle.Solid;
                ImageC3.BorderStyle = BorderStyle.Solid;
                ImageC4.BorderStyle = BorderStyle.Solid;
                ImageC5.BorderStyle = BorderStyle.Solid;
                bool checkBoxesShown = ShowCheckBoxes(dices);
                if (!checkBoxesShown)
                    ShowStrikeThroughPens();
            }
            CheckComputerWin(sender, e);
            
        }


        public void PokerCheckBox_Changed(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                string pokerScore = checkBox.ID.ToString().Replace("CheckBox", "");
                Enum.TryParse<JokerDiceScore>(pokerScore, out JokerDiceScore score);
                checkBox.Checked = true;
                DisableCheckBoxesPenImageButtons(sender, e);
                SpanScoreCheck.Visible = true;
                PlayerScore = PlayerScore + (int)score;
                SpanScore.InnerText = PlayerScore.ToString();
                DiceCupClickable = true;
                CheckWin(sender, e);
            }
        }
        
        protected void ImageButton_Pencil_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (sender is ImageButton imgButton)
            {
                dices = (Session["JokerDices"] != null) ? (JokerDiceEnum[])Session["JokerDices"] : new JokerDiceEnum[5];
                bool hasCheckBoxes = ShowCheckBoxes(dices);
                DiceCupClickable = true;
                switch (imgButton.ID.ToString())
                {
                    case "ImageButtonGrande":
                        if (!CheckBoxGrande.Enabled)
                        {
                            if (ImageButtonGrande.ImageUrl.EndsWith(".gif"))
                            {
                                this.SpanGrande.Visible = false;
                                this.SpanGrandeStrikeThrough.Visible = true;
                            }
                            CheckBoxGrande.Checked = true;
                        }
                        else if (!CheckBoxGrande.Checked)
                            PokerCheckBox_Changed(CheckBoxGrande, e);
                        break;
                    case "ImageButtonPoker":
                        if (!CheckBoxPoker.Enabled)
                        {
                            if (ImageButtonPoker.ImageUrl.EndsWith(".gif"))
                            {
                                this.SpanPoker.Visible = false;
                                this.SpanPokerStrikeThrough.Visible = true;
                            }
                            CheckBoxPoker.Checked = true;
                        }
                        else if (!CheckBoxPoker.Checked)
                            PokerCheckBox_Changed(CheckBoxPoker, e);
                        break;
                    case "ImageButtonFullHouse":
                        if (!CheckBoxFullHouse.Enabled)
                        {
                            if (ImageButtonFullHouse.ImageUrl.EndsWith(".gif"))
                            {
                                this.SpanFullHouse.Visible = false;
                                this.SpanFullHouseStrikeThrough.Visible = true;
                            }                                
                            CheckBoxFullHouse.Checked = true;
                        }
                        else if (!CheckBoxFullHouse.Checked)
                            PokerCheckBox_Changed(CheckBoxFullHouse, e);
                        break;
                    case "ImageButtonStraight":
                        if (!CheckBoxStraight.Enabled)
                        {
                            if (ImageButtonStraight.ImageUrl.EndsWith(".gif"))
                            {
                                this.SpanStraight.Visible = false;
                                this.SpanStraightStrikeThrough.Visible = true;
                            }
                            CheckBoxStraight.Checked = true;
                        }
                        else if (!CheckBoxStraight.Checked)
                            PokerCheckBox_Changed(CheckBoxStraight, e);
                        break;
                    case "ImageButtonTriple":
                        if (!CheckBoxTriple.Enabled)
                        {
                            if (ImageButtonTriple.ImageUrl.EndsWith(".gif"))
                            {
                                this.SpanTriple.Visible = false;
                                this.SpanTripleStrikeThrough.Visible = true;
                            }                                
                            CheckBoxTriple.Checked = true;
                        }
                        else if (!CheckBoxTriple.Checked)
                            PokerCheckBox_Changed(CheckBoxTriple, e);
                        break;
                    case "ImageButtonTwoPairs":
                        if (!CheckBoxTwoPairs.Enabled)
                        {
                            if (ImageButtonTwoPairs.ImageUrl.EndsWith(".gif"))
                            {
                                this.SpanTwoPairs.Visible = false;  
                                this.SpanTwoPairsStrikeThrough.Visible = true;
                            };
                            CheckBoxTwoPairs.Checked = true;
                        } else if (!CheckBoxTwoPairs.Checked)
                            PokerCheckBox_Changed(CheckBoxTwoPairs, e);
                        break;
                    case "ImageButtonPair":
                        if (!CheckBoxPair.Enabled)
                        {
                            if (ImageButtonPair.ImageUrl.EndsWith(".gif"))
                            {
                                this.SpanPair.Visible = false;  
                                this.SpanPairStrikeThrough.Visible = true;
                            }                                
                            CheckBoxPair.Checked = true;
                        }
                        else if (!CheckBoxPair.Checked)
                            PokerCheckBox_Changed(CheckBoxPair, e);
                        break;
                    case "ImageButtonBust":
                        if (!CheckBoxBust.Enabled)
                        {
                            CheckBoxBust.Checked = true;
                        }
                        else if (!CheckBoxBust.Checked) 
                            PokerCheckBox_Changed(CheckBoxBust, e);
                        break;
                    default: break;
                }
                DisableCheckBoxesPenImageButtons(sender, e);
                CheckWin(sender, e);
            }
        }


        public void CheckWin(object sender, EventArgs e)
        {
            if (CheckBoxGrande.Checked && CheckBoxPoker.Checked && CheckBoxFullHouse.Checked && CheckBoxStraight.Checked &&
                CheckBoxTriple.Checked && CheckBoxTwoPairs.Checked && CheckBoxPair.Checked && CheckBoxBust.Checked)
            {
                this.Literal_End = new Literal { Text = "<h2>Congratulations! You have completed the game!</h2>" };

                DiceCupClickable = false; 
                PokerDiceImage.Visible = true;                
            }
        }

        public void CheckComputerWin(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ShowCheckBoxes shows all scoring checkboxes, which can be selected from current dice result
        /// </summary>
        /// <param name="myDices">2nd round result of 5 dices</param>
        /// <returns>true, if any scoring checkboxes could be selected</returns>

        public bool ShowCheckBoxes(JokerDiceEnum[] myDices)
        {
            if (myDices == null || myDices.Length == 0 || myDices.Length < 5)
                return false;
            
            bool shouldReturn = false;
            DisableCheckBoxesPenImageButtons("ShowCheckBoxes", EventArgs.Empty);
            Dictionary<JokerDiceEnum, int> resultDict = new Dictionary<JokerDiceEnum, int>();
            
            for (int i = 0; i < myDices.Length; i++)
            {
                if (resultDict.ContainsKey(myDices[i]))
                    resultDict[myDices[i]]++;
                else
                    resultDict.Add(myDices[i], 1);
            }
            if (resultDict.Count == 1) // Grande
            {
                if (!CheckBoxGrande.Checked)
                {
                    CheckBoxGrande.Enabled = true;
                    ImageButtonGrande.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                    ImageButtonGrande.Enabled = true;
                    shouldReturn = true;
                }
                if (!CheckBoxPoker.Checked)
                {
                    CheckBoxPoker.Enabled = true;
                    ImageButtonPoker.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                    ImageButtonPoker.Enabled = true;
                    shouldReturn = true;
                }
                if (!CheckBoxTriple.Checked)
                {
                    CheckBoxTriple.Enabled = true;
                    ImageButtonTriple.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                    ImageButtonTriple.Enabled = true;
                    shouldReturn = true;
                }
                if (!CheckBoxPair.Checked)
                {
                    CheckBoxPair.Enabled = true;
                    ImageButtonPair.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                    ImageButtonPair.Enabled = true;
                    shouldReturn = true;
                }
                if (!CheckBoxBust.Checked)
                {
                    CheckBoxBust.Enabled = true;
                    ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                    ImageButtonBust.Enabled = true;
                    shouldReturn = true;
                }
                if (shouldReturn) 
                    return shouldReturn;
            }
            else if (resultDict.Count == 2) // Poker or Full House
            {
                foreach (var item in resultDict)
                {
                    if (item.Value == 4) // Poker
                    {
                        if (!CheckBoxPoker.Checked)
                        {
                            CheckBoxPoker.Enabled = true;
                            ImageButtonPoker.Enabled = true;
                            ImageButtonPoker.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (!CheckBoxTriple.Checked)
                        {
                            CheckBoxTriple.Enabled = true;
                            ImageButtonTriple.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            ImageButtonTriple.Enabled = true;
                            shouldReturn = true;
                        }
                        if (!CheckBoxPair.Checked)
                        {
                            CheckBoxPair.Enabled = true;
                            ImageButtonPair.Enabled = true;
                            ImageButtonPair.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (!CheckBoxBust.Checked)
                        {
                            CheckBoxBust.Enabled = true;
                            ImageButtonBust.Enabled = true;
                            ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (shouldReturn) return shouldReturn;
                    }
                    else if (item.Value == 3) // Full House
                    {
                        if (!CheckBoxFullHouse.Checked)
                        {
                            CheckBoxFullHouse.Enabled = true;
                            ImageButtonFullHouse.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            ImageButtonFullHouse.Enabled = true;
                            shouldReturn = true;
                        }
                        if (!CheckBoxTriple.Checked)
                        {
                            CheckBoxTriple.Enabled = true;
                            ImageButtonTriple.Enabled = true;
                            ImageButtonTriple.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (!CheckBoxTwoPairs.Checked)
                        {
                            CheckBoxTwoPairs.Enabled = true;
                            ImageButtonTwoPairs.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            ImageButtonTwoPairs.Enabled = true;
                            shouldReturn = true;
                        }
                        if (!CheckBoxPair.Checked)
                        {
                            CheckBoxPair.Enabled = true;
                            ImageButtonPair.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            ImageButtonPair.Enabled = true;
                            shouldReturn = true;
                        }
                        if (!CheckBoxBust.Checked)
                        {
                            CheckBoxBust.Enabled = true;
                            ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            ImageButtonBust.Enabled = true;
                            shouldReturn = true;
                        }
                        if (shouldReturn) return shouldReturn;
                    }
                }
            }
            else if (resultDict.Count == 3) // Triple or Two Pair
            {
                foreach (var item in resultDict)
                {
                    if (item.Value == 3) // Triple
                    {
                        if (!CheckBoxTriple.Checked)
                        {
                            CheckBoxTriple.Enabled = true;
                            ImageButtonTriple.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            ImageButtonTriple.Enabled = true;
                            shouldReturn = true;
                        }
                        if (!CheckBoxPair.Checked)
                        {
                            CheckBoxPair.Enabled = true;
                            ImageButtonPair.Enabled = true;
                            ImageButtonPair.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (!CheckBoxBust.Checked)
                        {
                            CheckBoxBust.Enabled = true;
                            ImageButtonBust.Enabled = true;
                            ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (shouldReturn) return shouldReturn;
                    }
                    if (item.Value == 2) // Two Pair
                    {                        
                        if (!CheckBoxTwoPairs.Checked)
                        {
                            CheckBoxTwoPairs.Enabled = true;
                            ImageButtonTwoPairs.Enabled = true;
                            ImageButtonTwoPairs.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (!CheckBoxPair.Checked)
                        {
                            CheckBoxPair.Enabled = true;
                            ImageButtonPair.Enabled = true;
                            ImageButtonPair.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (!CheckBoxBust.Checked)
                        {
                            CheckBoxBust.Enabled = true;
                            ImageButtonBust.Enabled = true;
                            ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (shouldReturn) return shouldReturn;
                    }
                }
                
            }
            else if (resultDict.Count == 4) // One Pair
            {
                if (!CheckBoxPair.Checked)
                {
                    CheckBoxPair.Enabled = true;
                    ImageButtonPair.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                    ImageButtonPair.Enabled = true;
                    shouldReturn = true;
                }
                if (!CheckBoxBust.Checked)
                {
                    CheckBoxBust.Enabled = true;
                    ImageButtonBust.Enabled = true;
                    ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                    shouldReturn = true;
                }
                if (shouldReturn) return shouldReturn;
            }
            else if (resultDict.Count == 5) // Bust
            {
                for (int k = 0; k < myDices.Length - 1; k++)
                {
                    if ((int)myDices[k] + 1 != (int)myDices[k + 1])
                        break;
                    if (k == myDices.Length - 2) // Straight
                    {
                        if (!CheckBoxStraight.Checked)
                        {
                            CheckBoxStraight.Enabled = true;
                            ImageButtonStraight.Enabled = true;
                            ImageButtonStraight.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (!CheckBoxBust.Checked)
                        {
                            CheckBoxBust.Enabled = true;
                            ImageButtonBust.Enabled = true;
                            ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                            shouldReturn = true;
                        }
                        if (shouldReturn) return shouldReturn;
                    }
                }
                if (!CheckBoxBust.Checked)
                {
                    CheckBoxBust.Enabled = true;
                    ImageButtonBust.Enabled = true;
                    ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_olive.gif";
                    shouldReturn = true;
                }
            }

            return shouldReturn;
        }


        /// <summary>
        /// ShowStrikeThroughPens, shows alle elements, that can strike through
        /// </summary>
        /// <returns>true, if any elements fount to strike through</returns>
        public bool ShowStrikeThroughPens()
        {
            bool canStrikeThrough = false;
            if (!CheckBoxGrande.Checked)
            {
                ImageButtonGrande.Enabled = true;                
                ImageButtonGrande.ImageUrl = "../res/gamez/dicepoker/pencil_blue.gif";
                canStrikeThrough = true;
            }
            else ImageButtonGrande.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            if (!CheckBoxPoker.Checked)
            {
                ImageButtonPoker.Enabled = true;
                ImageButtonPoker.ImageUrl = "../res/gamez/dicepoker/pencil_blue.gif";
                canStrikeThrough = true;
            }
            else ImageButtonPoker.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            if (!CheckBoxFullHouse.Checked)
            {
                ImageButtonFullHouse.Enabled = true;
                ImageButtonFullHouse.ImageUrl = "../res/gamez/dicepoker/pencil_blue.gif";
                canStrikeThrough = true;
            }
            else ImageButtonFullHouse.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            if (!CheckBoxStraight.Checked)
            {
                ImageButtonStraight.Enabled = true;
                ImageButtonStraight.ImageUrl = "../res/gamez/dicepoker/pencil_blue.gif";
                canStrikeThrough = true;
            }
            else ImageButtonStraight.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            if (!CheckBoxTwoPairs.Checked)
            {
                ImageButtonTwoPairs.Enabled = true;
                ImageButtonTwoPairs.ImageUrl = "../res/gamez/dicepoker/pencil_blue.gif";
                canStrikeThrough = true;
            }
            else ImageButtonTwoPairs.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            if (!CheckBoxTriple.Checked) 
            {
                ImageButtonTriple.Enabled = true;
                ImageButtonTriple.ImageUrl = "../res/gamez/dicepoker/pencil_blue.gif";
                canStrikeThrough = true;
            }
            else ImageButtonTriple.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            if (!CheckBoxPair.Checked)
            {
                ImageButtonPair.Enabled = true;
                ImageButtonPair.ImageUrl = "../res/gamez/dicepoker/pencil_blue.gif";
                canStrikeThrough = true;
            }
            else ImageButtonPair.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            if (!CheckBoxBust.Checked)
            {
                ImageButtonBust.Enabled = true;
                ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_blue.gif";
                canStrikeThrough = true;
            }
            else ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";

            return canStrikeThrough;
            
        }

        public void PrepareCheckBoxes(object sender, EventArgs e)
        {
            if (!CheckBoxGrande.Checked && !CheckBoxGrande.Enabled)
            {
                ImageButtonGrande.Enabled = true;
                ImageButtonGrande.ImageUrl = "../res/gamez/dicepoker/pencil_red.gif";
            }
            if (!CheckBoxPoker.Checked && !CheckBoxPoker.Enabled)
            {
                ImageButtonPoker.Enabled = true;
                ImageButtonPoker.ImageUrl = "../res/gamez/dicepoker/pencil_red.gif";
            }
            if (!CheckBoxFullHouse.Checked && !CheckBoxFullHouse.Enabled)
            {
                ImageButtonFullHouse.Enabled = true;
                ImageButtonFullHouse.ImageUrl = "../res/gamez/dicepoker/pencil_red.gif";
            }
            if (!CheckBoxStraight.Checked && !CheckBoxStraight.Enabled)
            {
                ImageButtonStraight.Enabled = true;
                ImageButtonStraight.ImageUrl = "../res/gamez/dicepoker/pencil_red.gif";
            }
            if (!CheckBoxTriple.Checked && !CheckBoxTriple.Enabled)
            {
                ImageButtonStraight.Enabled = true;
                ImageButtonStraight.ImageUrl = "../res/gamez/dicepoker/pencil_red.gif";
            }
                
            CheckBoxPoker.Checked = false;
            CheckBoxFullHouse.Checked = false;
            CheckBoxTwoPairs.Checked = false;
            CheckBoxTriple.Checked = false;
            CheckBoxPair.Checked = false;
            CheckBoxStraight.Checked = false;
            CheckBoxBust.Checked = false;
            ImageP1.BorderStyle = BorderStyle.None;
            ImageP2.BorderStyle = BorderStyle.None;
            ImageP3.BorderStyle = BorderStyle.None;
            ImageP4.BorderStyle = BorderStyle.None;
            ImageP5.BorderStyle = BorderStyle.None;
            Session["Round"] = 0;
            this.Literal_Action.Text = "Click on the cup to roll the dices.";
        }



        public void PokerDiceImage_Click(object sender, EventArgs e)
        {
            ResetCheckBoxes(sender, e);
            DiceCupClickable = true;
            Session["PlayerScore"] = 0;            
            SpanScore.InnerText = "0";
            ImageP1.AlternateText = "";
            ImageP1.ImageUrl = "../res/gamez/dicepoker/JollyRoger.gif"; 
            ImageP2.AlternateText = "";
            ImageP2.ImageUrl = "../res/gamez/dicepoker/JollyRoger.gif";
            ImageP3.AlternateText = "";
            ImageP3.ImageUrl = "../res/gamez/dicepoker/JollyRoger.gif";
            ImageP4.AlternateText = "";
            ImageP4.ImageUrl = "../res/gamez/dicepoker/JollyRoger.gif";
            ImageP5.AlternateText = "";
            ImageP5.ImageUrl = "../res/gamez/dicepoker/JollyRoger.gif";
            PokerDiceImage.Visible = false;
        }


        public void DisableCheckBoxesPenImageButtons(object sender, EventArgs e)
        {
            CheckBoxGrande.Enabled = false;
            ImageButtonGrande.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            ImageButtonGrande.Enabled = false;

            CheckBoxPoker.Enabled = false;
            ImageButtonPoker.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            ImageButtonPoker.Enabled = false;

            CheckBoxFullHouse.Enabled = false;
            ImageButtonFullHouse.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            ImageButtonFullHouse.Enabled = false;

            CheckBoxStraight.Enabled = false;
            ImageButtonStraight.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            ImageButtonStraight.Enabled = false;

            CheckBoxTriple.Enabled = false;
            ImageButtonTriple.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            ImageButtonTriple.Enabled = false;

            CheckBoxTwoPairs.Enabled = false;
            ImageButtonTwoPairs.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            ImageButtonTwoPairs.Enabled = false;

            CheckBoxPair.Enabled = false;
            ImageButtonPair.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            ImageButtonPair.Enabled = false;

            CheckBoxBust.Enabled = false;
            ImageButtonBust.ImageUrl = "../res/gamez/dicepoker/pencil_transparent.gif";
            ImageButtonBust.Enabled = false;
        }

        public void ResetCheckBoxes(object sender, EventArgs e)
        {
            DisableCheckBoxesPenImageButtons(sender, e);
            
            this.SpanGrande.InnerHtml = "Grande";
            this.SpanPoker.InnerHtml = "Poker";
            this.SpanFullHouse.InnerHtml = "Full House";
            this.SpanStraight.InnerHtml = "Straight";
            this.SpanTriple.InnerHtml = "Triple";
            this.SpanTwoPairs.InnerHtml = "2 Pairs";
            this.SpanPair.InnerHtml = "Pair";
            this.TableCellBust.Text = "Bust";

            this.SpanGrande.Visible = true;
            this.SpanPoker.Visible = true;
            this.SpanFullHouse.Visible = true;
            this.SpanStraight.Visible = true;
            this.SpanTriple.Visible = true;
            this.SpanTwoPairs.Visible = true;
            this.SpanPair.Visible = true;

            this.SpanGrandeStrikeThrough.Visible = false;
            this.SpanPokerStrikeThrough.Visible = false;
            this.SpanFullHouseStrikeThrough.Visible = false;
            this.SpanStraightStrikeThrough.Visible = false;
            this.SpanTripleStrikeThrough.Visible = false;
            this.SpanTwoPairsStrikeThrough.Visible = false;
            this.SpanPairStrikeThrough.Visible = false;
            CheckBoxGrande.Checked = false;
            CheckBoxPoker.Checked = false;
            CheckBoxFullHouse.Checked = false;
            CheckBoxTwoPairs.Checked = false;
            CheckBoxTriple.Checked = false;
            CheckBoxPair.Checked = false;
            CheckBoxStraight.Checked = false;
            CheckBoxBust.Checked = false;
            ImageP1.BorderStyle = BorderStyle.None;
            ImageP2.BorderStyle = BorderStyle.None;
            ImageP3.BorderStyle = BorderStyle.None;
            ImageP4.BorderStyle = BorderStyle.None;
            ImageP5.BorderStyle = BorderStyle.None;
            // GameRound = (GameRound % 2 == 0) ? GameRound + 2 : GameRound + 1;
            this.Literal_Action.Text = "Click on the cup to roll the dices.";
        }

    }

}
