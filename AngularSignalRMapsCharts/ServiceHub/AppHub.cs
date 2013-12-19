using AngularSignalRMapsCharts.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Runtime.Serialization.Json;
using AngularSignalRMapsCharts.DAL;

namespace AngularSignalRMapsCharts.ServiceHub
{
    public class AppHub : Hub
    {
        // HACK: Call th hub context from elsewhere:
        //IHubContext _context =  GlobalHost.ConnectionManager.GetHubContext<GlooHub>();
        //_context.Clients.All.AddJob(Newtonsoft.Json.JsonConvert.SerializeObject(job));

        public static Boolean SendingJobs = false;
        public static int AutoId = int.MaxValue / 2;
        private Random Rand = new Random(4141);
        private int[,] States = new int[,] { { 2000, 2599 }, { 2619, 2898 }, { 2921, 2999 }, { 2600, 2618 }, { 2900, 2920 }, { 3000, 3999 }, { 4000, 4999 }, { 5000, 5799 }, { 6000, 6797 }, { 7000, 7799 }, { 800, 899 } };

        public void Ping()
        {
            Clients.All.ping(DateTime.Now.ToString());
        }

        public void GetRandomJobs()
        {
            try
            {
                if (SendingJobs) return;
                SendingJobs = true;

                int Count = 20;
                for (int i = 0; i < Count; i++)
                {
                    Company c = GetRandomCompany();

                    MemoryStream ms = new MemoryStream();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Company));
                    ser.WriteObject(ms, c);
                    ms.Position = 0;
                    StreamReader sr = new StreamReader(ms);
                    string json = sr.ReadToEnd();

                    // Call client side function with parameter
                    Clients.All.addJob(json);

                    int wait = Rand.Next(1000, 3000);
                    System.Threading.Thread.Sleep(wait);
                }
            }
            catch (Exception ex)
            {
                String s = ex.Message;
            }

            SendingJobs = false;

        }

        private Company GetRandomCompany()
        {
            Company company = new Company();
            company.Name = GetRandomCompanyName();
            company.Url = "www." + company.Name.ToLower().Replace("'","").Replace(" ", "-") + ".com.au";
            company.DateCompleted = DateTime.Now;
            company.PostCode = GetRandomPostCode();
            company.SEO = (double)Rand.Next(20, 100);
            company.Web = (double)Rand.Next(20, 100);
            company.Directories = (double)Rand.Next(20, 100);
            company.Social = (double)Rand.Next(20, 100);
            
            // Manually set Id (otherwise AngularJS throws error)
            AutoId++;
            company.Id = AutoId;
            
            //// SAVE the Company, get new Id
            //UnitOfWork db = new UnitOfWork();
            //db.CompanyRepository.Insert(company);
            //db.Save();
            //db.Dispose();

            return company;

        }
        private int GetRandomPostCode()
        {
            int index = Rand.Next(0, 10);
            return Rand.Next(States[index, 0], States[index, 1]);
        }
        private string GetRandomCompanyName()
        {
            String[] Names = new string[] { "John's", "Bob's", "Mary's", "Ian's", "William's", "Jane's", "Anna's", "Chris'", "Gordon's" };
            String[] Industry = new string[] { "Electricians", "Florist", "Restaurant", "Cafe", "Dry Cleaners", "Newsagents", "Supermarket", "Auto Repair", "Plumbing" };

            return Names[Rand.Next(0, Names.Length - 1)] + " " + Industry[Rand.Next(0, Industry.Length - 1)];
        }
    }
}