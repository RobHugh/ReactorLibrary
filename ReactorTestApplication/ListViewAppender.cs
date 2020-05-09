using System;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace ReactorTestApplication
{
    class ListViewAppender : IAppender
    {
        private ListView listView;
        private readonly object lockObject = new object();

        public string Name { get; set; }

        public ListViewAppender(ListView lView)
        {
            var frm = lView.FindForm();
            if (frm == null)
                return;

            frm.FormClosing += delegate { Close(); };
            listView = lView;
            Name = "ListViewAppender";
        }

        public static void ConfigureListViewAppender(ListView lView)
        {
            var hierachy = (Hierarchy)LogManager.GetRepository();
            var appender = new ListViewAppender(lView);
            hierachy.Root.AddAppender(appender);
        }

        public void Close()
        {
            try
            {
                lock (lockObject)
                {
                    listView = null;
                }

                var hierarchy = (Hierarchy)LogManager.GetRepository();
                hierarchy.Root.RemoveAppender(this);
            }
            catch
            {

            }
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            try
            {
                if (listView == null)
                    return;

                string[] msg = { loggingEvent.Level.Name,
                    loggingEvent.LocationInformation.ClassName,
                    loggingEvent.LocationInformation.MethodName,
                    loggingEvent.RenderedMessage };

                var listViewItem = new ListViewItem(msg);

                lock(lockObject)
                {
                    if (listView == null)
                        return;

                    var del = new Action<ListViewItem>(s => { listView.BeginUpdate(); listView.Items.Add(s); listView.EndUpdate(); });
                    listView.BeginInvoke(del, listViewItem);
                }
            }
            catch(Exception ex)
            {
                string exception = ex.ToString();
            }
        }
    }
}
