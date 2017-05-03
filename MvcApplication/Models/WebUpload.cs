using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication.Models
{
    public class WebUpload
    {
        public WebUpload() {
            path = "";
        }
        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
    }
}