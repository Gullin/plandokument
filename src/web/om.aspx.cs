using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
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


            PropertyInfo[] propInfos = typeof(Documenttype).GetProperties();
            columnsDocumenttypes.InnerHtml = string.Empty;
            List<string> rowspec = new List<string>();
            int column = 0;
            foreach (PropertyInfo propInfo in propInfos)
            {
                columnsDocumenttypes.InnerHtml += "<li>" + propInfo.GetCustomAttribute<DescriptionAttribute>().Description + "</li>";
                column++;
                rowspec.Add("kolumn " + column.ToString());
            }


            nbrColumnDocumenttypes.Text = propInfos.Length.ToString();


            rowspecDocumenttypes.Text = "[" + String.Join("];[", rowspec.ToArray()) + "]";
            

            List<Documenttype> listDocumenttyper = PlanCache.GetPlandocumenttypesCache();
            nbrDocumenttypes.Text = listDocumenttyper.Where(s => s.Type.Length != 0).Count().ToString();


            StringBuilder tblDocumenttypes = new StringBuilder();
            tblDocumenttypes.Append("<table>");
            foreach (var itemDC in listDocumenttyper)
            {
                tblDocumenttypes.Append("<tr>");
                foreach (var item in itemDC.GetType().GetProperties())
                {
                    tblDocumenttypes.Append("<td>" + item.GetValue(itemDC).ToString() + "</td>");
                }
                tblDocumenttypes.Append("</tr>");
            }
            tblDocumenttypes.Append("</table>");
            tableDocumenttypes.Text = tblDocumenttypes.ToString();

        }
    }
}