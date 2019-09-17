using System;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

namespace Plan.Plandokument.MapLayerDefinition
{
	public class DefineAreaLayer
	{
        /// <summary>
        /// 
        /// </summary>
        public string FeatureSourceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FeatureName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GeometryColumnName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private const string featureNameType = "FeatureClass";
        public string FeatureNameType
        {
            get { return featureNameType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LayerScaleRangeCollection LayerScaleRanges { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public XmlDocument LayerDefinition
        //{
        //    get { return CreateLayerDefinition(); }
        //}

        public XmlDocument CreateLayerDefinitionAsXmlDocument()
        {
            string fSN = this.FeatureSourceName;
            string fN = this.FeatureName;
            string gCN = this.GeometryColumnName;
            string fNT = this.FeatureNameType;
            string filter = this.Filter;
            string url = this.Url;
            string toolTip = this.ToolTip;
            LayerScaleRangeCollection sRs = this.LayerScaleRanges;

            XmlDocument layerDefinition = null;

            if (!string.IsNullOrWhiteSpace(fSN) || !string.IsNullOrWhiteSpace(fN) || !string.IsNullOrWhiteSpace(gCN))
            {
                layerDefinition = new XmlDocument();
                XmlDeclaration xmlDeclaration = layerDefinition.CreateXmlDeclaration("1.0", "UTF-8", null);

                layerDefinition.AppendChild(xmlDeclaration);
                XmlElement root = layerDefinition.CreateElement("LayerDefinition");
                root.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                root.SetAttribute("xsi:noNamespaceSchemaLocation", "C:\\Program Files\\OSGeo\\MapGuide\\Server\\Schema\\LayerDefinition-1.0.0.xsd");
                // fungerar ej, skriver inte ut xsi:
                //root.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "LayerDefinition-1.0.0.xsd");
                //XmlAttribute xmlAttribute = new XmlAttribute();
                //xmlAttribute = layerDefinition.CreateAttribute("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                //xmlAttribute.Value = "LayerDefinition-1.0.0.xsd";
                //root.SetAttributeNode(xmlAttribute);

                root.SetAttribute("version", "1.0.0");

                XmlElement featureRoot = layerDefinition.CreateElement("VectorLayerDefinition");
                XmlElement el = layerDefinition.CreateElement("ResourceId");
                el.InnerText = fSN;
                featureRoot.AppendChild(el);
                el = layerDefinition.CreateElement("FeatureName");
                el.InnerText = fN;
                featureRoot.AppendChild(el);
                el = layerDefinition.CreateElement("FeatureNameType");
                el.InnerText = fNT;
                featureRoot.AppendChild(el);
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    el = layerDefinition.CreateElement("Filter");
                    el.InnerText = filter;
                    featureRoot.AppendChild(el);
                }
                el = layerDefinition.CreateElement("Geometry");
                el.InnerText = gCN;
                featureRoot.AppendChild(el);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    el = layerDefinition.CreateElement("Url");
                    el.InnerText = url;
                    featureRoot.AppendChild(el);
                }
                if (!string.IsNullOrWhiteSpace(toolTip))
                {
                    el = layerDefinition.CreateElement("ToolTip");
                    el.InnerText = toolTip;
                    featureRoot.AppendChild(el);
                }

                if (sRs != null)
                {
                    XmlElement layerScaleRange = null;
                    foreach (LayerScaleRange lsr in sRs)
                    {
                        layerScaleRange = layerDefinition.CreateElement("VectorScaleRange");
                        string minScale = lsr.MinScale;
                        if (!string.IsNullOrWhiteSpace(minScale))
                        {
                            el = layerDefinition.CreateElement("MinScale");
                            el.InnerText = minScale;
                            layerScaleRange.AppendChild(el);
                        }
                        string maxScale = lsr.MaxScale;
                        if (!string.IsNullOrWhiteSpace(maxScale))
                        {
                            el = layerDefinition.CreateElement("MaxScale");
                            el.InnerText = maxScale;
                            layerScaleRange.AppendChild(el);
                        }
                        XmlElement xmlTypeStyle = layerDefinition.CreateElement("AreaTypeStyle");
                        AreaTypeStyle typeStyle = lsr.AreaTypeStyle;
                        XmlElement areaRule = null;
                        foreach (AreaRule ar in typeStyle.AreaRules)
                        {
                            areaRule = layerDefinition.CreateElement("AreaRule");
                            el = layerDefinition.CreateElement("LegendLabel");
                            el.InnerText = ar.LegendLabel;
                            areaRule.AppendChild(el);
                            string areaRuleFilter = ar.Filter;
                            if (!string.IsNullOrWhiteSpace(areaRuleFilter))
                            {
                                el = layerDefinition.CreateElement("Filter");
                                el.InnerText = areaRuleFilter;
                                areaRule.AppendChild(el);
                            }
                            string areaRuleLabel = ar.Label;
                            if (!string.IsNullOrWhiteSpace(areaRuleLabel))
                            {
                                el = layerDefinition.CreateElement("Label");
                                el.InnerText = areaRuleLabel;
                                areaRule.AppendChild(el);
                            }

                            XmlElement xmlAreaSymb = layerDefinition.CreateElement("AreaSymbolization2D");

                            AreaSymbolization2D areaSymb = ar.Symbolization2D;
                            Fill fill = areaSymb.Fill;

                            XmlElement xmlFill = layerDefinition.CreateElement("Fill");
                            el = layerDefinition.CreateElement("FillPattern");
                            el.InnerText = fill.FillPattern;
                            xmlFill.AppendChild(el);
                            el = layerDefinition.CreateElement("ForegroundColor");
                            el.InnerText = fill.ForegroundColor;
                            xmlFill.AppendChild(el);
                            el = layerDefinition.CreateElement("BackgroundColor");
                            el.InnerText = fill.BackgroundColor;
                            xmlFill.AppendChild(el);

                            xmlAreaSymb.AppendChild(xmlFill);

                            Stroke stroke = areaSymb.Stroke;

                            XmlElement xmlStroke = layerDefinition.CreateElement("Stroke");
                            el = layerDefinition.CreateElement("LineStyle");
                            el.InnerText = stroke.LineStyle;
                            xmlStroke.AppendChild(el);
                            el = layerDefinition.CreateElement("Thickness");
                            el.InnerText = stroke.Thickness;
                            xmlStroke.AppendChild(el);
                            el = layerDefinition.CreateElement("Color");
                            el.InnerText = stroke.Color;
                            xmlStroke.AppendChild(el);
                            el = layerDefinition.CreateElement("Unit");
                            el.InnerText = stroke.Unit;
                            xmlStroke.AppendChild(el);
                            //el = layerDefinition.CreateElement("SizeContext");
                            //el.InnerText = stroke.SizeContext;
                            //xmlStroke.AppendChild(el);

                            xmlAreaSymb.AppendChild(xmlStroke);

                            areaRule.AppendChild(xmlAreaSymb);

                            xmlTypeStyle.AppendChild(areaRule);
                        }
                        layerScaleRange.AppendChild(xmlTypeStyle);

                        featureRoot.AppendChild(layerScaleRange);
                    }
                }

                root.AppendChild(featureRoot);
                layerDefinition.AppendChild(root);
            }
            

            return layerDefinition;
        }

        /// <summary>
        /// Endast för test, sekvens/ordning av element är viktigt
        /// </summary>
        /// <returns></returns>
        public string CreateLayerDefinitionAsXmlString()
        {
            string xml = "<?xml version='1.0' encoding='UTF-8'?>";
            xml += "<LayerDefinition xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:noNamespaceSchemaLocation='LayerDefinition-1.0.0.xsd' version='1.0.0'>";
            xml += "<VectorLayerDefinition>";
            xml += "<ResourceId>" + this.FeatureSourceName + "</ResourceId>";
            xml += "<FeatureName>" + this.FeatureName + "</FeatureName>";
            xml += "<FeatureNameType>" + this.FeatureNameType + "</FeatureNameType>";
            if (!string.IsNullOrWhiteSpace(this.Filter))
            {
                xml += "<Filter>" + this.Filter + "</Filter>";
            }
            xml += "<Geometry>" + this.GeometryColumnName + "</Geometry>";
            if (!string.IsNullOrWhiteSpace(this.Url))
            {
                xml += "<Url>" + this.Url + "</Url>";
            }
            if (!string.IsNullOrWhiteSpace(this.ToolTip))
            {
                xml += "<ToolTip>" + this.ToolTip + "</ToolTip>";
            }
            LayerScaleRangeCollection layerScaleRangeCollection = this.LayerScaleRanges;
            if (layerScaleRangeCollection != null)
            {
                foreach (LayerScaleRange layerScaleRange in layerScaleRangeCollection)
                {
                    xml += "<VectorScaleRange>";
                    if (!string.IsNullOrWhiteSpace(layerScaleRange.MinScale))
                    {
                        xml += "<MinScale>" + layerScaleRange.MinScale + "</MinScale>";
                    }
                    if (!string.IsNullOrWhiteSpace(layerScaleRange.MaxScale))
                    {
                        xml += "<MaxScale>" + layerScaleRange.MaxScale + "</MaxScale>";
                    }
                    xml += "<AreaTypeStyle>";
                    AreaTypeStyle areaTypeStyle = layerScaleRange.AreaTypeStyle;
                    foreach (AreaRule areaRule in areaTypeStyle.AreaRules)
                    {
                        xml += "<AreaRule>";
                        xml += "<LegendLabel>" + areaRule.LegendLabel + "</LegendLabel>";
                        if (!string.IsNullOrWhiteSpace(areaRule.Filter))
                        {
                            xml += "<Filter>" + areaRule.Filter + "</Filter>";
                        }
                        if (!string.IsNullOrWhiteSpace(areaRule.Label))
                        {
                            xml += "<Label>" + areaRule.Label + "</Label>";
                        }
                        AreaSymbolization2D areaSymbolization2D = areaRule.Symbolization2D;
                        xml += "<AreaSymbolization2D>";
                        Fill fill = areaSymbolization2D.Fill;
                        xml += "<Fill>";
                        xml += "<FillPattern>" + fill.FillPattern + "</FillPattern>";
                        xml += "<ForegroundColor>" + fill.ForegroundColor + "</ForegroundColor>";
                        xml += "<BackgroundColor>" + fill.BackgroundColor + "</BackgroundColor>";
                        xml += "</Fill>";
                        Stroke stroke = areaSymbolization2D.Stroke;
                        xml += "<Stroke>";
                        xml += "<LineStyle>" + stroke.LineStyle + "</LineStyle>";
                        xml += "<Thickness>" + stroke.Thickness + "</Thickness>";
                        xml += "<Color>" + stroke.Color + "</Color>";
                        xml += "<Unit>" + stroke.Unit + "</Unit>";
                        xml += "</Stroke>";
                        xml += "</AreaSymbolization2D>";
                        xml += "</AreaRule>";
                    }
                    xml += "</AreaTypeStyle>";
                    xml += "</VectorScaleRange>";
                }
            }
            xml += "</VectorLayerDefinition>";
            xml += "</LayerDefinition>";

            return xml;
        }

        public XDocument CreateLayerDefinitionAsXDocument()
        {
            XNamespace ns1 = XNamespace.Get("http://www.w3.org/2001/XMLSchema");
            XNamespace ns2 = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            XNamespace ns3 = XNamespace.Get("LayerDefinition-1.0.0.xsd");
            string layerDefinitionVersion = "1.0.0";

            XDocument xmlDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement(ns1 +  "LayerDefinition",
                    new XAttribute(XNamespace.Xmlns + "xsi", ns2),
                    new XAttribute(ns2 + "noNamespaceSchemaLocation", ns3),
                    new XAttribute("version", layerDefinitionVersion)
                    //,
                    //new XElement("VectorLayerDefinition",""
                        //new XElement("ResourceId", this.FeatureSourceName),
                        //new XElement("FeatureName", this.FeatureName),
                        //new XElement("FeatureNameType", this.FeatureNameType)
                        //)
                    )
                );

            if (!string.IsNullOrWhiteSpace(this.FeatureSourceName) || !string.IsNullOrWhiteSpace(this.FeatureName) || !string.IsNullOrWhiteSpace(this.FeatureNameType))
            {
                XmlDocument xmlTempDoc = new XmlDocument();
                XmlElement xmlRoot = xmlTempDoc.CreateElement("VectorLayerDefinition", ns1.NamespaceName);
                XmlElement el = xmlTempDoc.CreateElement("ResourceId", ns1.NamespaceName);
                el.InnerText = this.FeatureSourceName;
                xmlRoot.AppendChild(el);
                el = xmlTempDoc.CreateElement("FeatureName", ns1.NamespaceName);
                el.InnerText = this.FeatureName;
                xmlRoot.AppendChild(el);
                el = xmlTempDoc.CreateElement("FeatureNameType", ns1.NamespaceName);
                el.InnerText = this.FeatureNameType;
                xmlRoot.AppendChild(el);
                if (!string.IsNullOrWhiteSpace(this.Filter))
                {
                    el = xmlTempDoc.CreateElement("Filter", ns1.NamespaceName);
                    el.InnerText = this.Filter;
                    xmlRoot.AppendChild(el);
                }
                el = xmlTempDoc.CreateElement("Geometry", ns1.NamespaceName);
                el.InnerText = this.GeometryColumnName;
                xmlRoot.AppendChild(el);
                if (!string.IsNullOrWhiteSpace(this.Url))
                {
                    el = xmlTempDoc.CreateElement("Url", ns1.NamespaceName);
                    el.InnerText = this.Url;
                    xmlRoot.AppendChild(el);
                }
                if (!string.IsNullOrWhiteSpace(this.ToolTip))
                {
                    el = xmlTempDoc.CreateElement("ToolTip", ns1.NamespaceName);
                    el.InnerText = this.ToolTip;
                    xmlRoot.AppendChild(el);
                }

                xmlTempDoc.AppendChild(xmlRoot);
                
                xmlDoc.Root.Add(XDocument.Load(new XmlNodeReader(xmlTempDoc)).Root);
            }


            return xmlDoc;
        }
    }


