using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Web;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace InventoryCC.AWS.S3.Native.WinUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("** Native HTTP Calls **");

            //ListBuckets();
            GetBucketProperty();
            //GetBucketItems();
            //UploadToBucket();
        }

        private static void ListBuckets()
        {
            Console.WriteLine("** Query all buckets **");

            string timestamp = String.Format("{0:r}", DateTime.UtcNow); //need UtcNow, not just Now or get wrong time

            string stringToConvert = "GET\n" +      //HTTP verb
            "\n" +                                  //content-md5
            "\n" +                                  //conten-type
            "\n" +                                  //date
            "x-amz-date:" + timestamp + "\n" +      //optionally, AMZ headers
            "/";                                    //resource    

            // Here you Secret Access Key
            string awsPrivateKey = "TPgr5Ov5uPlVHtFVT2auZuu+4EJJyYohX9V6NJzY";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);
           
            //actual URL string
            //no parameters with this request
            string s3Url = "https://s3.amazonaws.com/";

            HttpWebRequest req = WebRequest.Create(s3Url) as HttpWebRequest;
            req.Method = "GET";
            req.Host = "s3.amazonaws.com";
            req.Date = DateTime.Parse(timestamp);
            req.Headers["x-amz-date"] = timestamp;

                                            //Here your AWS_Access_KeyID signature
            req.Headers["Authorization"] = "AWS AKIAI5BFBPCKMWXOBX5A:" + encodedCanonical;
            
            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
                doc.LoadXml(responseXml);
            }

            Console.WriteLine("S3 bucket list queried.");
            Console.WriteLine(doc.OuterXml);
            Console.ReadLine();

        }

        private static void GetBucketProperty()
        {
            Console.WriteLine("** Query bucket properties **");

            string timestamp = String.Format("{0:r}", DateTime.UtcNow); //need UtcNow, not just Now or get wrong time

            string stringToConvert = "GET\n" +      //HTTP verb
            "\n" +                                  //content-md5
            "\n" +                                  //conten-type
            "\n" +                                  //date
            "x-amz-date:" + timestamp + "\n" +      //optionally, AMZ headers
            "/images-inv/?lifecycle";                        //resource    

            string awsPrivateKey = "TPgr5Ov5uPlVHtFVT2auZuu+4EJJyYohX9V6NJzY";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);

            //actual URL string
            //no parameters with this request, but bucketname in URL
            string s3Url = "https://s3.amazonaws.com/?lifecycle";

            HttpWebRequest req = WebRequest.Create(s3Url) as HttpWebRequest;
            req.Method = "GET";
            req.Host = "s3.amazonaws.com";
            req.Date = DateTime.Parse(timestamp);
            req.Headers["x-amz-date"] = timestamp;
            //AWS AWS_Access_KeyID signature
            req.Headers["Authorization"] = "AWS AKIAI5BFBPCKMWXOBX5A:" + encodedCanonical;

            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
                doc.LoadXml(responseXml);
            }

            Console.WriteLine("S3 bucket list queried.");
            Console.WriteLine(doc.OuterXml);
            Console.ReadLine();
        }

        private static void GetBucketItems()
        {
            Console.WriteLine("** Query bucket content**");

            string timestamp = String.Format("{0:r}", DateTime.UtcNow); //need UtcNow, not just Now or get wrong time

            string stringToConvert = "GET\n" +      //HTTP verb
            "\n" +                                  //content-md5
            "\n" +                                  //conten-type
            "\n" +                                  //date
            "x-amz-date:" + timestamp + "\n" +      //optionally, AMZ headers
            "/psbucket/";                        //resource    

            string awsPrivateKey = "N5sWY+nJUleQBFry5PifY73HVFdjt7bJa7dhdCCU";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);

            //actual URL string
            //no parameters with this request, but bucketname in URL
            string s3Url = "https://psbucket.s3.amazonaws.com/";

            HttpWebRequest req = WebRequest.Create(s3Url) as HttpWebRequest;
            req.Method = "GET";
            req.Host = "psbucket.s3.amazonaws.com";
            req.Date = DateTime.Parse(timestamp);
            req.Headers["x-amz-date"] = timestamp;
            //AWS AWS_Access_KeyID signature
            req.Headers["Authorization"] = "AWS AKIAJU2K3V2S2TO5P7OA:" + encodedCanonical;

            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
                doc.LoadXml(responseXml);
            }

            Console.WriteLine("S3 bucket list queried.");
            Console.WriteLine(doc.OuterXml);
            Console.ReadLine();
        }

        private static void UploadToBucket()
        {
            Console.WriteLine("** Upload object to bucket**");

            byte[] fileData = File.ReadAllBytes(@"..\..\..\watson.jpg");

            string timestamp = String.Format("{0:r}", DateTime.UtcNow); //need UtcNow, not just Now or get wrong time

            string stringToConvert = "PUT\n" +      //HTTP verb
            "\n" +                                  //content-md5
            "image/jpeg\n" +                        //conten-type
            "\n" +                                  //date
            "x-amz-date:" + timestamp + "\n" +      //optionally, AMZ headers
            "/psbucket/watson.jpg";             //resource    

            string awsPrivateKey = "N5sWY+nJUleQBFry5PifY73HVFdjt7bJa7dhdCCU";
            Encoding ae = new UTF8Encoding();
            HMACSHA1 signature = new HMACSHA1();
            signature.Key = ae.GetBytes(awsPrivateKey);
            byte[] bytes = ae.GetBytes(stringToConvert);
            byte[] moreBytes = signature.ComputeHash(bytes);
            string encodedCanonical = Convert.ToBase64String(moreBytes);

            //actual URL string
            //no parameters with this request, but bucketname in URL
            string s3Url = "https://psbucket.s3.amazonaws.com/watson.jpg";

            HttpWebRequest req = WebRequest.Create(s3Url) as HttpWebRequest;
            req.Method = "PUT";
            req.Host = "psbucket.s3.amazonaws.com";
            req.Date = DateTime.Parse(timestamp);
            req.Headers["x-amz-date"] = timestamp;
            req.ContentType = "image/jpeg";
            req.ContentLength = fileData.Length;
            //AWS AWS_Access_KeyID signature
            req.Headers["Authorization"] = "AWS AKIAJU2K3V2S2TO5P7OA:" + encodedCanonical;

            Stream reqStream = req.GetRequestStream();
            reqStream.Write(fileData, 0, fileData.Length);
            reqStream.Close();

            XmlDocument doc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseXml = reader.ReadToEnd();
            }

            Console.WriteLine("S3 object uploaded.");
            Console.ReadLine();
        }
    }
}
