// See https://aka.ms/new-console-template for more information
using CFCSV.TestClient;

Console.WriteLine("Hello, World!");


var file = @"D:\\Test\\CSVTest\\TestObjects.txt";

new CSVEntityWriterTest().Run(file);

new CSVEntityReaderTest().Run(file);

int xxx = 1000;