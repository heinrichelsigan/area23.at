using System;
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

            JokerDiceEnum[] dices = new JokerDiceEnum[5];
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
                ImageP1.BorderStyle = BorderStyle.Dashed;
                ImageP2.BorderStyle = BorderStyle.Dashed;
                ImageP3.BorderStyle = BorderStyle.Dashed;
                ImageP4.BorderStyle = BorderStyle.Dashed;
                ImageP5.BorderStyle = BorderStyle.Dashed;
            }
            if (round % 2 == 1)
            {
                ImageP1.BorderStyle = BorderStyle.Solid;
                ImageP2.BorderStyle = BorderStyle.Solid;
                ImageP3.BorderStyle = BorderStyle.Solid;
                ImageP4.BorderStyle = BorderStyle.Solid;
                ImageP5.BorderStyle = BorderStyle.Solid;
            }

            Session["Round"] = ++round;
        }   
    }
}