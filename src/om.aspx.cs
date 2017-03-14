using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Plan.Plandokument
{
    public partial class om : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string urlSplitter = ConfigurationManager.AppSettings["URLQueryStringSeparator"].ToString();
            string URLParcelBlockUnitSign = ConfigurationManager.AppSettings["URLParcelBlockUnitSign"].ToString();
            string UrlParameterSearchString = ConfigurationManager.AppSettings["UrlParameterSearchString"].ToString();
            string UrlParameterDocumentType = ConfigurationManager.AppSettings["UrlParameterDocumentType"].ToString();
            string UrlParameterSearchType = ConfigurationManager.AppSettings["UrlParameterSearchType"].ToString();
            lblPart2UrlSplitter.Text = urlSplitter;
            lblPart3UrlSplitter.Text = urlSplitter;
            lblPart4UrlSplitter.Text = urlSplitter;
            lblPart5UrlSplitter.Text = urlSplitter;
            ParcelBlockUnitSign.Text = URLParcelBlockUnitSign;
            UrlParameterSearchString1.Text = UrlParameterSearchString;
            UrlParameterDocumentType1.Text = UrlParameterDocumentType;
            UrlParameterSearchType1.Text = UrlParameterSearchType;
            UrlParameterSearchString2.Text = UrlParameterSearchString;
            UrlParameterDocumentType2.Text = UrlParameterDocumentType;
            UrlParameterSearchType2.Text = UrlParameterSearchType;
            UrlParameterSearchString3.Text = UrlParameterSearchString;
            UrlParameterDocumentType3.Text = UrlParameterDocumentType;
            UrlParameterSearchType3.Text = UrlParameterSearchType;
        }
    }
}