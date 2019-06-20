using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BadApiCodingChallenge;
using BadApiCodingChallenge.Controllers;
using BadApiCodingChallenge.Models;
using System.Diagnostics;

namespace BadApiCodingChallenge.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void StartPageRenderTest()
        {
            HomeController controller = new HomeController();
            
            ViewResult result = controller.Index() as ViewResult;
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTweetsTestDateRange()
        {
            BadApiIntegrationModel model = new BadApiIntegrationModel();

            DateTime start = DateTime.Parse("01/01/2016 00:00:00");
            DateTime end = DateTime.Parse("12/31/2017 23:59:59");
            model.GetTweetsInTimeSpan(start, end);

            foreach (TweetModel tweet in model.AllTweets)
            {
                if (tweet.Stamp > end)
                    Assert.Fail("Tweet is after the end date filter.");
                if (tweet.Stamp < start)
                    Assert.Fail("Tweet is before the start date filter.");
            }

            Assert.IsTrue(true, "All Tweets are within the date range");
        }

        [TestMethod]
        public void GetTweetsTestIdUniqueness()
        {
            BadApiIntegrationModel model = new BadApiIntegrationModel();

            DateTime start = DateTime.Parse("01/01/2016 00:00:00");
            DateTime end = DateTime.Parse("12/31/2017 23:59:59");
            model.GetTweetsInTimeSpan(start, end);

            IEnumerable<IGrouping<string, TweetModel>> groupedTweets = model.AllTweets.GroupBy(t => t.Id);
            foreach (IGrouping<string, TweetModel> group in groupedTweets)
            {
                if (group.Count() > 1)
                    Assert.Fail("There is more than 1 tweet with the id " + group.Key);
            }

            Assert.IsTrue(true, "All Tweets have unique IDs");
        }

        [TestMethod]
        public void GetTweetsTestReturnCount()
        {
            BadApiIntegrationModel model = new BadApiIntegrationModel();

            DateTime start = DateTime.Parse("01/01/2016 00:00:00");
            DateTime end = DateTime.Parse("12/31/2017 23:59:59");
            model.GetTweetsInTimeSpan(start, end);
            
            Assert.AreEqual(11692, model.AllTweets.Count());
        }

        [TestMethod]
        public void GetTweetsTestSpeed()
        {
            BadApiIntegrationModel model = new BadApiIntegrationModel();

            DateTime start = DateTime.Parse("01/01/2016 00:00:00");
            DateTime end = DateTime.Parse("12/31/2017 23:59:59");

            Stopwatch watch = new Stopwatch();
            watch.Start();
            model.GetTweetsInTimeSpan(start, end);
            watch.Stop();

            Assert.IsTrue(watch.ElapsedMilliseconds < 10000);
        }
    }
}
