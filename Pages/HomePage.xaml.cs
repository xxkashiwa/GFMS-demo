using GFMS.Services;
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
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GFMS.Pages
{
    /// <summary>
    /// 主页面，显示系统概览和快速操作
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            this.Loaded += HomePage_Loaded;
        }

        /// <summary>
        /// 页面加载事件
        /// </summary>
        private async void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPageDataAsync();
        }

        /// <summary>
        /// 加载页面数据
        /// </summary>
        private async Task LoadPageDataAsync()
        {
            try
            {
                // 更新欢迎信息
                UpdateWelcomeMessage();
                
                // 模拟数据加载
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                // TODO: 添加错误处理和日志记录
                System.Diagnostics.Debug.WriteLine($"加载数据时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新欢迎信息
        /// </summary>
        private void UpdateWelcomeMessage()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser != null)
            {
                // 假设WelcomeUserText是XAML中定义的TextBlock
                // WelcomeUserText.Text = $"欢迎您，{currentUser.Username}！";
            }
        }

        /// <summary>
        /// 导航到指定页面并更新导航栏选中状态
        /// </summary>
        /// <param name="pageType">目标页面类型</param>
        /// <param name="navigationTag">导航标签</param>
        private void NavigateToPageWithNavigation(Type pageType, string navigationTag)
        {
            // 导航到目标页面
            Frame.Navigate(pageType);
            
            // 更新主窗口导航栏的选中状态
            if (((App)App.Current).MainWindow is MainWindow mainWindow)
            {
                // 通过公共属性访问 NavigationView 控件
                var navigationView = mainWindow.NavigationView;
                if (navigationView != null)
                {
                    // 查找对应的导航项
                    var targetItem = navigationView.MenuItems
                        .OfType<NavigationViewItem>()
                        .FirstOrDefault(item => item.Tag as string == navigationTag);
                    
                    if (targetItem != null)
                    {
                        navigationView.SelectedItem = targetItem;
                    }
                }
            }
        }

        /// <summary>
        /// 新增学生按钮点击事件
        /// </summary>
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            // 导航到数据收集页面（用于新增学生）
            NavigateToPageWithNavigation(typeof(DataCollectionPage), "DataCollectionPage");
        }

        /// <summary>
        /// 批量导入按钮点击事件
        /// </summary>
        private void BatchImportButton_Click(object sender, RoutedEventArgs e)
        {
            // 导航到数据收集页面（用于批量导入）
            NavigateToPageWithNavigation(typeof(DataCollectionPage), "DataCollectionPage");
        }

        /// <summary>
        /// 档案管理按钮点击事件
        /// </summary>
        private void FileManagementButton_Click(object sender, RoutedEventArgs e)
        {
            // 导航到档案管理页面
            NavigateToPageWithNavigation(typeof(FileManagementPage), "FileManagementPage");
        }

        /// <summary>
        /// 档案转递按钮点击事件
        /// </summary>
        private void FileTransferButton_Click(object sender, RoutedEventArgs e)
        {
            // 导航到文件进度页面（用于档案转递）
            NavigateToPageWithNavigation(typeof(FileProgressPage), "FileProgressPage");
        }

        /// <summary>
        /// 查询服务按钮点击事件
        /// </summary>
        private void SearchServiceButton_Click(object sender, RoutedEventArgs e)
        {
            // 导航到搜索页面
            NavigateToPageWithNavigation(typeof(SearchPage), "SearchPage");
        }

        /// <summary>
        /// 统计报表按钮点击事件
        /// </summary>
        private void StatisticsReportButton_Click(object sender, RoutedEventArgs e)
        {
            // 导航到文件管理页面（用于统计报表）
            NavigateToPageWithNavigation(typeof(FileManagementPage), "FileManagementPage");
        }

        /// <summary>
        /// 查看详细报表按钮点击事件
        /// </summary>
        private void ViewDetailedReportButton_Click(object sender, RoutedEventArgs e)
        {
            // 导航到文件管理页面（用于详细报表）
            NavigateToPageWithNavigation(typeof(FileManagementPage), "FileManagementPage");
        }

        /// <summary>
        /// 材料审核-立即处理按钮点击事件
        /// </summary>
        private void ProcessMaterialsButton_Click(object sender, RoutedEventArgs e)
        {
            // 导航到文件管理页面（用于材料审核）
            NavigateToPageWithNavigation(typeof(FileManagementPage), "FileManagementPage");
        }

        /// <summary>
        /// 调档申请-查看申请按钮点击事件
        /// </summary>
        private void ViewApplicationsButton_Click(object sender, RoutedEventArgs e)
        {
            // 导航到搜索页面（用于查看申请）
            NavigateToPageWithNavigation(typeof(SearchPage), "SearchPage");
        }

        /// <summary>
        /// 退出登录按钮点击事件
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserManager.Instance.Logout();
        }
    }
}
