using System;
using System.Collections.Generic;
using System.IO;
using System;

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
     static string saveLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "_TXTinXMLFormat.xml");
    private string[] txtDoc;
    private string[] XMLArr;
    List<string>[] tagData = new List<string>[1000];
    int lineNum = 0;
    List<string> temp;
    double checkVar;
    string[] words = new string[0];

    //Variables
    string pkgtype = "";
    //CW
    static string type = Console.ReadLine();
    static string groupCarrier = "";
    static string code = "";
    static string chartName = "";
    //CW
    static string origin = Console.ReadLine();
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
    string[] extFilesSingle = new string[0]; //External files are populated here
    string[] extFilesMulti = new string[0];
    string prevService = "";
    //Chart
   
    string start = DateTime.Now.ToString("yyyyMMdd");
    static string end = DateTime.Today.AddYears(+100).ToString("yyyyMMdd");
    //CW
    static string userStart = Console.ReadLine();
    static string chart = "<Chart Type=" + "\"" + type + "\"" + " Start=" + "\"" + userStart + "\" " + "End=" + "\"" + end + "\" " + "Origin=" + "\"" + origin + "\">"; // Origin should be dynamic iso2 iso
    static string closingchart = "</Chart>";

File.AppendAllText(saveLocation, "<PBChartLoader>" + Environment.NewLine + chart + Environment.NewLine);

        string carrierString = "<Group Carrier=" + "\"" + groupCarrier + "\"" + " Code=" + "\"" + code + "\"" + " Name=" + "\"" + chartName + "\"";

    File.AppendAllText(saveLocation, Environment.NewLine);

        //Chart

    static string rateGroup = "<RateGroup Version=\"1\" QtyMethod=" + "\"" + qtymethod + "\"" + " QtyUnits=" + "\"" + qtyunits + "\"" + " RegionMethod=\"Zone\" " + "Currency=" + "\"" + currency + "\"" + " Service=" + "\"" + service + "\">";
    string closingRateGroupTag = "</RateGroup>";
    string rate = "";
    string closingRateTag = "</Rate>";

    //Variables


    rateGroup += closingRateGroupTag + Environment.NewLine;
    File.AppendAllText(saveLocation, rateGroup);

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

    private void zonesFun()
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
        else
        {
            qtyunits = "LB";
            rateGroup = "<RateGroup Version= " + "\"" + version + "\"" + " QtyMethod=" + "\"" + qtymethod + "\" " + "QtyUnits=" + "\"" + qtyunits + "\" " + "RegionMethod=" + "\"" + regionmethod + "\" " + "Currency=" + "\"" + currency + "\" " + "Service=" + "\"" + service + "\" " + ">";
        }

        if (!service.Contains("_CWT"))
        {
            File.AppendAllText(saveLocation, rateGroup + Environment.NewLine);
        }
    }

    private void StartFun()
    {
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

        //Adds what you want after the skippable tag

        if (words[2].Equals("XPP") || words[2].Equals("XPS") || words[2].Equals("STD") || words[2].Equals("XSS") || words[2].Equals("XPD") || words[2].Equals("GND") || words[2].Equals("THREEDAY") || words[2].Equals("TWODAY") || words[2].Equals("TWODAY_AM") || words[2].Equals("NEXTDAY_SAVER") || words[2].Equals("NEXTDAY") || words[2].Equals("NEXTDAY_EARLY") || words[2].Equals("XPSNA1") || words[2].Equals("WEF") || words[2].Contains("_CWT"))
        {

            if (!words[2].Equals(prevService))
            {
                service = words[2];
                prevService = service;
            }

            extFilesSingle = Directory.GetFiles(@"C:\Program Files (x86)\", service + "_SINGLE.txt", SearchOption.TopDirectoryOnly);

            if (extFilesSingle.Length > 0)
            {
                extFile = true;
            }

            extFilesMulti = Directory.GetFiles(@"C:\Program Files (x86)\", service + "_MULTI.txt", SearchOption.TopDirectoryOnly);

            if (extFilesMulti.Length > 0)
            {
                extFile = true;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Invalid service at line: {0}", lineNum);
            Console.ResetColor();
        }
    }

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

    private void TierFun()
    {

    }
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
    private void ConditionalFun(string s)
    {
        if ((s.Split(null))[1].Equals("DOCUMENTS"))
        {
            misc = "DOCS";
        }
        conditionalCheck = true;
    }
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


    private void EndFun()
    {
        File.AppendAllText(saveLocation, Environment.NewLine);
    }
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

    public static void Main(String[] args)
    {


        XMLFile test = new XMLFile();

        test.splitTags();
        Console.WriteLine("Press any key to exit");
        Console.ReadLine();

        System.Environment.Exit(1);

    }

}
