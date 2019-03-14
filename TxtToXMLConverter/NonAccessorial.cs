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
        txtDoc = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + "EXP.txt");
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
        int lineNum = 0;
        List<string> temp;
        double checkVar;

        //Variables
        string pkgtype = "";
        string groupCarrier = "";
        string code = "";
        string chartName = "";
        string origin = "";
        string[] CWTLine = new string[1];
        bool conditionalCheck = false;
        bool ratetypeCheck = false;
        bool validDoc = true;
        string misc = "";
        string qtymethod = "Combination";
        string currency = "";
        string service = "DEFAULT";
        string version = "1";
        string qtyunits = "";
        string regionmethod = "Zone";
        List<string> zones = new List<string>();
        string weight = "";
        string rateRead = "";
        //Chart
        string type = "Rate";
        string start = DateTime.Now.ToString("yyyyMMdd");
        DateTime enddate = DateTime.Today.AddYears(+100);
        string end = enddate.ToString("yyyyMMdd");
        string userStart = "";
        string userEnd = "";
        string chart = "<Chart Type=" + "\"" + type + "\"" + " Start=" + "\"" + userStart + "\" " + "End=" + "\"" + end + "\" " + "Origin=" + "\"" + origin + "\">"; // Origin should be dynamic iso2 iso
        string closingchart = "</Chart>";

        File.AppendAllText(saveLocation, "<PBChardLoader>" + Environment.NewLine + chart + Environment.NewLine);

        string carrierString = "<Group Carrier=" + "\"" + groupCarrier + "\"" + " Code=" + "\"" + code + "\"" + " Name=" + "\"" + chartName + "\"";

        File.AppendAllText(saveLocation, Environment.NewLine);

        //Chart

        string rateGroup = "<RateGroup Version=\"1\" QtyMethod=" + "\"" + qtymethod + "\"" + " QtyUnits=" + "\"" + qtyunits + "\"" + " RegionMethod=\"Zone\" " + "Currency=" + "\"" + currency + "\"" + " Service=" + "\"" + service + "\">";
        string closingRateGroupTag = "</RateGroup>";
        string rate = "";
        string closingRateTag = "</Rate>";

        //Variables


        rateGroup += closingRateGroupTag + Environment.NewLine;
        File.AppendAllText(saveLocation, rateGroup);


        //reading rates and nothing else
        foreach (string s in txtDoc)
        {
            lineNum++;
            string[] words = s.Split(null);
            if (s.Contains("[START]")/*or anything else you wanna skip*/)
            {

                pkgtype = "";
                validDoc = true;
                misc = "";
                qtymethod = "Combination";
                currency = "";
                service = "";
                version = "1";
                qtyunits = "";
                regionmethod = "Zone";
                zones = new List<string>();
                weight = "";
                rateRead = "";
                conditionalCheck = false;

                //Adds what you want after the skippable tag

                if (words[2].Equals("XPP") || words[2].Equals("XPS") || words[2].Equals("STD") || words[2].Equals("XSS") || words[2].Equals("XPD") || words[2].Equals("GND") || words[2].Equals("THREEDAY") || words[2].Equals("TWODAY") || words[2].Equals("TWODAY_AM") || words[2].Equals("NEXTDAY_SAVER") || words[2].Equals("NEXTDAY") || words[2].Equals("NEXTDAY_EARLY") || words[2].Equals("XPSNA1") || words[2].Equals("WEF") || words[2].Contains("_CWT"))
                {
                    service = words[2];
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Invalid service at line: {0}", lineNum);
                    Console.ResetColor();
                }
            }
            else if (words[0].Equals("-"))// CWT related files access here
            {
                for (int z = 1; z < words.Length; z++)
                {
                    if (!s[z].Equals("-") && !s[z].Equals(" "))
                    {
                        rate = "\t<Rate Zone=" + "\"" + zones[z-1] + "\"" + " Weight=" + "\"" + "999999" + "\" " + "WeightBasis=" + "\"" + CWTLine[0] + "\"" + " AdditionalAmount=" + "\"" + words[z] + "\"" + " WeightIncrement=" + "\"" + "1" + "\"" + ">" + CWTLine[z] + "</Rate>" + Environment.NewLine;
                        File.AppendAllText(saveLocation, rate);
                    }
                }

                File.AppendAllText(saveLocation, closingRateGroupTag + Environment.NewLine);
            }
            else if (s.Contains("[TIER]"))
            {
                //ADD later once we get a definition of what to include
            }
            else if (s.Contains("[RATEMETHOD]"))//Stopping point
            {
                //Adds what you want after the skippable tag
                temp = new List<string>();
                foreach (var word in words)
                {
                    if (word != "[RATEMETHOD]" && word != " ")
                    {
                        temp.Add(word);
                    }
                }
            }
            else if (s.Contains("[RATETYPE]"))
            {
                if ((s.Split(null))[1].Equals("SINGLE_PIECE"))
                {
                    misc = "SINGLE";
                    
                }
                else if ((s.Split(null))[1].Equals("MULTI_PIECE"))
                {
                    misc = "MULTI";
                    
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[RATETYPE] needs value SINGLE_PIECE OR MULTIPIECE at line: {0}", lineNum);
                    Console.ResetColor();
                }
                conditionalCheck = true;
            }
            else if (s.Contains("[CONDITIONAL]"))
            {
                if ((s.Split(null))[1].Equals("DOCUMENTS"))
                {
                    misc = "DOCS";
                }
                conditionalCheck = true;
            }
            else if (s.Contains("[ZONES]"))
            {
                zones = new List<string>();
               
                foreach (var word in words)
                {
                    int index = word.IndexOf("-");
                    if (word != "[ZONES]" && word != " ")
                    {
                        //to turn it back to zones with DOM and INTL and country code just comment out wordzone, index and replace zones.add with word
                       
                        string wordzone = "";
                        wordzone = word.Substring(0, index);
                        zones.Add(wordzone);
                        
                       
                    }
                }

                version = "1";
                qtymethod = "Combination";
                regionmethod = "Zone";
                currency = "EUR";
                if (service.Equals("XPP") || service.Equals("XPS") || service.Equals("STD") || service.Equals("XSS") || service.Equals("XPD"))
                {
                    qtyunits = "KG";
                    rateGroup = "<RateGroup Version= " + "\"" + version + "\"" + " QtyMethod=" + "\"" + qtymethod + "\" " + "QtyUnits=" + "\"" + qtyunits + "\" " + "RegionMethod=" + "\"" + regionmethod + "\" " + "Currency=" + "\"" + currency + "\" " + "Service=" + "\"" + service + "\" " + ">";
                }
                else {
                    qtyunits = "LB";
                    rateGroup = "<RateGroup Version= " + "\"" + version + "\"" + " QtyMethod=" + "\"" + qtymethod + "\" " + "QtyUnits=" + "\"" + qtyunits + "\" " + "RegionMethod=" + "\"" + regionmethod + "\" " + "Currency=" + "\"" + currency + "\" " + "Service=" + "\"" + service + "\" " + ">";
                }

                if (!service.Contains("_CWT"))
                {
                    File.AppendAllText(saveLocation, rateGroup + Environment.NewLine);
                }
            }
            else if (s.Contains("[LETTER]"))
            {

                int bracketIndex = s.IndexOf("]")+1;
                int bracketSpace = s.IndexOf(" ");
                if (bracketIndex != bracketSpace)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Need a space after [LETTER] at line: {0}", lineNum);
                    Console.ResetColor();
                }
                
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
            else if (s.Contains("[END]"))
            {
                File.AppendAllText(saveLocation, Environment.NewLine);
            }
            else if (s.Length >= 0 && (s[1].Equals('.') || s[2].Equals('.')))
            {
                rate = Environment.NewLine;
                File.AppendAllText(saveLocation, rate);
                rate = "";
                //Length cannot be less than 0 RUNTIME error
                if (Double.TryParse(s.Substring(0, s.IndexOf('\t')), out checkVar))
                {
                    string[] nums = s.Split(null);
                    CWTLine = nums;
                    weight = nums[0];
                    for (int i = 1; i < nums.Length; i++)
                    {
                        if (nums[i] != "_" && conditionalCheck == false)
                        {
                            rateRead = nums[i];
                            //For packages
                            rate += "\t<Rate Zone=" + "\"" + zones[i - 1] + "\"" + " Weight=" + "\"" + weight + "\" " + ">" + rateRead + "</Rate>" + Environment.NewLine;
                        }
                        else if (nums[i] != "_" && conditionalCheck == true)
                        {
                            rateRead = nums[i];
                            //For packages
                            rate += "\t<Rate Zone=" + "\"" + zones[i - 1] + "\"" + " Weight=" + "\"" + weight + "\" " + "Misc=" + "\"" + misc + "\"" + ">" + rateRead + "</Rate>" + Environment.NewLine;
                        }                   
                        else
                        {

                        }
                    }
                    File.AppendAllText(saveLocation, rate);
                }
            }
            else
            {

            }
        }

        File.AppendAllText(saveLocation, closingchart + Environment.NewLine + "</PBChardLoader>");
    }

    public static void Main(String[] args)
    {
       
        
        XMLFile test = new XMLFile();
      
        test.splitTags();
        Console.WriteLine("Press any key to exit");
        Console.ReadLine();
        
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
  int normalvaluelineNumer = 0;
  for(int i = 0; i < rateValuesUnderScore.length; i++){
  if(rateValuesUnderScore[i] != "_"){
        normalvaluelineNumer++;
    }
  }
  
  int indexRateStart = rateValuesUnderscore.length - normalvaluelineNumer; // maybe +1 or -1
  int indexZoneStart = zonearray.length - normalvaluelineNumer; //maybe +1 or -1
  
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


