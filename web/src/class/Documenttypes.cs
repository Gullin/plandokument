using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Plan.Plandokument
{

    public class Documenttypes
    {

        public List<Documenttype> GetDocumenttypes { get; private set; }

        public Documenttypes()
        {
            this.GetDocumenttypes = GetDocumenttypesFromFile();
        }

        private List<Documenttype> GetDocumenttypesFromFile()
        {
            List<Documenttype> documenttypes = new List<Documenttype>();

            //string domainFileDocumenttypes = HttpContext.Current.Server.MapPath(
            //        "~/dokumenttyper.csv"
            //        );
            string domainFileDocumenttypes = Path.Combine(HttpRuntime.AppDomainAppPath, "dokumenttyper.csv");
            try
            {
                if (File.Exists(domainFileDocumenttypes))
                {
                    string[] lines = File.ReadAllLines(domainFileDocumenttypes, System.Text.Encoding.GetEncoding("Windows-1252"));

                    //TODO: DOKUMENTTYP: Kontrollera att kolumner innehåller värde
                    List<int> felrader = new List<int>();
                    List<int> felraderLogiskDatatyp = new List<int>();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] lineParts = lines[i].Split(';');
                        if (lineParts.Length != 5)
                        {
                            felrader.Add(i);
                        }
                        else
                        {
                            bool _isPlanhandling;
                            if (!Boolean.TryParse(lineParts[4].Trim(), out _isPlanhandling))
                            {
                                felraderLogiskDatatyp.Add(i);
                            }
                            Documenttype documenttype = new Documenttype()
                            {
                                Type = lineParts[0].ToString().Trim(),
                                UrlFilter = lineParts[1].ToString().Trim(),
                                Suffix = lineParts[2].ToString().Trim(),
                                Description = lineParts[3].ToString().Trim(),
                                IsPlanhandling = _isPlanhandling
                            };
                            documenttypes.Add(documenttype);
                        }
                    }

                    if (felrader.Count > 0)
                    {
                        throw new Exception("Domänvärden dokumenttyp fel antal", new Exception("Antalet kolumner är fel för raderna (" + String.Join(", ", felrader) + ")."));
                    }
                    if (felraderLogiskDatatyp.Count > 0)
                    {
                        throw new Exception("Dokumenttyps domänvärde från 5:e kolumnen är inte av logiskt (boolean) datatyp.", new Exception("Kolumner är fel för raderna (" + String.Join(", ", felraderLogiskDatatyp) + ")."));
                    }

                }
                else
                {
                    throw new FileNotFoundException("Kunde ej hitta sökt fil", new Exception("Sökt fil " + domainFileDocumenttypes + " med domänvärde för dokumenttyper."));
                }
            }
            catch (Exception ex)
            {
                // Klassens namn för loggning
                string className = this.GetType().Name;
                // Metod i klassen som används
                string methodName = MethodBase.GetCurrentMethod().Name;

                UtilityException.LogException(ex, className + " : " + methodName, true);
            }

            return documenttypes;
        }
    }

    public class Documenttype
    {

        [Description("Dokumenttyp")]
        public string Type { get; set; }
        [Description("Filter för användning i URL för begränsning av dokumenttyper")]
        public string UrlFilter { get; set; }
        [Description("Ändelse i dokumentets filnamn som indikerar dokumenttypen")]
        public string Suffix { get; set; }
        [Description("Beskrivning som valfri förklarande text")]
        public string Description { get; set; }
        [Description("Signalerar om dokumenttypen är en planhandling eller inte. För värden som inte är en dokumenttyp ges värde false.")]
        public bool IsPlanhandling { get; set; }

    }
}