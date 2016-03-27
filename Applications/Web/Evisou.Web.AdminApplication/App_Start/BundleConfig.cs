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
            //begin PageSpecificStyleSheetLevelPluginsIncludes
            bundles.Add(new StyleBundle("~/assets/global/plugins/select2/css").Include(
                "~/assets/global/plugins/select2/select2.css"));

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
            //end PageSpecificStyleSheetLevelPluginsIncludes

            //begin PageSpecificStyleSheetLevelStylesIncludes
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
                ,"~/assets/global/plugins/jquery-file-upload/css/jquery.fileupload-ui.css"));

            //end PageSpecificStyleSheetLevelStylesIncludes

            #endregion

            #region /css/Agent/Index

            #endregion

            #region /Js/Layout

            bundles.Add(new ScriptBundle("~/bundles/ifie9").Include(
                      "~/assets/global/plugins/respond.min.js",
                      "~/assets/global/plugins/excanvas.min.js"));


            bundles.Add(new ScriptBundle("~/Bundles/jquery").Include(
                      "~/assets/global/plugins/jquery.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/Bundles/jquery-migrate").Include(                     
                     "~/assets/global/plugins/jquery-migrate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/assets/global/plugins/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-hover-dropdown").Include(
                "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-slimscroll").Include(
                "~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-hover-dropdown").Include(
                "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-blockui-cokie").Include(
                "~/assets/global/plugins/jquery.blockui.min.js",
                "~/assets/global/plugins/jquery.cokie.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/uniform").Include(
                "~/assets/global/plugins/uniform/jquery.uniform.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-switch").Include(
                "~/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-modal").Include(
                "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js",
                "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js"));

            bundles.Add(new ScriptBundle("~/Core/Plugins").Include(
                      //"~/assets/global/plugins/jquery.min.js",
                      //"~/assets/global/plugins/jquery-migrate.min.js",
                //IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip
                      "~/assets/global/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js",
                      "~/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                      "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                      "~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                      "~/assets/global/plugins/jquery.blockui.min.js",
                      "~/assets/global/plugins/jquery.cokie.min.js",
                      "~/assets/global/plugins/uniform/jquery.uniform.min.js",
                      "~/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                      "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js",
                      "~/Assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js"
                      , "~/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js"
                     , "~/assets/global/plugins/jquery.sparkline.min.js"

                      ));

            bundles.Add(new ScriptBundle("~/Level/Script").Include(
                      "~/assets/global/plugins/jquery-idle-timeout/jquery.idletimeout.js"
                      , "~/assets/global/plugins/jquery-idle-timeout/jquery.idletimer.js"
                      ,"~/assets/global/scripts/app.js",
                      "~/assets/global/scripts/jquery.jqprint-0.3.js",
                      "~/assets/admin/layout/scripts/layout.js",
                      "~/assets/admin/layout/scripts/quick-sidebar.js",
                      "~/Assets/admin/layout/scripts/admin.main.js",
                      "~/assets/admin/layout/scripts/demo.js",
                      "~/assets/admin/pages/scripts/ui-idletimeout-x.js",
                      "~/assets/admin/pages/scripts/index.js",
                      "~/assets/admin/pages/scripts/tasks.js"
                      , "~/assets/admin/pages/scripts/profile.js"
                      ));
            #endregion

            #region IMS            
            #region Js/Ims/TransactionDetail/Index
            bundles.Add(new ScriptBundle("~/Plugins/TransactionDetail/Index").Include(
                     "~/assets/global/plugins/select2/select2.min.js"
                     ,"~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"
                     ,"~/assets/global/plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js"
                     ,"~/assets/global/plugins/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js"
                     ,"~/assets/global/plugins/datatables/extensions/Scroller/js/dataTables.scroller.min.js"
                     ,"~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     ,"~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     ,"~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     ,"~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     ,"~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     , "~/Assets/global/plugins/bootstrap-daterangepicker/moment/moment-with-locales.min.js"
                     , "~/assets/global/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js"
                     , "~/Assets/global/plugins/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js"
                     , "~/assets/global/plugins/bootstrap-daterangepicker/moment.min.js"
                     , "~/assets/global/plugins/bootstrap-daterangepicker/daterangepicker.js"
                     , "~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"
                     , "~/Assets/global/plugins/bootstrap-datepicker/js/locales/bootstrap-datepicker.zh-CN.js"
                     , "~/assets/global/plugins/jquery-inputmask/jquery.inputmask.bundle.min.2.4.18.js"
                     , "~/assets/global/plugins/bootstrap-wizard/jquery.bootstrap.wizard.min.js"
                     , "~/assets/global/plugins/bootstrap-editable/bootstrap-editable/js/bootstrap-editable.js"
                     , "~/assets/global/plugins/bootstrap-editable/inputs-ext/address/address.js"
                     , "~/Assets/admin/pages/scripts/association-bootstrap-editor.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/TransactionDetail/Index").Include(
                      "~/assets/global/scripts/datatable.js"                      
                      ,"~/assets/admin/layout/scripts/modalform.js"
                      , "~/Assets/admin/pages/scripts/ims/transactiondetail.js"

                      ));
        
            #endregion

            #region Js/Ims/Agent/Index
            bundles.Add(new ScriptBundle("~/Plugins/Agent/Index").Include(                    
                      "~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"
                     , "~/assets/global/plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js"
                     , "~/assets/global/plugins/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js"
                     , "~/assets/global/plugins/datatables/extensions/Scroller/js/dataTables.scroller.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"                    
                     , "~/assets/global/plugins/jquery-inputmask/jquery.inputmask.bundle.min.2.4.18.js"                     
                     ));

            bundles.Add(new ScriptBundle("~/Script/Agent/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                      , "~/assets/admin/layout/scripts/modalform.js"
                      , "~/Assets/admin/pages/scripts/ims/agent.js"
                      ));

            #endregion

            #region Js/Ims/Association/Index
            bundles.Add(new ScriptBundle("~/Plugins/Association/Index").Include(
                     "~/assets/global/plugins/select2/select2.min.js"
                     , "~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"
                     , "~/assets/global/plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js"
                     , "~/assets/global/plugins/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js"
                     , "~/assets/global/plugins/datatables/extensions/Scroller/js/dataTables.scroller.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/Association/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                      , "~/assets/admin/layout/scripts/modalform.js"
                      , "~/Assets/admin/pages/scripts/ims/association.js"
                      ));

            #endregion

            #region Js/Ims/Product/Index
            bundles.Add(new ScriptBundle("~/Plugins/Product/Index").Include(
                     "~/assets/global/plugins/select2/select2.min.js"
                     , "~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"
                     , "~/assets/global/plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js"
                     , "~/assets/global/plugins/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js"
                     , "~/assets/global/plugins/datatables/extensions/Scroller/js/dataTables.scroller.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
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
                      "~/assets/global/scripts/datatable.js"
                      , "~/assets/admin/layout/scripts/modalform.js"
                       , "~/Assets/admin/pages/scripts/ims/product.js"
                      ));

            #endregion

            #region Js/Ims/PaypalApi/Index
            bundles.Add(new ScriptBundle("~/Plugins/PaypalApi/Index").Include(
                     "~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"
                     , "~/assets/global/plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js"
                     , "~/assets/global/plugins/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js"
                     , "~/assets/global/plugins/datatables/extensions/Scroller/js/dataTables.scroller.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"                   
                     ));

            bundles.Add(new ScriptBundle("~/Script/PaypalApi/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                      , "~/assets/admin/layout/scripts/modalform.js"
                       , "~/Assets/admin/pages/scripts/ims/paypalapi.js"
                      ));

            #endregion

            #region Js/Ims/Purchase/Index
            bundles.Add(new ScriptBundle("~/Plugins/Purchase/Index").Include(
                     "~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"
                     , "~/assets/global/plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js"
                     , "~/assets/global/plugins/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js"
                     , "~/assets/global/plugins/datatables/extensions/Scroller/js/dataTables.scroller.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     , "~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"
                     , "~/Assets/global/plugins/bootstrap-datepicker/js/locales/bootstrap-datepicker.zh-CN.js"
                     , "~/assets/global/plugins/jquery-inputmask/jquery.inputmask.bundle.min.2.4.18.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/Purchase/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                      , "~/assets/admin/layout/scripts/modalform.js"
                      , "~/Assets/admin/pages/scripts/ims/purchase.js"
                      , "~/Assets/global/scripts/angular.min.js"
                      ));

            #endregion

            #region Js/Ims/Supplier/Index
            bundles.Add(new ScriptBundle("~/Plugins/Supplier/Index").Include(
                     "~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"
                     , "~/assets/global/plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js"
                     , "~/assets/global/plugins/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js"
                     , "~/assets/global/plugins/datatables/extensions/Scroller/js/dataTables.scroller.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     , "~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"
                     , "~/Assets/global/plugins/bootstrap-datepicker/js/locales/bootstrap-datepicker.zh-CN.js"
                    
                     ));

            bundles.Add(new ScriptBundle("~/Script/Supplier/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                      , "~/assets/admin/layout/scripts/modalform.js"
                       , "~/Assets/admin/pages/scripts/ims/supplier.js"

                      ));

            #endregion
            #endregion

            #region Account
            #region Js/Account/User/Index
            bundles.Add(new ScriptBundle("~/Plugins/User/Index").Include(
                     "~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"
                     , "~/assets/global/plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js"
                     , "~/assets/global/plugins/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js"
                     , "~/assets/global/plugins/datatables/extensions/Scroller/js/dataTables.scroller.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     
                     ));

            bundles.Add(new ScriptBundle("~/Script/User/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                      , "~/assets/admin/layout/scripts/modalform.js"
                      , "~/Assets/admin/pages/scripts/account/users.js"

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
                    , "~/Assets/admin/pages/scripts/account/myprofile.js"
                     ));
            #endregion


            #region Js/Account/Role/Index
            bundles.Add(new ScriptBundle("~/Plugins/Role/Index").Include(
                     "~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js"
                     , "~/assets/global/plugins/datatables/extensions/TableTools/js/dataTables.tableTools.min.js"
                     , "~/assets/global/plugins/datatables/extensions/ColReorder/js/dataTables.colReorder.min.js"
                     , "~/assets/global/plugins/datatables/extensions/Scroller/js/dataTables.scroller.min.js"
                     , "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js"
                     , "~/assets/global/plugins/jquery-validation/js/jquery.validate.unobtrusive.js"
                     , "~/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/Script/Role/Index").Include(
                      "~/assets/global/scripts/datatable.js"
                      , "~/assets/admin/layout/scripts/modalform.js"
                      , "~/Assets/admin/pages/scripts/account/roles.js"
                      ));

            #endregion
            #endregion
            BundleTable.EnableOptimizations = false;

        }
    }
}