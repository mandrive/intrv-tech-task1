using System;
using System.Xml.Serialization;

namespace WebApi.Models
{
    [XmlRoot(ElementName = "request")]
    public class XmlRequestModel
    {
        [XmlElement(ElementName = "ix")]
        public int Index { get; set; }
        [XmlElement(ElementName = "content")]
        public XmlRequestContentModel Content { get; set; }
    }

    public class XmlRequestContentModel
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "visits", IsNullable = true)]
        public int? Visits { get; set; }
        public bool ShouldSerializeVisits() { return Visits.HasValue; }
        [XmlElement(ElementName = "dateRequested")]
        public DateTime Date { get; set; }
    }
}