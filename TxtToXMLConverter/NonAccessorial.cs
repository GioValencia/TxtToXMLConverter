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
    List<string>[] tagData = new List<string>[1000];
    int lineNum = 0;
    List<string> temp;
    double checkVar;
    string[] words = new string[0];

    //Variables
    string pkgtype = "";
    string groupCarrier = "";
    string code = "";
    string chartName = "";
    string origin = "";
    string[] CWTLine = new string[1];
    bool conditionalCheck = false;
    bool ratetypeCheck = false;
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
    bool extFile = false; //External files for Single and Multi refer to this
    string[] extFilesSingle = new string[0]; //External files are populated here
    string[] extFilesMulti = new string[0];
    string prevService = "";
    //Chart
    string type = "Rate";
    string start = DateTime.Now.ToString("yyyyMMdd");
    string end = DateTime.Today.AddYears(+100).ToString("yyyyMMdd");
    string userStart = "";
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

    public void splitTags()
    {
        //reading rates and nothing else
        foreach (string s in txtDoc)
        {
            lineNum++;
            words = s.Split(null);
            if (s.Contains("[START]")/*or anything else you wanna skip*/)
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
                zonesFun();
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