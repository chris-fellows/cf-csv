// See https://aka.ms/new-console-template for more information
using CFCSV.TestClient;
using CFCSV.Utilities;

Console.WriteLine("Hello, World!");

/*
// Test split line in to column values
var quotes = '"';
var columnWithQuotes = $"Column 2{quotes}XXX";
var line = $"Column 1,{quotes}{CSVUtilities.EscapeQuotes(columnWithQuotes, quotes)}{quotes},Column 3";
var columns = CSVUtilities.GetColumnValuesV2(line, ',', '"');
*/

var file = @"D:\\Test\\CSVTest\\TestObjects.txt";

new CSVEntityWriterTest().Run(file);

new CSVEntityReaderTest().Run(file);

int xxx = 1000;