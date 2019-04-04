# TxtToXMLConverter
Convert a TXT doc of a specific format to an XML of specific format. Kept low level to be flexible for various format needs

Instructions on how to use GSA rate conversion program written in C#.
Current default location for readable files is the Desktop.
The file that will be read must be named EXP.txt, this can be changed in the source
You can run the TextReader executible or run the solution from within Microsoft Visual Studio.
THere are 3 prompts that require input. Anything can be inserted as error checking for those sections has not been added yet; the user input reader can be removed altogether and still function fine
Once the program finishes then _TXTinXMLFormat.xml will be created on the desktop with all the rates formatted in XML. 
In case it does not format it properly, open it in Notepad++, Visual Code, or any other editor that accepts plugins and format document (in Visual Code, it is simply right click + "format document")

*********************

Instructions on how to use GSA accessorial conversion program written in C#.
Current default location for readable files is the Desktop.
The file that will be read must be named accessorials.txt
You can run the AccessorialReader executible or the solution from within Microsoft Visual Studio
Once the program finishes then _Accessorial_XMLFormat.xml will be created on the desktop with all the rates
formatted in XML.
