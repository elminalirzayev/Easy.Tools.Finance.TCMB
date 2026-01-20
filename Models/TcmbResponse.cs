using System.Xml.Serialization;

namespace Easy.Tools.Finance.TCMB.Models
{
    /// <summary>
    /// Root object representing the XML response from TCMB.
    /// </summary>
    [XmlRoot("Tarih_Date")]
    public class TcmbResponse
    {
        /// <summary>
        /// Date string in Turkish format.
        /// </summary>
        [XmlAttribute("Tarih")]
        public string? Date { get; set; }

        /// <summary>
        /// Date string code.
        /// </summary>
        [XmlAttribute("Date")]
        public string? DateCode { get; set; }

        /// <summary>
        /// List of currencies.
        /// </summary>
        [XmlElement("Currency")]
        public List<TcmbCurrency>? Currencies { get; set; }
    }
}