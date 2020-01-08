using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace IC.AWS
{
    public class AWSService
    {
        private const string bucketName = "inventarycontrol.samsung2019";
        private string keyName = "*** object key ***";
        private string filePath = string.Empty;
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.SAEast1;
        private static IAmazonS3 client;
        private Stream stream;

        public void Upload(string objectKey, Stream objectStream)
        {
            keyName = objectKey;
            stream = objectStream;
            client = new AmazonS3Client(bucketRegion);
            UploadFileAsync().Wait();
        }

        public string GetUrl(string objectKey, string objectPath)
        {
            keyName = objectKey;
            filePath = objectPath;
            client = new AmazonS3Client(bucketRegion);
            return GeneratePreSignedURL();
        }

        public void Read(string objectKey, string objectPath)
        {
            keyName = objectKey;
            filePath = objectPath;
            client = new AmazonS3Client(bucketRegion);
            ReadObjectDataAsync().Wait();
        }

        private string GeneratePreSignedURL()
        {
            string urlString = "";
            try
            {
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                    //,Expires = DateTime.Now.AddMinutes(5)
                };
                urlString = client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            return urlString;
        }

        private async Task UploadFileAsync()
        {
            try
            {
                var fileTransferUtility = new TransferUtility(client);
                
                await fileTransferUtility.UploadAsync(stream, bucketName, keyName);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }

        private async Task<string> ReadObjectDataAsync()
        {
            string responseBody = string.Empty;
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };
                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                    string contentType = response.Headers["Content-Type"];
                    Console.WriteLine("Object metadata, Title: {0}", title);
                    Console.WriteLine("Content type: {0}", contentType);

                    responseBody =  reader.ReadToEnd(); // Now you process the response body.
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            return responseBody;
        }
    }
}
