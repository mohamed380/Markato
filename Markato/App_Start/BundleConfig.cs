using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace Markato.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/Scripts/mjs").Include(
                "~/Scripts/MainFiles/jquery.min.js",
                "~/Scripts/MainFiles/popper.min.js",
                "~/Scripts/MainFiles/bootstrap.min.js",
                "~/Scripts/MainFiles/main.js",
                "~/Scripts/MainFiles/Chart.min.js",
                "~/Scripts/MainFiles/utils.js"
                
                ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
               "~/content/MainFiles/bootstrap.min.css",
                "~/content/MainFiles/font-awesome.min.css",
                 "~/content/MainFiles/themify-icons.css",
                  "~/content/MainFiles/flag-icon.min.css",
                   "~/content/MainFiles/cs-skin-elastic.css",
                     "~/content/MainFiles/style.css")
           );

            bundles.Add(new StyleBundle("~/Content/Homecss").Include(
                "~/content/MainFiles/bootstrap.min.css",
                 "~/content/MainFiles/font-awesome.min.css",
                  "~/content/MainFiles/themify-icons.css",
                   "~/content/MainFiles/flag-icon.min.css",
                    "~/content/MainFiles/cs-skin-elastic.css",
                      "~/content/MainFiles/HomePageCss.css",
                        "~/content/jqvmap.min.css")
                 );
                        bundles.Add(new StyleBundle("~/Content/SUcss").Include(
                "~/content/MainFiles/bootstrap.min.css",
                 "~/content/MainFiles/font-awesome.min.css",
                  "~/content/MainFiles/themify-icons.css",
                   "~/content/MainFiles/flag-icon.min.css",
                    "~/content/MainFiles/cs-skin-elastic.css",
                      "~/content/MainFiles/SUStyle.css",
                        "~/content/jqvmap.min.css")
                 );



        }
    }
}