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
        content = File.ReadAllLines(path);
    }

}

public class XMLFile
{
    //change filename to dynamic
    private string saveLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "_TXTinXMLFormat.xml");
    private string[] txtDoc;
    private string[] XMLArr;

    public XMLFile(/*ReadFile file*/)
    {
        //saveLocation = Path.Combine(file.filePath, "_TXTinXMLFormat.txt");
        txtDoc = File.ReadAllLines(@"C:\Users\GiovanniValencia\Desktop\GSAApp\EXP.txt");
    }

    //Saves XML file at location specified, currently set to Desktop by default
    public void saveXML(string[] XMLarr)
    {
        File.WriteAllLines(saveLocation, XMLArr);
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
        List<string>[] tagData = new List<string>[1000];
        int count = 0;
        List<string> temp;
        double checkVar;

        //Variables
        string pkgtype = "";
        string misc = "";
        string qtymethod = "";
        string currency = "";
        string service = "";
        string min = "";
        string zone = "";
        List<string> zones = new List<string>();
        string weight = "";
        string rateRead = ""; // make sure the ints are toString() so they work in the string

        string rateGroup = "<RateGroup Version=\"1\" QtyMethod=" + "\"" + qtymethod + "\"" + " QtyUnits=\"KG\" " + "RegionMethod=\"Zone\" " + "Currency=" + "\"" + currency + "\"" + " Service=" + "\"" + service + "\">";
        string closingRateGroupTag = "</RateGroup>";
        string rate = "";
        string closingRateTag = "</Rate>";
        //Variables



        //reading rates and nothing else
        foreach (string s in txtDoc)
        {
            if (s.Contains("[START]")/*or anything else you wanna skip*/)
            {
                //Adds what you want after the skippable tag
                string[] words = s.Split(null);
                temp = new List<string>();

                foreach (var word in words)
                {
                    if (word != "[START]" && word != " ")
                    {
                        temp.Add(word);
                    }
                }

                tagData[count] = temp;

                count++;

            }
            else if (s.Contains("[RATEMETHOD]"))//Stopping point
            {
                //Adds what you want after the skippable tag
                temp = new List<string>();
                // pkgtype = "DISCOUNT";
                string[] words = s.Split(null);
                foreach (var word in words)
                {
                    if (word != "[RATEMETHOD]" && word != " ")
                    {
                        temp.Add(word);
                    }
                }
                tagData[count] = temp;

                count++;
            }
            else if (s.Contains("[ZONES]"))
            {
                zones = new List<string>();
                string[] words = s.Split(null);
                foreach (var word in words)
                {
                    if (word != "[ZONES]" && word != " ")
                    {
                        zones.Add(word);
                    }
                }

                count++;

                File.AppendAllText(saveLocation, "<RateGroup>" + Environment.NewLine);
            }
            else if (s.Contains("[LETTER]"))
            {
                //adds space between sections
                rate += Environment.NewLine;
                File.AppendAllText(saveLocation, rate);
                rate = "";
                weight = "0";
                pkgtype = "CARRIER_LETTER";
                string[] nums = s.Split(null);
                for (int i = 1; i < nums.Length; i++)
                {
                    if (nums[i] != "_")
                    {
                        rateRead = nums[i];
                        //For packages
                        rate += "\t<Rate Zone=" + "\"" + zones[i - 1] + "\"" + " Weight=" + "\"" + weight + "\" " + "PkgType=" + "\"" + pkgtype + "\"" + ">" + rateRead + "</Rate>" + Environment.NewLine;
                    }
                }
                File.AppendAllText(saveLocation, rate);

            }
            else if (s[0] == '-')
            {
                //TIERS sections
            }
            else if (s.Length >= 0 && (s[1].Equals('.') || s[2].Equals('.')))
            {
                rate += Environment.NewLine;
                File.AppendAllText(saveLocation, rate);
                rate = "";
                //Length cannot be less than 0 RUNTIME error
                if (Double.TryParse(s.Substring(0, s.IndexOf('\t')), out checkVar))
                {
                    string[] nums = s.Split(null);
                    weight = nums[0];
                    for (int i = 1; i < nums.Length; i++)
                    {
                        if (nums[i] != "_")
                        {
                            rateRead = nums[i];
                            //For packages
                            rate += "\t<Rate Zone=" + "\"" + zones[i - 1] + "\"" + " Weight=" + "\"" + weight + "\" " + ">" + rateRead + "</Rate>" + Environment.NewLine;
                        }            
                    }
                }
                File.AppendAllText(saveLocation, rate);
            }
            else if (s.Contains("[END]"))
            {

            }
            else
            {
                File.AppendAllText(saveLocation, "</RateGroup>" + Environment.NewLine);
            }
        }
    }

    public static void Main(String[] args)
    {

        XMLFile test = new XMLFile();

        test.splitTags();

        System.Environment.Exit(1);

    }

}

/*









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
//asdasd
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

//rate += "\t<Rate Zone=" + "\"" + zones[i - 1] + "\"" + " Weight=" + "\"" + weight + "\" " + "PkgType=" + "\"" + pkgtype + "\"" + ">" + rateRead + "</Rate>" + Environment.NewLine;

/*
 * public static void Print2DArray<T>(T[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i,j] + "\t");
            }
            Console.WriteLine();
        }
    } 
*/
/*
 * string version = "1";
   string qtymethod = "";
   string qtyunits = "";
   string regionmethod = "Zone";
   string currency = "EUR";
   string service = "";
   string rategroup = "<RateGroup Version= " + "\"" + version + "\"" + " QtyMethod=" + "\"" + qtymethod + "\" " + "QtyUnits=" + "\"" + qtyunits + "\"" + "RegionMethod=" + "\"" + regionmethod + "\"" + "Currency=" + "\"" + currency + "\""  + "Service=" + "\"" + service + "\"" + ">";
*/