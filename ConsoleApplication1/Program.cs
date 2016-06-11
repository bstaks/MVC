using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ivony.Html.Parser;
using System.Globalization;


namespace ConsoleApplication1
{
    class Program
    {
        public static Product obj = new Product();

        static void Main(string[] args)
        {
            CultureInfo obj = new CultureInfo("en-In");
            string arrivalDate = "2016-03-16 2:55:32";
            string DepartureDate = "2016-03-15 15:33:33";
            double daydiff = (Convert.ToDateTime(arrivalDate).Date - Convert.ToDateTime(DepartureDate).Date).Days;
            Console.Write(daydiff);

            //GetMyValue();
            Console.Read();
        }

        private static void GetMyValue()
        {
            string path = "D://HtmlPage//AnderaPradesh.txt";
            string readLine = string.Empty;
            List<string> listStr = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                while ((readLine =reader.ReadLine()) != null)
                {
               //     readLine = reader.ReadLine();
                    int startIndex = readLine.IndexOf("<a");
                   
                    if (startIndex != -1)
                    {
                        int LastIndex = readLine.IndexOf("</a>");
                        
                      listStr.Add(readLine.Substring(startIndex+30,readLine.Substring(startIndex+30).LastIndexOf("</a>")));
                    }


                }

                Console.WriteLine(String.Join( ",",listStr));
                readLine = null;
                listStr = null;
            }
        }
    }
}
