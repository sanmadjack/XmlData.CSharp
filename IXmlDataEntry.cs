using System.Xml;
namespace XmlData {
    public interface IXmlDataEntry {
        void LoadData(XmlElement element);
        XmlElement exportXml();
    }
}
