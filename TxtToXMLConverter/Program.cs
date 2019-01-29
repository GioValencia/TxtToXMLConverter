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
    //change filename to dynamic
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

    public void splitTags()
    {
        List<string> tags = new List<string>();
        //reading rates and nothing else
        foreach (string s in txtDoc)
        {
            if (s.Contains("[START]")/*or anything else you wanna skip*/)
            {
                //Adds what you want after the skippable tag
                string[] words = s.Split(' ');
                foreach (var word in words)
                {
                    if (word != "[START]" && word !=" ")
                    {
                        tags.Add(word);
                    }
                }

            }
            else if (s.Contains("[RATEMETHOD]"))//Stopping point
            {
                //Adds what you want after the skippable tag
                string[] words = s.Split(' ');
                foreach (var word in words)
                {
                    if (word != "[RATEMETHOD]" && word != " ")
                    {
                        tags.Add(word);
                    }
                }
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

/*
string misc = "";
string qtymethod = "";
string currency = "";
string service = "";
string rateGroup = "<RateGroup Version=\"1\" QtyMethod="+ "\""+qtymethod+"\"" + " QtyUnits=\"KG\" " + "RegionMethod=\"Zone\" " + "Currency=" + "\"" + currency + "\"" + " Service=" + "\"" + service + "\">"; 
string closingTag = "</RateGroup>";

string misc = "";
string min = "";
string zone = "";
string weight = "";
string pkgtype = "";
string rateRead = ""; // make sure the ints are toString() so they work in the string
string rate = "<Rate Zone="+ "\"" + zone+ "\"" + " Weight="+ "\""+weight+ "\" " + "PkgType=" + "\"" + pkgtype+ "\"" + ">" +rateRead+"</Rate>"; 
string closingTag = "</Rate>";


if (line of text file contains[CONDITIONAL] DOCUMENTS the next line after that)
            {
                rate = "<Rate Zone=" + "\"" + zone + "\"" + " Weight=" + "\"" + weight + "\"" + ">" + rateRead + "</Rate>";

            }
            else if (rate.Contains("misc")){
                rate = "<Rate Zone=" + "\"" + zone + "\"" + " Weight=" + "\"" + weight + "\" " + "Misc=" + "\"" + misc + "\"" + ">" + rateRead + "</Rate>";
            }
            else
            {
                rate = "<Rate Zone=" + "\"" + zone + "\"" + " Weight=" + "\"" + weight + "\" " + "Min=" + "\"" + min + "\"" + ">" + rateRead + "</Rate>";
            }
*/

// Arraylist for letters

/*
ArrayList<string> letter = new ArrayList<string>();
if(s.Contains("[LETTER]")){
   pkgtype="CARRIER_LETTER";



   foreach(item in that letter array){
   letter.add();//adds every other item inside the letter arraylist
   finalfile.add(rate);
   }

   }


   make 1 loop for zones to save them and have them for reference everytime there is going to be new rates coming out.
   
   Inside the document there could be empty spaces for rates we used "_" to fill them up. So we can match the zone to the rate. Or to somehow
   leave it empty but without it not matching the zones to rates at the same time.
    
    string rateReadTemp = "";
   
    assign the array 1 spot ahead. so it lines up with the zones
    we can use a bubble sort of somekind and switch it up.
    arraylist or array
    
    foreach i in loop{
    if(rateRead == "_"){
    rateReadTemp = array[i];
    array[i] = " ";
    array[i+1] = rateReadTemp;
    }
    
   }
    
   */
