using BadApiCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BadApiCodingChallenge.Controllers
{
    public class BadApiController : Controller
    {
        public ActionResult GetTweets(DateTime startDate, DateTime endDate)
        {
            BadApiIntegrationModel model = new BadApiIntegrationModel();
            model.GetTweetsInTimeSpan(startDate, endDate);

            return PartialView("~/Views/Tweets/_TweetResults.cshtml", model);
        }
    }
}