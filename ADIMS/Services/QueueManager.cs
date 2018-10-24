using ADIMS.Models;
using EasyNetQ;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ADIMS.Services
{
    public class QueueManager
    {
        IBus bus = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest");

        public void PublishInsurancePolicy(string company, farm_policy model, farmer _farmer)
        {
            string xmlMessage = BuildMessage(model, _farmer);

            bus.Send(company, xmlMessage);
            
        }

        private string BuildMessage(farm_policy model, farmer _farmer)
        {
            string message = "";
            InsurancePolicyMessage _rawMessage = new InsurancePolicyMessage()
            {
                farmer = _farmer.name,
                id_number = Convert.ToString(_farmer.idnumber)
            };
            message = Serialize(_rawMessage);
            return message;
        }

        private string Serialize<T>(T item)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            var subReq = item;
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);
                    xml = sww.ToString(); // Your XML
                }
            }
            return xml;
        }
    }

}