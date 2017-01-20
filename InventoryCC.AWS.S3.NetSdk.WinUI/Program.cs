using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace InventoryCC.AWS.S3.NetSdk.WinUI
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Console.Write(GetServiceOutput());
            //Console.Read();

            Console.WriteLine("** SDK Calls **");
            //ListBuckets();
            //AddToBucket();
            DeleteObjectFromBucket();
        }

        private static void ListBuckets()
        {
            Console.WriteLine("** List buckets **");

            AmazonS3Client client = new AmazonS3Client();
            ListBucketsResponse response = client.ListBuckets();
            foreach (S3Bucket bucket in response.Buckets)
            {
                Console.WriteLine("Bucket: " + bucket.BucketName);
            }

            Console.ReadLine();
        }

        private static void AddToBucket()
        {
            Console.WriteLine("** Add object to bucket **");

            AmazonS3Client client = new AmazonS3Client();
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = "psbucket";
            request.FilePath = @"..\..\..\..\watson.jpg";
            request.Key = "watson10.jpg";
            request.ContentType = "image/jpeg";
            //request.InputStream;
            //request.MD5Digest;

            PutObjectResponse response = client.PutObject(request);
            Console.WriteLine("S3 object added.");

            Console.ReadLine();
        }

        private static void DeleteObjectFromBucket()
        {
            Console.WriteLine("** Delete object from bucket **");

            AmazonS3Client client = new AmazonS3Client();
            DeleteObjectRequest request = new DeleteObjectRequest().|
                .BucketName("psbucket")
                .WithKey("watson10.jpg");

            DeleteObjectResponse response = client.DeleteObject(request);
            Console.WriteLine("S3 object deleted.");
            Console.ReadLine();
        }

        public static string GetServiceOutput()
        {
            StringBuilder sb = new StringBuilder(1024);
            using (StringWriter sr = new StringWriter(sb))
            {
                sr.WriteLine("===========================================");
                sr.WriteLine("Welcome to the AWS .NET SDK!");
                sr.WriteLine("===========================================");

                // Print the number of Amazon S3 Buckets.
                AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client();

                try
                {
                    ListBucketsResponse response = s3Client.ListBuckets();
                    int numBuckets = 0;
                    if (response.Buckets != null &&
                        response.Buckets.Count > 0)
                    {
                        numBuckets = response.Buckets.Count;
                    }
                    sr.WriteLine("You have " + numBuckets + " Amazon S3 bucket(s) in the US Standard region.");
                }
                catch (AmazonS3Exception ex)
                {
                    if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") ||
                        ex.ErrorCode.Equals("InvalidSecurity")))
                    {
                        sr.WriteLine("Please check the provided AWS Credentials.");
                        sr.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                    }
                    else
                    {
                        sr.WriteLine("Caught Exception: " + ex.Message);
                        sr.WriteLine("Response Status Code: " + ex.StatusCode);
                        sr.WriteLine("Error Code: " + ex.ErrorCode);
                        sr.WriteLine("Request ID: " + ex.RequestId);
                        sr.WriteLine("XML: " + ex.XML);
                    }
                }
                sr.WriteLine("Press any key to continue...");
            }
            return sb.ToString();
        }
    }
}