using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.UI.WebControls;

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

    public static class JokerDiceExtensions
    {
        public static string ToImgUrl(this JokerDiceEnum value)
        {
            switch (value)
            {
                case JokerDiceEnum.Joker:
                    return "../res/img/symbol/Joker.png";
                case JokerDiceEnum.Ace:
                    return "../res/img/symbol/Ace.png";
                case JokerDiceEnum.King:
                    return "../res/img/symbol/King.png";
                case JokerDiceEnum.Queen:
                    return "../res/img/symbol/Queen.png";
                case JokerDiceEnum.Jack:
                    return "../res/img/symbol/Jack.png";
                case JokerDiceEnum.Ten:
                    return "../res/img/symbol/Ten.png";
                default:
                    return value.ToString();
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
        JokerDiceEnum[] dices = new JokerDiceEnum[5];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Session["Round"] = 0;
        }


        protected void ImageP_Click(object sender, EventArgs e)
        {
            if (Session["Round"] != null && (int)Session["Round"] % 2 == 0)
                return;

            if (sender is ImageButton im)
            {
                if (im.BorderStyle == BorderStyle.Solid)
                    im.BorderStyle = BorderStyle.Dashed;
                else 
                    im.BorderStyle = BorderStyle.Solid;
            }

        }

        protected void ImageButton_DiceCup_Click(object sender, EventArgs e)
        {
            int round = (Session["Round"] != null) ? (int)Session["Round"] : 0;
            if (round % 2 == 0)
            {
                ImageP1.BorderStyle = BorderStyle.None;
                ImageP2.BorderStyle = BorderStyle.None;
                ImageP3.BorderStyle = BorderStyle.None;
                ImageP4.BorderStyle = BorderStyle.None;
                ImageP5.BorderStyle = BorderStyle.None;
            }

            Random rnd = new Random(DateTime.Now.Second);
            int[] dice = new int[5];
            if (ImageP1.BorderStyle != BorderStyle.Solid)
                dice[0] = rnd.Next(0, 6);
            else
                dice[0] = (int)ImageP1.AlternateText.ToJokerDiceEnum();
            if (ImageP2.BorderStyle != BorderStyle.Solid)
                dice[1] = rnd.Next(0, 6);
            else
                dice[1] = (int)ImageP2.AlternateText.ToJokerDiceEnum();
            if (ImageP3.BorderStyle != BorderStyle.Solid)
                dice[2] = rnd.Next(0, 6);
            else
                dice[2] = (int)ImageP3.AlternateText.ToJokerDiceEnum();
            if (ImageP4.BorderStyle != BorderStyle.Solid)
                dice[3] = rnd.Next(0, 6);
            else
                dice[3] = (int)ImageP4.AlternateText.ToJokerDiceEnum();
            if (ImageP5.BorderStyle != BorderStyle.Solid)
                dice[4] = rnd.Next(0, 6);
            else
                dice[4] = (int)ImageP5.AlternateText.ToJokerDiceEnum();
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

            if (round % 2 == 0)
            {
                DisableCheckBoxes(sender, e);
                ImageP1.BorderStyle = BorderStyle.Dashed;
                ImageP2.BorderStyle = BorderStyle.Dashed;
                ImageP3.BorderStyle = BorderStyle.Dashed;
                ImageP4.BorderStyle = BorderStyle.Dashed;
                ImageP5.BorderStyle = BorderStyle.Dashed;
                this.Literal_Action.Text = "Select dices you want to keep<br /> and roll the others again.";
            }
            if (round % 2 == 1)
            {
                ImageP1.BorderStyle = BorderStyle.Solid;
                ImageP2.BorderStyle = BorderStyle.Solid;
                ImageP3.BorderStyle = BorderStyle.Solid;
                ImageP4.BorderStyle = BorderStyle.Solid;
                ImageP5.BorderStyle = BorderStyle.Solid;
                ShowCheckBoxes(dices);
                this.Literal_Action.Text = "Select your best combination.";
            }
            Computer_DiceCup(sender, e);
            CheckWin(sender, e);
            Session["Round"] = ++round;
        }


        protected void Computer_DiceCup(object sender, EventArgs e)
        {
            int round = (Session["Round"] != null) ? (int)Session["Round"] : 0;
            if (round % 2 == 0)
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

            if (round % 2 == 0)
            {
                DisableCheckBoxes(sender, e);
                ImageP1.BorderStyle = BorderStyle.Dashed;
                ImageP2.BorderStyle = BorderStyle.Dashed;
                ImageP3.BorderStyle = BorderStyle.Dashed;
                ImageP4.BorderStyle = BorderStyle.Dashed;
                ImageP5.BorderStyle = BorderStyle.Dashed;
                this.Literal_Action.Text = "Select dices you want to keep<br /> and roll the others again.";
            }
            if (round % 2 == 1)
            {
                ImageC1.BorderStyle = BorderStyle.Solid;
                ImageC2.BorderStyle = BorderStyle.Solid;
                ImageC3.BorderStyle = BorderStyle.Solid;
                ImageC4.BorderStyle = BorderStyle.Solid;
                ImageC5.BorderStyle = BorderStyle.Solid;
                ShowCheckBoxes(dices);
            }
            CheckComputerWin(sender, e);
            
        }


        public void PokerCheckBox_Changed(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                checkBox.Checked = true;
                DisableCheckBoxes(sender, e);
                CheckWin(sender, e);
            }
        }

        public void CheckWin(object sender, EventArgs e)
        {
            if (CheckBoxPoker.Checked && CheckBoxPoker.Checked && CheckBoxFullHouse.Checked && CheckBoxStraight.Checked &&
                CheckBoxTriple.Checked && CheckBoxTwoPairs.Checked && CheckBoxPair.Checked && CheckBoxBust.Checked)
            {
                this.Literal_End = new Literal { Text = "<h2>Congratulations! You have completed the game!</h2>" };
                ResetCheckBoxes(sender, e);
            }
        }

        public void CheckComputerWin(object sender, EventArgs e)
        {

        }


        

        public void DisableCheckBoxes(object sender, EventArgs e)
        {
            CheckBoxGrande.Enabled = false;
            CheckBoxPoker.Enabled = false;
            CheckBoxFullHouse.Enabled = false;
            CheckBoxStraight.Enabled = false;
            CheckBoxTriple.Enabled = false;
            CheckBoxTwoPairs.Enabled = false;
            CheckBoxPair.Enabled = false;
            CheckBoxBust.Enabled = false;
        }

        public void ResetCheckBoxes(object sender, EventArgs e)
        {
            DisableCheckBoxes(sender, e);
            CheckBoxPoker.Checked = false;
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

        public void ShowCheckBoxes(JokerDiceEnum[] myDices)
        {
            if (myDices == null || myDices.Length == 0 || myDices.Length < 5)
                return;
            DisableCheckBoxes("ShowCheckBoxes", EventArgs.Empty);
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
                    CheckBoxGrande.Enabled = true;
                if (!CheckBoxPoker.Checked)
                    CheckBoxPoker.Enabled = true;
                if (!CheckBoxTriple.Checked)
                    CheckBoxTriple.Enabled = true;
                if (!CheckBoxPair.Checked)
                    CheckBoxPair.Enabled = true;
                if (!CheckBoxBust.Checked)
                    CheckBoxBust.Enabled = true;
                return;
            }
            else if (resultDict.Count == 2) // Poker or Full House
            {
                foreach (var item in resultDict)
                {
                    if (item.Value == 4) // Poker
                    {
                        if (!CheckBoxPoker.Checked)
                            CheckBoxPoker.Enabled = true;
                        if (!CheckBoxTriple.Checked)
                            CheckBoxTriple.Enabled = true;
                        if (!CheckBoxPair.Checked)
                            CheckBoxPair.Enabled = true;
                        if (!CheckBoxBust.Checked)
                            CheckBoxBust.Enabled = true;
                        return;
                    }
                    else if (item.Value == 3) // Full House
                    {
                        if (!CheckBoxFullHouse.Checked)
                            CheckBoxFullHouse.Enabled = true;
                        if (!CheckBoxTriple.Checked)
                            CheckBoxTriple.Enabled = true;
                        if (!CheckBoxPair.Checked)
                            CheckBoxPair.Enabled = true;
                        if (!CheckBoxBust.Checked)
                            CheckBoxBust.Enabled = true;
                        return;
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
                            CheckBoxTriple.Enabled = true;
                        if (!CheckBoxPair.Checked)
                            CheckBoxPair.Enabled = true;
                        if (!CheckBoxBust.Checked)
                            CheckBoxBust.Enabled = true;
                        return;
                    }
                }
                // Two Pair
                if (!CheckBoxTwoPairs.Checked)
                    CheckBoxTwoPairs.Enabled = true;
                if (!CheckBoxPair.Checked)
                    CheckBoxPair.Enabled = true;
                if (!CheckBoxBust.Checked)
                    CheckBoxBust.Enabled = true;
                return;
            }
            else if (resultDict.Count == 4) // One Pair
            {
                if (!CheckBoxPair.Checked)
                    CheckBoxPair.Enabled = true;
                if (!CheckBoxBust.Checked)
                    CheckBoxBust.Enabled = true;
                return;
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
                            CheckBoxStraight.Enabled = true;
                        if (!CheckBoxBust.Checked)
                            CheckBoxBust.Enabled = true;
                        return;
                    }
                }
                if (!CheckBoxBust.Checked)
                    CheckBoxBust.Enabled = true;
            }
        }
    }
}