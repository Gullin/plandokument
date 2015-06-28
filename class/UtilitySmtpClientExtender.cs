using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Plan.Plandokument
{
    /// <summary>
    /// Utökar System.Net.Mail.SmtpClient med ett logiskt värde för om funktionen ska stängas av globalt.
    /// Frivilligt att kontrollera egenskapen.
    /// </summary>
	public class UtilitySmtpClientExtender : SmtpClient
	{

        private static bool isToBeSent = false;

        public UtilitySmtpClientExtender()
        {
            if (ConfigurationManager.AppSettings["isToBeSent"] != null)
            {
                isToBeSent = bool.TryParse(ConfigurationManager.AppSettings["isToBeSent"].ToString(), out isToBeSent) ? isToBeSent : false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value>
        /// Värde <c>IsToBeSent</c> representerar globalt logiskt värde för om e-post önskas skickas.
        /// </value>
        public bool IsToBeSent
        {
            get
            {
                return isToBeSent;
            }
            set
            {
                isToBeSent = value;
            }
        }
	}
}