    public class LayerScaleRange
    {
        /// <summary>
        /// 
        /// </summary>
        public string MinScale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaxScale { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AreaTypeStyle AreaTypeStyle { get; set; }
    }

    public class LayerScaleRangeCollection : CollectionBase
    {
        public void Add(LayerScaleRange aLayerScaleRange)
        {
            List.Add(aLayerScaleRange);
        }

        public void Remove(int index)
        {
            List.RemoveAt(index);
        }

        public LayerScaleRange Item(int Index)
        {
            return (LayerScaleRange)List[Index];
        }
    }

    public class AreaTypeStyle
    {
        /// <summary>
        /// 
        /// </summary>
        public AreaRuleCollection AreaRules { get; set; }
    }

    public class AreaRule
    {
        /// <summary>
        /// 
        /// </summary>
        public string LegendLabel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AreaSymbolization2D Symbolization2D { get; set; }
    }


    public class AreaRuleCollection : CollectionBase
    {
        public void Add(AreaRule aAreaRule)
        {
            List.Add(aAreaRule);
        }

        public void Remove(int index)
        {
            List.RemoveAt(index);
        }

        public AreaRule Item(int Index)
        {
            return (AreaRule)List[Index];
        }
    }


    public class AreaSymbolization2D
    {
        /// <summary>
        /// 
        /// </summary>
        public Fill Fill { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Stroke Stroke { get; set; }
    }


    public class Fill
    {
        /// <summary>
        /// 
        /// </summary>
        public string FillPattern { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ForegroundColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BackgroundColor { get; set; }
    }


    public class Stroke
    {
        /// <summary>
        /// 
        /// </summary>
        public string LineStyle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Thickness { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }
    }

}   