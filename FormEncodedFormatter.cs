using System;
using System.Collections.Specialized;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Web;

namespace FormsEncodedData
{
    public class FormEncodedFormatter : IDispatchMessageFormatter
    {
        IDispatchMessageFormatter original;
        Type parameterType;
        public FormEncodedFormatter(IDispatchMessageFormatter original, Type parameterType)
        {
            this.original = original;
            this.parameterType = parameterType;
        }

        public void DeserializeRequest(Message message, object[] parameters)
        {
            HttpRequestMessageProperty prop = (HttpRequestMessageProperty)message.Properties[HttpRequestMessageProperty.Name];
            string contentType = prop.Headers["Content-Type"];
            if (contentType.StartsWith("application/x-www-form-urlencoded"))
            {
                object parameter = Activator.CreateInstance(this.parameterType);
                parameters[0] = parameter;

                var bodyReader = message.GetReaderAtBodyContents();
                bodyReader.ReadStartElement("Binary");
                byte[] bodyBytes = bodyReader.ReadContentAsBase64();
                string requestBody = Encoding.UTF8.GetString(bodyBytes);
                NameValueCollection nvc = HttpUtility.ParseQueryString(requestBody);
                foreach (var name in nvc.AllKeys)
                {
                    PropertyInfo property = this.parameterType.GetProperty(name);
                    if (property != null)
                    {
                        property.SetValue(parameter, GetValue(property.PropertyType, nvc[name]), null);
                    }
                }
            }
            else
            {
                this.original.DeserializeRequest(message, parameters);
            }
        }

        private object GetValue(Type type, string value)
        {
            return new QueryStringConverter().ConvertStringToValue(value, type);
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            return this.original.SerializeReply(messageVersion, parameters, result);
        }
    }
}