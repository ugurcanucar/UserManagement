using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using UserManagement.Fake;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private List<User> _users = FakeData.GetUsers(200);

        [HttpGet]
        public void Get()
        {
            List<string> district = new List<string>();
            List<string> pharmacyName = new List<string>();
            List<string> pharmacyAdress = new List<string>();
            List<string> pharmacyNumber = new List<string>();
            Uri url = new Uri("https://www.antalyaeo.org.tr/tr/nobetci-eczaneler");

            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Encoding = System.Text.Encoding.UTF8;

            string html = client.DownloadString(url);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            var xpath = "//text()[not(normalize-space())]";
            var emptyNodes = document.DocumentNode.SelectNodes(xpath);

            //replace each and all empty text nodes with single new-line text node
            foreach (HtmlNode emptyNode in emptyNodes)
            {
                emptyNode.ParentNode
                         .ReplaceChild(HtmlTextNode.CreateNode(""), emptyNode);
            }
            HtmlNodeCollection xdistrict = document.DocumentNode.SelectNodes("//span");
            //foreach (var x in xdistrict)
            //{
            //    if(x.InnerText!="Nöbetçi Eczaneler")
            //    {
            //        district.Add(x.InnerText.Substring(1));

            //    }
               

            //}

            HtmlNodeCollection title = document.DocumentNode.SelectNodes("//div[contains(@class,'nobetciler')]");

            foreach (var item in title)
            { 

                html = item.InnerHtml;
               
                HtmlAgilityPack.HtmlDocument xz = new HtmlAgilityPack.HtmlDocument();
               xz.LoadHtml(html);
                
                HtmlNode x = item.SelectSingleNode("//div[contains(@class,'ilcebas')]/span");
                HtmlEntity
                district.Add(x.InnerText.Substring(1));
                HtmlNode pharmacies = xz.DocumentNode.SelectSingleNode("//div[contains(@class,'col-md-4')]");
                HtmlNode adress = xz.DocumentNode.SelectSingleNode("//div[contains(@class,'col-md-8')]");
                
                var z = pharmacies.InnerText.IndexOf("ECZANESİ") + 8;
                pharmacyName.Add(pharmacies.InnerText.Substring(0, z));
                pharmacyNumber.Add(pharmacies.InnerText.Substring(z, pharmacies.InnerText.Length - z));
                //string link = item.Attributes["href"].Value;

            }

        }
       
        [HttpGet("{id}")]
        public User Get(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        [HttpPost]
        public User Post([FromBody]User User)
        {
            _users.Add(User);
            return User;
        }

        [HttpPut]
        public User Put([FromBody]User user)
        {
            var editUser = _users.FirstOrDefault(x => x.Id == user.Id);
            editUser.FirstName = user.FirstName;
            editUser.LastName = user.LastName;
            editUser.Address = user.Address;
            return user;
           
        }

        [HttpDelete("{id}")] 
        public void Delete(int id)
        {
            var deletedUser = _users.FirstOrDefault(x => x.Id == id);
            _users.Remove(deletedUser);

        }

    }
}
