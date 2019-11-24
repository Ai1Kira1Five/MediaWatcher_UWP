using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Devices.Power;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Power;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace MediaWatcher
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            myFrame.Navigate(typeof(HomePage));
            #region StartBatterySetup
            var batteryReport = Battery.AggregateBattery.GetReport();
            var percentage = Math.Round((batteryReport.RemainingCapacityInMilliwattHours.Value /
                             (double)batteryReport.FullChargeCapacityInMilliwattHours.Value) * 100);
            batteryStatus_text.Text = percentage.ToString() + "%";
            if (percentage < 40.0) batteryStatus_icon.Text = "\uE851";
            else if (percentage >= 40.0 && percentage < 70.0) batteryStatus_icon.Text = "\uE855";
            else batteryStatus_icon.Text = "\uE83F";
            Battery.AggregateBattery.ReportUpdated += BatteryReportUpdated;
            #endregion

            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
        }

        #region Buttons
        private void homeBtn_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(typeof(HomePage));
            textHeader.Text = "Home";
        }

        private void gamburgerBtn_Click(object sender, RoutedEventArgs e)
        {
            mySplit.IsPaneOpen = !mySplit.IsPaneOpen;
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void listBoxButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (videos_page.IsSelected)
            {
                myFrame.Navigate(typeof(VideosWatcher));
                textHeader.Text = "Videos";
            }
            if (lb2Item.IsSelected)
            {
                //myFrame.Navigate(typeof(LabPage2));
                textHeader.Text = "Pictures";
            }
            if (settingsPage.IsSelected)
            {
                //myFrame.Navigate(typeof(LabPage4));
                textHeader.Text = "Settings";
            }
        }
        #endregion

        #region Background activity
        private async void BatteryReportUpdated(Battery sender, object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

                var batteryReport = Battery.AggregateBattery.GetReport();
                var percentage = Math.Round((batteryReport.RemainingCapacityInMilliwattHours.Value /
                             (double)batteryReport.FullChargeCapacityInMilliwattHours.Value) * 100);
                batteryStatus_text.Text = percentage.ToString() + "%";

                switch (batteryReport.Status)
                {
                    case BatteryStatus.Discharging:
                        if (percentage < 40.0) batteryStatus_icon.Text = "\uE851";
                        else if (percentage >= 40.0 && percentage < 70.0) batteryStatus_icon.Text = "\uE855";
                        else batteryStatus_icon.Text = "\uE83F";
                        break;
                    case BatteryStatus.Charging:
                        if (percentage < 40.0) batteryStatus_icon.Text = "\uE85B";
                        else if (percentage >= 40.0 && percentage < 70.0) batteryStatus_icon.Text = "\uE860";
                        else batteryStatus_icon.Text = "\uEBB5";
                        break;
                }
            });
        }
        #endregion
    }
}
