using System;
using Windows.Networking.PushNotifications;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.WindowsAzure.MobileServices;

namespace MFAzureNotificationWin8
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //Init Function for Registering for Push Notifications
        private async void InitNotificationsAsync()
        {
            // Request a push notification channel.
            PushNotificationChannel channel = await PushNotificationChannelManager
                .CreatePushNotificationChannelForApplicationAsync();

            // Register for notifications using the new channel
            Exception exception = null;
            try
            {
                await App.MobileService.GetPush().RegisterNativeAsync(channel.Uri);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (exception != null)
            {
                var dialog = new MessageDialog(exception.Message, "Registering Channel URI");
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
            else
            {
                TextBlock.Text = "Successfully registered for WNS";
            }
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            InitNotificationsAsync();
        }
    }
}