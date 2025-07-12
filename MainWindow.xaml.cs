using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using GFMS.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
            this.SetTitleBar(mainNavigationView);
            contentFrame.Navigate(typeof(HomePage));
        }

        private void mainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
            if (pageType != null)
            {
                contentFrame.Navigate(pageType);
            }
        }
    }
}
