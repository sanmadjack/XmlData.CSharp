using System.Collections.Generic;
using System.IO;
using System.Xml;
namespace XmlData {
    public abstract class AXmlDataFile<T> : XmlFile where T : AXmlDataEntry {
        protected AXmlDataFile(FileInfo file, bool create, string root_elemnent)
            : base(file, create, root_elemnent) {
            Entries.Clear();
            if (DocumentElement == null) {
                this.AppendChild(CreatRootNode());
            }
            loadXmlFile();

        }

        protected virtual void loadXmlFile() {
            foreach (XmlNode node in DocumentElement.ChildNodes) {
                //                try {

                if (node is XmlElement) {
                    XmlElement element = node as XmlElement;
                    T entry = CreateDataEntry(element);
                    entry.SourceFile = this;
                    Entries.Add(entry);
                }

                //              } catch (Exception e) {
                //                continue;
                //          }
            }

        }

        public void removeEntry(T entry) {
            this.DocumentElement.RemoveChild(entry.XML);
            this.Entries.Remove(entry);
        }

        public List<T> Entries = new List<T>();
        public void sortEntries() {
            Entries.Sort();
        }
        //        protected virtual XmlElement LoadRootNode(string name) {
        //            foreach (XmlNode node in this.ChildNodes) {

        //                if (node.Name == name) {
        //                    return (XmlElement)node;
        //                }
        //            }

        //            return (XmlElement)this.AppendChild(this.CreatRootNode(name));
        ////            throw new XmlException("Missing root: " + name);
        //        }

        protected abstract XmlElement CreatRootNode();

        protected abstract T CreateDataEntry(XmlElement element);

        public void Add(T entry) {
            XmlElement ele = entry.XML;
            entry.SourceFile = this;
            if (ele != null) {
                DocumentElement.AppendChild(ele);
                Entries.Add(entry);
            }
        }

    }
}
