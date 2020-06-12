using AutoCorrection;
using System;
using System.IO;
using System.Net;

namespace TestService
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpoint = "http://localhost:5000/CreateIndexTest";
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception exc)
            {
            }
            Console.WriteLine("Hello World!");
        }
    }
}
