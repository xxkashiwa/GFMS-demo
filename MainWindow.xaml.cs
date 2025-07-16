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

            UserManager.Instance.OnAuthedUserChanged += UserManager_OnAuthedUserChanged;
            NavigationWithAuth();

        }
        private void NavigationItemsAllEnable()
        {
            foreach (var obj in MainNavigationView.MenuItems)
            {

                if (obj is NavigationViewItem item)
                {
                    item.IsEnabled = true;
                }
            }
        }
        private void NavigationItemsDisable(NavigationViewItem[] items)
        {
            foreach (var item in items)
            {
                item.IsEnabled = false;
            }
        }

        private void NavigationWithAuth()
        {
            NavigationItemsAllEnable();
            if (UserManager.Instance.IsAuthed)
            {
                contentFrame.Navigate(typeof(HomePage));

                // 使用模式匹配简化角色判断
                NavigationViewItem[] disableItems = UserManager.Instance.AuthedUser!.GrantedType switch
                {
                    "Admin" => [],
                    "Teacher" => [FileProgressPageNavigationItem],
                    "Student" => [DataCollectionPageNavigationItem],
                    _ => MainNavigationView.MenuItems.OfType<NavigationViewItem>()
                        .Where(item => item.Tag as string != "HomePage").ToArray()

                };

                if (disableItems.Length > 0)
                    NavigationItemsDisable(disableItems);
            }
            else
            {
                contentFrame.Navigate(typeof(AuthPage));
                MainNavigationView.SelectedItem = HomePageNavigationItem;
                NavigationItemsDisable(
                    MainNavigationView.MenuItems.OfType<NavigationViewItem>()
                        .Where(item => item.Tag as string != "HomePage").ToArray()
                    );
            }
        }
        private void UserManager_OnAuthedUserChanged(object sender, EventArgs e)
        {
            NavigationWithAuth();
        }
        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (UserManager.Instance.IsAuthed == false && args.SelectedItemContainer.Tag as string != "HomePage")
            {
                contentFrame.Navigate(typeof(AuthPage));
                sender.SelectedItem = HomePageNavigationItem;
                return;
            }
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
