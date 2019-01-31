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
        List<string>[] tagData = new List<string>[1000];
        int count = 0;
        List<string> temp;
        string pkgtype;
        //reading rates and nothing else
        foreach (string s in txtDoc)
        {
            if (s.Contains("[START]")/*or anything else you wanna skip*/)
            {
                //Adds what you want after the skippable tag
                string[] words = s.Split(' ');
                temp = new List<string>();
                pkgtype = "START";
                foreach (var word in words)
                {
                    if (word != "[START]" && word !=" ")
                    {
                        temp.Add(word);
                    }
                }

                tagData[count] = temp;
                tags.Add(pkgtype);
                count++;

            }
            else if (s.Contains("[RATEMETHOD]"))//Stopping point
            {
                //Adds what you want after the skippable tag
                temp = new List<string>();
               // pkgtype = "DISCOUNT";
                string[] words = s.Split(' ');
                foreach (var word in words)
                {
                    if (word != "[RATEMETHOD]" && word != " ")
                    {
                        temp.Add(word);
                    }
                }
                tagData[count] = temp;
                tags.Add(pkgtype);
                count++;
            }
            else if (s.Contains("[ZONES]") || s[0] != '-')
            {
                temp = new List<string>();
                string[] words = s.Split(' ');
                int underScoreCount = 0;
                foreach (var word in words)
                {
                    if (word != "[ZONES]" && word != "[END]" && word != " ")
                    {
                        if (word == "_")
                        {
                            underScoreCount++;
                        }
                        else
                        {
                            temp.Add(word);
                        }
                    }
                    if (word == "[END]")
                    {
                        break;
                    }
                }
                tagData[count] = temp;
                //tags.Add(pkgtype);
                count++;

            }
            else if (s.Contains("[LETTER]"))
            {
                pkgtype = "CARRIER_LETTER";
                temp = new List<string>();
                string[] words = s.Split(' ');
                foreach (var word in words)
                {
                    if (word != "[LETTER]" && word != " ")
                    {
                        temp.Add(word);
                    }
                }
                tagData[count] = temp;
                tags.Add(pkgtype);
                count++;

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
asdasd
/*
ArrayList<string> letter = new ArrayList<string>();
if(s.Contains("[LETTER]")){
   pkgtype="CARRIER_LETTER";



   foreach(item in that letter array){
   letter.add();//adds every other item inside the letter arraylist


    //DO LATER WHEN STRUCTURE IS READY
   finalfile.add(rate);
   }

   }


  // This is a possible solution for the rate values that have the "_" as a value but we still need to line them up with zones.
  string[] zonearray = new string[];
  string[] rateValuesUnderScore = new string[]
  int normalvaluecounter = 0;
  for(int i = 0; i < rateValuesUnderScore.length; i++){
  if(rateValuesUnderScore[i] != "_"){
        normalvaluecounter++;
    }
  }
  
  int indexRateStart = rateValuesUnderscore.length - normalvaluecounter; // maybe +1 or -1
  int indexZoneStart = zonearray.length - normalvaluecounter; //maybe +1 or -1
  
  for(int i=0; i < indexRateStart; i++){
  weight = indexRateStart[i];
  zone = indexZoneStart[i];
  arraylist.push(rate);
  }
    
   */

/*
 * // This is a possible solution for the rate values that have the "_" as a value but we still need to line them up with zones.
 * 
 * 
 string[] zonearray = new string[];
string[] rateValuesUnderScore = new string[]
int normalvaluecounter = 0;
for(int i = 0; i < rateValuesUnderScore.length; i++){
if(rateValuesUnderScore[i] != "_"){
    normalvaluecounter++;
}
}

int indexRateStart = rateValuesUnderscore.length - normalvaluecounter; // maybe +1 or -1
int indexZoneStart = zonearray.length - normalvaluecounter; //maybe +1 or -1

for(int i=0; i < indexRateStart; i++){
weight = indexRateStart[i];
zone = indexZoneStart[i];
arraylist.push(rate);
}
*/

/*
            string weightBasis = "";
            string additionalAmount = "";
            string weightIncrement = "";
            string rateRead = additionalAmount * weightBasis;// parse into a double and then back into a toString for the rates
            if ([TIER]) {
                rate = "<Rate Zone=" + "\"" + zone + "\"" + " Weight=" + "\"" + weight + "\" " + "WeightBasis=" + "\"" + weightBasis + "\"" + "AdditionalAmount=" + "\"" + additionalAmount + "\"" + "WeightIncrement=" + "\"" + weightIncrement + "\"" + ">" + rateRead + "</Rate>";
            }
            */
            
