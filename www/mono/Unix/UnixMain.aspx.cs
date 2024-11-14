using Area23.At.Mono.Unix;
using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Unix
{
    public partial class UnixMain : System.Web.UI.Page
    {
        static object fortuneLock;

        public UnixMain()
        {
            fortuneLock = new object();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetDigiTime();
        }

        
        protected void SetDigiTime()
        {
            lock (fortuneLock)
            {
                int seconds = DateTime.Now.Second;
                string digiSeconds = (seconds < 10) ? "0" + seconds : seconds.ToString();
                int minutes = DateTime.Now.Minute;
                string digiMinutes = (minutes < 10) ? "0" + minutes : minutes.ToString();
                int hours = DateTime.Now.Hour;
                string digiHours = (hours < 10) ? "0" + hours : hours.ToString();

                spanSecondsId.InnerText = digiSeconds;
                spanMinutesId.InnerText = digiMinutes;
                spanHoursId.InnerText = digiHours;
            }
        }

       
    }
}