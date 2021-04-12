using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public static class EndpointParser
    {

        public static bool TryParse(string json, out IEnumerable<ApiEndpoint> endpoints, out string message)
        {
            bool b = false;
            List<ApiEndpoint> list = new List<ApiEndpoint>();
            message = String.Empty;
            string jsonInput = json;
            int i = 1;

            if (!String.IsNullOrWhiteSpace(json))
            {
                try
                {
                    Dto.RegistrationCall call = JsonConvert.DeserializeObject<Dto.RegistrationCall>(jsonInput);
                    //List<Dto.ControllerRegistration> dataset = JsonConvert.DeserializeObject<List<Dto.ControllerRegistration>>(jsonInput);
                    if (call != null && call.Registration.Count > 0)
                    {
                        foreach (var data in call.Registration)
                        {
                            int registrationOrder = data.Order;                           
                            string name = data.Name;
                            foreach (var dataEndpoint in data.Endpoints)
                            {
                                HttpMethodOption option;
                                if (Enum.TryParse<HttpMethodOption>(dataEndpoint.HttpMethod,true,out option))
                                {
                                    ApiEndpoint item = new ApiEndpoint()
                                    {
                                        RegisteredAs = name,
                                        RegistrationOrder = data.Order,
                                        Moniker = String.Format("{0}.{1}", dataEndpoint.Controller, dataEndpoint.ControllerMethod),
                                        Order = i++,
                                        Pattern = dataEndpoint.Pattern,
                                        HttpMethod = option
                                    };
                                    if (dataEndpoint.Parameters != null && dataEndpoint.Parameters.Count > 0)
                                    {
                                        item.Parameters = (from x in dataEndpoint.Parameters
                                                           select new ApiParameter()
                                                           {
                                                               Name = x.Name,
                                                               Datatype = x.Type,
                                                               Source = x.Source }).ToList();
                                    }
                                    list.Add(item);
                                }                                
                            }                            
                        }
                    }
                    b = true;

                }
                catch (Exception ex)
                {
                    message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
            }
            endpoints = list;
            return b;
        }
    }
}
