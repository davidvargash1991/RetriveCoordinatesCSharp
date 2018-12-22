using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RetriveCoordinates.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace RetriveCoordinates
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing....");

            CoordinateContext _db = new CoordinateContext();
            List<Cities> cities = _db.Cities.Where(x => x.Latitude == null).ToList();

            Console.WriteLine(cities.Count.ToString() + " Cities without coordinates found");

            foreach (var item in cities)
            {
                string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false&key=YOUR_API_KEY", Uri.EscapeDataString(item.Name + "," + item.Country.Name));

                WebRequest request = WebRequest.Create(requestUri);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string apiJson = reader.ReadToEnd();

                Dictionary<string, object> responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiJson);

                if (responseJson["status"].ToString() != "OK")
                {
                    Console.WriteLine(responseJson["error_message"].ToString());
                    Console.Read();
                    return;
                }

                List<Dictionary<string,object>> results = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseJson["results"].ToString());
                Dictionary<string, object> geometry = JsonConvert.DeserializeObject<Dictionary<string, object>>(results[0]["geometry"].ToString());
                Dictionary<string, object> location = JsonConvert.DeserializeObject<Dictionary<string, object>>(geometry["location"].ToString());

                string lat = location["lat"].ToString();
                string lng = location["lng"].ToString();

                if (!string.IsNullOrEmpty(lat))
                {
                    try
                    {
                        decimal latitude = decimal.Parse(lat, NumberStyles.Number ^ NumberStyles.AllowThousands);
                        decimal longitude = decimal.Parse(lng, NumberStyles.Number ^ NumberStyles.AllowThousands);

                        item.Latitude = latitude;
                        item.Longitude = longitude;

                        _db.Entry(item).State = EntityState.Modified;
                        _db.SaveChanges();

                        Console.WriteLine(item.Name + "," + item.Country.Name + " updated");
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine("Error on city " + item.Name + "," + item.Country.Name + ": " + ex.Message + "," + ex.InnerException);
                        }
                        else
                        {
                            Console.WriteLine("Error on city " + item.Name + "," + item.Country.Name + ": " + ex.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(item.Name + "," + item.Country.Name + " Coordinates not found, please try again");
                }
            }

            Console.Read();
        }
    }
}
