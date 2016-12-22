using System.Web;
using System.Web.Optimization;

namespace Petroleance
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {                
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js",
                         "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/Scripts/angular.js").Include(
                        "~/Scripts/angular.min.js"));
            bundles.Add(new ScriptBundle("~/Scripts/app/quiz-controller.js").Include(
                       "~/Scripts/app/quiz-controller.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/materialize").Include(
                      "~/Scripts/materialize/materialize.js",
                      "~/Scripts/respond.js"
                    ));         

            bundles.Add(new ScriptBundle("~/bundles/just").Include(
                "~/Scripts/justified.min.js",
                "~/Scripts/imagesloaded.pkgd.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bluiemp").Include(
                "~/Scripts/blueimp-gallery.js"));

            bundles.Add(new ScriptBundle("~/bundles/jcrop").Include("~/Scripts/jquery.Jcrop.min.js",
               "~/Scropt/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/jupload").Include("~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-ui.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/SiteStyle/style.css",
                     "~/Content/site.css",
                     "~/Content/blueimp-gallery.css"));

            bundles.Add(new StyleBundle("~/scripts/fileupload").Include("~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                "~/Content/jquery.Jcrop.min.css"));

            bundles.Add(new StyleBundle("~/scripts/stylesheet").Include("~/Content/StyleSheet.css"));
            bundles.Add(new StyleBundle("~/scripts/stylesheetAdmin").Include("~/Content/StyleSheetAdmin.css"));
            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                "~/Scripts/ckeditor/ckeditor.js",
                "~/Scripts/ckeditor/adapters/jquery.js"));
        }
    }
}
