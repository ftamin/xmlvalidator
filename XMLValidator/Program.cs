using System.Xml;
using System.Xml.Schema;
using System.IO;
using System;

public class Program
{
    public const string sampleFolder = "sample/";

    public static void Main()
    {

        // Set the validation settings.
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas.Add("http://creditinfo.com/schemas/Sample/Data", "XSD/data.xsd");
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
        settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
        settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

        var files = Directory.GetFiles(sampleFolder, "*.zip");

        foreach (var file in files)
        {
            Console.WriteLine("Extracting: " + file);

            try
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(file, sampleFolder, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting: " + file + ", message:" + ex.Message);
                continue; // error extracting, continuing...
            }
            Console.WriteLine();
        }

        files = Directory.GetFiles(sampleFolder, "*.xml");
        
        foreach(var file in files)
        {
            Console.WriteLine("Processing: " + file);
            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(file, settings);

           // Parse the file. 
            while (reader.Read());
            reader.Close();

            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Finished. Press Enter to continue.");
        Console.ReadLine();

    }

    // Display any warnings or errors.
    private static void ValidationCallBack(object sender, ValidationEventArgs args)
    {
        if (args.Severity == XmlSeverityType.Warning)
            Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
        else
            Console.WriteLine("\tValidation error: Line Number:" + args.Exception.LineNumber + ", Line Position:" + args.Exception.LinePosition + ", Error Message: " + args.Message);

    }
}