﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
namespace XmlData {
    public class XmlFile : XmlDocument {
        private static XmlReaderSettings xml_settings = new XmlReaderSettings();
        
		static XmlFile() {
			
            xml_settings.ConformanceLevel = ConformanceLevel.Fragment;
            xml_settings.IgnoreWhitespace = true;
            xml_settings.IgnoreComments = true;
            xml_settings.IgnoreProcessingInstructions = false;
            xml_settings.DtdProcessing = DtdProcessing.Parse;
            //xml_settings.ProhibitDtd = false;
            xml_settings.ValidationType = ValidationType.Schema;
            xml_settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;
            xml_settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            xml_settings.ValidationEventHandler += new ValidationEventHandler(validationHandler);

        }
        public FileInfo File { get; private set; }

        public XmlFile(string xml) {
            this.LoadXml(xml);
        }
		
		
        public XmlFile(FileInfo file, bool create, string root_name) {
            this.File = file;
            file.Refresh();
            if (!file.Exists) {
				if(create) {
                	XmlTextWriter write_here = new XmlTextWriter(file.FullName, System.Text.Encoding.UTF8);
                	write_here.Formatting = Formatting.Indented;
                	write_here.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
					//write_here.WriteStartElement(root_name);
					//write_here.WriteEndElement();
					write_here.Close();
				} else {
					throw new FileNotFoundException("XMl file not found",file.FullName);
				}
            }
            XmlReader parse_me = XmlReader.Create(file.FullName, xml_settings);
            try {
                this.Load(parse_me);
            } catch (Exception ex) {
                IXmlLineInfo info = parse_me as IXmlLineInfo;
                throw new XmlException(file.FullName + Environment.NewLine + Environment.NewLine + "Line: " + info.LineNumber + " Column: " + info.LinePosition, ex);
            } finally {
                parse_me.Close();
            }
        }

        public void Save() {
            this.Save(File.FullName);
        }

        // Event handler to take care of XML errors while reading game configs
        private static void validationHandler(object sender, ValidationEventArgs args) {
            throw new XmlException(args.Message);
        }

    }
}
