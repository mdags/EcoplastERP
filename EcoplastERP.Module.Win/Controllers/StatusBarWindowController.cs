using System.Reflection;
using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.SystemModule;

namespace EcoplastERP.Module.Win.Controllers
{
    public partial class StatusBarWindowController : WindowController
    {
        WindowTemplateController controller;

        public StatusBarWindowController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            controller = Frame.GetController<WindowTemplateController>();
            controller.CustomizeWindowStatusMessages += controller_CustomizeWindowStatusMessages;
        }
        void controller_CustomizeWindowStatusMessages(object sender, CustomizeWindowStatusMessagesEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            var version = new AssemblyName(this.Application.GetType().Assembly.FullName).Version;
            SqlConnection connection = ((XPObjectSpace)objectSpace).Session.Connection as SqlConnection;
            if (connection != null)
            {
                e.StatusMessages.Add(string.Format("Veri Tabanı: {0}:{1}", connection.DataSource, connection.Database));
            }
            e.StatusMessages.Add(string.Format("Versiyon: {0}.{1}.{2}.{3} (Build: {4})", version.Major, version.Minor, version.Build, version.Revision, version.Build));
        }
    }
}
