using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System;

namespace Plan.Plandokument
{
    public static class JSONHelpers
    {
        /// <summary>
        /// Konverterar datatabell till lista av dictionary.
        /// (Anledningen är avsaknad av stöd för serialisering av datatabell till JSON i .NET)
        /// </summary>
        /// <param name="dataTable">
        /// Datatabell enligt .Net DataTable.
        /// </param>
        /// <returns>
        /// JSON-serialiserad string.
        /// </returns>
        internal static string getObjectAsJson(DataTable dataTable)
        {
            List<Dictionary<string, Object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, Object> row = new Dictionary<string, object>();

            foreach (DataRow dr in dataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(rows);
        }

        internal static string getObjectAsJson(List<Documenttype> list)
        {
            List<Dictionary<string, Object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, Object> row = new Dictionary<string, object>();

            foreach (var dt in list)
            {
                row = new Dictionary<string, object>();
                row.Add("Type", dt.Type);
                row.Add("UrlFilter", dt.UrlFilter);
                row.Add("Suffix", dt.Suffix);
                row.Add("Description", dt.Description);
                row.Add("IsPlanhandling", dt.IsPlanhandling);
                rows.Add(row);
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(rows);
        }

    }
}