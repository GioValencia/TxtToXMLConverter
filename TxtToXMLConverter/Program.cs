using System;
using System.Collections.Generic;
using System.IO;

public class ReadFile
{
    public string filePath;
    public string[] content;

    public ReadFile(string path)
    {
        //filePath = path;
        content = System.IO.File.ReadAllLines(path);
    }

}

public class XMLFile
{
    private string saveLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "_TXTinXMLFormat.xml");
    private string[] txtDoc;
    private string[] XMLArr;

    public XMLFile(ReadFile file)
    {
        //saveLocation = Path.Combine(file.filePath, "_TXTinXMLFormat.txt");
        txtDoc = file.content;
    }

    //Saves XML file at location specified, currently set to Desktop by default
    public void saveXML(string[] XMLarr)
    {
        System.IO.File.WriteAllLines(saveLocation, XMLArr);
    }

    //Changes the txtDoc array to a format readable in XML
    public void formatFile()
    {
        //Need to double check exact formatting for the test files before i include anything

        //Grab txtDoc
        //change each line to have appropriate labels in XML format   (delimiter == " ") (if (txtDoc[0].contains("[START]")))
        //if makes easier, make different arrays for each label
        //make another string array adding each previous line in the order they will be printed


        saveXML(XMLArr);
    }

    public void spitTags()
    {
        List<string> tags = new List<string>();
        //reading rates and nothing else
        foreach (string s in txtDoc)
        {
            if (s.Contains("[START]")/*or anything else you wanna skip*/)
            {

            }
            else if (s.Contains("RATEGROUP"))//Stopping point
            {
                break;
            }
            else
            {
                tags.Add(s);//adds line to list for later
            }
        }
    }

    public static void main()
    {

    }

}