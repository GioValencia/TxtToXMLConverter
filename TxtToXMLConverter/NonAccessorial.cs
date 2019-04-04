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
    int count = 0;


    //change filename to an appropriate name, save location can also be changed
    static string saveLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "_TXTinXMLFormat.xml");
    static string saveLocationTest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "_Test.xml");
    private string[] txtDoc;
    
    List<string>[] tagData = new List<string>[1000];
    int lineNum = 0;
    List<string> temp;
    double checkVar;
    string[] words = new string[0];

    //Variables
    string pkgtype = "";

    static string groupCarrier = "";
    static string code = "";
    static string chartName = "";
    //CW
    
        //some defaults set here, and when START is read, the defaults are reset
    string[] CWTLine = new string[1];
    bool conditionalCheck = false;
    bool ratetypeCheck = false;
    string misc = "";
    static string qtymethod = "Combination";
    static string currency = "";
    static string service = "DEFAULT";
    string version = "1";
    static string qtyunits = "";
    string regionmethod = "Zone";
    List<string> zones = new List<string>();
    string weight = "";
    string rateRead = "";
    bool extFile = false; //External files for Single and Multi refer to this
   static string[] extFilesSingle = new string[0]; //External files are populated here
   static string[] extFilesMulti = new string[0];
    string prevService = "";
    //Chart
   
    string start = DateTime.Now.ToString("yyyyMMdd");
    static string end = DateTime.Today.AddYears(+100).ToString("yyyyMMdd");
    //CW
    string rateGroup = "<RateGroup Version=\"1\" QtyMethod=" + "\"" + qtymethod + "\"" + " QtyUnits=" + "\"" + qtyunits + "\"" + " RegionMethod=\"Zone\" " + "Currency=" + "\"" + currency + "\"" + " Service=" + "\"" + service + "\">";
    string closingchart = "</Chart>";
    string closingRateGroupTag = "</RateGroup>";
    string rate = "";
    string rateSingle = "";
    string rateMulti = "";
    


    //Currently default location is set to Desktop and specific file for testing purposes. This can be changed with no issue as long as the new file matches the format of EXP.txt
    public XMLFile(/*ReadFile file*/)
    {
        txtDoc = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + "EXP.txt");
        
    }

    //Currently Zones is setting only the Numeric value it sees. To change this, change the index value to any section of the word
    private void zonesFun()
    {
        
        if (count == 0)
        {
            Variables();
            count++;
        }
        
        
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
        //EUR Specific values
        version = "1";
        qtymethod = "Combination";
        regionmethod = "Zone";
        currency = "EUR";
        if (service.Equals("XPP") || service.Equals("XPS") || service.Equals("STD") || service.Equals("XSS") || service.Equals("XPD"))
        {
            qtyunits = "KG";
          rateGroup = "<RateGroup Version= " + "\"" + version + "\"" + " QtyMethod=" + "\"" + qtymethod + "\" " + "QtyUnits=" + "\"" + qtyunits + "\" " + "RegionMethod=" + "\"" + regionmethod + "\" " + "Currency=" + "\"" + currency + "\" " + "Service=" + "\"" + service + "\" " + ">";
        }
        else
        {
            qtyunits = "LB";
          rateGroup = "<RateGroup Version= " + "\"" + version + "\"" + " QtyMethod=" + "\"" + qtymethod + "\" " + "QtyUnits=" + "\"" + qtyunits + "\" " + "RegionMethod=" + "\"" + regionmethod + "\" " + "Currency=" + "\"" + currency + "\" " + "Service=" + "\"" + service + "\" " + ">";
        }

        if (!service.Contains("_CWT"))
        {
            File.AppendAllText(saveLocation,  rateGroup + Environment.NewLine);
            
        }
        else if(service.Contains("_CWT"))
        {
            File.AppendAllText(saveLocation,  Environment.NewLine);
        }
        
    }
    


    //For Single - not finished yet, not implemented yet
    private void SingleP()
    {
        // make a new value for rate but call it singlerate and assignit to  rate += combine with multirate in the end


        foreach (string s in extFilesSingle)
        {
            // string[] rateS = s.Split(null);
            words = s.Split(null);
            for (int i = 1; i < words.Length; i++)
            {
                double Num;
                bool isNum = double.TryParse(words[i], out Num);
                weight = words[0];
                if (isNum)
                {

                    rateRead = Num.ToString();
                    rateSingle += "\t<Rate Zone=" + "\"" + zones[i - 1] + "\"" + " Weight=" + "\"" + weight + "\" " + "Misc=" + "\"" + "SINGLE" + "\"" + ">" + rateRead + "</Rate>" + Environment.NewLine;
                }

            }
            
        }
        File.AppendAllText(saveLocation, rateSingle + Environment.NewLine);
    }

    //For Multi, not finished yet, not implemented yet
    private void MultiP()
    {
        foreach (string s in extFilesMulti)
        {
            // string[] rateS = s.Split(null);
            words = s.Split(null);
            for (int i = 1; i < words.Length; i++)
            {
                double Num;
                bool isNum = double.TryParse(words[i], out Num);
                weight = words[0];
                if (isNum && !words[0].Contains("-") && !words[0].Contains("TIER"))
                {
                     
                    rateRead = Num.ToString();
                    rateMulti += "\t<Rate Zone=" + "\"" + zones[i - 1] + "\"" + " Weight=" + "\"" + weight + "\" " + "Misc=" + "\"" + "MULTI" + "\"" + ">" + rateRead + "</Rate>" + Environment.NewLine;
                   
                }

            }

        }
        
            File.AppendAllText(saveLocation, rateMulti + Environment.NewLine);
       
        

    }

    //When [START] is read
    private void StartFun()
    {
        //Setting defaults
        pkgtype = "";
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
        extFile = false;
        extFilesSingle = new string[1];
        extFilesMulti = new string[1];
   


        //Adds what you want after the START tag

        if (words[2].Equals("XPP") || words[2].Equals("XPS") || words[2].Equals("STD") || words[2].Equals("XSS") || words[2].Equals("XPD") || words[2].Equals("GND") || words[2].Equals("THREEDAY") || words[2].Equals("TWODAY") || words[2].Equals("TWODAY_AM") || words[2].Equals("NEXTDAY_SAVER") || words[2].Equals("NEXTDAY") || words[2].Equals("NEXTDAY_EARLY") || words[2].Equals("XPSNA1") || words[2].Equals("WEF") || words[2].Contains("_CWT"))
        {
            //This logic was implemented for DOCUMENTS section so it may be added without creating a new rategroup. If the current service being read is different, it would change the current service variable
            if (!words[2].Equals(prevService))
            {
                service = words[2];
                prevService = service;
            }

            //Validate that a SINGLE file exists
            if (extFilesSingle.Length > 1)
            {
                extFile = true;
            }

            //Validate that a MULTI file exists
            if (extFilesMulti.Length > 1)
            {
                extFile = true;
            }
          
        }

        //Error message
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Invalid service at line: {0}", lineNum);
            Console.ResetColor();
        }
    }

    //CWT section parser
    private void CWTFun(string s)
    {
        for (int z = 1; z < words.Length; z++)
        {
            if (!s[z].Equals("-") && !s[z].Equals(" "))
            {
                rate = "\t<Rate Zone=" + "\"" + zones[z - 1] + "\"" + " Weight=" + "\"" + "999999" + "\" " + "WeightBasis=" + "\"" + CWTLine[0] + "\"" + " AdditionalAmount=" + "\"" + words[z] + "\"" + " WeightIncrement=" + "\"" + "1" + "\"" + ">" + CWTLine[z] + "</Rate>" + Environment.NewLine;
                File.AppendAllText(saveLocation, rate);
            }
        }

       File.AppendAllText(saveLocation, closingRateGroupTag + Environment.NewLine);
    }

    //Tiers would be implemented here
    private void TierFun()
    {

    }
    //Ratemethod title parser
    private void RateMethodFun()
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
    //RateType parser
    private void RateTypeFun(string s)
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
    //Documents tag parser
    private void ConditionalFun(string s)
    {
        if ((s.Split(null))[1].Equals("DOCUMENTS"))
        {
            misc = "DOCS";
        }
        conditionalCheck = true;
    }
    //Letter parser and rates
    private void LetterFun(string s)
    {
        int bracketIndex = s.IndexOf("]") + 1;
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

    // Write a new line, just shortened to a smaller function call, can be editted to include more information
    private void EndFun()
    {
        File.AppendAllText(saveLocation, Environment.NewLine);
    }
    //General rates processor, once ZONES are implemented, this function will need to be altered to include the CORRECT zones info
    private void RatesProcessorFun(string s)
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

    //For each tag in the TXT file, this function calls each appropriate function
    public void splitTags()
    {
       
        //reading rates and nothing else
        foreach (string s in txtDoc)
        {
            lineNum++;
            words = s.Split(null);
            if (s.Contains("[START]")/*or anything else you wanna skip*/)
            {
                StartFun();
            //    SingleP();
              //  MultiP();

            }
            else if (words[0].Equals("-"))// CWT related files access here
            {
                CWTFun(s);
            }
            else if (s.Contains("[TIER]"))
            {
                //ADD later once we get a definition of what to include
                TierFun();
            }
            else if (s.Contains("[RATEMETHOD]"))//Stopping point
            {
                RateMethodFun();
            }
            else if (s.Contains("[RATETYPE]"))
            {
                RateTypeFun(s);
                
            }
            else if (s.Contains("[CONDITIONAL]"))
            {
                ConditionalFun(s);
            }
            else if (s.Contains("[ZONES]"))
            {
                zonesFun();
            }
            else if (s.Contains("[LETTER]"))
            {
                LetterFun(s);
               // SingleP();
            }
            else if (s.Contains("[END]"))
            {
                
                EndFun();
            }
            else if (s.Length >= 0 && (s[1].Equals('.') || s[2].Equals('.')))
            {
                RatesProcessorFun(s);
            }
            else
            {

            }
        }

        File.AppendAllText(saveLocation, closingchart + Environment.NewLine + "</PBChartLoader>");
    }

    //Set the dynamic variables at the beginning of the file, this is technically unneeded but not sure if that information is static
    private void Variables()
    {
        
        Console.WriteLine("Write the start Date");
        string userStart = Console.ReadLine();
        Console.WriteLine("Type in Rate or Zone");
        string type = Console.ReadLine();
        Console.WriteLine("Type in Origin.");
        string origin = Console.ReadLine();
        string chart = "<Chart Type=" + "\"" + type + "\"" + " Start=" + "\"" + userStart + "\" " + "End=" + "\"" + end + "\" " + "Origin=" + "\"" + origin + "\">"; // Origin should be dynamic iso2 iso


        File.AppendAllText(saveLocation, "<PBChartLoader>" + Environment.NewLine + chart + Environment.NewLine);

        string carrierString = "<Group Carrier=" + "\"" + groupCarrier + "\"" + " Code=" + "\"" + code + "\"" + " Name=" + "\"" + chartName + "\"";

        File.AppendAllText(saveLocation, Environment.NewLine);

        //Chart

        //Variables


        rateGroup += closingRateGroupTag + Environment.NewLine;
        File.AppendAllText(saveLocation, rateGroup);
       
    }


    public static void Main(String[] args)
    {

        
        XMLFile test = new XMLFile();

         test.splitTags();
        
        System.Environment.Exit(1);

    }

}
