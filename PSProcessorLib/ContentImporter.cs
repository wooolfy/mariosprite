using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

// TODO: Ersetzen Sie dies durch den Typ, den  Sie importieren möchten.
using TImport = System.String;

namespace PSProcessorLib
{
    /// <summary>
    /// Für diese Klasse wird eine Instanz durch die XNA Framework Inhalts-Pipeline erstellt,
    /// um eine Datei vom Datenträger in den angegebenen Typ, TImport, zu importieren.
    /// 
    /// Dies sollte Teil des Inhalts-Pipeline-Erweiterungsbibliothek-Projekts sein.
    /// 
    /// TODO: Ändern Sie das Attribut ContentImporter so, dass die richtige Dateierweiterung
    /// der richtige Anzeigename und der richtige Standardprozessor für diesen Importer angegeben werden.
    /// </summary>
    [ContentImporter(".xjm", DisplayName = "XML Content Importer", DefaultProcessor = "None")]  //TImport was defined as System.Xml.XmlDocument a bit higher
    public class ContentImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            string t;
            try
            {
                t = System.IO.File.ReadAllText(filename);
            }
            catch (Exception e)
            {
                //Write error logger here or update the exception with more information  
                throw e;
            }
            return t;
        }
    } 
}
