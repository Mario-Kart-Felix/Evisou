using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Evisou.Web;
using Evisou.Framework.Utility;
using Evisou.Framework.Web.Controls;
using System.Web.Optimization;

namespace Evisou.Web.AdminApplication.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region /css/layout
            bundles.Add(new StyleBundle("~/Assets/Global/Plugins/LayoutCSS").Include(
               // "~/Assets/global/plugins/font-awesome/css/font-awesome.min.css"
               // , "~/Assets/global/plugins/simple-line-icons/simple-line-icons.min.css"
                 "~/assets/global/plugins/bootstrap/css/bootstrap.min.css"
                , "~/Assets/global/plugins/uniform/css/uniform.default.css"
                , "~/Assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css"
                , "~/Assets/global/plugins/bootstrap-modal/css/bootstrap-modal-bs3patch.css"
                , "~/Assets/global/plugins/bootstrap-modal/css/bootstrap-modal.css"
                ));


            bundles.Add(new StyleBundle("~/Assets/global/plugins/font-awesome/css/css").Include(
                "~/Assets/global/plugins/font-awesome/css/font-awesome.min.css"
                ));

            bundles.Add(new StyleBundle("~/Assets/global/plugins/simple-line-icons/css").Include(
                "~/Assets/global/plugins/simple-line-icons/simple-line-icons.min.css"
                ));


            bundles.Add(new StyleBundle("~/assets/global/plugins/bootstrap/css/css").Include(
               "~/assets/global/plugins/bootstrap/css/bootstrap.min.css"
               ));

            bundles.Add(new StyleBundle("~/Assets/global/plugins/uniform/css/css").Include(
                "~/Assets/global/plugins/uniform/css/uniform.default.css"
                ));

            bundles.Add(new StyleBundle("~/Assets/global/plugins/bootstrap-switch/css/css").Include(
                "~/Assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css"
                ));

            bundles.Add(new StyleBundle("~/Assets/global/plugins/bootstrap-modal/css/css").Include(
                      "~/Assets/global/plugins/bootstrap-modal/css/bootstrap-modal-bs3patch.css",
                      "~/Assets/global/plugins/bootstrap-modal/css/bootstrap-modal.css"
                     ));

            //bundles.Add(new StyleBundle("~/Theme/Global/Styles").Include(
            //    "`/assets/global/css/components.min.css",
            //    "`/assets/global/css/plugins.min.css"
            //    ));

            bundles.Add(new StyleBundle("~/assets/admin/pages/css/css").Include(
               "~/assets/admin/pages/css/tasks.css"
               , "~/assets/admin/pages/css/profile.css"
               ));

            bundles.Add(new StyleBundle("~/assets/global/plugins/bootstrap-fileinput/css").Include(
                "~/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css"
               ));

            bundles.Add(new StyleBundle("~/assets/admin/layout/css/css").Include(
               "~/assets/admin/layout/css/layout.css"
               , "~/assets/admin/layout/css/custom.css"
               ));

            bundles.Add(new StyleBundle("~/assets/global/css/css").Include(
               "~/assets/global/css/plugins.css"

               ));
            #endregion

            #region /css/transaction/index
            //begin PageLevelPluginsCSS
            bundles.Add(new StyleBundle("~/assets/global/plugins/select2/css/select2").Include(
                "~/assets/global/plugins/select2/css/select2.min.css"
                , "~/assets/global/plugins/select2/css/select2-bootstrap.min.css"
                ));

            bundles.Add(new StyleBundle("~/assets/global/plugins/datatables/extensions/Scroller/css/css").Include(
                "~/assets/global/plugins/datatables/extensions/Scroller/css/dataTables.scroller.min.css"));

            bundles.Add(new StyleBundle("~/assets/global/plugins/datatables/extensions/ColReorder/css/css").Include(
                "~/assets/global/plugins/datatables/extensions/ColReorder/css/dataTables.colReorder.min.css"));

            bundles.Add(new StyleBundle("~/assets/global/plugins/datatables/plugins/bootstrap/css").Include(
                "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.css"));

            bundles.Add(new StyleBundle("~/assets/global/plugins/bootstrap-editable/bootstrap-editable/css/css").Include(
                "~/assets/global/plugins/bootstrap-editable/bootstrap-editable/css/bootstrap-editable.css"));

            bundles.Add(new StyleBundle("~/Assets/global/plugins/bootstrap-editable/inputs-ext/address/css").Include(
                "~/Assets/global/plugins/bootstrap-editable/inputs-ext/address/address.css"));
            //end PageLevelPluginsCSS

            //begin PageLevelCSS
            bundles.Add(new StyleBundle("~/assets/global/plugins/bootstrap-datepicker/css/css").Include(
                "~/assets/global/plugins/bootstrap-datepicker/css/datepicker.css"));

            bundles.Add(new StyleBundle("~/assets/global/plugins/bootstrap-datetimepicker/css/css").Include(
                "~/assets/global/plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css"));

            bundles.Add(new StyleBundle("~/assets/global/plugins/bootstrap-daterangepicker/css").Include(
                "~/assets/global/plugins/bootstrap-daterangepicker/daterangepicker-bs3.css"));

            bundles.Add(new StyleBundle("~/assets/global/plugins/fancybox/source/css").Include(
                "~/assets/global/plugins/fancybox/source/jquery.fancybox.css"));

            bundles.Add(new StyleBundle("~/assets/global/plugins/jquery-file-upload/blueimp-gallery/css").Include(
                "~/assets/global/plugins/jquery-file-upload/blueimp-gallery/blueimp-gallery.min.css"));

            bundles.Add(new StyleBundle("~/assets/global/plugins/jquery-file-upload/css/css").Include(
                "~/assets/global/plugins/jquery-file-upload/css/jquery.fileupload.css"
                , "~/assets/global/plugins/jquery-file-upload/css/jquery.fileupload-ui.css"));

            //end PageLevelCSS

            #endregion

            #region /css/Agent/Index

            #endregion

            #region Account

            #region Auth
            #region /css/Account/Auth/Login
            bundles.Add(new StyleBundle("~/Assets/Global/Login/CSS").Include(
                "~/assets/global/css/components.min.css"
                , "~/assets/global/css/plugins.min.css"
               
                ));
            bundles.Add(new StyleBundle("~/Assets/Page/LoginCSS").Include(
                "~/assets/pages/css/login.min.css"
                ));
            #endregion

            #region /JS/Account/Auth/Login
            bundles.Add(new ScriptBundle("~/Script/Account/Auth/Login").Include(
                       "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                       , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                       , "~/assets/global/plugins/select2/js/select2.full.min.js"
                       , "~/assets/global/scripts/app.min.js"
                       , "~/assets/pages/scripts/login.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/Script/Account/Auth/Login2").Include(
                      "~/assets/global/plugins/js.cookie.min.js",
                      "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                      "~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                      "~/assets/global/plugins/jquery.blockui.min.js",
                      "~/assets/global/plugins/uniform/jquery.uniform.min.js",
                      "~/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                      "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js",
                      "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js"

                      , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                      , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                      , "~/assets/global/plugins/select2/js/select2.full.min.js"
                      , "~/assets/global/scripts/app.min.js"
                      , "~/assets/pages/scripts/login.min.js"
                     ));
            #endregion

            #endregion


            #region /css/Account/Auth/Index
            bundles.Add(new StyleBundle("~/Assets/Global/Plugins/AuthCSS").Include(
                "~/assets/global/plugins/bootstrap-daterangepicker/daterangepicker.min.css"
                , "~/assets/global/plugins/morris/morris.css"
                , "~/assets/global/plugins/fullcalendar/fullcalendar.min.css"
                , "~/assets/global/plugins/jqvmap/jqvmap/jqvmap.css"
                ));

            #endregion

            #endregion


            #region /Js/Layout

            bundles.Add(new ScriptBundle("~/bundles/ifie9").Include(
                      "~/assets/global/plugins/respond.min.js",
                      "~/assets/global/plugins/excanvas.min.js"));


            bundles.Add(new ScriptBundle("~/Bundles/jquery").Include(
                      "~/assets/global/plugins/jquery.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/assets/global/plugins/layout").Include(
                "~/assets/global/plugins/morris/morris.min.js"
                 , "~/assets/global/plugins/morris/raphael-min.js"
                 , "~/assets/global/plugins/jquery.cookie.js"
                ));

            bundles.Add(new ScriptBundle("~/assets/global/app/layout").Include(
                "~/Assets/global/scripts/app.min.js"
                 , "~/Assets/global/scripts/modalform.js"

                ));
            //bundles.Add(new ScriptBundle("~/Bundles/jquery-migrate").Include(
            //         "~/assets/global/plugins/jquery-migrate.min.js"));

            //bundles.Add(new ScriptBundle("~/Bundles/jqueryui").Include(
            //    "~/assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                 "~/assets/global/plugins/bootstrap/js/bootstrap.min.js"));

            //bundles.Add(new ScriptBundle("~/Bundles/bootstrap-hover-dropdown").Include(
            //    "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery-slimscroll").Include(
            //    "~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap-hover-dropdown").Include(
            //    "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery-blockui-cokie").Include(
            //    "~/assets/global/plugins/jquery.blockui.min.js",
            //    "~/assets/global/plugins/jquery.cokie.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/uniform").Include(
            //    "~/assets/global/plugins/uniform/jquery.uniform.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap-switch").Include(
            //    "~/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-modal").Include(
               "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js",
                "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js"));

            bundles.Add(new ScriptBundle("~/Core/Plugins").Include(
                      //"~/assets/global/plugins/jquery.min.js",
                      //"~/assets/global/plugins/jquery-migrate.min.js",
                      //IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip
                      "~/assets/global/plugins/js.cookie.min.js",
                      "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                      "~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                      "~/assets/global/plugins/jquery.blockui.min.js",
                      "~/assets/global/plugins/uniform/jquery.uniform.min.js",
                      "~/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",

                      "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js",
                      "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js"
                      //,"~/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js"


                      ));

            bundles.Add(new ScriptBundle("~/Level/Script").Include(
                        "~/assets/pages/scripts/dashboard.min.js"
                        , "~/assets/global/plugins/jquery-idle-timeout/jquery.idletimeout.js"
                        , "~/assets/global/plugins/jquery-idle-timeout/jquery.idletimer.js"
                        , "~/Assets/pages/scripts/layout/ui-idletimeout.js"
                        ));

            bundles.Add(new ScriptBundle("~/Theme/Global/Script").Include(
                       //"~/assets/global/scripts/app.js",
                       "~/assets/global/scripts/admin.main.js"//,"~/assets/global/scripts/metronic.js",
                      ));

            bundles.Add(new ScriptBundle("~/Theme/Layout/Script").Include(
                      "~/assets/layouts/layout/scripts/layout.min.js"
                      , "~/assets/layouts/layout/scripts/demo.min.js"
                      , "~/assets/layouts/global/scripts/quick-sidebar.min.js"
                      ));
            #endregion

            #region IMS            
            #region Js/Ims/TransactionDetail/Index
            bundles.Add(new ScriptBundle("~/Plugins/TransactionDetail/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                     , "~/assets/global/plugins/datatables/datatables.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     , "~/assets/global/plugins/moment.min.js"
                     , "~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js"
                     , "~/assets/global/plugins/bootstrap-daterangepicker/daterangepicker.min.js"
                     
                     , "~/assets/global/plugins/jquery-inputmask/jquery.inputmask.bundle.min.2.4.18.js"
                     , "~/assets/global/plugins/bootstrap-wizard/jquery.bootstrap.wizard.min.js"
                     , "~/assets/global/plugins/bootstrap-editable/bootstrap-editable/js/bootstrap-editable.js"
                     , "~/assets/global/plugins/bootstrap-editable/inputs-ext/address/address.js"
                     , "~/Assets/global/plugins/select2/js/select2.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/TransactionDetail/Index").Include(
                       "~/Assets/global/scripts/modalform.js"
                       , "~/Assets/pages/scripts/ims/transactiondetail.js"
                      ));

            #endregion

            #region Js/Ims/Agent/Index
            bundles.Add(new ScriptBundle("~/Plugins/Agent/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                     , "~/assets/global/plugins/datatables/datatables.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     , "~/assets/global/plugins/jquery-inputmask/jquery.inputmask.bundle.min.2.4.18.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/Agent/Index").Include(
                      "~/Assets/global/scripts/modalform.js"
                       , "~/Assets/pages/scripts/ims/agent.js"
                      ));

            #endregion

            #region Js/Ims/Association/Index
            bundles.Add(new ScriptBundle("~/Plugins/Association/Index").Include(
                     "~/assets/global/plugins/select2/js/select2.min.js"
                     , "~/assets/global/scripts/datatable.js"
                     , "~/assets/global/plugins/datatables/datatables.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/Association/Index").Include(
                     "~/Assets/global/scripts/modalform.js"
                     , "~/Assets/pages/scripts/ims/association.js"
                      ));

            #endregion

            #region Js/Ims/Product/Index
            bundles.Add(new ScriptBundle("~/Plugins/Product/Index").Include(
                     "~/assets/global/plugins/select2/select2.min.js"
                     , "~/assets/global/scripts/datatable.js"
                     , "~/assets/global/plugins/datatables/datatables.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     , "~/assets/global/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js"
                     , "~/Assets/global/plugins/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js"
                     , "~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"
                     , "~/Assets/global/plugins/bootstrap-datepicker/js/locales/bootstrap-datepicker.zh-CN.js"
                     , "~/assets/global/plugins/bootstrap-maxlength/bootstrap-maxlength.min.js"
                     , "~/assets/global/plugins/bootstrap-touchspin/bootstrap.touchspin.js"
                     , "~/assets/global/plugins/fancybox/source/jquery.fancybox.pack.js"
                     , "~/assets/global/plugins/plupload/js/plupload.full.min.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/vendor/jquery.ui.widget.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/vendor/tmpl.min.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/vendor/load-image.min.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/vendor/canvas-to-blob.min.js"
                     , "~/assets/global/plugins/jquery-file-upload/blueimp-gallery/jquery.blueimp-gallery.min.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/jquery.iframe-transport.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/jquery.fileupload.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/jquery.fileupload-process.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/jquery.fileupload-image.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/jquery.fileupload-audio.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/jquery.fileupload-video.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/jquery.fileupload-validate.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/jquery.fileupload-ui.js"
                     , "~/assets/global/plugins/jquery-file-upload/js/cors/jquery.xdr-transport.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/Product/Index").Include(
                      "~/Assets/global/scripts/modalform.js"
                       , "~/Assets/pages/scripts/ims/product.js"
                      ));

            #endregion

            #region Js/Ims/PaypalApi/Index
            bundles.Add(new ScriptBundle("~/Plugins/PaypalApi/Index").Include(
                     "~/assets/global/scripts/datatable.js"
                     , "~/assets/global/plugins/datatables/datatables.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/PaypalApi/Index").Include(
                      "~/Assets/global/scripts/modalform.js"
                       , "~/Assets/pages/scripts/ims/paypalapi.js"
                      ));

            #endregion

            #region Js/Ims/Purchase/Index
            bundles.Add(new ScriptBundle("~/Plugins/Purchase/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                     , "~/assets/global/plugins/datatables/datatables.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     , "~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"
                     , "~/Assets/global/plugins/bootstrap-datepicker/js/locales/bootstrap-datepicker.zh-CN.js"
                     , "~/assets/global/plugins/jquery-inputmask/jquery.inputmask.bundle.min.2.4.18.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/Purchase/Index").Include(
                       "~/Assets/global/scripts/modalform.js"
                      , "~/Assets/pages/scripts/ims/purchase.js"
                      , "~/Assets/global/plugins/angularjs/angular.min.js"
                      ));

            #endregion

            #region Js/Ims/Supplier/Index
            bundles.Add(new ScriptBundle("~/Plugins/Supplier/Index").Include(
                       "~/assets/global/scripts/datatable.js"
                     , "~/assets/global/plugins/datatables/datatables.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     , "~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"
                     , "~/Assets/global/plugins/bootstrap-datepicker/js/locales/bootstrap-datepicker.zh-CN.js"

                     ));

            bundles.Add(new ScriptBundle("~/Script/Supplier/Index").Include(
                      "~/Assets/global/scripts/modalform.js"
                       , "~/Assets/pages/scripts/ims/supplier.js"

                      ));

            #endregion
            #endregion

            #region Account
            #region Js/Account/User/Index
            bundles.Add(new ScriptBundle("~/Plugins/User/Index").Include(
                     "~/assets/global/scripts/datatable.js"
                     // , "~/assets/global/plugins/datatables/datatables.min.js"
                      , "~/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js"
                      , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/User/Index").Include(
                       "~/Assets/pages/scripts/account/users.js"
                       , "~/Assets/global/scripts/modalform.js"
                      ));

            #endregion

            #region Js/Account/User/Myprofile
            bundles.Add(new ScriptBundle("~/Plugins/User/Myprofile").Include(
                      "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"

                     ));

            bundles.Add(new ScriptBundle("~/Script/User/Myprofile").Include(
                     "~/assets/admin/layout/scripts/modalform.js"

                     ));
            #endregion

            #region Js/Account/Auth
            bundles.Add(new ScriptBundle("~/Assets/Global/Plugins/AuthJS").Include(
                     "~/assets/global/plugins/moment.min.js"
                      , "~/assets/global/plugins/bootstrap-daterangepicker/daterangepicker.min.js"
                      , "~/assets/global/plugins/counterup/jquery.waypoints.min.js"
                      , "~/assets/global/plugins/counterup/jquery.counterup.min.js"
                     , "~/assets/global/plugins/amcharts/amcharts/amcharts.js"
                     , "~/assets/global/plugins/amcharts/amcharts/serial.js"
                     , "~/assets/global/plugins/amcharts/amcharts/pie.js"
                     , "~/assets/global/plugins/amcharts/amcharts/radar.js"
                     , "~/assets/global/plugins/amcharts/amcharts/themes/light.js"
                     , "~/assets/global/plugins/amcharts/amcharts/themes/patterns.js"
                     , "~/assets/global/plugins/amcharts/amcharts/themes/chalk.js"
                     , "~/assets/global/plugins/amcharts/ammap/ammap.js"
                     , "~/assets/global/plugins/amcharts/ammap/maps/js/worldLow.js"
                     , "~/assets/global/plugins/amcharts/amstockcharts/amstock.js"
                     , "~/assets/global/plugins/fullcalendar/fullcalendar.min.js"
                     , "~/assets/global/plugins/flot/jquery.flot.min.js"
                     , "~/assets/global/plugins/flot/jquery.flot.resize.min.js"
                     , "~/assets/global/plugins/flot/jquery.flot.categories.min.js"
                     , "~/assets/global/plugins/jquery-easypiechart/jquery.easypiechart.min.js"
                     , "~/assets/global/plugins/jquery.sparkline.min.js"
                     , "~/assets/global/plugins/jqvmap/jqvmap/jquery.vmap.js"
                     , "~/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.russia.js"
                     , "~/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.world.js"
                     , "~/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.europe.js"
                     , "~/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.germany.js"
                     , "~/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.usa.js"
                     , "~/assets/global/plugins/jqvmap/jqvmap/data/jquery.vmap.sampledata.js"
                     ));
            #endregion


            #region Js/Account/Role/Index
            bundles.Add(new ScriptBundle("~/Plugins/Role/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                      , "~/assets/global/plugins/datatables/datatables.min.js"
                      , "~/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js"
                      , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/Role/Index").Include(
                       "~/Assets/global/scripts/modalform.js"
                       , "~/Assets/pages/scripts/account/roles.js"
                      ));

            #endregion
            #endregion
            BundleTable.EnableOptimizations = false;

        }
    }
}