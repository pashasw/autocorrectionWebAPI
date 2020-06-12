using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoCorrection.Searcher;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Unity;
using Unity.Resolution;
using Excel = Microsoft.Office.Interop.Excel;

namespace AutoCorrection.Controllers
{
    [ApiController]
    public class AutoCorrectionController : Controller
    {
        [Route("AutoCorrection")]
        public string GetCorrectWord(string input)
        {
            var dld = new DLDistance();
            int maxDistance = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["MaxDistance"].ToString(), out maxDistance);
            //var result = dld.Calculate("баннво", "иванов");

            //  var searcher = new NGramSearcher(StaticVariables.Index,1, false );
            // StaticVariables.Index = (new BKTreeIndexer()).CreateIndex(StaticVariables.Dictionary); //бесконечно строит дерево
            var startup = new Stopwatch();
            startup.Start();
            ISearcher searcher = StaticVariables.Container.Resolve<ISearcher>(new ResolverOverride[] {
                                                                      new ParameterOverride("index", StaticVariables.Index),
                                                                      new ParameterOverride("maxDistance", maxDistance),
                                                                      new ParameterOverride("prefix", false)
                                                                      });
            startup.Stop();
            Console.WriteLine("Search: "+ startup.ElapsedMilliseconds.ToString());
           // var hashSearcher = new HashSearcher(StaticVariables.HashIndex, 1,1);
            List<string> words;
            string output = "";
            var res = searcher.Search(input,out words);
            output = String.Join(" ",words.ToArray());
            return output;
        }

        [Route("AutoCorrectionTest")]
        public string GetCorrectWordTest()
        {
          
            var dld = new DLDistance();
            int maxDistance = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["MaxDistance"].ToString(), out maxDistance);
            ISearcher searcher = StaticVariables.Container.Resolve<ISearcher>(new ResolverOverride[] {
                                                                      new ParameterOverride("index", StaticVariables.Index),
                                                                      new ParameterOverride("maxDistance", maxDistance),
                                                                      new ParameterOverride("prefix", false)
                                                                      });
            int place = 0;
            List<string> words;
            string output = "";

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = "VDWWD";
                excelPackage.Workbook.Properties.Title = "Title of Document";
                excelPackage.Workbook.Properties.Subject = "EPPlus demo export data";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                //Add some text to cell A1
                worksheet.Cells["A1"].Value = "My first EPPlus spreadsheet!";
                try
                {
                    foreach (var str in StaticVariables.Dictionary)
                    {
                        if (str.Length > 2)
                        {
                            var startup = new Stopwatch();
                            startup.Start();
                            var res = searcher.Search(str, out words);
                            startup.Stop();
                            Console.WriteLine("Search: " + startup.ElapsedMilliseconds.ToString());
                            if (startup.ElapsedMilliseconds != 0)
                            {
                                place++;

                                worksheet.Cells[place, 1].Value = startup.ElapsedMilliseconds;
                                worksheet.Cells[place, 2].Value = str.Length;

                                worksheet.Cells[1, str.Length + 5].Value = (worksheet.Cells[1, str.Length + 5].GetValue<int>()) + startup.ElapsedMilliseconds;
                                worksheet.Cells[2, str.Length + 5].Value = (worksheet.Cells[2, str.Length + 5].GetValue<int>()) + 1;

                            }
                            if (place == 3000) break;
                        }
                    }
                }
                catch (Exception exc)
                { }
                //Save your file
                FileInfo fi = new FileInfo(@"C:\Users\Pasha\Documents\AutoCorrection\Test.xlsx");
                excelPackage.SaveAs(fi);

            }
            return null;
        }

        [Route("CreateIndexTest")]
        public string CreateIndexTest()
        {
            var alphabet = new RussianAlphabet();
            var time = new Stopwatch();
            IIndexer indexer = StaticVariables.Container.Resolve<IIndexer>(new ResolverOverride[] {
                                                                      new ParameterOverride("alphabet", alphabet)
                                                                      });

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = "VDWWD";
                excelPackage.Workbook.Properties.Title = "Title of Document";
                excelPackage.Workbook.Properties.Subject = "EPPlus demo export data";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                //Add some text to cell A1
                worksheet.Cells["A1"].Value = "My first EPPlus spreadsheet!";
                try
                {
                    for(int i =0;i < 100; i++)
                    {
                            var startup = new Stopwatch();
                            startup.Start();
                            indexer.CreateIndex(StaticVariables.Dictionary);
                            startup.Stop();
                            Console.WriteLine("Search: " + startup.ElapsedMilliseconds.ToString());
                           worksheet.Cells[i+1, 1].Value = startup.ElapsedMilliseconds.ToString();

                    }
                }
                catch (Exception exc)
                { }
                //Save your file
                FileInfo fi = new FileInfo(@"C:\Users\Pasha\Documents\AutoCorrection\Index.xlsx");
                excelPackage.SaveAs(fi);

            }
            return null;
        }
    }
}
