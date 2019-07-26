using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF.JoshSmith.ServiceProviders.UI;

namespace Distribution_Point_Priority
{
    /// <summary>
    /// An app for altering ConfigMgr Distribution Point content distribution priority values
    /// by dragging and dropping on a list to reorder servers.
    /// 
    /// These priority values are only exposed via WMI on the site server and are not visible
    /// or editable in the ConfigMgr admin console. For a full description of how these priorities
    /// work, see this article:
    /// https://techcommunity.microsoft.com/t5/Configuration-Manager-Archive/Content-Distribution-Priorities/ba-p/273393
    /// 
    /// No claim of suitability, guarantee, or any warranty whatsoever is provided. This software is provided "as-is".
    /// 
    /// Attributions:
    /// ListViewDragDropManager and associated classes by Josh Smith
    /// https://www.codeproject.com/Articles/17266/Drag-and-Drop-Items-in-a-WPF-ListView
    /// Used in accordance with the Code Project Open License.
    /// </summary>
    public partial class MainWindow : Window
    {

        ListViewDragDropManager<DistributionPoint> dragMgr;
        private ObservableCollection<DistributionPoint> list = new ObservableCollection<DistributionPoint>();
        private readonly System.Threading.SynchronizationContext synchronizationContext;

        public MainWindow()
        {
            InitializeComponent();
            synchronizationContext = System.Threading.SynchronizationContext.Current;
            btnApply.IsEnabled = false;
            btnReset.IsEnabled = false;
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = false;
            UpdateStatus("Contacting site server...");

            string computer = this.tbSiteServer.Text;
            string siteCode = this.tbSiteCode.Text;

            list.Clear();
            ObservableCollection<DistributionPoint> temp = await Task.Run(() =>
            {
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\SMS\site_{1}", computer, siteCode));
                scope.Connect();
                ObjectQuery query = new ObjectQuery(@"SELECT * FROM SMS_SCI_SysResUse WHERE RoleName = 'SMS Distribution Point' AND NetworkOSPath LIKE '%'");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection colDPs = searcher.Get();

                ObservableCollection<DistributionPoint> t = new ObservableCollection<DistributionPoint>();

                foreach (ManagementObject objDP in colDPs)
                {
                    string server = (string)objDP["NetworkOSPath"];
                    int index = Array.FindIndex((System.Management.ManagementBaseObject[])objDP["Props"], IsPriority);
                    int currentPriority = Convert.ToInt32(((System.Management.ManagementBaseObject[])objDP["Props"])[index].GetPropertyValue("Value"));

                    Debug.WriteLine("Adding Server: {0}", objDP["NetworkOSPath"]);
                    UpdateStatus(String.Format("Adding {0}", objDP["NetworkOSPath"]));
                    t.Add(new DistributionPoint(server, currentPriority, currentPriority, false));
                }

                return t;
            });

            // sort by priority
            ObservableCollection<DistributionPoint> sorted = new ObservableCollection<DistributionPoint>(temp.OrderBy(p => p.NewPriority));
            foreach (DistributionPoint i in sorted) {
                list.Add(i);
                if ((list.Count - 1 > 0) && (list[list.Count - 1].Priority == list[list.Count - 2].Priority))
                {
                    list[list.Count - 1].Shared = true;
                    list[list.Count - 2].Shared = true;
                }
            }

            this.listView.ItemsSource = list;
            this.dragMgr = new ListViewDragDropManager<DistributionPoint>(this.listView);
            this.listView.Drop += OnListViewDrop;

            btnApply.IsEnabled = true;
            btnReset.IsEnabled = true;
            UpdateStatus("Finished Loading Distribution Points");
        }

        private async void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            btnApply.IsEnabled = false;
            UpdateStatus("Contacting site server...");

            string computer = this.tbSiteServer.Text;
            string siteCode = this.tbSiteCode.Text;

