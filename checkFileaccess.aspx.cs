using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Plan.Plandokument
{
    public partial class checkFileaccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rote = ConfigurationManager.AppSettings["testFileAccess"].ToString();

            DirectoryInfo di = new DirectoryInfo(Server.MapPath(rote));

            List<FileInfo> files = di.EnumerateFiles().ToList();

            if (files != null)
            {
                Label fileNames = new Label();

                foreach (FileInfo fi in files)
                {
                    fileNames.Text += "<br />" + fi.Name;
                }

                form1.Controls.Add(fileNames);
            }
        }
    }
}