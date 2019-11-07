using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace WebService1
{


    /// <summary>
    /// Summary description for NumberService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NumberService : System.Web.Services.WebService
    {
        private protected BogsEntities db = new BogsEntities();

        [WebMethod]
        public bool Matcher(string input1, string input2)
        {
            if (input1.Length != input2.Length)
            {
                return false;
            }
            Dictionary<char, int> numericValues = new Dictionary<char, int>();
            for (int i = 33; i < 127; i++)
            {

            }
            //Dictionary<char, int> inputCheck = new Dictionary<char, int>();
            //for (int i = 0; i < input1.Length; i++)
            //{
            //    if (inputCheck.ContainsKey(input1[i]))
            //    {
            //        inputCheck[input1[i]]++;
            //    }
            //    else
            //    {
            //        inputCheck.Add(input1[i], 1);
            //    }
            //}
            //for (int i = 0; i < input2.Length; i++)
            //{
            //    if (inputCheck.ContainsKey(input2[i]))
            //    {
            //        inputCheck[input2[i]]--;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            //return inputCheck.Values.Max() == 0 ? true : false;
            ////string v2 = input2;
            //for (int i = 0; i < input1.Length; i++)
            //{
            //    if (v2.Contains(input1[i]))
            //    {
            //        v2 = v2.Remove(i, 1);
            //    }
            //}

            //return v2.Length > 0 ? true : false;
            return false;
        }

        //[WebMethod]
        //public string GetAllBlogs()
        //{
        //    var result = db.Blogs.ToArray();
        //    return JsonConvert.(result, Formatting.Indented);
        //}

        [WebMethod]
        public string GetAllComments()
        {
            var result = (object)db.Comments.ToList();
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
        [WebMethod]
        public string GetAllTags()
        {
            var result = (object)db.Tags.ToList();
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        [WebMethod]
        public string GetCars()
        {
            var result = new[] {
                new Car { id = 1, Name = "Mazda", Price = 2000 },
                new Car { id = 2, Name = "Nisan", Price = 3000 },
                new Car { id = 3, Name = "opw", Price = 4000 }
                };
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        [WebMethod]
        public string Ping()
        {
            //JsonConvert.Serialized
            return "The service is up and running";

        }

        [WebMethod]
        public string GetData(int num)
        {
            return (num * num).ToString();
        }

        [WebMethod]
        public string TemperatureCity(string city)
        {
            if (city.ToLower() == "winnipeg")
            {
                return new Random().Next(-45, -20).ToString();
            }
            else
            {
                return new Random().Next(5, 20).ToString();
            }
        }
        [WebMethod]
        public string CadToUsa(double num, bool isCadEntered)
        {
            if (isCadEntered)
            {
                return (num * 0.76).ToString() + "$ US";
            }
            else
            {
                return (num / 0.76).ToString() + "$ CAD";
            }

        }

        [WebMethod]
        public string AllBlogsWithAuthorName()
        {
            var blogsObject = db.Blogs.Select(x => new { Name = x.Author.AuthorName.Trim(), Title = x.BlogTitle.Trim(), details = x.Details.Trim(), tags = new { tagName = x.BlogTags.Select(y => y.Tag.tag1).ToList() } }).ToList();
            return JsonConvert.SerializeObject(blogsObject, Formatting.Indented);
        }

        [WebMethod]
        public string AllCommentsOnBlog(string blogTitle)
        {
            //var id = db.Blogs.Where(x => x.BlogTitle == blogTitle).FirstOrDefault().Id;
            var comments = db.Comments.Where(x => x.BlogId == db.Blogs.Where(y => y.BlogTitle == blogTitle.Trim()).FirstOrDefault().Id).ToList().Select(x => new { author = x.Author.AuthorName, details = x.Comment1 }).ToList();
            return JsonConvert.SerializeObject(comments, Formatting.Indented);
        }

        [WebMethod]
        public string HighestNumberOfBlogsOfAuthor()
        {
            //name and blogs number of blogs .. blogs
            int authorId = 0;
            Dictionary<int, int> authorWithCount = new Dictionary<int, int>();
            db.Blogs.Select(x => x.AuthorId).ToList().ForEach(y =>
            {
                if (authorWithCount.ContainsKey(y))
                {
                    authorWithCount[y]++;
                }
                else
                {
                    authorWithCount.Add(y, 1);
                }
            });
            authorId = authorWithCount.Where(x => x.Value == authorWithCount.Values.Max()).Select(y => y.Key).FirstOrDefault();
            var authorName = db.Authors.Find(authorId).AuthorName.Trim();
            var blogs = db.Blogs.Where(x => x.AuthorId == authorId).Select(x => new {author = authorName, title = x.BlogTitle ,details = x.Details, comments = new {commentArray = x.Comments.Select(y => new {author = y.Author.AuthorName.Trim() ,comment = y.Comment1.Trim() }).ToList()} }).ToList();
            return JsonConvert.SerializeObject(blogs , Formatting.Indented);
        }
    }
}
