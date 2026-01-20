using System.Globalization;
using System.Xml.Serialization;

namespace Easy.Tools.Finance.TCMB.Models
{
    /// <summary>
    /// Represents a single currency unit from TCMB XML data.
    /// </summary>
    [XmlType("Currency")]
    public class TcmbCurrency
    {
        /// <summary>
        /// Currency Code (e.g., USD, EUR).
        /// </summary>
        [XmlAttribute("CurrencyCode")]
        public string? Code { get; set; }

        /// <summary>
        /// Unit of the currency (e.g., 1 for USD, 100 for JPY).
        /// </summary>
        [XmlElement("Unit")]
        public int Unit { get; set; }

        /// <summary>
        /// Turkish name of the currency (e.g., ABD DOLARI).
        /// </summary>
        [XmlElement("Isim")]
        public string? Name { get; set; }

        /// <summary>
        /// English name of the currency (e.g., US DOLLAR).
        /// </summary>
        [XmlElement("CurrencyName")]
        public string? CurrencyName { get; set; }

        /// <summary>
        /// Raw Forex Buying rate as string from XML.
        /// </summary>
        [XmlElement("ForexBuying")]
        public string? ForexBuyingStr { get; set; }

        /// <summary>
        /// Raw Forex Selling rate as string from XML.
        /// </summary>
        [XmlElement("ForexSelling")]
        public string? ForexSellingStr { get; set; }

        /// <summary>
        /// Raw Banknote Buying rate as string from XML.
        /// </summary>
        [XmlElement("BanknoteBuying")]
        public string? BanknoteBuyingStr { get; set; }

        /// <summary>
        /// Raw Banknote Selling rate as string from XML.
        /// </summary>
        [XmlElement("BanknoteSelling")]
        public string? BanknoteSellingStr { get; set; }

        // --- Helper Properties (Decimal) ---

        /// <summary>
        /// Forex Buying rate parsed as decimal.
        /// </summary>
        [XmlIgnore]
        public decimal ForexBuying => ParseDecimal(ForexBuyingStr);

        /// <summary>
        /// Forex Selling rate parsed as decimal.
        /// </summary>
        [XmlIgnore]
        public decimal ForexSelling => ParseDecimal(ForexSellingStr);

        /// <summary>
        /// Banknote Buying rate parsed as decimal.
        /// </summary>
        [XmlIgnore]
        public decimal BanknoteBuying => ParseDecimal(BanknoteBuyingStr);

        /// <summary>
        /// Banknote Selling rate parsed as decimal.
        /// </summary>
        [XmlIgnore]
        public decimal BanknoteSelling => ParseDecimal(BanknoteSellingStr);

        private decimal ParseDecimal(string? val)
        {
            if (string.IsNullOrWhiteSpace(val)) return 0;
            // TCMB uses dot (.) as decimal separator. We use InvariantCulture to handle this safely.
            return decimal.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result) ? result : 0;
        }
    }
}