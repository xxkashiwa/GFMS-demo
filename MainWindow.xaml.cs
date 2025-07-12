using GFMS.Models;
using GFMS.Pages;
using GFMS.Services;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GFMS
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
            SetTitleBar(AppTitleBar);
            contentFrame.Navigate(typeof(HomePage));

            UserManager.Instance.OnAuthedUserChanged += UserManager_OnAuthedUserChanged;

        }
        private void UserManager_OnAuthedUserChanged(object sender, EventArgs e)

        {
            if (UserManager.Instance.IsAuthed)
            {
                HomePageNavigationItem.Content = "µÇÂ½ÑéÖ¤le";
            }
        }
        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var pageTag = args.SelectedItemContainer.Tag as string;
            Type? pageType = pageTag switch
            {
                "HomePage" => typeof(HomePage),
                "DataCollectionPage" => typeof(DataCollectionPage),
                "FileManagementPage" => typeof(FileManagementPage),
                "FileProgressPage" => typeof(FileProgressPage),
                "SearchPage" => typeof(SearchPage),
                _ => null
            };
            // todo delete this line
            if (pageTag == "DataCollectionPage")
            {
                var user = new User { UserId = "12312" };
                UserManager.Instance.AuthenticateUser(user);
            }
            if (pageType != null)
            {
                contentFrame.Navigate(pageType);
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            OverlappedPresenter presenter = (OverlappedPresenter)AppWindow.Presenter;
            presenter.Minimize();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            OverlappedPresenter presenter = (OverlappedPresenter)AppWindow.Presenter;
            if (presenter.State == OverlappedPresenterState.Maximized)
            {
                presenter.Restore();
            }
            else
            {
                presenter.Maximize();
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
