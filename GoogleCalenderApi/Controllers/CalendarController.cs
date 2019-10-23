using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleCalendarApi.Models;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace GoogleCalendarApi.Controllers
{
    public class CalendarController : Controller
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET Test";
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewEvent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewEvent(EventModel model)
        {

            UserCredential credential;

            using (var stream =
                new FileStream(@"" + Convert.ToString(ConfigurationManager.AppSettings["credentials"]), FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = @"" + Convert.ToString(ConfigurationManager.AppSettings["tokenpath"]) + "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            //create new event using input data

            EventsResource.InsertRequest createEventRequest =
                service.Events.Insert(
                    new Event
                    {
                        Summary = model.Summary,
                        Description = model.Description,
                        Start = model.Start.ToEventDateTime(),
                        End = model.End.ToEventDateTime()
                    }
                    , "primary");

            Event createdEvent = createEventRequest.Execute();
            if (string.IsNullOrEmpty(Convert.ToString(createdEvent.Created)))
                TempData["msg"] = "Issue in event creation";
            else
                TempData["msg"] = "Event created on " + Convert.ToString(createdEvent.Created);

            return View();
        }

    }
    public static class Extensions
    {
        public static EventDateTime ToEventDateTime(this string dDateTime)
        {
            EventDateTime edtDateTime = new EventDateTime();
            edtDateTime.DateTime = DateTime.ParseExact(Convert.ToDateTime(dDateTime).ToString("yyyy/MM/dd HH:mm"), "yyyy/MM/dd HH:mm", null);
            return edtDateTime;
        }

    }
}