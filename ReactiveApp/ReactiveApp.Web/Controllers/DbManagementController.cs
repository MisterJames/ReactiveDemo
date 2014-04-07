using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ReactiveApp.Web.Controllers
{
    public class DbManagementController : ApiController
    {
        [Route("~/api/dbmanagement/upgrade")]
        [HttpGet]
        public HttpResponseMessage UpgradeDatabase()
        {
            switch (MvcApplication.DbServiceState)
            {
                case DbServiceState.LocalCache:
                    MvcApplication.DbServiceState = DbServiceState.TableStorage;
                    break;

                default:
                    MvcApplication.DbServiceState = DbServiceState.RealTime;
                    break;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("~/api/dbmanagement/degrade")]
        [HttpGet]
        public HttpResponseMessage DegradeDatabase()
        {
            switch (MvcApplication.DbServiceState)
            {
                case DbServiceState.RealTime:
                    MvcApplication.DbServiceState = DbServiceState.TableStorage;
                    break;

                default:
                    MvcApplication.DbServiceState = DbServiceState.LocalCache;
                    break;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
