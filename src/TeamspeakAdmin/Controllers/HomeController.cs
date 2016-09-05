using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeamspeakWebAdmin.Models;
using TeamspeakWebAdmin.Core;
using System.Net;

namespace TeamspeakWebAdmin.Controllers
{
    public class HomeController : Controller
    {

        private ConnectModel ConnectModelSession
        {
            get
            {
                return new ConnectModel()
                {
                    Username = HttpContext.Session.GetString("Username"),
                    IP = HttpContext.Session.GetString("IP"),
                    Guid = HttpContext.Session.GetString("Guid"),
                    Password = HttpContext.Session.GetString("Password"),
                    ErrorMessage = HttpContext.Session.GetString("ErrorMessage"),
                    Port = HttpContext.Session.GetInt32("Port") != null ? (int)HttpContext.Session.GetInt32("Port") : 0,
                    Error = HttpContext.Session.GetInt32("Error") == 1 ? true : false
                };
            }
            set
            {
                if (!string.IsNullOrEmpty(value.Username))
                    HttpContext.Session.SetString("Username", value.Username);
                if (!string.IsNullOrEmpty(value.IP))
                    HttpContext.Session.SetString("IP", value.IP);
                if (!string.IsNullOrEmpty(value.Guid))
                    HttpContext.Session.SetString("Guid", value.Guid);
                if (!string.IsNullOrEmpty(value.Password))
                    HttpContext.Session.SetString("Password", value.Password);
                if (!string.IsNullOrEmpty(value.ErrorMessage))
                    HttpContext.Session.SetString("ErrorMessage", value.ErrorMessage);
                HttpContext.Session.SetInt32("Port", value.Port);
                HttpContext.Session.SetInt32("Error", value.Error ? 1 : 0);
            }
        }

        private void ClearSession()
        {
            HttpContext.Session.Clear();
        }

        public ActionResult Index(ConnectModel model)
        {
            if (ConnectModelSession.Error)
            {
                model = ConnectModelSession;
                ClearSession();
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Connect(ConnectModel model)
        {
            if (ConnectModelSession.ConnectSufficent())
            {
                model = ConnectModelSession;
                if (Connections.Get(model.Guid) != null)
                    return View(model);
            }

            if (model.ConnectSufficent())
            {
                model.Error = true;
                ConnectModelSession = model;
                return RedirectToAction("Index");
            }

            try
            {
                model.Guid = Connections.Connect(model);
            }
            catch (Exception ex)
            {
                model.Error = true;
                model.ErrorMessage = ex.Message;
                ConnectModelSession = model;
                return RedirectToAction("Index");
            }
            ConnectModelSession = model;
            return View(model);
        }
    }
}