            await Task.Run(() =>
            {
                ManagementScope scope = new ManagementScope(String.Format(@"\\{0}\root\SMS\site_{1}", computer, siteCode));
                scope.Connect();
                ObjectQuery query = new ObjectQuery(@"SELECT * FROM SMS_SCI_SysResUse WHERE RoleName = 'SMS Distribution Point' AND NetworkOSPath LIKE '%'");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection colDPs = searcher.Get();

                foreach (ManagementObject objDP in colDPs)
                {
                    // get the Props array
                    string server = (string)objDP["NetworkOSPath"];
                    UpdateStatus(String.Format("Saving changes to {0}...", server));
                    System.Management.ManagementBaseObject[] tempArr = (System.Management.ManagementBaseObject[])objDP["Props"];

                    // get the Priority property index
                    int index = Array.FindIndex(tempArr, IsPriority);

                    // write the new Priority Value for this server to temp object
                    int newPriority = list.Where(o => o.Server == server).First().NewPriority;
                    tempArr[index].SetPropertyValue("Value", newPriority);

                    // set the Props array
                    objDP.SetPropertyValue("Props", tempArr);
                    objDP.Put();
                }
            });

            UpdateCurrentColumn();
            listView.Items.Refresh();
            btnApply.IsEnabled = true;
            UpdateStatus("Finished Saving Priorities");
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            int maxIndex = list.Count - 1;
            int workingPosition = 0;

            while (workingPosition <= maxIndex)
            {
                list[workingPosition].NewPriority = 200;
                list[workingPosition].Shared = true;
                workingPosition++;
            }
            
            listView.Items.Refresh();
            UpdateStatus("All priorities reset to default");
        }

        private void ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            RenumberAll();
            listView.Items.Refresh();
        }

        private void ChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RenumberAll();
            listView.Items.Refresh();
        }

        private void OnListViewDrop(object sender, DragEventArgs e)
        {
            // Code to recalculate and update new priorities
            DistributionPoint dp = e.Data.GetData(typeof(DistributionPoint)) as DistributionPoint;

            int myPosition = list.IndexOf(dp);
            int maxIndex = list.Count - 1;

            // determine if moved item should use "shared" priority based on context of drop
            if ((myPosition == 0) | (myPosition == maxIndex) | ((myPosition > 0) && (list[myPosition - 1].Shared == false)) | ((myPosition < maxIndex) && (list[myPosition + 1].Shared == false)))
                list[myPosition].Shared = false;
            else
                list[myPosition].Shared = true;

            RenumberAll();
            listView.Items.Refresh();
        }

        private void RenumberAll()
        {
            int maxIndex = list.Count - 1;
            int workingPosition = 0;
            list[workingPosition].NewPriority = 200;

            while (workingPosition < maxIndex)
            {
                if (list[workingPosition].Shared && list[workingPosition + 1].Shared)
                {
                    list[workingPosition + 1].NewPriority = list[workingPosition].NewPriority;
                }
                else
                {
                    list[workingPosition + 1].NewPriority = list[workingPosition].NewPriority + 1;
                }

                workingPosition++;
            }
        }

        private void UpdateCurrentColumn()
        {
            int maxIndex = list.Count - 1;
            int workingPosition = 0;

            while (workingPosition <= maxIndex)
            {
                list[workingPosition].Priority = list[workingPosition].NewPriority;
                workingPosition++;
            }

            listView.Items.Refresh();
        }

        private void UpdateStatus(string value)
        {
            synchronizationContext.Post(new System.Threading.SendOrPostCallback(o =>
            {
                tbStatus.Text = String.Format("Status: {0}", o);
            }), value);
        }

        private bool IsPriority(System.Management.ManagementBaseObject obj)
        {
            if ((string)obj.GetPropertyValue("PropertyName") == "Priority")
                return true;
            else
                return false;
        }

        private void TbSiteServer_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)e.OriginalSource;
            tb.SelectAll();
        }

        private void TbSiteCode_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)e.OriginalSource;
            tb.SelectAll();
        }
    }
}