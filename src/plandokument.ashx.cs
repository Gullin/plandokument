using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;


namespace Plan.Plandokument
{
    /// <summary>
    /// Summary description for plandokument
    /// </summary>
    public class WsAshxPlandokument : IHttpHandler
    {

        /// <summary>
        /// Erhåller en paketerad zip-fil binärt efter inskickade sökvägar och namnperfix
        /// </summary>
        /// <param name="context">JSON-teststräng med list-objekt av sökvägar till plandokument och sträng med prefix
        /// Exempel:
        /// {"planDocsPaths":["plandokument/geotiff/DP1282K-P13_1.tif","plandokument/pdf/DP1282K-P13_1.pdf"],"zipFileNamePart":"1282K-P13/1"}
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            var jsonSerializer = new JavaScriptSerializer();
            var jsonString = String.Empty;

            context.Request.InputStream.Position = 0;
            using (var inputStream = new StreamReader(context.Request.InputStream))
            {
                jsonString = inputStream.ReadToEnd();
            }

            var plansDocsList = jsonSerializer.Deserialize<PlansDocsPaths>(jsonString);

            PackagingZip zipFile = new PackagingZip();
            string zipFileNamePath = zipFile.zipFiles(plansDocsList.planDocsPaths, plansDocsList.zipFileNamePart);

            BinaryReader binReader = new BinaryReader(File.Open(context.Server.MapPath(zipFileNamePath), FileMode.Open, FileAccess.Read));
            binReader.BaseStream.Position = 0;
            byte[] binFile = binReader.ReadBytes(Convert.ToInt32(binReader.BaseStream.Length));
            binReader.Close();

            context.Response.Clear();
            context.Response.BufferOutput = false;
            context.Response.ContentType = "application/zip";
            context.Response.AddHeader("content-disposition", "attachment; filename=" + context.Request.ApplicationPath + zipFileNamePath);
            context.Response.BinaryWrite(binFile);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


    public class PlansDocsPaths
    {
        public List<object> planDocsPaths { get; set; }
        public string zipFileNamePart { get; set; }
    }


}