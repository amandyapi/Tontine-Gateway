using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using TontineGateway.Models;

namespace TontineGateway
{
    public static class WebRequestExtention
    {
        public static ResponseModel<T> Execute<T>(this WebRequest request, object body, string method = "POST")
        {
            var result = new ResponseModel<T>();
            request.Method = method.ToUpperInvariant();
            request.ContentType = "application/json";
            Stream dataStream;
            if (!request.Method.Equals("GET", System.StringComparison.OrdinalIgnoreCase))
            {
                var postData = JsonConvert.SerializeObject(body);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            using (WebResponse response = request.GetResponse())
            {
                var httpResponse = ((HttpWebResponse)response);
                result.Status = new StatusModel
                {
                    Code = (int)httpResponse.StatusCode,
                    Description = httpResponse.StatusDescription
                };
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    result.Data = JsonConvert.DeserializeObject<T>(responseFromServer);
                    reader.Close();

                }
            }
            // dataStream.Close();
            return result;
        }

        public static ResponseModel<string> ExecuteAsString(this WebRequest request, object body, string method = "POST")
        {
            var result = new ResponseModel<string>();
            request.Method = method.ToUpperInvariant();
            request.ContentType = "application/json";
            Stream dataStream;
            if (!request.Method.Equals("GET", System.StringComparison.OrdinalIgnoreCase))
            {
                var postData = JsonConvert.SerializeObject(body);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            using (WebResponse response = request.GetResponse())
            {
                var httpResponse = ((HttpWebResponse)response);
                result.Status = new StatusModel
                {
                    Code = (int)httpResponse.StatusCode,
                    Description = httpResponse.StatusDescription
                };
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    result.Data = responseFromServer;
                    reader.Close();

                }
            }
            // dataStream.Close();
            return result;
        }
    }
}
