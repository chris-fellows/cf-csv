// See https://aka.ms/new-console-template for more information
using CFCSV.TestClient;
using CFCSV.TestClient2;
using CFCSV.Writer;
//using CFCSV.Utilities;

Console.WriteLine("Hello, World!");

/*
// Test split line in to column values
var quotes = '"';
var columnWithQuotes = $"Column 2{quotes}XXX";
var line = $"Column 1,{quotes}{CSVUtilities.EscapeQuotes(columnWithQuotes, quotes)}{quotes},Column 3";
var columns = CSVUtilities.GetColumnValuesV2(line, ',', '"');
*/

var customObjectsFile = @"D:\\Test\\CSVTest\\TestObjectsCustom.txt";
var dictionaryObjectsFile = @"D:\\Test\\CSVTest\\TestObjectsDictionary.txt";
var objectArrayObjectsFile = @"D:\\Test\\CSVTest\\TestObjectsObjectArray.txt";

//new CSVEntityWriterTest().WriteCustomObjects(customObjectsFile);
//new CSVEntityReaderTest().ReadCustomObjects(customObjectsFile);

new CSVDictionaryWriterTest().WriteDictionaryObjects(dictionaryObjectsFile);

new CSVDictionaryReaderTest().ReadDictionaryObjects(dictionaryObjectsFile);

int xxx = 1000;