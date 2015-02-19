using Microsoft.VisualStudio.TestTools.WebTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RavenDBOrdersWebTest
{
    public static class WebTestUtiles
    {
        public static void GeneratePostRequestBody(WebTestRequest request, object data, string key, string entityName)
        {
            request.Body = new StringHttpBody
            {
                ContentType = "application/json; charset=utf-8",
                InsertByteOrderMark = false,
                BodyString = new JArray(new JObject
                    {
                        {"Method", "PUT"},
                        {"Key", key},
                        {
                            "Metadata", new JObject
                            {
                                {"Raven-Entity-Name", entityName}
                            }
                        },
                        {"Document", JObject.FromObject(data)}
                    }).ToString(Formatting.None)
            };
        }
        public static void GeneratePostRequestBody(WebTestRequest request, JArray array)
        {
            request.Body = new StringHttpBody
            {
                ContentType = "application/json; charset=utf-8",
                InsertByteOrderMark = false,
                BodyString = array.ToString(Formatting.None)
            };
        }

        public static void AddJObjectToJArrayForPost(object data, string key, string entityName,JArray array)
        {
            array.Add(new JObject
            {
                {"Method", "PUT"},
                {"Key", key},
                {
                    "Metadata", new JObject
                    {
                        {"Raven-Entity-Name", entityName}
                    }
                },
                {"Document", JObject.FromObject(data)}
            });        
        }

    }
}
